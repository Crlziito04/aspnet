using AppStore.Models.Domain;
using AppStore.Models.DTO;
using AppStore.Repository.Abstract;
namespace AppStore.Repository.Implementation;

public class LibroService : IlibroService
{
  private readonly DatabaseContext ctx;
  public LibroService(DatabaseContext ctxParam)
  {
    ctx = ctxParam;
  }
  public bool Add(Libro libro)
  {
    try
    {
      ctx.Libros.Add(libro);
      ctx.SaveChanges();
      foreach (int categoriaId in libro.Categorias!)
      {
        var libroCat = new LibroCategoria
        {
          LibroId = libro.Id,
          CategoriaId = categoriaId
        };
        ctx.LibroCategorias!.Add(libroCat);
      }
      ctx.SaveChanges();
      return true;
    }
    catch (Exception e)
    {
      Console.WriteLine($"Error {e.Message}");
      return false;
    }
  }

  public bool Delete(int id)
  {
    try
    {
      var libro = GetById(id);
      if (libro is null)
      {
        return false;
      }

      var libroCat = ctx.LibroCategorias.Where(a => a.LibroId == libro.Id);
      ctx.LibroCategorias.RemoveRange(libroCat);
      ctx.Libros!.Remove(libro);

      ctx.SaveChanges();

      return true;
    }
    catch (Exception)
    {
      return false;
    }
  }

  public Libro GetById(int id)
  {
    return ctx.Libros.Find(id)!;
  }

  public List<int> GetCategoryByLibroId(int LibroId)
  {
    return ctx.LibroCategorias.Where(a => a.LibroId == LibroId).Select(a => a.CategoriaId).ToList()!;
  }

  public LibroListVm List(string term = "", bool paging = false, int currentPage = 0)
  {
    var data = new LibroListVm();
    var list = ctx.Libros.ToList();

    if (!string.IsNullOrEmpty(term))
    {
      term = term.ToLower();
      list = list.Where(l => l.Titulo!.ToLower().StartsWith(term)).ToList();

    }
    if (paging)
    {
      int pageSize = 5;
      int count = list.Count;
      int TotalPages = (int)Math.Ceiling(count / (double)pageSize);
      list = list.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
      data.PageSize = pageSize;
      data.CurrentPage = currentPage;
      data.TotalPages = TotalPages;
    }
    foreach (var item in list)
    {
      var categorias = (
        from categoria in ctx.Categorias
        join lc in ctx.LibroCategorias
        on categoria.Id equals lc.CategoriaId
        where lc.LibroId == item.Id
        select categoria.Nombre
        ).ToList();

      var catgoriaNombres = string.Join(",", categorias);
      item.CategoriasNames = catgoriaNombres;
    }
    data.LibroList = list.AsQueryable();
    return data;
  }

  public bool Update(Libro libro)
  {
    try
    {
      var catgEliminar = ctx.LibroCategorias!.Where(a => a.LibroId == libro.Id);
      foreach (var catg in catgEliminar)
      {
        ctx.LibroCategorias.Remove(catg);
      }
      foreach (int categoriId in libro.Categorias!)
      {
        var librCat = new LibroCategoria
        {
          CategoriaId = categoriId,
          LibroId = libro.Id
        };
        ctx.LibroCategorias.Add(librCat);
      }
      ctx.Libros!.Update(libro);
      ctx.SaveChanges();
      return true;
    }
    catch (Exception)
    {
      return false;
    }
  }
}
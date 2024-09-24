using AppStore.Models.Domain;
using AppStore.Repository.Abstract;

namespace AppStore.Repository.Implementation;

public class CategoriaService : ICategoriaService
{
  private readonly DatabaseContext _ctx;

  public CategoriaService(DatabaseContext ctx)
  {
    _ctx = ctx;
  }

  public IQueryable<Categoria> List()
  {
    return _ctx.Categorias.AsQueryable();
  }
}
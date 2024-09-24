using AppStore.Models.Domain;
using AppStore.Repository.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Appstore.Controllers;
[Authorize]
public class LibroController : Controller
{
  private readonly IlibroService _libroService;
  private readonly IFileService _fileService;
  private readonly ICategoriaService _categoriaService;
  public LibroController(IlibroService libroService, IFileService fileService, ICategoriaService categoriaService)
  {
    _libroService = libroService;
    _fileService = fileService;
    _categoriaService = categoriaService;
  }

  //*metodos api
  [HttpPost]
  public IActionResult Add(Libro libro)
  {

    libro.CategoriasListItem = _categoriaService.List().Select(a => new SelectListItem { Text = a.Nombre, Value = a.Id.ToString() });

    if (!ModelState.IsValid)
    {
      return View();
    }
    if (libro.ImageFile != null)
    {
      var res = _fileService.SaveImg(libro.ImageFile);
      Console.WriteLine(res.Item2);
      if (res.Item1 == 0)
      {
        TempData["msg"] = "Image no puedo guardarse exitosamente";
        return View(libro);
      }
      var imgName = res.Item2;
      libro.Image = imgName;
    }
    var resLibro = _libroService.Add(libro);
    if (resLibro)
    {
      TempData["msg"] = "Se agrego libro Exitosamente";
      return RedirectToAction(nameof(Add));
    }
    TempData["msg"] = "Error guardando libro";
    return View(libro);
  }

  [HttpPost]
  public IActionResult Edit(Libro libro)
  {
    var ctgLibro = _libroService.GetCategoryByLibroId(libro.Id);
    var MultiSelectListCtg = new MultiSelectList(_categoriaService.List(), "Id", "Nombre", ctgLibro);
    libro.MultiCategoriasList = MultiSelectListCtg;

    if (!ModelState.IsValid)
    {
      return View(libro);
    }

    if (libro.ImageFile != null)
    {
      var fileRes = _fileService.SaveImg(libro.ImageFile);
      if (fileRes.Item1 == 0)
      {
        TempData["msg"] = "Imagen no fue guardada";
        return View(libro);
      }
      var imagenName = fileRes.Item2;
      libro.Image = imagenName;
    }

    var res = _libroService.Update(libro);
    if (!res)
    {
      TempData["msg"] = "Error, no se pudo actualizar libro";
      return View(libro);
    }
    TempData["msg"] = "Se actualizo el libro";
    return View(libro);
  }

  //*Metodos navegacion
  public IActionResult Add()
  {
    var libre = new Libro();
    libre.CategoriasListItem = _categoriaService.List().Select(a => new SelectListItem { Text = a.Nombre, Value = a.Id.ToString() });

    return View(libre);
  }

  public IActionResult Edit(int id)
  {
    var libro = _libroService.GetById(id);
    var ctgLibro = _libroService.GetCategoryByLibroId(id);
    var MultiSelectListCtg = new MultiSelectList(_categoriaService.List(), "Id", "Nombre", ctgLibro);
    libro.MultiCategoriasList = MultiSelectListCtg;

    return View(libro);
  }
  public IActionResult LibroList()
  {
    var libros = _libroService.List();
    return View(libros);
  }
  public IActionResult Delete(int id)
  {
    _libroService.Delete(id);
    return RedirectToAction(nameof(LibroList));
  }


}

using AppStore.Repository.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace AppStore.Controllers;
public class HomeController : Controller
{

  private readonly IlibroService _libroService;

  public HomeController(IlibroService libroService)
  {
    _libroService = libroService;
  }
  public IActionResult Index(string term = "", int currentPage = 1)
  {
    var libros = _libroService.List(term, true, currentPage);
    return View(libros);
  }
  public IActionResult LibroDetail(int libroId)
  {
    var libro = _libroService.GetById(libroId);
    return View(libro);
  }
  public IActionResult About()
  {
    return View();
  }
}
using AppStore.Models.DTO;
using AppStore.Repository.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace AppStore.Controllers;
public class UserAuthController : Controller
{
  private readonly IUserAuth _userAuth;

  public UserAuthController(IUserAuth userAuth)
  {
    _userAuth = userAuth;
  }

  public IActionResult Login()
  {
    return View();
  }

  [HttpPost]
  public async Task<ActionResult> login(LoginModel login)
  {
    if (!ModelState.IsValid)
    {
      return View(login);
    }

    var res = await _userAuth.LoginAsync(login);
    if (res.StatusCode == 1)
    {
      return RedirectToAction("Index", "Home");
    }
    else
    {
      TempData["msg"] = res.Msg;
      return RedirectToAction(nameof(Login));
    }
  }

  public async Task<IActionResult> logOut()
  {
    await _userAuth.LogOutAsync();
    return RedirectToAction(nameof(Login));
  }
}
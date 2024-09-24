using AppStore.Models.Domain;
using AppStore.Models.DTO;
using AppStore.Repository.Abstract;
using Microsoft.AspNetCore.Identity;

namespace AppStore.Repository.Implementation;

public class UserAuthServices : IUserAuth
{
  private readonly UserManager<ApplicationUser> userManager;
  private readonly SignInManager<ApplicationUser> signInManager;

  public UserAuthServices(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
  {
    this.userManager = userManager;
    this.signInManager = signInManager;
  }

  public async Task<Status> LoginAsync(LoginModel login)
  {
    var status = new Status();
    var user = await userManager.FindByNameAsync(login.Username!);


    if (user is null)
    {
      status.StatusCode = 0;
      status.Msg = "User not found";
      return status;
    }

    var checkPass = await userManager.CheckPasswordAsync(user, login.Password!);
    if (!checkPass)
    {
      status.StatusCode = 0;
      status.Msg = "Password not valid";
      return status;
    }

    var res = await signInManager.PasswordSignInAsync(user, login.Password!, true, false);
    if (!res.Succeeded)
    {
      status.StatusCode = 0;
      status.Msg = "Credenciales incorrectas";
    }

    status.StatusCode = 1;
    status.Msg = "Login Exitoso";
    return status;
  }

  public async Task LogOutAsync()
  {
    await signInManager.SignOutAsync();
  }
}
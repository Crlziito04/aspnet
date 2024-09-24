using AppStore.Models.DTO;

namespace AppStore.Repository.Abstract;

public interface IUserAuth
{
  Task<Status> LoginAsync(LoginModel login);
  Task LogOutAsync();
}
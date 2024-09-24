using AppStore.Models.Domain;
using AppStore.Models.DTO;

namespace AppStore.Repository.Abstract;

public interface IlibroService
{
  bool Add(Libro libro);
  bool Update(Libro libro);
  Libro GetById(int id);
  bool Delete(int id);
  LibroListVm List(string term = "", bool paging = false, int currentPage = 0);
  List<int> GetCategoryByLibroId(int LibroId);
}
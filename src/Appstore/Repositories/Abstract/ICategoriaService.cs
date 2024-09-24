using AppStore.Models.Domain;

namespace AppStore.Repository.Abstract;

public interface ICategoriaService
{
  IQueryable<Categoria> List();
}
using System.ComponentModel.DataAnnotations;

namespace AppStore.Models.Domain;
public class Categoria
{
  [Key]
  [Required]
  public int Id { get; set; }
  public string? Nombre { get; set; }
  public virtual ICollection<Libro>? LibrosLista { get; set; }
  public virtual ICollection<LibroCategoria>? LibrosRelationList { get; set; }
}
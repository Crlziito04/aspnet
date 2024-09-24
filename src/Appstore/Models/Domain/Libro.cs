using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AppStore.Models.Domain;
public class Libro
{
  [Key]
  [Required]
  public int Id { get; set; }
  [Required]
  public string? Titulo { get; set; }
  public string? FechaCreacion { get; set; }
  public string? Image { get; set; }
  [Required]
  public string? Autor { get; set; }
  public virtual ICollection<Categoria>? CategoriasList { get; set; }
  public virtual ICollection<LibroCategoria>? CategoriasRelationList { get; set; }

  [NotMapped]
  public List<int>? Categorias { get; set; }
  [NotMapped]
  public string? CategoriasNames { get; set; }
  [NotMapped]
  public IFormFile? ImageFile { get; set; }
  [NotMapped]
  public IEnumerable<SelectListItem>? CategoriasListItem { get; set; }
  [NotMapped]
  public MultiSelectList? MultiCategoriasList { get; set; }

}
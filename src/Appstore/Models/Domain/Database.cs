using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace AppStore.Models.Domain;
public class DatabaseContext : IdentityDbContext<ApplicationUser>
{

  public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
  {

  }

  protected override void OnModelCreating(ModelBuilder builder)
  {
    base.OnModelCreating(builder);

    builder.Entity<LibroCategoria>()
          .HasKey(lc => new { lc.LibroId, lc.CategoriaId });
    builder.Entity<LibroCategoria>()
            .HasOne(lc => lc.Libro)
            .WithMany(l => l.CategoriasRelationList)
            .HasForeignKey(lc => lc.LibroId);

    builder.Entity<LibroCategoria>()
        .HasOne(lc => lc.Categoria)
        .WithMany(c => c.LibrosRelationList)
        .HasForeignKey(lc => lc.CategoriaId);
  }

  public DbSet<Libro> Libros { get; set; }
  public DbSet<Categoria> Categorias { get; set; }
  public DbSet<LibroCategoria> LibroCategorias { get; set; }
}

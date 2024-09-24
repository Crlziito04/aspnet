using Microsoft.AspNetCore.Identity;

namespace AppStore.Models.Domain;
public class LoadDatabase
{
  public static async Task InsertarData(DatabaseContext context, UserManager<ApplicationUser> manager, RoleManager<IdentityRole> roleManager)
  {
    if (!roleManager.Roles.Any())
    {
      await roleManager.CreateAsync(new IdentityRole("ADMIN"));
    }

    if (!manager.Users.Any())
    {
      var user = new ApplicationUser
      {
        Nombre = "crlz",
        Email = "admin@example.com",
        UserName = "dito12"
      };
      await manager.CreateAsync(user, "P@ssword123!");
      await manager.AddToRoleAsync(user, "ADMIN");
    }
    if (!context.Categorias.Any())
    {
      await context.Categorias.AddRangeAsync(
         new Categoria
         {
           Nombre = "Drama"
         },
         new Categoria
         {
           Nombre = "Terro"
         },
         new Categoria
         {
           Nombre = "Comedia"
         },
         new Categoria
         {
           Nombre = "Crimen"
         },
         new Categoria
         {
           Nombre = "Aventura"
         }
       );
      await context.SaveChangesAsync();
    }
    if (!context.Libros.Any())
    {
      await context.Libros.AddRangeAsync(
         new Libro
         {
           Titulo = "HarryPotter",
           FechaCreacion = "06/06/2015",
           Image = "HP.jpg",
           Autor = "Cervenza",
         },
           new Libro
           {
             Titulo = "Don Quijote",
             FechaCreacion = "06/06/2020",
             Image = "quijote.jpg",
             Autor = "NOSE mol",
           }
       );
      await context.SaveChangesAsync();
    }
    if (!context.LibroCategorias.Any())
    {
      await context.LibroCategorias.AddRangeAsync(
          new LibroCategoria
          {
            CategoriaId = 1,
            LibroId = 1,
          },
          new LibroCategoria
          {
            CategoriaId = 2,
            LibroId = 2,
          }
        );
      await context.SaveChangesAsync();
    }
    context.SaveChanges();
  }
}
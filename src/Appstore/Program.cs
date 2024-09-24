using AppStore.Models.Domain;
using AppStore.Repository.Abstract;
using AppStore.Repository.Implementation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IlibroService, LibroService>();
builder.Services.AddScoped<IUserAuth, UserAuthServices>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
.AddEntityFrameworkStores<DatabaseContext>()
.AddDefaultTokenProviders();

builder.Services.AddDbContext<DatabaseContext>(opt =>
{
  opt.LogTo(Console.WriteLine, new[]{
  DbLoggerCategory.Database.Command.Name},
      LogLevel.Information).EnableSensitiveDataLogging();
  opt.UseSqlite(builder.Configuration.GetConnectionString("SqliteDatabase"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Home/Error");
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var ambiente = app.Services.CreateScope())
{
  var services = ambiente.ServiceProvider;
  try
  {
    var context = services.GetRequiredService<DatabaseContext>();
    var manager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    await context.Database.MigrateAsync();
    await LoadDatabase.InsertarData(context, manager, roleManager);
  }
  catch (Exception e)
  {
    var logging = services.GetRequiredService<ILogger<Program>>();
    logging.LogError(e + "Error en la insercion de datos");
  }
}


app.Run();

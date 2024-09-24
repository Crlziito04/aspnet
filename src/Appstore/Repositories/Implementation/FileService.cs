using AppStore.Repository.Abstract;
namespace AppStore.Repository.Implementation;
public class FileService : IFileService
{
  private readonly IWebHostEnvironment _webHostEnvironment;

  public FileService(IWebHostEnvironment webHostEnvironment)
  {
    _webHostEnvironment = webHostEnvironment;
  }

  public bool DeleteImg(string ImgFileName)
  {
    try
    {
      var wwwPath = _webHostEnvironment.WebRootPath;
      var path = Path.Combine(wwwPath, "Uploads\\", ImgFileName);
      if (!System.IO.File.Exists(path))
      {
        System.IO.File.Delete(path);
        return true;
      }
      return false;
    }
    catch (Exception)
    {
      return false;
    }
  }

  public Tuple<int, string> SaveImg(IFormFile imageFile)
  {
    try
    {
      var wwwPath = _webHostEnvironment.WebRootPath;
      var path = Path.Combine(wwwPath, "Uploads");

      if (!Directory.Exists(path))
      {
        Directory.CreateDirectory(path);
      }

      var ext = Path.GetExtension(imageFile.FileName);
      var allowedExtensions = new String[] { ".jpg", ".jpeg", ".png" };
      if (!allowedExtensions.Contains(ext))
      {
        var msg = $"Solo esta permitido las extensiones {allowedExtensions}";
        return new Tuple<int, string>(0, msg);
      }

      var uniqueString = Guid.NewGuid().ToString();
      var newFileName = uniqueString + ext;
      var fileWithPath = Path.Combine(path, newFileName);

      var stream = new FileStream(fileWithPath, FileMode.Create);
      imageFile.CopyTo(stream);
      stream.Close();

      return new Tuple<int, string>(1, newFileName);
    }
    catch (Exception)
    {
      return new Tuple<int, string>(0, "Error guardando img");
    }
  }
}
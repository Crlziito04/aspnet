namespace AppStore.Repository.Abstract;

public interface IFileService
{

  public Tuple<int, string> SaveImg(IFormFile imageFile);
  public bool DeleteImg(string ImgFileName);
}
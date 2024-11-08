namespace TestProj.Infrastructure.Utilities;

public interface IFileUtility
{
    Task<string> SaveImageAsync(IFormFile imageFile);

    string ConvertImageToBase64(string imageName);
}

namespace TestProj.Core.Services;

public interface IFileService
{
    Task<string> SaveImageAsync(IFormFile imageFile);

    string ConvertImageToBase64(string imageName);

    Task DeleteImageAsync(string imageName);
}

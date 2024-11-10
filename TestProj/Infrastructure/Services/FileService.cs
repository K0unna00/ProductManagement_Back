using TestProj.Core.Services;

namespace TestProj.Infrastructure.Services;

public class FileService : IFileService
{
    private readonly string _imageFolder;

    public FileService(IConfiguration configuration)
    {
        _imageFolder = configuration["FileSettings:FileFolderPath"];

        if (!Directory.Exists(_imageFolder))
        {
            Directory.CreateDirectory(_imageFolder);
        }
    }

    public async Task<string> SaveImageAsync(IFormFile imageFile)
    {
        if (imageFile == null || imageFile.Length == 0)
            throw new ArgumentException("Image file is not valid");

        string uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";

        string filePath = Path.Combine(_imageFolder, uniqueFileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await imageFile.CopyToAsync(fileStream);
        }

        return uniqueFileName;
    }

    public string ConvertImageToBase64(string imageName)
    {
        var imagePath = Path.Combine("wwwroot/images", imageName);
        if (!File.Exists(imagePath))
        {
            return null;
        }

        var imageBytes = File.ReadAllBytes(imagePath);
        return Convert.ToBase64String(imageBytes);
    }
}

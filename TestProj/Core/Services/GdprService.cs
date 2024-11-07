using TestProj.Core.Entities;

namespace TestProj.Core.Services;

public class GdprService
{
    public string AnonymizeData(string data) => new string('*', data.Length);

    public void RemoveSensitiveData(Product product)
    {
        product.Name = AnonymizeData(product.Name);
        product.Description = AnonymizeData(product.Description);
    }
}

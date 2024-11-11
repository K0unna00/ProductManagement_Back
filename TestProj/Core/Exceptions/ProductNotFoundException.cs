namespace TestProj.Core.Exceptions;

public class ProductNotFoundException : Exception
{
    public ProductNotFoundException(string message = "Product was not found.") : base(message)
    {
    }
}
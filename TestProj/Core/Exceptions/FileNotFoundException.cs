namespace TestProj.Core.Exceptions;

public class FileNotFoundException : Exception
{
    public FileNotFoundException(string message = "File does not exist.") : base(message)
    {
    }
}
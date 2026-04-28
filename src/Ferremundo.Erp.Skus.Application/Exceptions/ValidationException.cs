namespace Ferremundo.Erp.Skus.Application.Exceptions;

public sealed class ValidationException : Exception
{
    public ValidationException(string code, string message)
        : base(message)
    {
        Code = code;
    }

    public string Code { get; }
}

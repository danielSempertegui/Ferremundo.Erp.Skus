namespace Ferremundo.Erp.Skus.Application.Exceptions;

public class BusinessException : Exception
{
    public BusinessException(string code, string message)
        : base(message)
    {
        Code = code;
    }

    public string Code { get; }
}

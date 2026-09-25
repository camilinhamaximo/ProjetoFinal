namespace ProjetoFinal.API.Contracts;

public sealed record ApiErrorResponse(
    int Status,
    string Code,
    string Message,
    IReadOnlyDictionary<string, string[]>? Errors = null);

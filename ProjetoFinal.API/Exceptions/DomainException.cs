namespace ProjetoFinal.API.Exceptions;

public sealed class ResourceNotFoundException(string message) : Exception(message);

public sealed class BusinessRuleViolationException(string message) : Exception(message);

public sealed class RequestValidationException(string message) : Exception(message);

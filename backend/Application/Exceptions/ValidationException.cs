namespace CustomerRegistrationOnboarding.Application.Exceptions;

public class AppValidationException : Exception
{
    public IReadOnlyDictionary<string, string[]> Errors { get; }

    public AppValidationException(IDictionary<string, string[]> errors)
        : base("One or more validation errors occurred.")
    {
        Errors = new Dictionary<string, string[]>(errors);
    }
}

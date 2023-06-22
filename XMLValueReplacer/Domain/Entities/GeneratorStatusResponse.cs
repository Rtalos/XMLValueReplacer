namespace XMLValueReplacer.Domain.Entities;

internal class GeneratorStatusResponse
{
    public bool IsSuccessful { get; set; } = true;
    public Exception? Exception { get; set; }
    public string ErrorMessage => Exception?.Message ?? "";
}


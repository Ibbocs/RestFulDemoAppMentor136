namespace RestfullApiNet6M136.Models;

public class ErrorDTO
{
    public List<String>? Errors { get; private set; } = new List<String>();


    public ErrorDTO(string error)
    {
        Errors.Add(error);
    }

    public ErrorDTO(List<string> errors)
    {
        Errors = errors;
    }

    public ErrorDTO()
    {

    }
}

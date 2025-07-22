using System.Collections.Generic;

namespace Parkman.Frontend.Models;

public class ProblemDetails
{
    public string? Title { get; set; }
    public string? Detail { get; set; }
    public int? Status { get; set; }
}

public class ValidationProblemDetails : ProblemDetails
{
    public IDictionary<string, string[]> Errors { get; set; } = new Dictionary<string, string[]>();
}

namespace webForwarder_VIETTHAI.Models.Api;
public sealed class ApiEnvelope
{
    public int Status { get; set; }
    public string? Data { get; set; }
    public string? ErrorMsg { get; set; }
}

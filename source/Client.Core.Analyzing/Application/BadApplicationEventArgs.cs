namespace Client.Core.Analyzing.Application;

public class BadApplicationEventArgs
{
    public string Application { get; set; }
    public string Reason { get; set; }
    public string? Message { get; set; }

    public BadApplicationEventArgs() { }

    public BadApplicationEventArgs(string application, string reason)
    {
        Application = application;
        Reason = reason;
    }
    public BadApplicationEventArgs(string application, string reason, string message)
    {
        Application = application;
        Reason = reason;
        Message = message;
    }
}

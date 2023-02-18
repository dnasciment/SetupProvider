namespace SetupProvider.Domain;

public class Email : IEmail
{
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}
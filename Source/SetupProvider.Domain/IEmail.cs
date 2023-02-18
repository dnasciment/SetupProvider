namespace SetupProvider.Domain;

public interface IEmail
{
    public string Subject { get; }
    public string Body { get; }
}
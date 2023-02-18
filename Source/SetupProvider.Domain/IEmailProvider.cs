namespace SetupProvider.Domain;

public interface IEmailProvider<in TContext>
{
    Task<IEmail> Provide(TContext context, CancellationToken cancellationToken = default);
}

public interface IEmailProvider
{
    Task<IEmail> Provide<TContext>(TContext context, CancellationToken cancellationToken = default);
}
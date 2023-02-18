using Microsoft.Extensions.DependencyInjection;

namespace SetupProvider.Domain;

public class EmailProvider : IEmailProvider
{
    private readonly IServiceProvider _serviceProvider;

    public EmailProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public async Task<IEmail> Provide<TContext>(TContext context, CancellationToken cancellationToken = default)
    {
        var provider = _serviceProvider.GetService<IEmailProvider<TContext>>();
        if (provider == null)
        {
            throw new InvalidOperationException($"No email provider found for context {typeof(TContext).Name}");
        }

        return await provider.Provide(context, cancellationToken);
    }
}
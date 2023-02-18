using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace SetupProvider.Domain.Tests;

public class WhenProvidingEmails
{
    private readonly IServiceProvider _serviceProvider;

    private readonly EmailProvider _sut;
    private readonly object _context;
    private readonly IEmailProvider<object> _contextEmailProvider;
    private readonly IEmail _email;

    public WhenProvidingEmails()
    {
        _context = new object();

        _email = Substitute.For<IEmail>();
        _contextEmailProvider = Substitute.For<IEmailProvider<object>>();
        _contextEmailProvider.Provide(_context).Returns(_email);

        _serviceProvider = Substitute.For<IServiceProvider>();
        _serviceProvider.GetService<IEmailProvider<object>>().Returns(_contextEmailProvider);


        _sut = new EmailProvider(_serviceProvider);
    }

    [Fact]
    public void GivenNoServiceProviderWhileConstructingTheEmailProviderAnArgumentNullExceptionWillBeThrown()
    {
        Assert.Throws<ArgumentNullException>(() => new EmailProvider(null!));
    }

    [Fact]
    public async Task ThenAnEmailProviderForTheGivenContextWillBeRequested()
    {
        await Provide();
        _serviceProvider.Received(1).GetService<IEmailProvider<object>>();
    }

    [Fact]
    public async Task ThenTheEmailProviderForTheGivenContextWillBeUsedToProvideTheEmail()
    {
        await Provide();
        await _contextEmailProvider.Received(1).Provide(_context);
    }

    [Fact]
    public async Task ThenTheProvidedEmailWillBeReturned()
    {
        var actual = await Provide();
        Assert.Equal(_email, actual);
    }

    [Fact]
    public async Task GivenThereIsNoProviderForTheRequestedContextAnInvalidOperationWillBeThrown()
    {
        _serviceProvider.GetService<IEmailProvider<object>>().ReturnsNull();
        await Assert.ThrowsAsync<InvalidOperationException>(Provide);
    }

    private Task<IEmail> Provide() => _sut.Provide(_context);
}
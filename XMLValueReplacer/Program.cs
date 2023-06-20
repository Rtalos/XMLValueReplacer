using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using XMLValueReplacer;
using XMLValueReplacer.Infrastructure;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<IConsoleHandler, ConsoleHandler>();
        services.AddSingleton<IXmlHandler, XmlHandler>();
    })
    .Build();

var consoleHandler = host.Services.GetRequiredService<IConsoleHandler>();
var xmlHandler = host.Services.GetRequiredService<IXmlHandler>();

var application = new Application(consoleHandler, xmlHandler);
application.Run();
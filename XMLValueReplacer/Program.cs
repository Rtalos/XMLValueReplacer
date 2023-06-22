using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using XMLValueReplacer;
using XMLValueReplacer.Generators;
using XMLValueReplacer.Infrastructure;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<IConsoleHandler, ConsoleHandler>();
        services.AddSingleton<IXmlHandler, XmlHandler>();
        services.AddSingleton<IFileGeneratorFactory, FileGeneratorFactory>();
        services.AddSingleton<ExcelGenerator>().AddSingleton<IFileGenerator, ExcelGenerator>(s => s.GetRequiredService<ExcelGenerator>());
        services.AddSingleton<TextGenerator>().AddSingleton<IFileGenerator, TextGenerator>(s => s.GetRequiredService<TextGenerator>());
        services.AddSingleton<XmlGenerator>().AddSingleton<IFileGenerator, XmlGenerator>(s => s.GetRequiredService<XmlGenerator>());
    })
    .Build();

var fileGeneratorFactory = host.Services.GetRequiredService<IFileGeneratorFactory>();
var consoleHandler = host.Services.GetRequiredService<IConsoleHandler>();
var xmlHandler = host.Services.GetRequiredService<IXmlHandler>();

var application = new Application(consoleHandler, xmlHandler, fileGeneratorFactory);
application.Run();
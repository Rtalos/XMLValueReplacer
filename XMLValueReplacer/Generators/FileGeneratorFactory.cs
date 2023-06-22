using Microsoft.Extensions.DependencyInjection;

namespace XMLValueReplacer.Generators;

internal class FileGeneratorFactory : IFileGeneratorFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IServiceProviderIsService _serviceProviderIsService;

    public FileGeneratorFactory(IServiceProvider serviceProvider, IServiceProviderIsService serviceProviderIsService)
    {
        _serviceProvider = serviceProvider;
        _serviceProviderIsService = serviceProviderIsService;
    }

    public IFileGenerator GetFileGenerator<TFileGenerator>()
    {
        if (!_serviceProviderIsService.IsService(typeof(TFileGenerator)))
            throw new NullReferenceException("Generator does not exist in DI");

        var generator = _serviceProvider.GetService(typeof(TFileGenerator)) as IFileGenerator;

        if (generator is null)
            throw new NullReferenceException("Generator does not implement IFileGenerator");

        return generator;
    }
}


using XMLValueReplacer.Domain.Entities;

namespace XMLValueReplacer.Generators;

internal interface IFileGenerator
{
    public GeneratorStatusResponse Generate(ApplicationContext applicationContext);
}

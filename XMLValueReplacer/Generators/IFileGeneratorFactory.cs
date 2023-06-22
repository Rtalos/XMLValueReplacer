namespace XMLValueReplacer.Generators;

internal interface IFileGeneratorFactory
{
    IFileGenerator GetFileGenerator<TFileGenerator>();
}


using System.Xml.Linq;

namespace Tests;

[TestClass]
public class TemplateGeneratorTests
{
    private XDocument? _document;

    [TestInitialize]
    public void TestInitialize()
    {
        _document = XDocument.Parse(TestData.XMLDocument);
    }

    [TestMethod]
    public void GeneratedXML_Correct()
    {
        var generator = new TemplateGenerator(_document!, "prefix:", "original.xml", XPathOptionsEnum.XPath);

        var result = generator.Generate();

        var templateXml = $"{result.Document.Declaration}{Environment.NewLine}{result.Document}";

        Assert.AreEqual(TestData.AssertionTemplateXML, templateXml);
    }
}

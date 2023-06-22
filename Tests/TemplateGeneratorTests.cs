using System.Xml.Linq;
using XMLValueReplacer.Domain.Entities;
using XMLValueReplacer.Domain.Enums;
using XMLValueReplacer.Generators;

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
        var applicationContext = new ApplicationContext(_document!, "prefix:", XPathOptions.XPath, "original.xml");
        var xmlGenerator = new XmlGenerator();

        var status = xmlGenerator.Generate(applicationContext);

        var templateXml = $"{applicationContext.Document.Declaration}{Environment.NewLine}{applicationContext.Document}";

        Assert.IsTrue(status.IsSuccessful);
        Assert.AreEqual(TestData.AssertionTemplateXML, templateXml);
    }
}

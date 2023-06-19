namespace Tests;

internal class TestData
{
    internal const string XMLDocument = 
@"<?xml version=""1.0""?>
<catalog>
   <book id=""bk101"">
      <author>Gambardella, Matthew</author>
      <title>XML Developer's Guide</title>
      <genre>Computer</genre>
      <price>44.95</price>
      <publish_date>2000-10-01</publish_date>
      <description>An in-depth look at creating applications 
      with XML.</description>
   </book>
   <book id=""bk102"">
      <author>Ralls, Kim</author>
      <title>Midnight Rain</title>
      <genre>Fantasy</genre>
      <price>5.95</price>
      <publish_date>2000-12-16</publish_date>
      <description>A former architect battles corporate zombies, 
      an evil sorceress, and her own childhood to become queen 
      of the world.</description>
   </book>
   <book id=""bk103"">
      <author>Corets, Eva</author>
      <title>Maeve Ascendant</title>
      <genre>Fantasy</genre>
      <price>5.95</price>
      <publish_date>2000-11-17</publish_date>
      <description>After the collapse of a nanotechnology 
      society in England, the young survivors lay the 
      foundation for a new society.</description>
   </book>
   <book id=""bk104"">
      <author>Corets, Eva</author>
      <title>Oberon's Legacy</title>
      <genre>Fantasy</genre>
      <price>5.95</price>
      <publish_date>2001-03-10</publish_date>
      <description>In post-apocalypse England, the mysterious 
      agent known only as Oberon helps to create a new life 
      for the inhabitants of London. Sequel to Maeve 
      Ascendant.</description>
   </book>
   <book id=""bk105"">
      <author>Corets, Eva</author>
      <title>The Sundered Grail</title>
      <genre>Fantasy</genre>
      <price>5.95</price>
      <publish_date>2001-09-10</publish_date>
      <description>The two daughters of Maeve, half-sisters, 
      battle one another for control of England. Sequel to 
      Oberon's Legacy.</description>
   </book>
   <book id=""bk106"">
      <author>Randall, Cynthia</author>
      <title>Lover Birds</title>
      <genre>Romance</genre>
      <price>4.95</price>
      <publish_date>2000-09-02</publish_date>
      <description>When Carla meets Paul at an ornithology 
      conference, tempers fly as feathers get ruffled.</description>
   </book>
   <book id=""bk107"">
      <author>Thurman, Paula</author>
      <title>Splish Splash</title>
      <genre>Romance</genre>
      <price>4.95</price>
      <publish_date>2000-11-02</publish_date>
      <description>A deep sea diver finds true love twenty 
      thousand leagues beneath the sea.</description>
   </book>
   <book id=""bk108"">
      <author>Knorr, Stefan</author>
      <title>Creepy Crawlies</title>
      <genre>Horror</genre>
      <price>4.95</price>
      <publish_date>2000-12-06</publish_date>
      <description>An anthology of horror stories about roaches,
      centipedes, scorpions  and other insects.</description>
   </book>
   <book id=""bk109"">
      <author>Kress, Peter</author>
      <title>Paradox Lost</title>
      <genre>Science Fiction</genre>
      <price>6.95</price>
      <publish_date>2000-11-02</publish_date>
      <description>After an inadvertant trip through a Heisenberg
      Uncertainty Device, James Salway discovers the problems 
      of being quantum.</description>
   </book>
   <book id=""bk110"">
      <author>O'Brien, Tim</author>
      <title>Microsoft .NET: The Programming Bible</title>
      <genre>Computer</genre>
      <price>36.95</price>
      <publish_date>2000-12-09</publish_date>
      <description>Microsoft's .NET initiative is explored in 
      detail in this deep programmer's reference.</description>
   </book>
   <book id=""bk111"">
      <author>O'Brien, Tim</author>
      <title>MSXML3: A Comprehensive Guide</title>
      <genre>Computer</genre>
      <price>36.95</price>
      <publish_date>2000-12-01</publish_date>
      <description>The Microsoft MSXML3 parser is covered in 
      detail, with attention to XML DOM interfaces, XSLT processing, 
      SAX and more.</description>
   </book>
   <book id=""bk112"">
      <author>Galos, Mike</author>
      <title>Visual Studio 7: A Comprehensive Guide</title>
      <genre>Computer</genre>
      <price>49.95</price>
      <publish_date>2001-04-16</publish_date>
      <description>Microsoft Visual Studio 7 is explored in depth,
      looking at how Visual Basic, Visual C++, C#, and ASP+ are 
      integrated into a comprehensive development 
      environment.</description>
   </book>
</catalog>";

    internal const string AssertionTemplateXML =
@"<?xml version=""1.0""?>
<catalog>
  <book id=""bk101"">
    <author>{prefix:catalog-book-author1}</author>
    <title>{prefix:catalog-book-title1}</title>
    <genre>{prefix:catalog-book-genre1}</genre>
    <price>{prefix:catalog-book-price1}</price>
    <publish_date>{prefix:catalog-book-publish_date1}</publish_date>
    <description>{prefix:catalog-book-description1}</description>
  </book>
  <book id=""bk102"">
    <author>{prefix:catalog-book-author2}</author>
    <title>{prefix:catalog-book-title2}</title>
    <genre>{prefix:catalog-book-genre2}</genre>
    <price>{prefix:catalog-book-price2}</price>
    <publish_date>{prefix:catalog-book-publish_date2}</publish_date>
    <description>{prefix:catalog-book-description2}</description>
  </book>
  <book id=""bk103"">
    <author>{prefix:catalog-book-author3}</author>
    <title>{prefix:catalog-book-title3}</title>
    <genre>{prefix:catalog-book-genre3}</genre>
    <price>{prefix:catalog-book-price3}</price>
    <publish_date>{prefix:catalog-book-publish_date3}</publish_date>
    <description>{prefix:catalog-book-description3}</description>
  </book>
  <book id=""bk104"">
    <author>{prefix:catalog-book-author4}</author>
    <title>{prefix:catalog-book-title4}</title>
    <genre>{prefix:catalog-book-genre4}</genre>
    <price>{prefix:catalog-book-price4}</price>
    <publish_date>{prefix:catalog-book-publish_date4}</publish_date>
    <description>{prefix:catalog-book-description4}</description>
  </book>
  <book id=""bk105"">
    <author>{prefix:catalog-book-author5}</author>
    <title>{prefix:catalog-book-title5}</title>
    <genre>{prefix:catalog-book-genre5}</genre>
    <price>{prefix:catalog-book-price5}</price>
    <publish_date>{prefix:catalog-book-publish_date5}</publish_date>
    <description>{prefix:catalog-book-description5}</description>
  </book>
  <book id=""bk106"">
    <author>{prefix:catalog-book-author6}</author>
    <title>{prefix:catalog-book-title6}</title>
    <genre>{prefix:catalog-book-genre6}</genre>
    <price>{prefix:catalog-book-price6}</price>
    <publish_date>{prefix:catalog-book-publish_date6}</publish_date>
    <description>{prefix:catalog-book-description6}</description>
  </book>
  <book id=""bk107"">
    <author>{prefix:catalog-book-author7}</author>
    <title>{prefix:catalog-book-title7}</title>
    <genre>{prefix:catalog-book-genre7}</genre>
    <price>{prefix:catalog-book-price7}</price>
    <publish_date>{prefix:catalog-book-publish_date7}</publish_date>
    <description>{prefix:catalog-book-description7}</description>
  </book>
  <book id=""bk108"">
    <author>{prefix:catalog-book-author8}</author>
    <title>{prefix:catalog-book-title8}</title>
    <genre>{prefix:catalog-book-genre8}</genre>
    <price>{prefix:catalog-book-price8}</price>
    <publish_date>{prefix:catalog-book-publish_date8}</publish_date>
    <description>{prefix:catalog-book-description8}</description>
  </book>
  <book id=""bk109"">
    <author>{prefix:catalog-book-author9}</author>
    <title>{prefix:catalog-book-title9}</title>
    <genre>{prefix:catalog-book-genre9}</genre>
    <price>{prefix:catalog-book-price9}</price>
    <publish_date>{prefix:catalog-book-publish_date9}</publish_date>
    <description>{prefix:catalog-book-description9}</description>
  </book>
  <book id=""bk110"">
    <author>{prefix:catalog-book-author10}</author>
    <title>{prefix:catalog-book-title10}</title>
    <genre>{prefix:catalog-book-genre10}</genre>
    <price>{prefix:catalog-book-price10}</price>
    <publish_date>{prefix:catalog-book-publish_date10}</publish_date>
    <description>{prefix:catalog-book-description10}</description>
  </book>
  <book id=""bk111"">
    <author>{prefix:catalog-book-author11}</author>
    <title>{prefix:catalog-book-title11}</title>
    <genre>{prefix:catalog-book-genre11}</genre>
    <price>{prefix:catalog-book-price11}</price>
    <publish_date>{prefix:catalog-book-publish_date11}</publish_date>
    <description>{prefix:catalog-book-description11}</description>
  </book>
  <book id=""bk112"">
    <author>{prefix:catalog-book-author12}</author>
    <title>{prefix:catalog-book-title12}</title>
    <genre>{prefix:catalog-book-genre12}</genre>
    <price>{prefix:catalog-book-price12}</price>
    <publish_date>{prefix:catalog-book-publish_date12}</publish_date>
    <description>{prefix:catalog-book-description12}</description>
  </book>
</catalog>";

}


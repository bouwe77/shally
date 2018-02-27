using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Shally;
using Shally.Hal;
using Shally.Json;

namespace SallyTest
{
   [TestClass]
   public class ResourceTest
   {
      [TestMethod]
      public void Constructor_Success()
      {
         var resource = Factory.GetValidResource();

         Assert.AreEqual("href", resource.Json["_links"]["self"]["href"]);
      }

      [TestMethod]
      [ExpectedException(typeof(ArgumentNullException))]
      public void Constructor_ThrowsArgumentNullException_WhenSelfLinkIsNull()
      {
         var resource = new Resource(null);
      }

      [TestMethod]
      [ExpectedException(typeof(ArgumentNullException))]
      public void AddLinkObject_ThrowsArgumentNullException_WhenPropertyNameIsNull()
      {
         var resource = Factory.GetValidResource();
         resource.AddLink(null, Factory.GetValidLink());
      }

      [TestMethod]
      public void AddLinkObject_Success_WhenObjectIsNull()
      {
         var resource = Factory.GetValidResource();
         resource.AddLink("link", (Link)null);
      }

      [TestMethod]
      public void AddLinkObject_Success()
      {
         var link = Factory.GetValidLink();
         link.Name = "name";
         link.Templated = true;
         link.Title = "title";

         var resource = Factory.GetValidResource();
         resource.AddLink("link", link);

         Assert.AreEqual("name", resource.Json["_links"]["link"]["name"]);
         Assert.AreEqual(true, resource.Json["_links"]["link"]["templated"]);
         Assert.AreEqual("title", resource.Json["_links"]["link"]["title"]);
      }

      [TestMethod]
      [ExpectedException(typeof(ArgumentNullException))]
      public void AddLinkCollection_ThrowsArgumentNullException_WhenPropertyNameIsNull()
      {
         var resource = Factory.GetValidResource();
         resource.AddLink(null, new[] { Factory.GetValidLink() });
      }

      [TestMethod]
      public void AddLinkCollection_Success_WhenCollectionIsNull()
      {
         var resource = Factory.GetValidResource();
         resource.AddLink("links", (IEnumerable<Link>)null);
      }

      [TestMethod]
      public void AddLinkCollection_Success()
      {
         var link = Factory.GetValidLink();
         link.Name = "name";
         link.Templated = true;
         link.Title = "title";

         var resource = Factory.GetValidResource();
         resource.AddLink("link", new[] { link });

         Assert.AreEqual("name", resource.Json["_links"]["link"][0]["name"]);
         Assert.AreEqual(true, resource.Json["_links"]["link"][0]["templated"]);
         Assert.AreEqual("title", resource.Json["_links"]["link"][0]["title"]);
      }

      [TestMethod]
      [ExpectedException(typeof(ArgumentNullException))]
      public void AddStringProperty_ThrowsArgumentNullException_WhenPropertyNameIsNull()
      {
         var resource = Factory.GetValidResource();
         resource.AddProperty(null, "string");
      }

      [TestMethod]
      public void AddStringProperty_Success_WhenPropertyNameIsEmpty()
      {
         var resource = Factory.GetValidResource();
         resource.AddProperty(string.Empty, "string");
      }

      [TestMethod]
      public void AddStringProperty_Success_WhenStringValueIsNull()
      {
         var resource = Factory.GetValidResource();
         resource.AddProperty("mystring", (string)null);
      }

      [TestMethod]
      public void AddStringProperty_Success()
      {
         var resource = Factory.GetValidResource();
         resource.AddProperty("mystring", "string");

         Assert.AreEqual(resource.Json["mystring"], "string");
      }

      [TestMethod]
      public void AddStringPropertyCollection_Success()
      {
         var resource = Factory.GetValidResource();
         resource.AddProperty("mystring", new[] { "string" });

         Assert.AreEqual(resource.Json["mystring"][0], "string");
      }

      [TestMethod]
      public void AddIntProperty_Success()
      {
         var resource = Factory.GetValidResource();
         resource.AddProperty("int", 1);

         Assert.AreEqual(resource.Json["int"], 1);
      }

      [TestMethod]
      public void AddBooleanProperty_Success()
      {
         var resource = Factory.GetValidResource();
         resource.AddProperty("boolean", true);

         Assert.AreEqual(resource.Json["boolean"], true);
      }

      [TestMethod]
      public void AddPocoObjectProperty_Success()
      {
         var dummy = new Dummy
         {
            MyString = "my string",
            MyNumber = 1,
            MyBoolean = true,
            MyNumbers = new[] { 1, 2, 3 }
         };

         var resource = Factory.GetValidResource();
         resource.AddProperty("dummy", dummy);

         Assert.AreEqual("my string", resource.Json["dummy"]["mystring"]);
         Assert.AreEqual(1, resource.Json["dummy"]["mynumber"]);
         Assert.AreEqual(true, resource.Json["dummy"]["myboolean"]);
         Assert.AreEqual(3, resource.Json["dummy"]["mynumbers"].Count());
         Assert.AreEqual(1, resource.Json["dummy"]["mynumbers"][0]);
         Assert.AreEqual(2, resource.Json["dummy"]["mynumbers"][1]);
         Assert.AreEqual(3, resource.Json["dummy"]["mynumbers"][2]);
      }

      [TestMethod]
      public void AddResourceObject_Success()
      {
         var embeddedResource = Factory.GetValidResource();
         embeddedResource.AddProperty("string", "string");

         var resource = Factory.GetValidResource();
         resource.AddResource("resource", embeddedResource);

         Assert.AreEqual("string", resource.Json["_embedded"]["resource"]["string"]);
      }

      [TestMethod]
      public void AddResourceObject_Success_WhenObjectIsNull()
      {
         var resource = Factory.GetValidResource();
         resource.AddResource("resource", (Resource)null);
      }

      [TestMethod]
      public void AddResourceCollection_Success_WhenCollectionIsNull()
      {
         var resource = Factory.GetValidResource();
         resource.AddResource("resource", (IEnumerable<Resource>)null);
      }

      [TestMethod]
      public void AddResourceCollection_Success()
      {
         var embeddedResource = Factory.GetValidResource();
         embeddedResource.AddProperty("string", "string");

         var resource = Factory.GetValidResource();
         resource.AddResource("resources", new[] { embeddedResource });

         Assert.AreEqual("string", resource.Json["_embedded"]["resources"][0]["string"]);
      }

      /// <summary>
      /// Test the example HAL document from the specification, see http://stateless.co/hal_specification.html.
      /// </summary>
      [TestMethod]
      public void TestHalDocumentFromTheSpecification()
      {
         var halDocument = new Resource(new Link("/orders"));

         halDocument.AddLink("curies", new[]
         {
            new Link("http://example.com/docs/rels/{rel}")
            {
               Name = "ea",
               Templated = true
            }
         });

         halDocument.AddLink("next", new Link("/orders?page=2"));

         halDocument.AddLink("ea:find", new Link("/orders{?id}")
         {
            Templated = true
         });

         halDocument.AddLink("ea:admin", new[]
         {
            new Link("/admins/2")
            {
               Title = "Fred"
            },
            new Link("/admins/5")
            {
               Title = "Kate"
            }
         });

         halDocument.AddProperty("currentlyProcessing", 14);
         halDocument.AddProperty("shippedToday", 20);

         var order1 = new Resource(new Link("/orders/123"));
         order1.AddLink("ea:basket", new Link("/baskets/98712"));
         order1.AddLink("ea:customer", new Link("/customers/7809"));
         order1.AddProperty("total", 30);
         order1.AddProperty("currency", "USD");
         order1.AddProperty("status", "shipped");

         var order2 = new Resource(new Link("/orders/124"));
         order2.AddLink("ea:basket", new Link("/baskets/97213"));
         order2.AddLink("ea:customer", new Link("/customers/12369"));
         order2.AddProperty("total", 20);
         order2.AddProperty("currency", "USD");
         order2.AddProperty("status", "processing");

         halDocument.AddResource("ea:order", new[] {
            order1,
            order2
         });

         var settings = new JsonSerializerSettings
         {
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.Indented,
            Converters = new List<JsonConverter>
            {
               new DecimalJsonConverter()
            }
         };

         File.WriteAllText(@"D:\Temp\hal.json", halDocument.Json.ToString());

         // The document contains 4 properties: _links, currentlyProcessing, shippedToday and _embedded.
         Assert.AreEqual(4, halDocument.Json.Count);

         // The _links object contains 5 links.
         Assert.AreEqual(5, halDocument.Json["_links"].Count());

         // The self link only contains a href property.
         Assert.AreEqual(1, halDocument.Json["_links"]["self"].Count());
         Assert.AreEqual("/orders", halDocument.Json["_links"]["self"]["href"]);

         // The curies link array contains one link with 3 properties, name, href and templated.
         Assert.AreEqual(1, halDocument.Json["_links"]["curies"].Count());
         Assert.AreEqual(3, halDocument.Json["_links"]["curies"][0].Count());
         Assert.AreEqual("ea", halDocument.Json["_links"]["curies"][0]["name"]);
         Assert.AreEqual("http://example.com/docs/rels/{rel}", halDocument.Json["_links"]["curies"][0]["href"]);
         Assert.AreEqual(true, halDocument.Json["_links"]["curies"][0]["templated"]);

         // The next link only contains a href property.
         Assert.AreEqual(1, halDocument.Json["_links"]["next"].Count());
         Assert.AreEqual("/orders?page=2", halDocument.Json["_links"]["next"]["href"]);

         // The ea:find link contains an href and templated property.
         Assert.AreEqual(2, halDocument.Json["_links"]["ea:find"].Count());
         Assert.AreEqual("/orders{?id}", halDocument.Json["_links"]["ea:find"]["href"]);

         // The ea:admin link array contains two links, with 2 properties each: href and title.
         Assert.AreEqual(2, halDocument.Json["_links"]["ea:admin"].Count());
         Assert.AreEqual(2, halDocument.Json["_links"]["ea:admin"][0].Count());
         Assert.AreEqual("/admins/2", halDocument.Json["_links"]["ea:admin"][0]["href"]);
         Assert.AreEqual("Fred", halDocument.Json["_links"]["ea:admin"][0]["title"]);
         Assert.AreEqual(2, halDocument.Json["_links"]["ea:admin"][1].Count());
         Assert.AreEqual("/admins/5", halDocument.Json["_links"]["ea:admin"][1]["href"]);
         Assert.AreEqual("Kate", halDocument.Json["_links"]["ea:admin"][1]["title"]);

         // This resource contains two properties: currentlyProcessing and shippedToday.
         Assert.AreEqual(14, halDocument.Json["currentlyProcessing"]);
         Assert.AreEqual(20, halDocument.Json["shippedToday"]);

         // The _embedded object contains one property: ea:order, which is an array of two orders with 4 properties each.
         Assert.AreEqual(1, halDocument.Json["_embedded"].Count());
         Assert.AreEqual(2, halDocument.Json["_embedded"]["ea:order"].Count());
         Assert.AreEqual(4, halDocument.Json["_embedded"]["ea:order"][0].Count());
         Assert.AreEqual(3, halDocument.Json["_embedded"]["ea:order"][0]["_links"].Count());
         Assert.AreEqual("/orders/123", halDocument.Json["_embedded"]["ea:order"][0]["_links"]["self"]["href"]);
         Assert.AreEqual("/baskets/98712", halDocument.Json["_embedded"]["ea:order"][0]["_links"]["ea:basket"]["href"]);
         Assert.AreEqual("/customers/7809", halDocument.Json["_embedded"]["ea:order"][0]["_links"]["ea:customer"]["href"]);
         Assert.AreEqual(30, halDocument.Json["_embedded"]["ea:order"][0]["total"]);
         Assert.AreEqual("USD", halDocument.Json["_embedded"]["ea:order"][0]["currency"]);
         Assert.AreEqual("shipped", halDocument.Json["_embedded"]["ea:order"][0]["status"]);
         Assert.AreEqual(4, halDocument.Json["_embedded"]["ea:order"][1].Count());
         Assert.AreEqual(3, halDocument.Json["_embedded"]["ea:order"][1]["_links"].Count());
         Assert.AreEqual("/orders/124", halDocument.Json["_embedded"]["ea:order"][1]["_links"]["self"]["href"]);
         Assert.AreEqual("/baskets/97213", halDocument.Json["_embedded"]["ea:order"][1]["_links"]["ea:basket"]["href"]);
         Assert.AreEqual("/customers/12369", halDocument.Json["_embedded"]["ea:order"][1]["_links"]["ea:customer"]["href"]);
         Assert.AreEqual(20, halDocument.Json["_embedded"]["ea:order"][1]["total"]);
         Assert.AreEqual("USD", halDocument.Json["_embedded"]["ea:order"][1]["currency"]);
         Assert.AreEqual("processing", halDocument.Json["_embedded"]["ea:order"][1]["status"]);
      }
   }
}

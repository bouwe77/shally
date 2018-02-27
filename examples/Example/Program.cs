using System;
using Shally.Hal;

namespace Example
{
   class Program
   {
      static void Main(string[] args)
      {
         // Create a HAL document that describes a fictious person John doe.
         // According to the spec, a HAL document SHOULD have a self link.
         var selfLink = new Link("/people/john-doe");
         var halDocument = new Resource(selfLink);

         // John Doe is 32 years old:
         halDocument.AddProperty("name", "John Doe");
         halDocument.AddProperty("age", 32);

         // He has a collection of friends:
         var friend1 = new Resource(new Link("/people/pete-smith") { Title = "Pete Smith" });
         friend1.AddProperty("name", "Pete Smith");

         var friend2 = new Resource(new Link("/people/mary-jones") { Title = "Mary Jones" });
         friend2.AddProperty("name", "Mary Jones");

         halDocument.AddResource("friends", new[] { friend1, friend2 });

         // And he has an employer:
         var company = new Resource(new Link("/companies/acme"));
         company.AddLink("websites", new[] { new Link("http://acme.com"), new Link("http://acme-corporation.com") });
         company.AddProperty("name", "Acme Corporation");
         halDocument.AddResource("employer", company);

         Console.WriteLine(halDocument.Json);
      }
   }
}

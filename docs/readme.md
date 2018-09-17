# Shally
A .NET library for creating hal+json (Hypertext Application Language) documents.

# hal+json specification
The hal+json spec can be found here: http://tools.ietf.org/html/draft-kelly-json-hal.

# Example

Let's create a HAL document for a guy named John Doe:

```C#
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
```

The code above results in the following HAL document:

```JSON
{
  "_links": {
    "self": {
      "href": "/people/john-doe"
    }
  },
  "name": "John Doe",
  "age": 32,
  "_embedded": {
    "friends": [
      {
        "_links": {
          "self": {
            "href": "/people/pete-smith",
            "title": "Pete Smith"
          }
        },
        "name": "Pete Smith"
      },
      {
        "_links": {
          "self": {
            "href": "/people/mary-jones",
            "title": "Mary Jones"
          }
        },
        "name": "Mary Jones"
      }
    ],
    "employer": {
      "_links": {
        "self": {
          "href": "/companies/acme"
        },
        "websites": [
          {
            "href": "http://acme.com"
          },
          {
            "href": "http://acme-corporation.com"
          }
        ]
      },
      "name": "Acme Corporation"
    }
  }
}
```

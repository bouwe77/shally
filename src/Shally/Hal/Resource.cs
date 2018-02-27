using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shally.Helpers;
using Shally.Json;

namespace Shally.Hal
{
   /// <summary>
   /// Represents a HAL Resource.
   /// </summary>
   public class Resource
   {
      private const string Self = "self";
      private const string Links = "_links";
      private const string Embedded = "_embedded";

      private JsonSerializer JsonSerializer = new JsonSerializer()
      {
         ContractResolver = new LowercaseContractResolver(),
         NullValueHandling = NullValueHandling.Ignore,
         Formatting = Formatting.Indented,
      };

      public JObject Json { get; protected set; }

      public Resource(Link selfLink)
      {
         Enforce.ArgumentNotNull(selfLink, "Resource.SelfLink is required");
         Json = new JObject();
         AddLinkInternal(Self, selfLink);
      }

      public void AddLink(string propertyName, Link link)
      {
         AddLinkInternal(propertyName, link);
      }

      public void AddLink(string propertyName, IEnumerable<Link> links)
      {
         AddLinkInternal(propertyName, links);
      }

      private void AddLinkInternal(string propertyName, object links)
      {
         Enforce.ArgumentNotNull(propertyName, "PropertyName can not be null");

         AddObjectIfNecessary(Links);

         JToken jsonLinks = null;
         if (links != null)
         {
            jsonLinks = JToken.FromObject(links, JsonSerializer);
         }

         Json[Links][propertyName] = jsonLinks;
      }

      public void AddProperty(string propertyName, object value)
      {
         Enforce.ArgumentNotNull(propertyName, "PropertyName can not be null");

         JToken jsonValue = null;
         if (value != null)
         {
            jsonValue = JToken.FromObject(value, JsonSerializer);
         }

         Json[propertyName] = jsonValue;
      }

      public void AddResource(string propertyName, Resource resource)
      {
         Enforce.ArgumentNotNull(propertyName, "PropertyName can not be null");

         AddObjectIfNecessary(Embedded);

         JObject jsonObject = null;
         if (resource != null)
         {
            jsonObject = JObject.FromObject(resource.Json, JsonSerializer);
         }

         Json[Embedded][propertyName] = jsonObject;
      }

      public void AddResource(string propertyName, IEnumerable<Resource> resources)
      {
         Enforce.ArgumentNotNull(propertyName, "PropertyName can not be null");

         AddObjectIfNecessary(Embedded);

         JArray jsonObjects = null;
         if (resources != null)
         {
            var resourcesAsJson = resources.Select(resource => resource.Json).ToList();
            jsonObjects = JArray.FromObject(resourcesAsJson, JsonSerializer);
         }

         Json[Embedded][propertyName] = jsonObjects;
      }

      private void AddObjectIfNecessary(string name)
      {
         Enforce.ArgumentNotNull(name, "Name can not be null");

         JToken links = Json[name];
         if (links == null)
         {
            Json.Add(name, new JObject());
         }
      }
   }
}
using Newtonsoft.Json;
using Shally.Helpers;

namespace Shally.Hal
{
   public class Link
   {
      public Link(string href)
      {
         Href = Enforce.ArgumentNotNull(href, "The Link.Href property is required");
      }

      [JsonProperty(Order = 1)]
      public string Name { get; set; }

      [JsonProperty(Order = 2)]
      public string Href { get; }

      [JsonProperty(Order = 3)]
      public string Title { get; set; }

      [JsonProperty(Order = 4, DefaultValueHandling = DefaultValueHandling.Ignore)]
      public bool Templated { get; set; }
   }
}
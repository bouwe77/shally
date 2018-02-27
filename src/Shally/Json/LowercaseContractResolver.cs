using Newtonsoft.Json.Serialization;

namespace Shally.Json
{
   public class LowercaseContractResolver : DefaultContractResolver
   {
      protected override string ResolvePropertyName(string propertyName)
      {
         return propertyName.ToLower();
      }
   }
}

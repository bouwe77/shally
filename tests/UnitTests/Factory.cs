using Shally.Hal;

namespace Shally
{
   public class Factory
   {
      public static Link GetValidLink()
      {
         return new Link("href");
      }

      public static Resource GetValidResource()
      {
         var validLink = GetValidLink();
         return new Resource(validLink);
      }
   }
}

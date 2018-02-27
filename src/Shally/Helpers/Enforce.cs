using System;

namespace Shally.Helpers
{
   internal static class Enforce
   {
      public static T ArgumentNotNull<T>(T argument, string description)
          where T : class
      {
         if (argument == null)
         {
            throw new ArgumentNullException(description);
         }

         return argument;
      }

      public static string StringNotEmpty(string argument, string description)
      {
         if (string.IsNullOrWhiteSpace(argument))
         {
            throw new ArgumentException(description);
         }

         return argument;
      }
   }
}

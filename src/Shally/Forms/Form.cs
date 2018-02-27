using System.Collections.Generic;
using Shally.Helpers;

namespace Shally.Forms
{
   public class Form
   {
      public Form(string name)
      {
         Name = Enforce.StringNotEmpty(name, nameof(name));
         Fields = new List<Field>();
      }

      public string Name { get; private set; }
      public string Title { get; set; }
      public string Method { get; set; }
      public string Action { get; set; }
      public string ContentType { get; set; }
      public IEnumerable<Field> Fields { get; private set; }
   }
}

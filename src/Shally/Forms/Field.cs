using Shally.Helpers;

namespace Shally.Forms
{
   public class Field
   {
      public Field(string name)
      {
         Name = Enforce.StringNotEmpty(name, nameof(name));
      }

      public string Name { get; private set; }
      public string Label { get; set; }
      public string Hint { get; set; }
      public string Type { get; set; }
      public bool ReadOnly { get; set; }
      public string Regex { get; set; }
      public string DefaultValue { get; set; }
      public bool Required { get; set; }
   }
}

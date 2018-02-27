using System.Linq;
using Newtonsoft.Json.Linq;
using Shally.Hal;

namespace Shally.Forms
{
   public static class ResourceExtensions
   {
      public static void AddForm(this Resource resource, Form form)
      {
         var jsonForm = new JObject();
         jsonForm["title"] = form.Title;
         jsonForm["method"] = form.Method;
         jsonForm["action"] = form.Action;
         jsonForm["contentType"] = form.ContentType;

         if (form.Fields.Any())
         {
            jsonForm["fields"] = new JObject();

            foreach (var field in form.Fields)
            {
               var jsonField = new JObject();
               jsonField["name"] = field.Name;
               jsonField["type"] = field.Type;
               jsonField["label"] = field.Label;
               jsonField["hint"] = field.Hint;
               jsonField["readonly"] = field.ReadOnly;
               jsonField["defaultValue"] = field.DefaultValue;
               jsonField["regex"] = field.Regex;
               jsonField["required"] = field.Required;

               jsonForm["fields"][field.Name] = jsonField;
            }
         }

         if (resource.Json["_forms"] == null)
         {
            resource.Json["_forms"] = new JObject();
         }

         resource.Json["_forms"][form.Name] = jsonForm;
      }
   }
}

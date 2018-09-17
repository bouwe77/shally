using System.Linq;
using Newtonsoft.Json.Linq;
using Shally.Hal;

namespace Shally.Forms
{
    public static class ResourceExtensions
    {
        public static void AddForm(this Resource resource, Form form)
        {
            var jsonForm = new JObject
            {
                ["title"] = form.Title,
                ["method"] = form.Method,
                ["action"] = form.Action,
                ["contentType"] = form.ContentType,
                ["enabled"] = form.IsEnabled
            };

            if (form.Fields.Any())
            {
                jsonForm["fields"] = new JObject();

                foreach (var field in form.Fields)
                {
                    var jsonField = new JObject
                    {
                        ["name"] = field.Name,
                        ["type"] = field.Type,
                        ["label"] = field.Label,
                        ["hint"] = field.Hint,
                        ["readonly"] = field.ReadOnly,
                        ["defaultValue"] = field.DefaultValue,
                        ["regex"] = field.Regex,
                        ["required"] = field.Required
                    };

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

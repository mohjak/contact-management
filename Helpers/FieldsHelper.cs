using Mohjak.ContactManagement.DTOs;
using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Mohjak.ContactManagement.Helpers
{
    public static class FieldsHelper
    {
        public static ExpandoObject PopulateFields(IList<CustomField> fields)
        {
            var fieldObject = new ExpandoObject();

            foreach (var field in fields)
            {
                if (field.Type == "Date")
                {
                    if (DateTime.TryParse(field.Value, out DateTime dateResult))
                    {
                        ((IDictionary<string, object>)fieldObject)[field.Name] = dateResult;
                    }
                }

                if (field.Type == "Text")
                {
                    ((IDictionary<string, object>)fieldObject)[field.Name] = field.Value;
                }

                if (field.Type == "Number")
                {
                    if (int.TryParse(field.Value, out int numberResult))
                    {
                        ((IDictionary<string, object>)fieldObject)[field.Name] = numberResult;
                    }
                }
            }

            return fieldObject;
        }
    }
}

using Mohjak.ContactManagement.DTOs;
using System;
using System.Collections.Generic;

namespace Mohjak.ContactManagement.Helpers
{
    public static class FieldsHelper
    {
        public static IDictionary<string, object> PopulateFields(IList<FieldDTO> fields)
        {
            var dic = new Dictionary<string, object>();

            foreach (var field in fields)
            {
                if (field.DataType == "Date")
                {
                    if (DateTime.TryParse(field.Value, out DateTime dateResult))
                    {
                        dic[field.Name] = dateResult;
                    }
                }

                if (field.DataType == "Text")
                {
                    dic[field.Name] = field.Value;
                }

                if (field.DataType == "Number")
                {
                    if (int.TryParse(field.Value, out int numberResult))
                    {
                        dic[field.Name] = numberResult;
                    }
                }
            }

            return dic;
        }
    }
}

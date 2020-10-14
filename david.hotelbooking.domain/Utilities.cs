using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace david.hotelbooking.domain
{
    public static class Utilities
    {
        public static string PrettyJson(string unPrettyJson)
        {
            var options = new JsonSerializerOptions()
            {
                WriteIndented = true
            };

            var jsonElement = JsonSerializer.Deserialize<JsonElement>(unPrettyJson);

            return JsonSerializer.Serialize(jsonElement, options);
        }

    }

}

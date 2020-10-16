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

        /// <summary>
        /// Determines if a <code>DateTime</code> falls before another <code>DateTime</code> (inclusive)
        /// </summary>
        /// <param name="dt">The <code>DateTime</code> being tested</param>
        /// <param name="compare">The <code>DateTime</code> used for the comparison</param>
        /// <returns><code>bool</code></returns>
        public static bool IsBefore(this DateTime dt, DateTime compare)
        {
            return dt.Ticks <= compare.Ticks;
        }

        /// <summary>
        /// Determines if a <code>DateTime</code> falls after another <code>DateTime</code> (inclusive)
        /// </summary>
        /// <param name="dt">The <code>DateTime</code> being tested</param>
        /// <param name="compare">The <code>DateTime</code> used for the comparison</param>
        /// <returns><code>bool</code></returns>
        public static bool IsAfter(this DateTime dt, DateTime compare)
        {
            return dt.Ticks >= compare.Ticks;
        }

    }

}

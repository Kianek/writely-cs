using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace writely.integration_tests
{
    public static class HttpExtensions
    {
        public static StringContent AsStringContent<TValue>(this TValue value)
        {
            if (value == null)
            {
                throw new ArgumentNullException();
            }

            var options = new JsonSerializerOptions();
            var content = JsonSerializer.Serialize(value, options);
            return new StringContent(content, Encoding.UTF8, "application/json");
        }
    }
}
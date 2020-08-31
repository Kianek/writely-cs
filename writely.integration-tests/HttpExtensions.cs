using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace writely.integration_tests
{
    public static class HttpExtensions
    {
        public static StringContent AsStringContent(this object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException();
            }
            
            return new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");
        }
    }
}
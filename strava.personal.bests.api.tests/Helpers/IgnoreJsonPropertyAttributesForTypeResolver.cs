using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace strava.personal.bests.api.tests.Helpers
{
    /// <summary>
    /// Ignore the Json Property attribute. This is useful when you want to serialize or deserialize differently and not 
    /// let the JsonProperty control everything.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class IgnoreJsonPropertyAttributesForTypeResolver<T> : DefaultContractResolver
    {
        private Dictionary<string, string> PropertyMappings { get; set; }

        public IgnoreJsonPropertyAttributesForTypeResolver()
        {
            this.PropertyMappings = new Dictionary<string, string>();

            foreach (var propertyInfo in GetPropertiesForType(typeof(T)))
            {
                var jsonProperty = propertyInfo.GetCustomAttribute<JsonPropertyAttribute>();
                if (jsonProperty != null)
                {
                    PropertyMappings.Add(jsonProperty.PropertyName, propertyInfo.Name);
                }
            }
        }

        protected override string ResolvePropertyName(string propertyName)
        {
            var resolved = this.PropertyMappings.TryGetValue(propertyName, out var resolvedName);
            return (resolved) ? resolvedName : base.ResolvePropertyName(propertyName);
        }

        private static IEnumerable<PropertyInfo> GetPropertiesForType(Type objectType)
        {
            return objectType.GetProperties();
        }
    }
}


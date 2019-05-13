using System;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace StreamlabsEventReceiver
{
    class RealPropertyNameContractResolver : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            // Let the base class create all the JsonProperties 
            IList<JsonProperty> list = base.CreateProperties(type, memberSerialization);

            // Replace each JsonProperty name with real property name 
            foreach (JsonProperty prop in list)
            {
                prop.PropertyName = prop.UnderlyingName;
            }

            // Return list with re-named properties
            return list;
        }
    }
}

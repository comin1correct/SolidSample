using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ArdalisRating
{
    class PolicySerializer
    {
        public Policy GetPolicyFromJsonString(string policyJson)
        {
            return JsonConvert.DeserializeObject<Policy>(policyJson,          //<--- Hard coded Format json... yaml, xml, binary format this will have to change
                new StringEnumConverter());
        }
    }
}

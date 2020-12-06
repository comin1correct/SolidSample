using System.IO;

namespace ArdalisRating
{
    class FilePolicySource : IFilePolicySource
    {
        const string SOURCE = "policy.json";

        public string GetPolicyFromSource()
        {
            return File.ReadAllText(SOURCE);
        }
    }
}

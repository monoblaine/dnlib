//ş
using System;
using Newtonsoft.Json;

namespace PublicKeyHackingTool.Config {
    internal class ReferenceAssemblyInfo {
        [JsonProperty("old")]
        public String Old { get; set; }

        [JsonProperty("new")]
        public String New { get; set; }
    }
}

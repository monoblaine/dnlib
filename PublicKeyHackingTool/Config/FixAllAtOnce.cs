//ş
using System;
using Newtonsoft.Json;

namespace PublicKeyHackingTool.Config {
    internal class FixAllAtOnce : IGeneralConfig {
        [JsonProperty("inputModules")]
        public InputModule[] InputModules { get; set; }

        [JsonProperty("outputPath")]
        public String OutputPath { get; set; }

        [JsonProperty("snkPath")]
        public String SnkPath { get; set; }

        [JsonProperty("attrFilterTypeFullNames")]
        public String[] AttrFilterTypeFullNames { get; set; }

        [JsonProperty("referenceAssemblyInfo")]
        public ReferenceAssemblyInfo ReferenceAssemblyInfo { get; set; }
    }
}

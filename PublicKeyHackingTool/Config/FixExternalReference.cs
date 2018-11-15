//ş
using System;
using Newtonsoft.Json;

namespace PublicKeyHackingTool.Config {
    internal class FixExternalReference : IGeneralConfig {
        [JsonProperty("inputModules")]
        public String[] InputModules { get; set; }

        [JsonProperty("outputPath")]
        public String OutputPath { get; set; }

        [JsonProperty("referenceAssemblyInfo")]
        public ReferenceAssemblyInfo ReferenceAssemblyInfo { get; set; }

        [JsonProperty("snkPath")]
        public String SnkPath { get; set; }
    }
}

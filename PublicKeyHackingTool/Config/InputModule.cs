//ş
using System;
using Newtonsoft.Json;

namespace PublicKeyHackingTool.Config {
    internal class InputModule {
        [JsonProperty("path")]
        public String Path { get; set; }

        [JsonProperty("newAsmRef")]
        public String NewAsmRef { get; set; }
    }
}

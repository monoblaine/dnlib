//ş
using System;
using dnlib.DotNet;

namespace PublicKeyHackingTool.Extensions {
    internal static class XString {
        public static AssemblyRef AsAssemblyRef (this String assemblyFullName) {
            return new AssemblyNameInfo(assemblyFullName).ToAssemblyRef();
        }
    }
}

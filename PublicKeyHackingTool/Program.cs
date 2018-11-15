//ş
using System;
using System.IO;
using System.Linq;
using dnlib.DotNet;
using dnlib.DotNet.Writer;
using Newtonsoft.Json;
using PublicKeyHackingTool.Config;
using PublicKeyHackingTool.Extensions;

namespace PublicKeyHackingTool {
    internal class Program {
        private static void Main (String[] args) {
            var operationType = args[0];
            var configFile = args[1];

            switch (operationType) {
                case "fix-external-ref":
                    _fixExternalReference(JsonConvert.DeserializeObject<FixExternalReference>(File.ReadAllText(configFile)));
                    break;

                case "fix-self-ref":
                    _fixSelfReferences(JsonConvert.DeserializeObject<FixSelfReferences>(File.ReadAllText(configFile)));
                    break;

                case "fix-all":
                    _fixAllAtOnce(JsonConvert.DeserializeObject<FixAllAtOnce>(File.ReadAllText(configFile)));
                    break;
            }
        }

        private static void _fixExternalReference (FixExternalReference config) {
            var oldAsmRef = config.ReferenceAssemblyInfo.Old.AsAssemblyRef();
            var newAsmRef = config.ReferenceAssemblyInfo.New.AsAssemblyRef();

            foreach (var pathToModule in config.InputModules) {
                var module = ModuleDefMD.Load(pathToModule);

                _fixExternalReferenceImpl(module, oldAsmRef, newAsmRef);
                _saveModule(module, config);
            }
        }

        private static void _fixExternalReferenceImpl (ModuleDefMD module, AssemblyRef oldAsmRef, AssemblyRef newAsmRef) {
            foreach (var asmRef in module.GetAssemblyRefs()) {
                if (_isEqual(asmRef, oldAsmRef)) {
                    asmRef.PublicKeyOrToken = newAsmRef.PublicKeyOrToken;
                    asmRef.Version = newAsmRef.Version;
                }
            }
        }

        private static void _fixSelfReferences (FixSelfReferences config) {
            Boolean attrFilter (CustomAttribute ca) =>
                config.AttrFilterTypeFullNames.Contains(ca.TypeFullName) &&
                ca.ConstructorArguments.Count > 0 &&
                ca.ConstructorArguments.Any(ctArg => ctArg.Value is TypeSig);

            foreach (var moduleInfo in config.InputModules) {
                var module = ModuleDefMD.Load(moduleInfo.Path);
                var oldAsmRef = module.Assembly.ToAssemblyRef();
                var newAsmRef = moduleInfo.NewAsmRef.AsAssemblyRef();

                _fixSelfReferencesImpl(module, oldAsmRef, newAsmRef, attrFilter);
                _saveModule(module, config);
            }
        }

        private static void _fixSelfReferencesImpl (ModuleDefMD module, AssemblyRef oldAsmRef, AssemblyRef newAsmRef, Func<CustomAttribute, Boolean> attrFilter) {
            var types = module
                .GetTypes()
                .OrderBy(r => r.FullName)
                .Where(r =>
                    r.CustomAttributes.Any(attrFilter) ||
                    r.Properties.Any(p => p.CustomAttributes.Any(attrFilter))
                )
                .ToList();

            foreach (var type in types) {
                var attributes = type
                    .CustomAttributes
                    .Where(attrFilter)
                    .Concat(
                        type.Properties.SelectMany(p => p.CustomAttributes.Where(attrFilter))
                    )
                    .ToList();

                foreach (var attr in attributes) {
                    var ctArgs = attr.ConstructorArguments.Where(ctArg => ctArg.Value is TypeSig).ToList();

                    foreach (var ctArg in ctArgs) {
                        var typeSig = ctArg.Value as TypeSig;

                        if (_isEqual(typeSig.DefinitionAssembly.ToAssemblyRef(), oldAsmRef)) {
                            if (typeSig.DefinitionAssembly is AssemblyDef assDef) {
                                assDef.PublicKey = new PublicKey(newAsmRef.PublicKeyOrToken.Data);
                                assDef.Version = newAsmRef.Version;
                            }
                            else if (typeSig.DefinitionAssembly is AssemblyRef assRef) {
                                assRef.PublicKeyOrToken = newAsmRef.PublicKeyOrToken;
                                assRef.Version = newAsmRef.Version;
                            }
                            else {
                                throw new NotImplementedException();
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Caution: Use this method with the *original* dll files.
        /// </summary>
        /// <param name="config"></param>
        private static void _fixAllAtOnce (FixAllAtOnce config) {
            var oldExternalAsmRef = config.ReferenceAssemblyInfo.Old.AsAssemblyRef();
            var newExternalAsmRef = config.ReferenceAssemblyInfo.New.AsAssemblyRef();

            Boolean attrFilter (CustomAttribute ca) =>
                config.AttrFilterTypeFullNames.Contains(ca.TypeFullName) &&
                ca.ConstructorArguments.Count > 0 &&
                ca.ConstructorArguments.Any(ctArg => ctArg.Value is TypeSig);

            foreach (var moduleInfo in config.InputModules) {
                var module = ModuleDefMD.Load(moduleInfo.Path);
                var oldAsmRef = module.Assembly.ToAssemblyRef();
                var newAsmRef = moduleInfo.NewAsmRef.AsAssemblyRef();

                _fixExternalReferenceImpl(module, oldExternalAsmRef, newExternalAsmRef);
                _fixSelfReferencesImpl(module, oldAsmRef, newAsmRef, attrFilter);
                _saveModule(module, config);
            }
        }

        private static Boolean _isEqual (AssemblyRef asmRef, AssemblyRef oldAsmRef) {
            return (
                asmRef.Name == oldAsmRef.Name &&
                asmRef.Version.Equals(oldAsmRef.Version) &&
                PublicKeyBase.TokenEquals(asmRef.PublicKeyOrToken, oldAsmRef.PublicKeyOrToken)
            );
        }

        private static void _saveModule (ModuleDefMD module, IGeneralConfig config) {
            ModuleWriterOptions moduleWriterOptions = null;

            if (!String.IsNullOrWhiteSpace(config.SnkPath)) {
                moduleWriterOptions = new ModuleWriterOptions(module);

                moduleWriterOptions.InitializeStrongNameSigning(module, new StrongNameKey(config.SnkPath));
            }

            module.Write(
                filename: Path.Combine(config.OutputPath, new FileInfo(module.Location).Name),
                options: moduleWriterOptions
            );
        }
    }
}

﻿using System;
using System.Collections.Generic;
using dot10.dotNET.MD;

namespace dot10.dotNET {
	/// <summary>
	/// Created from a row in the Module table
	/// </summary>
	public sealed class ModuleDefMD : ModuleDef, ICorLibTypes {
		/// <summary>The file that contains all .NET metadata</summary>
		DotNetFile dnFile;
		/// <summary>The raw table row. It's null until <see cref="InitializeRawRow"/> is called</summary>
		RawModuleRow rawRow;

		UserValue<ushort> generation;
		UserValue<UTF8String> name;
		UserValue<Guid?> mvid;
		UserValue<Guid?> encId;
		UserValue<Guid?> encBaseId;
		UserValue<AssemblyDef> assembly;

		SimpleLazyList<ModuleDefMD> listModuleDefMD;
		SimpleLazyList<TypeRefMD> listTypeRefMD;
		SimpleLazyList<TypeDefMD> listTypeDefMD;
		SimpleLazyList<FieldDefMD> listFieldDefMD;
		SimpleLazyList<MethodDefMD> listMethodDefMD;
		SimpleLazyList<ParamDefMD> listParamDefMD;
		SimpleLazyList<InterfaceImplMD> listInterfaceImplMD;
		SimpleLazyList<MemberRefMD> listMemberRefMD;
		SimpleLazyList<DeclSecurityMD> listDeclSecurityMD;
		SimpleLazyList<StandAloneSigMD> listStandAloneSigMD;
		SimpleLazyList<EventDefMD> listEventDefMD;
		SimpleLazyList<PropertyDefMD> listPropertyDefMD;
		SimpleLazyList<ModuleRefMD> listModuleRefMD;
		SimpleLazyList<TypeSpecMD> listTypeSpecMD;
		SimpleLazyList<AssemblyDefMD> listAssemblyDefMD;
		SimpleLazyList<AssemblyRefMD> listAssemblyRefMD;
		SimpleLazyList<FileDefMD> listFileDefMD;
		SimpleLazyList<ExportedTypeMD> listExportedTypeMD;
		SimpleLazyList<ManifestResourceMD> listManifestResourceMD;
		SimpleLazyList<GenericParamMD> listGenericParamMD;
		SimpleLazyList<MethodSpecMD> listMethodSpecMD;
		SimpleLazyList<GenericParamConstraintMD> listGenericParamConstraintMD;

		LazyList<TypeDef> types;

		CorLibTypeSig typeVoid;
		CorLibTypeSig typeBoolean;
		CorLibTypeSig typeChar;
		CorLibTypeSig typeSByte;
		CorLibTypeSig typeByte;
		CorLibTypeSig typeInt16;
		CorLibTypeSig typeUInt16;
		CorLibTypeSig typeInt32;
		CorLibTypeSig typeUInt32;
		CorLibTypeSig typeInt64;
		CorLibTypeSig typeUInt64;
		CorLibTypeSig typeSingle;
		CorLibTypeSig typeDouble;
		CorLibTypeSig typeString;
		CorLibTypeSig typeTypedReference;
		CorLibTypeSig typeIntPtr;
		CorLibTypeSig typeUIntPtr;
		CorLibTypeSig typeObject;
		AssemblyRef corLibAssemblyRef;

		/// <summary>
		/// Returns the .NET file
		/// </summary>
		public DotNetFile DotNetFile {
			get { return dnFile; }
		}

		/// <summary>
		/// Returns the .NET metadata interface
		/// </summary>
		public IMetaData MetaData {
			get { return dnFile.MetaData; }
		}

		/// <summary>
		/// Returns the #~ or #- tables stream
		/// </summary>
		public TablesStream TablesStream {
			get { return dnFile.MetaData.TablesStream; }
		}

		/// <summary>
		/// Returns the #Strings stream
		/// </summary>
		public StringsStream StringsStream {
			get { return dnFile.MetaData.StringsStream; }
		}

		/// <summary>
		/// Returns the #Blob stream
		/// </summary>
		public BlobStream BlobStream {
			get { return dnFile.MetaData.BlobStream; }
		}

		/// <summary>
		/// Returns the #GUID stream
		/// </summary>
		public GuidStream GuidStream {
			get { return dnFile.MetaData.GuidStream; }
		}

		/// <summary>
		/// Returns the #US stream
		/// </summary>
		public USStream USStream {
			get { return dnFile.MetaData.USStream; }
		}

		/// <inheritdoc/>
		public override ushort Generation {
			get { return generation.Value; }
			set { generation.Value = value; }
		}

		/// <inheritdoc/>
		public override UTF8String Name {
			get { return name.Value; }
			set { name.Value = value; }
		}

		/// <inheritdoc/>
		public override Guid? Mvid {
			get { return mvid.Value; }
			set { mvid.Value = value; }
		}

		/// <inheritdoc/>
		public override Guid? EncId {
			get { return encId.Value; }
			set { encId.Value = value; }
		}

		/// <inheritdoc/>
		public override Guid? EncBaseId {
			get { return encBaseId.Value; }
			set { encBaseId.Value = value; }
		}

		/// <inheritdoc/>
		public override AssemblyDef Assembly {
			get { return assembly.Value; }
			set { assembly.Value = value; }
		}

		/// <inheritdoc/>
		public override IList<TypeDef> Types {
			get { return types; }
		}

		/// <inheritdoc/>
		CorLibTypeSig ICorLibTypes.Void {
			get { return typeVoid; }
		}

		/// <inheritdoc/>
		CorLibTypeSig ICorLibTypes.Boolean {
			get { return typeBoolean; }
		}

		/// <inheritdoc/>
		CorLibTypeSig ICorLibTypes.Char {
			get { return typeChar; }
		}

		/// <inheritdoc/>
		CorLibTypeSig ICorLibTypes.SByte {
			get { return typeSByte; }
		}

		/// <inheritdoc/>
		CorLibTypeSig ICorLibTypes.Byte {
			get { return typeByte; }
		}

		/// <inheritdoc/>
		CorLibTypeSig ICorLibTypes.Int16 {
			get { return typeInt16; }
		}

		/// <inheritdoc/>
		CorLibTypeSig ICorLibTypes.UInt16 {
			get { return typeUInt16; }
		}

		/// <inheritdoc/>
		CorLibTypeSig ICorLibTypes.Int32 {
			get { return typeInt32; }
		}

		/// <inheritdoc/>
		CorLibTypeSig ICorLibTypes.UInt32 {
			get { return typeUInt32; }
		}

		/// <inheritdoc/>
		CorLibTypeSig ICorLibTypes.Int64 {
			get { return typeInt64; }
		}

		/// <inheritdoc/>
		CorLibTypeSig ICorLibTypes.UInt64 {
			get { return typeUInt64; }
		}

		/// <inheritdoc/>
		CorLibTypeSig ICorLibTypes.Single {
			get { return typeSingle; }
		}

		/// <inheritdoc/>
		CorLibTypeSig ICorLibTypes.Double {
			get { return typeDouble; }
		}

		/// <inheritdoc/>
		CorLibTypeSig ICorLibTypes.String {
			get { return typeString; }
		}

		/// <inheritdoc/>
		CorLibTypeSig ICorLibTypes.TypedReference {
			get { return typeTypedReference; }
		}

		/// <inheritdoc/>
		CorLibTypeSig ICorLibTypes.IntPtr {
			get { return typeIntPtr; }
		}

		/// <inheritdoc/>
		CorLibTypeSig ICorLibTypes.UIntPtr {
			get { return typeUIntPtr; }
		}

		/// <inheritdoc/>
		CorLibTypeSig ICorLibTypes.Object {
			get { return typeObject; }
		}

		/// <inheritdoc/>
		AssemblyRef ICorLibTypes.AssemblyRef {
			get { return corLibAssemblyRef; }
		}

		/// <summary>
		/// Gets the <see cref="ICorLibTypes"/>
		/// </summary>
		public ICorLibTypes CorLibTypes {
			get { return this; }
		}

		/// <summary>
		/// Creates a <see cref="ModuleDefMD"/> instance from a file
		/// </summary>
		/// <param name="fileName">File name of an existing .NET module/assembly</param>
		/// <returns>A new <see cref="ModuleDefMD"/> instance</returns>
		public new static ModuleDefMD Load(string fileName) {
			DotNetFile dnFile = null;
			try {
				return Load(dnFile = DotNetFile.Load(fileName));
			}
			catch {
				if (dnFile != null)
					dnFile.Dispose();
				throw;
			}
		}

		/// <summary>
		/// Creates a <see cref="ModuleDefMD"/> instance from a byte[]
		/// </summary>
		/// <param name="data">Contents of a .NET module/assembly</param>
		/// <returns>A new <see cref="ModuleDefMD"/> instance</returns>
		public new static ModuleDefMD Load(byte[] data) {
			DotNetFile dnFile = null;
			try {
				return Load(dnFile = DotNetFile.Load(data));
			}
			catch {
				if (dnFile != null)
					dnFile.Dispose();
				throw;
			}
		}

		/// <summary>
		/// Creates a <see cref="ModuleDefMD"/> instance from a memory location
		/// </summary>
		/// <param name="addr">Address of a .NET module/assembly</param>
		/// <returns>A new <see cref="ModuleDefMD"/> instance</returns>
		public new static ModuleDefMD Load(IntPtr addr) {
			DotNetFile dnFile = null;
			try {
				return Load(dnFile = DotNetFile.Load(addr));
			}
			catch {
				if (dnFile != null)
					dnFile.Dispose();
				throw;
			}
		}

		/// <summary>
		/// Creates a <see cref="ModuleDefMD"/> instance from a <see cref="DotNetFile"/>
		/// </summary>
		/// <param name="dnFile">The loaded .NET file</param>
		/// <returns>A new <see cref="ModuleDefMD"/> instance that now owns <paramref name="dnFile"/></returns>
		public new static ModuleDefMD Load(DotNetFile dnFile) {
			return new ModuleDefMD(dnFile);
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="dnFile">The loaded .NET file</param>
		/// <exception cref="ArgumentNullException">If <paramref name="dnFile"/> is null</exception>
		ModuleDefMD(DotNetFile dnFile)
			: this(dnFile, 1) {
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="dnFile">The loaded .NET file</param>
		/// <param name="rid">Row ID</param>
		/// <exception cref="ArgumentNullException">If <paramref name="dnFile"/> is null</exception>
		/// <exception cref="ArgumentException">If <paramref name="rid"/> is <c>0</c> or &gt; <c>0x00FFFFFF</c></exception>
		ModuleDefMD(DotNetFile dnFile, uint rid) {
#if DEBUG
			if (dnFile == null)
				throw new ArgumentNullException("dnFile");
			if (rid == 0 || rid > 0x00FFFFFF)
				throw new ArgumentException("rid");
#endif
			this.rid = rid;
			this.dnFile = dnFile;
			Initialize();
			InitializeCorLibTypes();
		}

		void Initialize() {
			generation.ReadOriginalValue = () => {
				InitializeRawRow();
				return rawRow.Generation;
			};
			name.ReadOriginalValue = () => {
				InitializeRawRow();
				return dnFile.MetaData.StringsStream.Read(rawRow.Name);
			};
			mvid.ReadOriginalValue = () => {
				InitializeRawRow();
				return dnFile.MetaData.GuidStream.Read(rawRow.Mvid);
			};
			encId.ReadOriginalValue = () => {
				InitializeRawRow();
				return dnFile.MetaData.GuidStream.Read(rawRow.EncId);
			};
			encBaseId.ReadOriginalValue = () => {
				InitializeRawRow();
				return dnFile.MetaData.GuidStream.Read(rawRow.EncBaseId);
			};
			assembly.ReadOriginalValue = () => {
				if (rid != 1)
					return null;
				var asm = ResolveAssembly(1);
				if (asm != null)
					asm.ManifestModule = this;
				return asm;
			};
			var ts = dnFile.MetaData.TablesStream;
			listModuleDefMD = new SimpleLazyList<ModuleDefMD>(ts.Get(Table.Module).Rows, ReadModule);
			listTypeRefMD = new SimpleLazyList<TypeRefMD>(ts.Get(Table.TypeRef).Rows, ReadTypeRef);
			listTypeDefMD = new SimpleLazyList<TypeDefMD>(ts.Get(Table.TypeDef).Rows, ReadTypeDef);
			listFieldDefMD = new SimpleLazyList<FieldDefMD>(ts.Get(Table.Field).Rows, ReadField);
			listMethodDefMD = new SimpleLazyList<MethodDefMD>(ts.Get(Table.Method).Rows, ReadMethod);
			listParamDefMD = new SimpleLazyList<ParamDefMD>(ts.Get(Table.Param).Rows, ReadParam);
			listInterfaceImplMD = new SimpleLazyList<InterfaceImplMD>(ts.Get(Table.InterfaceImpl).Rows, ReadInterfaceImpl);
			listMemberRefMD = new SimpleLazyList<MemberRefMD>(ts.Get(Table.MemberRef).Rows, ReadMemberRef);
			listDeclSecurityMD = new SimpleLazyList<DeclSecurityMD>(ts.Get(Table.DeclSecurity).Rows, ReadDeclSecurity);
			listStandAloneSigMD = new SimpleLazyList<StandAloneSigMD>(ts.Get(Table.StandAloneSig).Rows, ReadStandAloneSig);
			listEventDefMD = new SimpleLazyList<EventDefMD>(ts.Get(Table.Event).Rows, ReadEvent);
			listPropertyDefMD = new SimpleLazyList<PropertyDefMD>(ts.Get(Table.Property).Rows, ReadProperty);
			listModuleRefMD = new SimpleLazyList<ModuleRefMD>(ts.Get(Table.ModuleRef).Rows, ReadModuleRef);
			listTypeSpecMD = new SimpleLazyList<TypeSpecMD>(ts.Get(Table.TypeSpec).Rows, ReadTypeSpec);
			listAssemblyDefMD = new SimpleLazyList<AssemblyDefMD>(ts.Get(Table.Assembly).Rows, ReadAssembly);
			listAssemblyRefMD = new SimpleLazyList<AssemblyRefMD>(ts.Get(Table.AssemblyRef).Rows, ReadAssemblyRef);
			listFileDefMD = new SimpleLazyList<FileDefMD>(ts.Get(Table.File).Rows, ReadFile);
			listExportedTypeMD = new SimpleLazyList<ExportedTypeMD>(ts.Get(Table.ExportedType).Rows, ReadExportedType);
			listManifestResourceMD = new SimpleLazyList<ManifestResourceMD>(ts.Get(Table.ManifestResource).Rows, ReadManifestResource);
			listGenericParamMD = new SimpleLazyList<GenericParamMD>(ts.Get(Table.GenericParam).Rows, ReadGenericParam);
			listMethodSpecMD = new SimpleLazyList<MethodSpecMD>(ts.Get(Table.MethodSpec).Rows, ReadMethodSpec);
			listGenericParamConstraintMD = new SimpleLazyList<GenericParamConstraintMD>(ts.Get(Table.GenericParamConstraint).Rows, ReadGenericParamConstraint);
			types = new LazyList<TypeDef>((int)ts.Get(Table.TypeDef).Rows, 1, ResolveTypeDef);
		}

		void InitializeCorLibTypes() {
			corLibAssemblyRef = FindCorLibAssemblyRef();
			typeVoid = new CorLibTypeSig(CreateCorLibTypeRef("Void"), ElementType.Void);
			typeBoolean = new CorLibTypeSig(CreateCorLibTypeRef("Boolean"), ElementType.Boolean);
			typeChar = new CorLibTypeSig(CreateCorLibTypeRef("Char"), ElementType.Char);
			typeSByte = new CorLibTypeSig(CreateCorLibTypeRef("SByte"), ElementType.I1);
			typeByte = new CorLibTypeSig(CreateCorLibTypeRef("Byte"), ElementType.U1);
			typeInt16 = new CorLibTypeSig(CreateCorLibTypeRef("Int16"), ElementType.I2);
			typeUInt16 = new CorLibTypeSig(CreateCorLibTypeRef("UInt16"), ElementType.U2);
			typeInt32 = new CorLibTypeSig(CreateCorLibTypeRef("Int32"), ElementType.I4);
			typeUInt32 = new CorLibTypeSig(CreateCorLibTypeRef("UInt32"), ElementType.U4);
			typeInt64 = new CorLibTypeSig(CreateCorLibTypeRef("Int64"), ElementType.I8);
			typeUInt64 = new CorLibTypeSig(CreateCorLibTypeRef("UInt64"), ElementType.U8);
			typeSingle = new CorLibTypeSig(CreateCorLibTypeRef("Single"), ElementType.R4);
			typeDouble = new CorLibTypeSig(CreateCorLibTypeRef("Double"), ElementType.R8);
			typeString = new CorLibTypeSig(CreateCorLibTypeRef("String"), ElementType.String);
			typeTypedReference = new CorLibTypeSig(CreateCorLibTypeRef("TypedReference"), ElementType.TypedByRef);
			typeIntPtr = new CorLibTypeSig(CreateCorLibTypeRef("IntPtr"), ElementType.I);
			typeUIntPtr = new CorLibTypeSig(CreateCorLibTypeRef("UIntPtr"), ElementType.U);
			typeObject = new CorLibTypeSig(CreateCorLibTypeRef("Object"), ElementType.Object);
		}

		TypeRef CreateCorLibTypeRef(string name) {
			return new TypeRefUser("System", name, corLibAssemblyRef);
		}

		/// <summary>
		/// Finds or creates a mscorlib <see cref="AssemblyRef"/>
		/// </summary>
		/// <returns>An existing or new <see cref="AssemblyRef"/></returns>
		AssemblyRef FindCorLibAssemblyRef() {
			var numAsmRefs = TablesStream.Get(Table.AssemblyRef).Rows;
			AssemblyRef corLibAsmRef = null;
			for (uint i = 1; i <= numAsmRefs; i++) {
				var asmRef = ResolveAssemblyRef(i);
				if (UTF8String.IsNullOrEmpty(asmRef.Name))
					continue;
				if (asmRef.Name != "mscorlib")
					continue;
				if (corLibAsmRef == null || corLibAsmRef.Version == null || (asmRef.Version != null && asmRef.Version >= corLibAsmRef.Version))
					corLibAsmRef = asmRef;
			}
			if (corLibAsmRef != null)
				return corLibAsmRef;
			return new AssemblyRefUser("mscorlib", new Version(2, 0, 0, 0), new PublicKeyToken("b77a5c561934e089"));
		}

		void InitializeRawRow() {
			if (rawRow != null)
				return;
			rawRow = dnFile.MetaData.TablesStream.ReadModuleRow(rid) ?? new RawModuleRow();
		}

		/// <inheritdoc/>
		protected override void Dispose(bool disposing) {
			if (disposing) {
				if (dnFile != null)
					dnFile.Dispose();
				dnFile = null;
			}
			base.Dispose(disposing);
		}

		/// <summary>
		/// Reads a <see cref="ModuleDefMD"/>
		/// </summary>
		/// <param name="rid">The row ID</param>
		/// <returns>A <see cref="ModuleDefMD"/> instance or null if <paramref name="rid"/> is invalid</returns>
		ModuleDefMD ReadModule(uint rid) {
			if (rid == this.rid)
				return this;
			return new ModuleDefMD(dnFile, rid);
		}

		/// <summary>
		/// Reads a <see cref="TypeRefMD"/>
		/// </summary>
		/// <param name="rid">The row ID</param>
		/// <returns>A <see cref="TypeRefMD"/> instance or null if <paramref name="rid"/> is invalid</returns>
		TypeRefMD ReadTypeRef(uint rid) {
			return new TypeRefMD(this, rid);
		}

		/// <summary>
		/// Reads a <see cref="TypeDefMD"/>
		/// </summary>
		/// <param name="rid">The row ID</param>
		/// <returns>A <see cref="TypeDefMD"/> instance or null if <paramref name="rid"/> is invalid</returns>
		TypeDefMD ReadTypeDef(uint rid) {
			return new TypeDefMD(this, rid);
		}

		/// <summary>
		/// Reads a <see cref="FieldDefMD"/>
		/// </summary>
		/// <param name="rid">The row ID</param>
		/// <returns>A <see cref="FieldDefMD"/> instance or null if <paramref name="rid"/> is invalid</returns>
		FieldDefMD ReadField(uint rid) {
			return new FieldDefMD(this, rid);
		}

		/// <summary>
		/// Reads a <see cref="MethodDefMD"/>
		/// </summary>
		/// <param name="rid">The row ID</param>
		/// <returns>A <see cref="MethodDefMD"/> instance or null if <paramref name="rid"/> is invalid</returns>
		MethodDefMD ReadMethod(uint rid) {
			return new MethodDefMD(this, rid);
		}

		/// <summary>
		/// Reads a <see cref="ParamDefMD"/>
		/// </summary>
		/// <param name="rid">The row ID</param>
		/// <returns>A <see cref="ParamDefMD"/> instance or null if <paramref name="rid"/> is invalid</returns>
		ParamDefMD ReadParam(uint rid) {
			return new ParamDefMD(this, rid);
		}

		/// <summary>
		/// Reads a <see cref="InterfaceImplMD"/>
		/// </summary>
		/// <param name="rid">The row ID</param>
		/// <returns>A <see cref="InterfaceImplMD"/> instance or null if <paramref name="rid"/> is invalid</returns>
		InterfaceImplMD ReadInterfaceImpl(uint rid) {
			return new InterfaceImplMD(this, rid);
		}

		/// <summary>
		/// Reads a <see cref="MemberRefMD"/>
		/// </summary>
		/// <param name="rid">The row ID</param>
		/// <returns>A <see cref="MemberRefMD"/> instance or null if <paramref name="rid"/> is invalid</returns>
		MemberRefMD ReadMemberRef(uint rid) {
			throw new NotImplementedException();	//TODO:
		}

		/// <summary>
		/// Reads a <see cref="DeclSecurityMD"/>
		/// </summary>
		/// <param name="rid">The row ID</param>
		/// <returns>A <see cref="DeclSecurityMD"/> instance or null if <paramref name="rid"/> is invalid</returns>
		DeclSecurityMD ReadDeclSecurity(uint rid) {
			return new DeclSecurityMD(this, rid);
		}

		/// <summary>
		/// Reads a <see cref="StandAloneSigMD"/>
		/// </summary>
		/// <param name="rid">The row ID</param>
		/// <returns>A <see cref="StandAloneSigMD"/> instance or null if <paramref name="rid"/> is invalid</returns>
		StandAloneSigMD ReadStandAloneSig(uint rid) {
			throw new NotImplementedException();	//TODO:
		}

		/// <summary>
		/// Reads a <see cref="EventDefMD"/>
		/// </summary>
		/// <param name="rid">The row ID</param>
		/// <returns>A <see cref="EventDefMD"/> instance or null if <paramref name="rid"/> is invalid</returns>
		EventDefMD ReadEvent(uint rid) {
			throw new NotImplementedException();	//TODO:
		}

		/// <summary>
		/// Reads a <see cref="PropertyDefMD"/>
		/// </summary>
		/// <param name="rid">The row ID</param>
		/// <returns>A <see cref="PropertyDefMD"/> instance or null if <paramref name="rid"/> is invalid</returns>
		PropertyDefMD ReadProperty(uint rid) {
			throw new NotImplementedException();	//TODO:
		}

		/// <summary>
		/// Reads a <see cref="ModuleRefMD"/>
		/// </summary>
		/// <param name="rid">The row ID</param>
		/// <returns>A <see cref="ModuleRefMD"/> instance or null if <paramref name="rid"/> is invalid</returns>
		ModuleRefMD ReadModuleRef(uint rid) {
			return new ModuleRefMD(this, rid);
		}

		/// <summary>
		/// Reads a <see cref="TypeSpecMD"/>
		/// </summary>
		/// <param name="rid">The row ID</param>
		/// <returns>A <see cref="TypeSpecMD"/> instance or null if <paramref name="rid"/> is invalid</returns>
		TypeSpecMD ReadTypeSpec(uint rid) {
			return new TypeSpecMD(this, rid);
		}

		/// <summary>
		/// Reads a <see cref="AssemblyDefMD"/>
		/// </summary>
		/// <param name="rid">The row ID</param>
		/// <returns>A <see cref="AssemblyDefMD"/> instance or null if <paramref name="rid"/> is invalid</returns>
		AssemblyDefMD ReadAssembly(uint rid) {
			return new AssemblyDefMD(this, rid);
		}

		/// <summary>
		/// Reads a <see cref="AssemblyRefMD"/>
		/// </summary>
		/// <param name="rid">The row ID</param>
		/// <returns>A <see cref="AssemblyRefMD"/> instance or null if <paramref name="rid"/> is invalid</returns>
		AssemblyRefMD ReadAssemblyRef(uint rid) {
			return new AssemblyRefMD(this, rid);
		}

		/// <summary>
		/// Reads a <see cref="FileDefMD"/>
		/// </summary>
		/// <param name="rid">The row ID</param>
		/// <returns>A <see cref="FileDefMD"/> instance or null if <paramref name="rid"/> is invalid</returns>
		FileDefMD ReadFile(uint rid) {
			return new FileDefMD(this, rid);
		}

		/// <summary>
		/// Reads a <see cref="ExportedTypeMD"/>
		/// </summary>
		/// <param name="rid">The row ID</param>
		/// <returns>A <see cref="ExportedTypeMD"/> instance or null if <paramref name="rid"/> is invalid</returns>
		ExportedTypeMD ReadExportedType(uint rid) {
			return new ExportedTypeMD(this, rid);
		}

		/// <summary>
		/// Reads a <see cref="ManifestResourceMD"/>
		/// </summary>
		/// <param name="rid">The row ID</param>
		/// <returns>A <see cref="ManifestResourceMD"/> instance or null if <paramref name="rid"/> is invalid</returns>
		ManifestResourceMD ReadManifestResource(uint rid) {
			return new ManifestResourceMD(this, rid);
		}

		/// <summary>
		/// Reads a <see cref="GenericParamMD"/>
		/// </summary>
		/// <param name="rid">The row ID</param>
		/// <returns>A <see cref="GenericParamMD"/> instance or null if <paramref name="rid"/> is invalid</returns>
		GenericParamMD ReadGenericParam(uint rid) {
			return new GenericParamMD(this, rid);
		}

		/// <summary>
		/// Reads a <see cref="MethodSpecMD"/>
		/// </summary>
		/// <param name="rid">The row ID</param>
		/// <returns>A <see cref="MethodSpecMD"/> instance or null if <paramref name="rid"/> is invalid</returns>
		MethodSpecMD ReadMethodSpec(uint rid) {
			throw new NotImplementedException();	//TODO:
		}

		/// <summary>
		/// Reads a <see cref="GenericParamConstraintMD"/>
		/// </summary>
		/// <param name="rid">The row ID</param>
		/// <returns>A <see cref="GenericParamConstraintMD"/> instance or null if <paramref name="rid"/> is invalid</returns>
		GenericParamConstraintMD ReadGenericParamConstraint(uint rid) {
			return new GenericParamConstraintMD(this, rid);
		}

		/// <summary>
		/// Resolves a token
		/// </summary>
		/// <param name="mdToken">The metadata token</param>
		/// <returns>A <see cref="ICodedToken"/> or null if <paramref name="mdToken"/> is invalid</returns>
		public ICodedToken ResolveToken(MDToken mdToken) {
			return ResolveToken(mdToken.Raw);
		}

		/// <summary>
		/// Resolves a token
		/// </summary>
		/// <param name="token">The metadata token</param>
		/// <returns>A <see cref="ICodedToken"/> or null if <paramref name="token"/> is invalid</returns>
		public ICodedToken ResolveToken(int token) {
			return ResolveToken((uint)token);
		}

		/// <summary>
		/// Resolves a token
		/// </summary>
		/// <param name="token">The metadata token</param>
		/// <returns>A <see cref="ICodedToken"/> or null if <paramref name="token"/> is invalid</returns>
		public ICodedToken ResolveToken(uint token) {
			uint rid = token & 0x00FFFFFF;
			switch ((Table)(token >> 24)) {
			case Table.Module: return ResolveModule(rid);
			case Table.TypeRef: return ResolveTypeRef(rid);
			case Table.TypeDef: return ResolveTypeDef(rid);
			case Table.Field: return ResolveField(rid);
			case Table.Method: return ResolveMethod(rid);
			case Table.Param: return ResolveParam(rid);
			case Table.InterfaceImpl: return ResolveInterfaceImpl(rid);
			case Table.MemberRef: return ResolveMemberRef(rid);
			case Table.DeclSecurity: return ResolveDeclSecurity(rid);
			case Table.StandAloneSig: return ResolveStandAloneSig(rid);
			case Table.Event: return ResolveEvent(rid);
			case Table.Property: return ResolveProperty(rid);
			case Table.ModuleRef: return ResolveModuleRef(rid);
			case Table.TypeSpec: return ResolveTypeSpec(rid);
			case Table.Assembly: return ResolveAssembly(rid);
			case Table.AssemblyRef: return ResolveAssemblyRef(rid);
			case Table.File: return ResolveFile(rid);
			case Table.ExportedType: return ResolveExportedType(rid);
			case Table.ManifestResource: return ResolveManifestResource(rid);
			case Table.GenericParam: return ResolveGenericParam(rid);
			case Table.MethodSpec: return ResolveMethodSpec(rid);
			case Table.GenericParamConstraint: return ResolveGenericParamConstraint(rid);
			}
			return null;
		}

		/// <summary>
		/// Resolves a <see cref="ModuleDef"/>
		/// </summary>
		/// <param name="rid">The row ID</param>
		/// <returns>A <see cref="ModuleDef"/> instance or null if <paramref name="rid"/> is invalid</returns>
		public ModuleDef ResolveModule(uint rid) {
			return listModuleDefMD[rid - 1];
		}

		/// <summary>
		/// Resolves a <see cref="TypeRef"/>
		/// </summary>
		/// <param name="rid">The row ID</param>
		/// <returns>A <see cref="TypeRef"/> instance or null if <paramref name="rid"/> is invalid</returns>
		public TypeRef ResolveTypeRef(uint rid) {
			return listTypeRefMD[rid - 1];
		}

		/// <summary>
		/// Resolves a <see cref="TypeDef"/>
		/// </summary>
		/// <param name="rid">The row ID</param>
		/// <returns>A <see cref="TypeDef"/> instance or null if <paramref name="rid"/> is invalid</returns>
		public TypeDef ResolveTypeDef(uint rid) {
			return listTypeDefMD[rid - 1];
		}

		/// <summary>
		/// Resolves a <see cref="FieldDef"/>
		/// </summary>
		/// <param name="rid">The row ID</param>
		/// <returns>A <see cref="FieldDef"/> instance or null if <paramref name="rid"/> is invalid</returns>
		public FieldDef ResolveField(uint rid) {
			return listFieldDefMD[rid - 1];
		}

		/// <summary>
		/// Resolves a <see cref="MethodDef"/>
		/// </summary>
		/// <param name="rid">The row ID</param>
		/// <returns>A <see cref="MethodDef"/> instance or null if <paramref name="rid"/> is invalid</returns>
		public MethodDef ResolveMethod(uint rid) {
			return listMethodDefMD[rid - 1];
		}

		/// <summary>
		/// Resolves a <see cref="ParamDef"/>
		/// </summary>
		/// <param name="rid">The row ID</param>
		/// <returns>A <see cref="ParamDef"/> instance or null if <paramref name="rid"/> is invalid</returns>
		public ParamDef ResolveParam(uint rid) {
			return listParamDefMD[rid - 1];
		}

		/// <summary>
		/// Resolves a <see cref="InterfaceImpl"/>
		/// </summary>
		/// <param name="rid">The row ID</param>
		/// <returns>A <see cref="InterfaceImpl"/> instance or null if <paramref name="rid"/> is invalid</returns>
		public InterfaceImpl ResolveInterfaceImpl(uint rid) {
			return listInterfaceImplMD[rid - 1];
		}

		/// <summary>
		/// Resolves a <see cref="MemberRef"/>
		/// </summary>
		/// <param name="rid">The row ID</param>
		/// <returns>A <see cref="MemberRef"/> instance or null if <paramref name="rid"/> is invalid</returns>
		public MemberRef ResolveMemberRef(uint rid) {
			return listMemberRefMD[rid - 1];
		}

		/// <summary>
		/// Resolves a <see cref="DeclSecurity"/>
		/// </summary>
		/// <param name="rid">The row ID</param>
		/// <returns>A <see cref="DeclSecurity"/> instance or null if <paramref name="rid"/> is invalid</returns>
		public DeclSecurity ResolveDeclSecurity(uint rid) {
			return listDeclSecurityMD[rid - 1];
		}

		/// <summary>
		/// Resolves a <see cref="StandAloneSig"/>
		/// </summary>
		/// <param name="rid">The row ID</param>
		/// <returns>A <see cref="StandAloneSig"/> instance or null if <paramref name="rid"/> is invalid</returns>
		public StandAloneSig ResolveStandAloneSig(uint rid) {
			return listStandAloneSigMD[rid - 1];
		}

		/// <summary>
		/// Resolves a <see cref="EventDef"/>
		/// </summary>
		/// <param name="rid">The row ID</param>
		/// <returns>A <see cref="EventDef"/> instance or null if <paramref name="rid"/> is invalid</returns>
		public EventDef ResolveEvent(uint rid) {
			return listEventDefMD[rid - 1];
		}

		/// <summary>
		/// Resolves a <see cref="PropertyDef"/>
		/// </summary>
		/// <param name="rid">The row ID</param>
		/// <returns>A <see cref="PropertyDef"/> instance or null if <paramref name="rid"/> is invalid</returns>
		public PropertyDef ResolveProperty(uint rid) {
			return listPropertyDefMD[rid - 1];
		}

		/// <summary>
		/// Resolves a <see cref="ModuleRef"/>
		/// </summary>
		/// <param name="rid">The row ID</param>
		/// <returns>A <see cref="ModuleRef"/> instance or null if <paramref name="rid"/> is invalid</returns>
		public ModuleRef ResolveModuleRef(uint rid) {
			return listModuleRefMD[rid - 1];
		}

		/// <summary>
		/// Resolves a <see cref="TypeSpec"/>
		/// </summary>
		/// <param name="rid">The row ID</param>
		/// <returns>A <see cref="TypeSpec"/> instance or null if <paramref name="rid"/> is invalid</returns>
		public TypeSpec ResolveTypeSpec(uint rid) {
			return listTypeSpecMD[rid - 1];
		}

		/// <summary>
		/// Resolves a <see cref="AssemblyDef"/>
		/// </summary>
		/// <param name="rid">The row ID</param>
		/// <returns>A <see cref="AssemblyDef"/> instance or null if <paramref name="rid"/> is invalid</returns>
		public AssemblyDef ResolveAssembly(uint rid) {
			return listAssemblyDefMD[rid - 1];
		}

		/// <summary>
		/// Resolves a <see cref="AssemblyRef"/>
		/// </summary>
		/// <param name="rid">The row ID</param>
		/// <returns>A <see cref="AssemblyRef"/> instance or null if <paramref name="rid"/> is invalid</returns>
		public AssemblyRef ResolveAssemblyRef(uint rid) {
			return listAssemblyRefMD[rid - 1];
		}

		/// <summary>
		/// Resolves a <see cref="FileDef"/>
		/// </summary>
		/// <param name="rid">The row ID</param>
		/// <returns>A <see cref="FileDef"/> instance or null if <paramref name="rid"/> is invalid</returns>
		public FileDef ResolveFile(uint rid) {
			return listFileDefMD[rid - 1];
		}

		/// <summary>
		/// Resolves a <see cref="ExportedType"/>
		/// </summary>
		/// <param name="rid">The row ID</param>
		/// <returns>A <see cref="ExportedType"/> instance or null if <paramref name="rid"/> is invalid</returns>
		public ExportedType ResolveExportedType(uint rid) {
			return listExportedTypeMD[rid - 1];
		}

		/// <summary>
		/// Resolves a <see cref="ManifestResource"/>
		/// </summary>
		/// <param name="rid">The row ID</param>
		/// <returns>A <see cref="ManifestResource"/> instance or null if <paramref name="rid"/> is invalid</returns>
		public ManifestResource ResolveManifestResource(uint rid) {
			return listManifestResourceMD[rid - 1];
		}

		/// <summary>
		/// Resolves a <see cref="GenericParam"/>
		/// </summary>
		/// <param name="rid">The row ID</param>
		/// <returns>A <see cref="GenericParam"/> instance or null if <paramref name="rid"/> is invalid</returns>
		public GenericParam ResolveGenericParam(uint rid) {
			return listGenericParamMD[rid - 1];
		}

		/// <summary>
		/// Resolves a <see cref="MethodSpec"/>
		/// </summary>
		/// <param name="rid">The row ID</param>
		/// <returns>A <see cref="MethodSpec"/> instance or null if <paramref name="rid"/> is invalid</returns>
		public MethodSpec ResolveMethodSpec(uint rid) {
			return listMethodSpecMD[rid - 1];
		}

		/// <summary>
		/// Resolves a <see cref="GenericParamConstraint"/>
		/// </summary>
		/// <param name="rid">The row ID</param>
		/// <returns>A <see cref="GenericParamConstraint"/> instance or null if <paramref name="rid"/> is invalid</returns>
		public GenericParamConstraint ResolveGenericParamConstraint(uint rid) {
			return listGenericParamConstraintMD[rid - 1];
		}

		/// <summary>
		/// Resolves a <see cref="ITypeDefOrRef"/>
		/// </summary>
		/// <param name="codedToken">A <c>TypeDefOrRef</c> coded token</param>
		/// <returns>A <see cref="ITypeDefOrRef"/> or null if <paramref name="codedToken"/> is invalid</returns>
		public ITypeDefOrRef ResolveTypeDefOrRef(uint codedToken) {
			uint token;
			if (!CodedToken.TypeDefOrRef.Decode(codedToken, out token))
				return null;
			uint rid = token & 0x00FFFFFF;
			switch ((Table)(token >> 24)) {
			case Table.TypeDef: return ResolveTypeDef(rid);
			case Table.TypeRef: return ResolveTypeRef(rid);
			case Table.TypeSpec: return ResolveTypeSpec(rid);
			}
			return null;
		}

		/// <summary>
		/// Resolves a <see cref="IHasConstant"/>
		/// </summary>
		/// <param name="codedToken">A <c>HasConstant</c> coded token</param>
		/// <returns>A <see cref="IHasConstant"/> or null if <paramref name="codedToken"/> is invalid</returns>
		public IHasConstant ResolveHasConstant(uint codedToken) {
			uint token;
			if (!CodedToken.HasConstant.Decode(codedToken, out token))
				return null;
			uint rid = token & 0x00FFFFFF;
			switch ((Table)(token >> 24)) {
			case Table.Field: return ResolveField(rid);
			case Table.Param: return ResolveParam(rid);
			case Table.Property: return ResolveProperty(rid);
			}
			return null;
		}

		/// <summary>
		/// Resolves a <see cref="IHasCustomAttribute"/>
		/// </summary>
		/// <param name="codedToken">A <c>HasCustomAttribute</c> coded token</param>
		/// <returns>A <see cref="IHasCustomAttribute"/> or null if <paramref name="codedToken"/> is invalid</returns>
		public IHasCustomAttribute ResolveHasCustomAttribute(uint codedToken) {
			uint token;
			if (!CodedToken.HasCustomAttribute.Decode(codedToken, out token))
				return null;
			uint rid = token & 0x00FFFFFF;
			switch ((Table)(token >> 24)) {
			case Table.Method: return ResolveMethod(rid);
			case Table.Field: return ResolveField(rid);
			case Table.TypeRef: return ResolveTypeRef(rid);
			case Table.TypeDef: return ResolveTypeDef(rid);
			case Table.Param: return ResolveParam(rid);
			case Table.InterfaceImpl: return ResolveInterfaceImpl(rid);
			case Table.MemberRef: return ResolveMemberRef(rid);
			case Table.Module: return ResolveModule(rid);
			case Table.DeclSecurity: return ResolveDeclSecurity(rid);
			case Table.Property: return ResolveProperty(rid);
			case Table.Event: return ResolveEvent(rid);
			case Table.StandAloneSig: return ResolveStandAloneSig(rid);
			case Table.ModuleRef: return ResolveModuleRef(rid);
			case Table.TypeSpec: return ResolveTypeSpec(rid);
			case Table.Assembly: return ResolveAssembly(rid);
			case Table.AssemblyRef: return ResolveAssemblyRef(rid);
			case Table.File: return ResolveFile(rid);
			case Table.ExportedType: return ResolveExportedType(rid);
			case Table.ManifestResource: return ResolveManifestResource(rid);
			case Table.GenericParam: return ResolveGenericParam(rid);
			case Table.GenericParamConstraint: return ResolveGenericParamConstraint(rid);
			case Table.MethodSpec: return ResolveMethodSpec(rid);
			}
			return null;
		}

		/// <summary>
		/// Resolves a <see cref="IHasFieldMarshal"/>
		/// </summary>
		/// <param name="codedToken">A <c>HasFieldMarshal</c> coded token</param>
		/// <returns>A <see cref="IHasFieldMarshal"/> or null if <paramref name="codedToken"/> is invalid</returns>
		public IHasFieldMarshal ResolveHasFieldMarshal(uint codedToken) {
			uint token;
			if (!CodedToken.HasFieldMarshal.Decode(codedToken, out token))
				return null;
			uint rid = token & 0x00FFFFFF;
			switch ((Table)(token >> 24)) {
			case Table.Field: return ResolveField(rid);
			case Table.Param: return ResolveParam(rid);
			}
			return null;
		}

		/// <summary>
		/// Resolves a <see cref="IHasDeclSecurity"/>
		/// </summary>
		/// <param name="codedToken">A <c>HasDeclSecurity</c> coded token</param>
		/// <returns>A <see cref="IHasDeclSecurity"/> or null if <paramref name="codedToken"/> is invalid</returns>
		public IHasDeclSecurity ResolveHasDeclSecurity(uint codedToken) {
			uint token;
			if (!CodedToken.HasDeclSecurity.Decode(codedToken, out token))
				return null;
			uint rid = token & 0x00FFFFFF;
			switch ((Table)(token >> 24)) {
			case Table.TypeDef: return ResolveTypeDef(rid);
			case Table.Method: return ResolveMethod(rid);
			case Table.Assembly: return ResolveAssembly(rid);
			}
			return null;
		}

		/// <summary>
		/// Resolves a <see cref="IMemberRefParent"/>
		/// </summary>
		/// <param name="codedToken">A <c>MemberRefParent</c> coded token</param>
		/// <returns>A <see cref="IMemberRefParent"/> or null if <paramref name="codedToken"/> is invalid</returns>
		public IMemberRefParent ResolveMemberRefParent(uint codedToken) {
			uint token;
			if (!CodedToken.MemberRefParent.Decode(codedToken, out token))
				return null;
			uint rid = token & 0x00FFFFFF;
			switch ((Table)(token >> 24)) {
			case Table.TypeDef: return ResolveTypeDef(rid);
			case Table.TypeRef: return ResolveTypeRef(rid);
			case Table.ModuleRef: return ResolveModuleRef(rid);
			case Table.Method: return ResolveMethod(rid);
			case Table.TypeSpec: return ResolveTypeSpec(rid);
			}
			return null;
		}

		/// <summary>
		/// Resolves a <see cref="IHasSemantic"/>
		/// </summary>
		/// <param name="codedToken">A <c>HasSemantic</c> coded token</param>
		/// <returns>A <see cref="IHasSemantic"/> or null if <paramref name="codedToken"/> is invalid</returns>
		public IHasSemantic ResolveHasSemantic(uint codedToken) {
			uint token;
			if (!CodedToken.HasSemantic.Decode(codedToken, out token))
				return null;
			uint rid = token & 0x00FFFFFF;
			switch ((Table)(token >> 24)) {
			case Table.Event: return ResolveEvent(rid);
			case Table.Property: return ResolveProperty(rid);
			}
			return null;
		}

		/// <summary>
		/// Resolves a <see cref="IMethodDefOrRef"/>
		/// </summary>
		/// <param name="codedToken">A <c>MethodDefOrRef</c> coded token</param>
		/// <returns>A <see cref="IMethodDefOrRef"/> or null if <paramref name="codedToken"/> is invalid</returns>
		public IMethodDefOrRef ResolveMethodDefOrRef(uint codedToken) {
			uint token;
			if (!CodedToken.MethodDefOrRef.Decode(codedToken, out token))
				return null;
			uint rid = token & 0x00FFFFFF;
			switch ((Table)(token >> 24)) {
			case Table.Method: return ResolveMethod(rid);
			case Table.MemberRef: return ResolveMemberRef(rid);
			}
			return null;
		}

		/// <summary>
		/// Resolves a <see cref="IMemberForwarded"/>
		/// </summary>
		/// <param name="codedToken">A <c>MemberForwarded</c> coded token</param>
		/// <returns>A <see cref="IMemberForwarded"/> or null if <paramref name="codedToken"/> is invalid</returns>
		public IMemberForwarded ResolveMemberForwarded(uint codedToken) {
			uint token;
			if (!CodedToken.MemberForwarded.Decode(codedToken, out token))
				return null;
			uint rid = token & 0x00FFFFFF;
			switch ((Table)(token >> 24)) {
			case Table.Field: return ResolveField(rid);
			case Table.Method: return ResolveMethod(rid);
			}
			return null;
		}

		/// <summary>
		/// Resolves a <see cref="IImplementation"/>
		/// </summary>
		/// <param name="codedToken">An <c>Implementation</c> coded token</param>
		/// <returns>A <see cref="IImplementation"/> or null if <paramref name="codedToken"/> is invalid</returns>
		public IImplementation ResolveImplementation(uint codedToken) {
			uint token;
			if (!CodedToken.Implementation.Decode(codedToken, out token))
				return null;
			uint rid = token & 0x00FFFFFF;
			switch ((Table)(token >> 24)) {
			case Table.File: return ResolveFile(rid);
			case Table.AssemblyRef: return ResolveAssemblyRef(rid);
			case Table.ExportedType: return ResolveExportedType(rid);
			}
			return null;
		}

		/// <summary>
		/// Resolves a <see cref="ICustomAttributeType"/>
		/// </summary>
		/// <param name="codedToken">A <c>CustomAttributeType</c> coded token</param>
		/// <returns>A <see cref="ICustomAttributeType"/> or null if <paramref name="codedToken"/> is invalid</returns>
		public ICustomAttributeType ResolveCustomAttributeType(uint codedToken) {
			uint token;
			if (!CodedToken.CustomAttributeType.Decode(codedToken, out token))
				return null;
			uint rid = token & 0x00FFFFFF;
			switch ((Table)(token >> 24)) {
			case Table.Method: return ResolveMethod(rid);
			case Table.MemberRef: return ResolveMemberRef(rid);
			}
			return null;
		}

		/// <summary>
		/// Resolves a <see cref="IResolutionScope"/>
		/// </summary>
		/// <param name="codedToken">A <c>ResolutionScope</c> coded token</param>
		/// <returns>A <see cref="IResolutionScope"/> or null if <paramref name="codedToken"/> is invalid</returns>
		public IResolutionScope ResolveResolutionScope(uint codedToken) {
			uint token;
			if (!CodedToken.ResolutionScope.Decode(codedToken, out token))
				return null;
			uint rid = token & 0x00FFFFFF;
			switch ((Table)(token >> 24)) {
			case Table.Module: return ResolveModule(rid);
			case Table.ModuleRef: return ResolveModuleRef(rid);
			case Table.AssemblyRef: return ResolveAssemblyRef(rid);
			case Table.TypeRef: return ResolveTypeRef(rid);
			}
			return null;
		}

		/// <summary>
		/// Resolves a <see cref="ITypeOrMethodDef"/>
		/// </summary>
		/// <param name="codedToken">A <c>TypeOrMethodDef</c>> coded token</param>
		/// <returns>A <see cref="ITypeOrMethodDef"/> or null if <paramref name="codedToken"/> is invalid</returns>
		public ITypeOrMethodDef ResolveTypeOrMethodDef(uint codedToken) {
			uint token;
			if (!CodedToken.TypeOrMethodDef.Decode(codedToken, out token))
				return null;
			uint rid = token & 0x00FFFFFF;
			switch ((Table)(token >> 24)) {
			case Table.TypeDef: return ResolveTypeDef(rid);
			case Table.Method: return ResolveMethod(rid);
			}
			return null;
		}

		/// <summary>
		/// Reads a signature from the #Blob stream
		/// </summary>
		/// <param name="sig">#Blob stream offset of signature</param>
		/// <returns>A new <see cref="CallingConventionSig"/> instance</returns>
		public CallingConventionSig ReadSignature(uint sig) {
			return SignatureReader.ReadSig(this, sig);
		}

		/// <summary>
		/// Reads a type signature from the #Blob stream
		/// </summary>
		/// <param name="sig">#Blob stream offset of signature</param>
		/// <returns>A new <see cref="ITypeSig"/> instance</returns>
		public ITypeSig ReadTypeSignature(uint sig) {
			return SignatureReader.ReadTypeSig(this, sig);
		}
	}
}
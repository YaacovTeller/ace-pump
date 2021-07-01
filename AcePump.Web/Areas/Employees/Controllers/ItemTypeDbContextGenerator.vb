Imports System.Reflection.Emit
Imports System.Reflection
Imports System.Data.Entity
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports Soris.Mvc.Modules.TypeManager.Models

Namespace Areas.Employees.Controllers
    Friend NotInheritable Class ItemTypeDbContextGenerator
        Private Shared Property [Module] As ModuleBuilder
        Private Shared Property GeneratedTypeCache As New Dictionary(Of String, GeneratedTypes)

        Private Property CurrentTypeBuilder As TypeBuilder
        Private Property TypesToGenerate As GeneratedTypes

        Public Sub New(itemTypeName As String)
            InitializeTypes(itemTypeName)
        End Sub

        Public Function GetDbContext() As DbContext
            Return CreateContextInstance()
        End Function

        Public Function GetItemTypeConstructor() As ConstructorInfo
            Return TypesToGenerate.ItemTypeConstructor
        End Function

        Public Function GetDbSet(context As DbContext) As Object
            Return TypesToGenerate.ItemTypeSetAccessor.GetValue(context, Nothing)
        End Function

        Private Sub InitializeTypes(itemTypeName As String)
            If Not GeneratedTypeCache.ContainsKey(itemTypeName) Then
                TypesToGenerate = New GeneratedTypes() With {.ItemTypeName = itemTypeName}
                GeneratedTypeCache.Add(itemTypeName, TypesToGenerate)

                InitializeModule()

                InitializeItemTypeBuilder()
                MapItemTypeToTable()
                ImplementIItemTypeInterface()
                ImplementItemTypeConstructor()
                SaveItemType()

                InitializeContextTypeBuiler()
                ImplementContextConstructor()
                AddDbSetForItemType()
                SaveDbContextType()

                DisableDatabaseInitialization()
            End If

            TypesToGenerate = GeneratedTypeCache(itemTypeName)
        End Sub

        Private Shared _SetDatabaseInitializerAccessor As MethodInfo
        Private Shared ReadOnly Property SetDatabaseInitializerAccessor As MethodInfo
            Get
                If _SetDatabaseInitializerAccessor Is Nothing Then
                    _SetDatabaseInitializerAccessor = GetType(Database).GetMethod("SetInitializer")
                End If

                Return _SetDatabaseInitializerAccessor
            End Get
        End Property

        Private Shared Sub InitializeModule()
            If [Module] Is Nothing Then
                Dim assemblyName As New AssemblyName(String.Format("{0}~{1}", "DbContextGenAsm", Guid.NewGuid().ToString("N")))
                Dim assemblyBuilder As AssemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run)

                [Module] = assemblyBuilder.DefineDynamicModule("core")
            End If
        End Sub

        Private Sub InitializeItemTypeBuilder()
            CurrentTypeBuilder = [Module].DefineType(String.Format("{0}", TypesToGenerate.ItemTypeName),
                                                     TypeAttributes.Class Or TypeAttributes.Public Or TypeAttributes.Sealed)
        End Sub

        Private Sub MapItemTypeToTable()
            Dim tableAttrCtor As ConstructorInfo = GetType(TableAttribute).GetConstructor({GetType(String)})
            Dim tableAttr As New CustomAttributeBuilder(tableAttrCtor, {ItemTypeGeneratedDbContextBase.TypesTablePrefix & TypesToGenerate.ItemTypeName})
            CurrentTypeBuilder.SetCustomAttribute(tableAttr)
        End Sub

        Private Sub ImplementIItemTypeInterface()
            CurrentTypeBuilder.AddInterfaceImplementation(GetType(IItemType))

            Dim itemTypeIdBuilder As PropertyBuilder = ImplementProperty("ItemTypeID", GetType(Integer))
            Dim keyAttrCtor As ConstructorInfo = GetType(KeyAttribute).GetConstructor({})
            Dim keyAttr As New CustomAttributeBuilder(keyAttrCtor, {})
            itemTypeIdBuilder.SetCustomAttribute(keyAttr)

            ImplementProperty("DisplayText", GetType(String))
        End Sub

        Private Sub InitializeContextTypeBuiler()
            CurrentTypeBuilder = [Module].DefineType(String.Format("{0}Context", TypesToGenerate.ItemTypeName),
                                                     TypeAttributes.Class Or TypeAttributes.Public Or TypeAttributes.Sealed)

            CurrentTypeBuilder.SetParent(GetType(ItemTypeGeneratedDbContextBase))
        End Sub

        Private Sub AddDbSetForItemType()
            Dim dbSetType As Type = GetType(DbSet(Of )).MakeGenericType({TypesToGenerate.ItemType})

            ImplementProperty("Items", dbSetType)
        End Sub

        Private Function ImplementProperty(propertyName As String, propertyType As Type, Optional canRead As Boolean = True, Optional canWrite As Boolean = True) As PropertyBuilder
            Dim propBuilder As PropertyBuilder = CurrentTypeBuilder.DefineProperty(propertyName, PropertyAttributes.None, propertyType, Type.EmptyTypes)
            Dim backingField As FieldBuilder = CurrentTypeBuilder.DefineField("_" & propertyName,
                                                                                       propertyType,
                                                                                       FieldAttributes.Private)

            If canRead Then
                Dim methodBuilder As MethodBuilder = CurrentTypeBuilder.DefineMethod(String.Format("get_{0}", propertyName),
                                                                                                  MethodAttributes.Public Or MethodAttributes.Virtual Or MethodAttributes.Final,
                                                                                                  propertyType,
                                                                                                  Type.EmptyTypes)

                Dim ilBuilder As ILGenerator = methodBuilder.GetILGenerator()
                ilBuilder.Emit(OpCodes.Ldarg_0)
                ilBuilder.Emit(OpCodes.Ldfld, backingField)
                ilBuilder.Emit(OpCodes.Ret)

                propBuilder.SetGetMethod(methodBuilder)
            End If

            If canWrite Then
                Dim methodBuilder As MethodBuilder = CurrentTypeBuilder.DefineMethod(String.Format("set_{0}", propertyName),
                                                                                        MethodAttributes.Public Or MethodAttributes.Virtual Or MethodAttributes.Final,
                                                                                        GetType(Void),
                                                                                        {propertyType})

                Dim ilBuilder As ILGenerator = methodBuilder.GetILGenerator()
                ilBuilder.Emit(OpCodes.Ldarg_0)
                ilBuilder.Emit(OpCodes.Ldarg_1)
                ilBuilder.Emit(OpCodes.Stfld, backingField)
                ilBuilder.Emit(OpCodes.Ret)

                propBuilder.SetSetMethod(methodBuilder)
            End If

            Return propBuilder
        End Function

        Private Sub ImplementItemTypeConstructor()
            Dim builder As ConstructorBuilder = CurrentTypeBuilder.DefineConstructor(MethodAttributes.Public,
                                                                                     CallingConventions.Standard,
                                                                                     {})
            Dim ilBuilder As ILGenerator = builder.GetILGenerator()
            ilBuilder.Emit(OpCodes.Ret)
        End Sub

        Private Sub SaveItemType()
            TypesToGenerate.ItemType = CurrentTypeBuilder.CreateType()
            TypesToGenerate.ItemTypeConstructor = TypesToGenerate.ItemType.GetConstructor({})
        End Sub

        Private Sub ImplementContextConstructor()
            Dim constructorBuilder As ConstructorBuilder = CurrentTypeBuilder.DefineConstructor(
                            MethodAttributes.Public,
                            CallingConventions.Standard,
                            {})

            Dim ilBuilder As ILGenerator = constructorBuilder.GetILGenerator()

            'Call base constructor
            Dim dbContextConstructorAccessor As ConstructorInfo = GetType(ItemTypeGeneratedDbContextBase).GetConstructor({})
            ilBuilder.Emit(OpCodes.Ldarg_0)                             'Load self
            ilBuilder.Emit(OpCodes.Call, dbContextConstructorAccessor)  'Call

            ilBuilder.Emit(OpCodes.Ret)
        End Sub

        Private Sub SaveDbContextType()
            TypesToGenerate.DbContextType = CurrentTypeBuilder.CreateType()
            TypesToGenerate.ItemTypeSetAccessor = TypesToGenerate.DbContextType.GetProperty("Items")
        End Sub

        Private Sub DisableDatabaseInitialization()
            SetDatabaseInitializerAccessor.MakeGenericMethod({TypesToGenerate.DbContextType}).Invoke(Nothing, {Nothing})
        End Sub

        Private Function CreateContextInstance() As Object
            Return Activator.CreateInstance(TypesToGenerate.DbContextType)
        End Function

        Private Class GeneratedTypes
            Public Property ItemTypeName As String
            Public Property DbContextType As Type
            Public Property ItemType As Type
            Public Property ItemTypeConstructor As ConstructorInfo
            Public Property ItemTypeSetAccessor As PropertyInfo
        End Class
    End Class
End Namespace
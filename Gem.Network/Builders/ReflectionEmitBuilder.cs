using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Linq;

namespace Gem.Network.Builders
{
    /// <summary>
    /// Creates POCO types
    /// </summary>
    public sealed class ReflectionEmitBuilder : IPocoBuilder
    {
        /// <summary>
        /// Creates a new POCO type
        /// </summary>
        /// <param name="className">The POCO's name</param>
        /// <param name="propertyFields">The property names and types <see cref="PropertyInfo"/></param>
        /// <returns>The new type</returns>
        public Type Build(string className, List<DynamicPropertyInfo> propertyFields)
        {
            return CompileResultType(className, propertyFields);
        }


        #region Private Helper Methods

        private Type CompileResultType(string className, List<DynamicPropertyInfo> propertyFields)
        {
            TypeBuilder tb = GetTypeBuilder(className);
            ConstructorBuilder constructorDefault = tb.DefineDefaultConstructor(MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);
            ConstructorBuilder constructorParameters = tb.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, propertyFields.Select(x=>x.PropertyType).ToArray() );

            ILGenerator ctorIL = constructorParameters.GetILGenerator();
            Type objType = Type.GetType("System.Object");
            ConstructorInfo objCtor = objType.GetConstructor(new Type[0]);
            ctorIL.Emit(OpCodes.Ldarg_0);
            ctorIL.Emit(OpCodes.Call, objCtor);

            int constructorIndexCount = 0;
            foreach (var field in propertyFields)
            {
                var dynamicField = CreateProperty(tb, field.PropertyName, field.PropertyType);
                ctorIL.Emit(OpCodes.Ldarg_0);
                ctorIL.Emit(OpCodes.Ldarg_S, ++constructorIndexCount);
                ctorIL.Emit(OpCodes.Stfld, dynamicField);
            }

            ctorIL.Emit(OpCodes.Ret);

            Type objectType = tb.CreateType();
            return objectType;
        }

        private TypeBuilder GetTypeBuilder(string className)
        {
            var typeSignature = className;
            var an = new AssemblyName(typeSignature);
            AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(an, AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");
            TypeBuilder tb = moduleBuilder.DefineType(typeSignature
                                , TypeAttributes.Public |
                                TypeAttributes.Class |
                                TypeAttributes.AutoClass |
                                TypeAttributes.AnsiClass |
                                TypeAttributes.BeforeFieldInit |
                                TypeAttributes.AutoLayout
                                , null);
            return tb;
        }

        private FieldBuilder CreateProperty(TypeBuilder tb, string propertyName, Type propertyType)
        {
            FieldBuilder fieldBuilder = tb.DefineField("_" + propertyName, propertyType, FieldAttributes.Private);

            PropertyBuilder propertyBuilder = tb.DefineProperty(propertyName, PropertyAttributes.HasDefault, propertyType, null);
            MethodBuilder getPropMthdBldr = tb.DefineMethod("get_" + propertyName, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, propertyType, Type.EmptyTypes);
            ILGenerator getIl = getPropMthdBldr.GetILGenerator();

            getIl.Emit(OpCodes.Ldarg_0);
            getIl.Emit(OpCodes.Ldfld, fieldBuilder);
            getIl.Emit(OpCodes.Ret);

            MethodBuilder setPropMthdBldr =
                tb.DefineMethod("set_" + propertyName,
                  MethodAttributes.Public |
                  MethodAttributes.SpecialName |
                  MethodAttributes.HideBySig,
                  null, new[] { propertyType });

            ILGenerator setIl = setPropMthdBldr.GetILGenerator();
            Label modifyProperty = setIl.DefineLabel();
            Label exitSet = setIl.DefineLabel();

            setIl.MarkLabel(modifyProperty);
            setIl.Emit(OpCodes.Ldarg_0);
            setIl.Emit(OpCodes.Ldarg_1);
            setIl.Emit(OpCodes.Stfld, fieldBuilder);

            setIl.Emit(OpCodes.Nop);
            setIl.MarkLabel(exitSet);
            setIl.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getPropMthdBldr);
            propertyBuilder.SetSetMethod(setPropMthdBldr);

            return fieldBuilder;
        }

        #endregion
    }
}

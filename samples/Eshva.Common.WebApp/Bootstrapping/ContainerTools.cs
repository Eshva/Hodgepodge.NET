#region Usings

using System;
using System.Collections.Generic;
using System.Reflection;
using SimpleInjector;

#endregion


namespace Eshva.Common.WebApp.Bootstrapping
{
    public static class ContainerTools
    {
        public static void RegisterTypesInAssemblies(Container container, Type typeToRegister, IEnumerable<Assembly> allAssemblies)
        {
            var serviceTypes = container.GetTypesToRegister(
                typeToRegister,
                allAssemblies,
                new TypesToRegisterOptions
                {
                    IncludeGenericTypeDefinitions = true,
                    IncludeComposites = false
                });
            container.Collection.Register(typeToRegister, serviceTypes);
        }
    }
}

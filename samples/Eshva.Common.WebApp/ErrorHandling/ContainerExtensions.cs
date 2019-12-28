#region Usings

using System.Collections.Generic;
using System.Reflection;
using FluentValidation;
using SimpleInjector;

#endregion


namespace Eshva.Common.WebApp.ErrorHandling
{
    public static class ContainerExtensions
    {
        public static void AddFluentValidation(this Container container, IEnumerable<Assembly> assemblies)
        {
            container.Collection.Register(typeof(IValidator<>), assemblies);
        }
    }
}

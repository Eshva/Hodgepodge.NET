#region Usings

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Eshva.Common.WebApp.Bootstrapping;
using MediatR;
using MediatR.Pipeline;
using SimpleInjector;

#endregion


namespace Eshva.Common.WebApp.MediatR
{
    public static class ContainerExtensions
    {
        /// <remarks>
        /// Взято отсюда: https://github.com/haison8x/MediatR.SimpleInjector/blob/master/src/MediatR.SimpleInjector/ContainerExtension.cs
        /// Пришлось доделывать, так как не регистрировались IPipelineBehavior.
        /// </remarks>
        public static void AddMediatR(this Container container, IEnumerable<Assembly> assemblies)
        {
            var allAssemblies = assemblies.Union(new[] { typeof(IMediator).Assembly }).ToArray();

            container.Register<IMediator, Mediator>();
            container.Register(typeof(IRequestHandler<,>), allAssemblies);

            ContainerTools.RegisterTypesInAssemblies(container, typeof(INotificationHandler<>), allAssemblies);
            ContainerTools.RegisterTypesInAssemblies(container, typeof(IPipelineBehavior<,>), allAssemblies);
            ContainerTools.RegisterTypesInAssemblies(container, typeof(IRequestPreProcessor<>), allAssemblies);
            ContainerTools.RegisterTypesInAssemblies(container, typeof(IRequestPostProcessor<,>), allAssemblies);
            ContainerTools.RegisterTypesInAssemblies(container, typeof(IRequestExceptionHandler<,,>), allAssemblies);
            ContainerTools.RegisterTypesInAssemblies(container, typeof(IRequestExceptionAction<,>), allAssemblies);

            container.Register(() => new ServiceFactory(container.GetInstance), Lifestyle.Singleton);
        }
    }
}

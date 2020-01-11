#region Usings

using System;
using Eshva.Common.WebApp.ApplicationConfiguration;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using IConfiguration = Eshva.Common.WebApp.ApplicationConfiguration.IConfiguration;

#endregion


namespace Eshva.Common.WebApp.WebService
{
    public abstract class WebServiceConfigurationBase : IWebServiceConfiguration, IConfiguration
    {
        public Uri BaseUri { get; private set; }

        void IConfiguration.LoadFrom(IConfigurationSection configurationSection)
        {
            var baseUriString = configurationSection.GetValue("BaseUri", string.Empty);
            BaseUri = Uri.TryCreate(baseUriString, UriKind.Absolute, out var baseUri) ? baseUri : null;
        }

        public virtual void Validate()
        {
            var result = new Validator().Validate(this);
            if (!result.IsValid)
            {
                throw new ApplicationConfigurationException(
                    $"In a web-service configuration have been found the following errors:{Environment.NewLine}{result}");
            }
        }

        private sealed class Validator : AbstractValidator<WebServiceConfigurationBase>
        {
            public Validator()
            {
                RuleFor(configuration => configuration.BaseUri)
                    .NotNull().WithMessage("BaseUri isn't specified or has a wrong format.")
                    .ChildRules(
                        rules => rules.RuleFor(uri => uri.IsAbsoluteUri)
                                      .Must(isAbsolute => isAbsolute)
                                      .WithMessage("BaseUri should be an absolute path to the web-service."));
            }
        }
    }
}

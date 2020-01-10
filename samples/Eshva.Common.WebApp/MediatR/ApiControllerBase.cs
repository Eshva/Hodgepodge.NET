#region Usings

using MediatR;
using Microsoft.AspNetCore.Mvc;

#endregion


namespace Eshva.Common.WebApp.MediatR
{
    [ApiController]
    public abstract class ApiControllerBase : ControllerBase
    {
        protected ApiControllerBase(IMediator mediator)
        {
            Mediator = mediator;
        }

        protected IMediator Mediator { get; }
    }
}

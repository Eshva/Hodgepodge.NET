#region Usings

using System.Threading.Tasks;
using Eshva.Common.WebApp.MediatR;
using Eshva.Polls.Admin.Application.SetClientConfiguration;
using MediatR;
using Microsoft.AspNetCore.Mvc;

#endregion


namespace Eshva.Polls.Admin.WebApp.Resources.ClientConfiguration
{
    public sealed class ClientConfigurationController : ApiControllerBase
    {
        public ClientConfigurationController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost(RoutingConstants.ClientConfigurationPath)]
        public async Task<IActionResult> Set([FromBody] SetClientConfigurationRequest request)
        {
            await Mediator.Send(request);
            return Ok();
        }
    }
}

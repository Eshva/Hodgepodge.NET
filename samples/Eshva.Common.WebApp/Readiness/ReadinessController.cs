#region Usings

using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

#endregion


namespace Eshva.Common.WebApp.Readiness
{
    public sealed class ReadinessController : ControllerBase
    {
        public ReadinessController(IReadinessTester tester = null)
        {
            _tester = tester ?? new DefaultReadinessTester();
            _cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(1));
        }

        [HttpGet("/readiness")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var isReady = await _tester.IsReady(_cancellationTokenSource.Token);
                return isReady ? Ok() : StatusCode((int)HttpStatusCode.ServiceUnavailable);
            }
            catch (TaskCanceledException)
            {
                return StatusCode((int)HttpStatusCode.ServiceUnavailable);
            }
        }

        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly IReadinessTester _tester;
    }
}

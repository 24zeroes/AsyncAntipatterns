
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApplication.Controllers
{
    [ApiController]
    public class WorkController : ControllerBase
    {
        private readonly ILogger<WorkController> _logger;
        private readonly IWorkService _workService;

        public WorkController(ILogger<WorkController> logger, IWorkService workService)
        {
            _logger = logger;
            _workService = workService;
        }

        [HttpGet]
        [Route("[controller]/GetWorkResultBlocking")]
        public async Task<string> GetWorkResultBlocking(int clientId, CancellationToken cancellationToken)
        {
            if (clientId < 1)
            {
                throw new Exception();
            }
            
            Console.WriteLine($"Client {clientId} on blocking call gets thread {Thread.CurrentThread.ManagedThreadId} in controller");
            return await _workService.GetWorkResultBlocking(clientId, cancellationToken);
        }
        
        [HttpGet]
        [Route("[controller]/GetWorkResult")]
        public async Task<string> GetWorkResult(int clientId, CancellationToken cancellationToken)
        {
            if (clientId < 1)
            {
                throw new Exception();
            }
            
            Console.WriteLine($"Client {clientId} gets thread {Thread.CurrentThread.ManagedThreadId} in controller");
            return await _workService.GetWorkResult(clientId, cancellationToken);
        }
    }
}
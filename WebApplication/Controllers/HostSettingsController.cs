
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApplication.Controllers
{
    [ApiController]
    public class HostSettingsController : ControllerBase
    {
        private readonly ILogger<HostSettingsController> _logger;

        public HostSettingsController(ILogger<HostSettingsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("[controller]/GetProcessorCount")]
        public int GetProcessorCount()
        {
            return Environment.ProcessorCount;
        }
        
        [HttpGet]
        [Route("[controller]/GetMinThreadsCount")]
        public IActionResult GetMinimumWorkerThreadsCount()
        {
            ThreadPool.GetMinThreads(out var workerThreads, out var completionPortThreads);
            return Ok($"minimumWorkerThreads = {workerThreads} {Environment.NewLine}minimumCompletionPortThreads = {completionPortThreads}");
        }
        
        [HttpGet]
        [Route("[controller]/GetMaxThreadsCount")]
        public IActionResult GetMaxThreadsCount()
        {
            ThreadPool.GetMaxThreads(out var maximumWorkerThreads, out var maximumCompletionPortThreads);
            return Ok($"maximumWorkerThreads = {maximumWorkerThreads} {Environment.NewLine}maximumCompletionPortThreads = {maximumCompletionPortThreads}");
        }
        
        [HttpPost]
        [Route("[controller]/SetMaxThreadsCount")]
        public IActionResult SetMaxThreadsCount(int workerThreadsCount, int completionPortThreads)
        {
            ThreadPool.GetMinThreads(out var minimumWorkerThreads, out var minimumCompletionPortThreads);
            if (workerThreadsCount < minimumWorkerThreads)
            {
                return BadRequest($"workerThreadsCount should be more than {minimumWorkerThreads}");
            }
            
            if (completionPortThreads < minimumCompletionPortThreads)
            {
                return BadRequest($"completionPortThreads should be more than {minimumCompletionPortThreads}");
            }

            ThreadPool.SetMaxThreads(workerThreadsCount, completionPortThreads);
            return Ok();
        }
    }
}
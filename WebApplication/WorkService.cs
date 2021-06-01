using System;
using System.Threading;
using System.Threading.Tasks;

namespace WebApplication
{
    public interface IWorkService
    {
        Task<string> GetWorkResultBlocking(int clientId, CancellationToken cancellationToken);
        Task<string> GetWorkResult(int clientId, CancellationToken cancellationToken);
    }
    
    public class WorkService : IWorkService
    {
        public async Task<string> GetWorkResultBlocking(int clientId, CancellationToken cancellationToken)
        {
            return DoWorkWithBlocking(clientId);
        }

        public Task<string> GetWorkResult(int clientId, CancellationToken cancellationToken)
        {
            return DoWork(clientId, cancellationToken);
        }

        private async Task<string> DoWork(int clientId, CancellationToken cancellationToken)
        {
            return await BlockingOperation(clientId, cancellationToken);
        }
        
        private string DoWorkWithBlocking(int clientId)
        {
            return BlockingOperation(clientId).ConfigureAwait(false).GetAwaiter().GetResult();
        }


        private async Task<string> BlockingOperation(int clientId, CancellationToken cancellationToken = default)
        {
            await Task.Delay(1000);
            Console.WriteLine($"Client {clientId} Releasing on thread {Thread.CurrentThread.ManagedThreadId}");
            return $"WorkFor_{clientId}";
        }
    }
}
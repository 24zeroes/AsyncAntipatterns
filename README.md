# C# Async Antipatterns
This repo shows a real examples of bad approaches of working with parallel and asynroniuos operations in C#.


## ConfigureAwait(false).GetAwaiter().GetResult()
Example shows, how your app would work using `.ConfigureAwait(false).GetAwaiter().GetResult()` on blocking async methods, instead of just awaiting them.

We have blocking method, emulationg some work on thread 
 ```csharp
private async Task<string> BlockingOperation(int clientId, CancellationToken cancellationToken = default)
{
    await Task.Delay(1000);
    Console.WriteLine($"Client {clientId} Releasing on thread {Thread.CurrentThread.ManagedThreadId}");
    return $"WorkFor_{clientId}";
}
 ```
 
And two approaches of working with method, first is using await 
```csharp
private async Task<string> DoWork(int clientId, CancellationToken cancellationToken)
{
    return await BlockingOperation(clientId, cancellationToken);
}
 ```
Second is using ConfigureAwait(false).GetAwaiter().GetResult()
```csharp
private string DoWorkWithBlocking(int clientId)
{
    return BlockingOperation(clientId).ConfigureAwait(false).GetAwaiter().GetResult();
}
 ```
 
WebApp is configured to use minimum threads avalible on your system to minimazi efforts on getting deadlock or pool starvation problems
 ```csharp
ThreadPool.GetMinThreads(out var workerThreads, out var completionPortThreads);
ThreadPool.SetMaxThreads(workerThreads, completionPortThreads);
 ```
Lets run WebApp
 
 `    dotnet run --project .\WebApplication\WebApplication.csproj`
 
We have a simple console application as a clent for service. In client we will try to test both of methods above, sending requests in parallel.

Command to test **non blocking** method with 256 connections
`dotnet run --project .\Client\Client.csproj  -- nonblocking 256`

Command to test **blocking** method with 256 connections
`dotnet run --project .\Client\Client.csproj  -- blocking 256`

You can test this by yourself, using different threadpool settings and different number of connections

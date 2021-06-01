# C# Async Antipatterns
This repo shows a real examples of bad approaches of working with parallel and asynroniuos operations in C#.


## Example ConfigureAwait(false).GetAwaiter().GetResult()
Example shows, how your app would work using `.ConfigureAwait(false).GetAwaiter().GetResult()` on blocking async methods, instead of just awaiting them.

We have our blocking method, emulationg some work on thread 

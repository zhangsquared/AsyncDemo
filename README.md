# AsyncDemo


## Simple Demo

`AwaitDemo` project

* SimpleDemo: Simple `await` and `Task` examples. Compare three methods: 

1. sync
2. async + parallel
3. async + serial

### .NET WinForm and WPF use case

`WindowsFormsAsync` project

* After `await`, it will come back to UI thread.
Button 1, 2, 3 is either using non-blocking threads, or return to UI thread to update label;
Button 4 has a deadlock, as a third thread is trying to return to UI thread, while UI thread is waiting for the 2nd thread.

* `start` button implementation shows how to update UI thread from a worker thread

### Async, serial, parallel

`WindowsFormsAsync` project and `MyWeb` project
`MyWeb` project only provide a reponse of "hello world" after 1 second of delay.

Both `FileRead` and `WebRequest` have 3 implementations

1. Sync
2. Await: use default thread management
3. Task: force to create a new task

The performance will differ for Await method
* `FileReaderDemo`: Sync method = Await method > Task method
* `WebRequestDemo`: Sync method > Await Method = Task method

Q: Why for "file reading", await method barely has parallel threads, but for "web request", await method has lots of parallel threads?

A: [DMA](https://docs.microsoft.com/en-us/windows-hardware/drivers/kernel/dma-programming-techniques)

| Web request | `HttpClient`, `SyndicationClient` |
| File: `StorageFile`, `StreamWrite`, `StreamReader`, `XmlReader` |
| Media processing: `MediaCapture`, `BitmapEncoder`, `BitmapDecoder` |
|  | `Socket` | 

`await` can improve **throughput**, but not **performance**

### FailFast
How to fast return false if any of parallel running tasks failed, and cancel the remaining parallel tasks?
* Start(): wait every tasks finished and return a final result. not fail fast
* Start2() and Start2Async(): fast fail, but cannot cancel the remaining tasks
* Start3Async(): fast fail, and also cancel the remaining tasks (need to modify mock job function to pass in the cancellation token)

**Question**: how to cancel a non-cancellable task?
[Cancel asynchronous operations in C#](https://johnthiriet.com/cancel-asynchronous-operation-in-csharp/) 


## Runtime requirement

#### AwaitDemo, MyWeb, FailFast
.NET Core 3.1

#### WindowsFormsAsync
.NET Framework v4.7.2



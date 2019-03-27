# MonitorAsync
A simple C# example of waiting for async events in distributed systems

## Concept

If you have a distributed messaging system, and you want to do some rare RPC patterned calls, you probably end up with an incoming replies queue, and the ability to send RFC messages.

This is a quick sketch of how to handle the waiting-for-reply part using C# async/await and Tasks.


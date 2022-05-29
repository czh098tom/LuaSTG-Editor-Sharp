using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Threading;
using System.Windows;

namespace LuaSTGEditorSharp
{
    public class SimpleIPC
    {
        public abstract class IPCBase
        {
            public bool IsHost { get; init; }

            public IPCBase(bool isHost)
            {
                IsHost = isHost;
            }
        }

        public class Server : IPCBase
        {
            public event EventHandler<string> MessageReceived;

            public Server() : base(true)
            {
                WaitForNewConnection();
            }

            void WaitForNewConnection()
            {
                ThreadPool.QueueUserWorkItem(delegate
                {
                    var pipe = new NamedPipeServerStream("LuaSTG-Sharp-Editor", PipeDirection.InOut, 10, PipeTransmissionMode.Message, PipeOptions.Asynchronous);
                    pipe.BeginWaitForConnection(async (o) =>
                    {
                        NamedPipeServerStream server = (NamedPipeServerStream)o.AsyncState;
                        server.EndWaitForConnection(o);
                        WaitForNewConnection();
                        StreamReader reader = new StreamReader(server);
                        string message;
                        while (!string.IsNullOrEmpty(message = await reader.ReadLineAsync()))
                        {
                            MessageReceived?.Invoke(this, message);
                        }
                        server.Disconnect();
                        await server.DisposeAsync();
                    }, pipe);
                });
            }
        }

        public class Client : IPCBase
        {
            Queue<string> MessageList { get; init; } = new Queue<string>();

            public Client() : base(false)
            {
                new Thread(SendMessageThread)
                {
                    IsBackground = true
                }.Start();
            }

            public void SendMessage(string message)
            {
                MessageList.Enqueue(message);
            }

            async void SendMessageThread()
            {
                while (true)
                {
                    if (MessageList.Count > 0)
                    {
                        var pipe = new NamedPipeClientStream("LuaSTG-Sharp-Editor");
                        await pipe.ConnectAsync();
                        var writer = new StreamWriter(pipe)
                        {
                            AutoFlush = true
                        };
                        while (MessageList.Count > 0)
                        {
                            await writer.WriteLineAsync(MessageList.Dequeue());
                        }
                        await pipe.DisposeAsync();
                    }
                }
            }
        }
    }
}

#region Copyright notice and license

// Copyright 2015 gRPC authors.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

using System;
using System.IO;
using System.Linq;
using Grpc.Core;
using Grpc.Core.Internal;
using Grpc.Core.Utils;
using NUnit.Framework;

namespace Grpc.Core.Tests
{
    public class ServerTest
    {
        [Test]
        public void StartAndShutdownServer()
        {
            Server server = new Server
            {
                Ports = { new ServerPort("localhost", ServerPort.PickUnused, ServerCredentials.Insecure) }
            };
            server.Start();
            server.ShutdownAsync().Wait();
        }

        [Test]
        public void StartAndKillServer()
        {
            Server server = new Server
            {
                Ports = { new ServerPort("localhost", ServerPort.PickUnused, ServerCredentials.Insecure) }
            };
            server.Start();
            server.KillAsync().Wait();
        }

        [Test]
        public void PickUnusedPort()
        {
            Server server = new Server
            {
                Ports = { new ServerPort("localhost", ServerPort.PickUnused, ServerCredentials.Insecure) }
            };

            var boundPort = server.Ports.Single();
            Assert.AreEqual(0, boundPort.Port);
            Assert.Greater(boundPort.BoundPort, 0);

            server.Start();
            server.ShutdownAsync().Wait();
        }

        [Test]
        public void StartThrowsWithUnboundPorts()
        {
            int twiceBoundPort = 9999;
            Server server = new Server(new[] { new ChannelOption(ChannelOptions.SoReuseport, 0) })
            {
                Ports = {
                    new ServerPort("localhost", twiceBoundPort, ServerCredentials.Insecure),
                    new ServerPort("localhost", twiceBoundPort, ServerCredentials.Insecure)
                }
            };
            Assert.Throws(typeof(IOException), () => server.Start());
            server.ShutdownAsync().Wait();
        }

        [Test]
        public void CannotModifyAfterStarted()
        {
            Server server = new Server
            {
                Ports = { new ServerPort("localhost", ServerPort.PickUnused, ServerCredentials.Insecure) }
            };
            server.Start();
            Assert.Throws(typeof(InvalidOperationException), () => server.Ports.Add("localhost", 9999, ServerCredentials.Insecure));
            Assert.Throws(typeof(InvalidOperationException), () => server.Services.Add(ServerServiceDefinition.CreateBuilder().Build()));

            server.ShutdownAsync().Wait();
        }

        [Test]
        public void UnstartedServerCanBeShutdown()
        {
            var server = new Server();
            server.ShutdownAsync().Wait();
            Assert.Throws(typeof(InvalidOperationException), () => server.Start());
        }

        [Test]
        public void UnstartedServerDoesNotPreventShutdown()
        {
            // just create a server, don't start it, and make sure it doesn't prevent shutdown.
            var server = new Server();
        }

        [Test]
        public void BindPortsNoStartAndShutdownServer()
        {
            // Test binding ports but no Start - that the bound ports are
            // released on Shutdown.
            // See https://github.com/grpc/grpc/issues/23390

            int boundPort = 0;

            try
            {
                // Create server, bind ports then shutdown (without start)
                Server server = new Server {
                    Ports = { new ServerPort("localhost", ServerPort.PickUnused, ServerCredentials.Insecure) }
                };

                // find the port chosen so we can use it later
                boundPort = server.Ports.Single().BoundPort;
                Assert.Greater(boundPort, 0);

                server.ShutdownAsync().Wait();
            }
            catch (Exception e)
            {
                Assert.Fail($"Failed to create and shutdown first server: {e}");
            }

            try
            {
                // Create new server, bind to same port as before, start and shutdown

                // Note: there is a small possibility that the port may have been reused by a different
                // test being run in parallel in between the previous Shutdown and using it here
                // causing this test to be flaky.

                Server server2 = new Server {
                    Ports = { new ServerPort("localhost", boundPort, ServerCredentials.Insecure) }
                };

                server2.Start();
                server2.ShutdownAsync().Wait();
            }
            catch (Exception e)
            {
                Assert.Fail($"Failed to create and shutdown second server: {e}");
            }

        }

    }
}

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
using System.Collections.Generic;
using System.Threading.Tasks;

using Grpc.Core.Internal;
using Grpc.Core.Utils;
using NUnit.Framework;

namespace Grpc.Core.Internal.Tests
{
    /// <summary>
    /// Uses fake native call to test interaction of <c>AsyncCall</c> wrapping code with C core in different situations.
    /// </summary>
    public class AsyncCallTest
    {
        Channel channel;
        FakeNativeCall fakeCall;
        AsyncCall<string, string> asyncCall;
        FakeBufferReaderManager fakeBufferReaderManager;

        [SetUp]
        public void Init()
        {
            channel = new Channel("localhost", ChannelCredentials.Insecure);

            fakeCall = new FakeNativeCall();

            var callDetails = new CallInvocationDetails<string, string>(channel, "someMethod", null, Marshallers.StringMarshaller, Marshallers.StringMarshaller, new CallOptions());
            asyncCall = new AsyncCall<string, string>(callDetails, fakeCall);
            fakeBufferReaderManager = new FakeBufferReaderManager();
        }

        [TearDown]
        public void Cleanup()
        {
            channel.ShutdownAsync().Wait();
            fakeBufferReaderManager.Dispose();
        }

        [Test]
        public void AsyncUnary_CanBeStartedOnlyOnce()
        {
            asyncCall.UnaryCallAsync("request1");
            Assert.Throws(typeof(InvalidOperationException),
                () => asyncCall.UnaryCallAsync("abc"));
        }

        [Test]
        public void AsyncUnary_StreamingOperationsNotAllowed()
        {
            asyncCall.UnaryCallAsync("request1");
            Assert.ThrowsAsync(typeof(InvalidOperationException),
                async () => await asyncCall.ReadMessageAsync());
            Assert.Throws(typeof(InvalidOperationException),
                () => asyncCall.SendMessageAsync("abc", new WriteFlags()));
        }

        [Test]
        public void AsyncUnary_Success()
        {
            var resultTask = asyncCall.UnaryCallAsync("request1");
            fakeCall.UnaryResponseClientCallback.OnUnaryResponseClient(true,
                new ClientSideStatus(Status.DefaultSuccess, new Metadata()),
                CreateResponsePayload(),
                new Metadata());

            AssertUnaryResponseSuccess(asyncCall, fakeCall, resultTask);
        }

        [Test]
        public void AsyncUnary_NonSuccessStatusCode()
        {
            var resultTask = asyncCall.UnaryCallAsync("request1");
            fakeCall.UnaryResponseClientCallback.OnUnaryResponseClient(true,
                CreateClientSideStatus(StatusCode.InvalidArgument),
                CreateNullResponse(),
                new Metadata());

            AssertUnaryResponseError(asyncCall, fakeCall, resultTask, StatusCode.InvalidArgument);
        }

        [Test]
        public void AsyncUnary_NullResponsePayload()
        {
            var resultTask = asyncCall.UnaryCallAsync("request1");
            fakeCall.UnaryResponseClientCallback.OnUnaryResponseClient(true,
                new ClientSideStatus(Status.DefaultSuccess, new Metadata()),
                null,
                new Metadata());

            // failure to deserialize will result in InvalidArgument status.
            AssertUnaryResponseError(asyncCall, fakeCall, resultTask, StatusCode.Internal);
        }

        [Test]
        public void AsyncUnary_RequestSerializationExceptionDoesntLeakResources()
        {
            string nullRequest = null;  // will throw when serializing
            Assert.Throws(typeof(ArgumentNullException), () => asyncCall.UnaryCallAsync(nullRequest));
            Assert.AreEqual(0, channel.GetCallReferenceCount());
            Assert.IsTrue(fakeCall.IsDisposed);
        }

        [Test]
        public void AsyncUnary_StartCallFailureDoesntLeakResources()
        {
            fakeCall.MakeStartCallFail();
            Assert.Throws(typeof(InvalidOperationException), () => asyncCall.UnaryCallAsync("request1"));
            Assert.AreEqual(0, channel.GetCallReferenceCount());
            Assert.IsTrue(fakeCall.IsDisposed);
        }

        [Test]
        public void SyncUnary_RequestSerializationExceptionDoesntLeakResources()
        {
            string nullRequest = null;  // will throw when serializing
            Assert.Throws(typeof(ArgumentNullException), () => asyncCall.UnaryCall(nullRequest));
            Assert.AreEqual(0, channel.GetCallReferenceCount());
            Assert.IsTrue(fakeCall.IsDisposed);
        }

        [Test]
        public void SyncUnary_StartCallFailureDoesntLeakResources()
        {
            fakeCall.MakeStartCallFail();
            Assert.Throws(typeof(InvalidOperationException), () => asyncCall.UnaryCall("request1"));
            Assert.AreEqual(0, channel.GetCallReferenceCount());
            Assert.IsTrue(fakeCall.IsDisposed);
        }

        [Test]
        public void ClientStreaming_StreamingReadNotAllowed()
        {
            asyncCall.ClientStreamingCallAsync();
            Assert.ThrowsAsync(typeof(InvalidOperationException),
                async () => await asyncCall.ReadMessageAsync());
        }

        [Test]
        public void ClientStreaming_NoRequest_Success()
        {
            var resultTask = asyncCall.ClientStreamingCallAsync();
            fakeCall.UnaryResponseClientCallback.OnUnaryResponseClient(true,
                new ClientSideStatus(Status.DefaultSuccess, new Metadata()),
                CreateResponsePayload(),
                new Metadata());

            AssertUnaryResponseSuccess(asyncCall, fakeCall, resultTask);
        }

        [Test]
        public void ClientStreaming_NoRequest_NonSuccessStatusCode()
        {
            var resultTask = asyncCall.ClientStreamingCallAsync();
            fakeCall.UnaryResponseClientCallback.OnUnaryResponseClient(true,
                CreateClientSideStatus(StatusCode.InvalidArgument),
                CreateNullResponse(),
                new Metadata());

            AssertUnaryResponseError(asyncCall, fakeCall, resultTask, StatusCode.InvalidArgument);
        }

        [Test]
        public void ClientStreaming_MoreRequests_Success()
        {
            var resultTask = asyncCall.ClientStreamingCallAsync();
            var requestStream = new ClientRequestStream<string, string>(asyncCall);

            var writeTask = requestStream.WriteAsync("request1");
            fakeCall.SendCompletionCallback.OnSendCompletion(true);
            writeTask.Wait();

            var writeTask2 = requestStream.WriteAsync("request2");
            fakeCall.SendCompletionCallback.OnSendCompletion(true);
            writeTask2.Wait();

            var completeTask = requestStream.CompleteAsync();
            fakeCall.SendCompletionCallback.OnSendCompletion(true);
            completeTask.Wait();

            fakeCall.UnaryResponseClientCallback.OnUnaryResponseClient(true,
                new ClientSideStatus(Status.DefaultSuccess, new Metadata()),
                CreateResponsePayload(),
                new Metadata());

            AssertUnaryResponseSuccess(asyncCall, fakeCall, resultTask);
        }

        [Test]
        public void ClientStreaming_WriteFailureThrowsRpcException()
        {
            var resultTask = asyncCall.ClientStreamingCallAsync();
            var requestStream = new ClientRequestStream<string, string>(asyncCall);

            var writeTask = requestStream.WriteAsync("request1");
            fakeCall.SendCompletionCallback.OnSendCompletion(false);

            // The write will wait for call to finish to receive the status code.
            Assert.IsFalse(writeTask.IsCompleted);

            fakeCall.UnaryResponseClientCallback.OnUnaryResponseClient(true,
                CreateClientSideStatus(StatusCode.Internal),
                CreateNullResponse(),
                new Metadata());

            var ex = Assert.ThrowsAsync<RpcException>(async () => await writeTask);
            Assert.AreEqual(StatusCode.Internal, ex.Status.StatusCode);

            AssertUnaryResponseError(asyncCall, fakeCall, resultTask, StatusCode.Internal);
        }

        [Test]
        public void ClientStreaming_WriteFailureThrowsRpcException2()
        {
            var resultTask = asyncCall.ClientStreamingCallAsync();
            var requestStream = new ClientRequestStream<string, string>(asyncCall);

            var writeTask = requestStream.WriteAsync("request1");

            fakeCall.UnaryResponseClientCallback.OnUnaryResponseClient(true,
                CreateClientSideStatus(StatusCode.Internal),
                CreateNullResponse(),
                new Metadata());

            fakeCall.SendCompletionCallback.OnSendCompletion(false);

            var ex = Assert.ThrowsAsync<RpcException>(async () => await writeTask);
            Assert.AreEqual(StatusCode.Internal, ex.Status.StatusCode);

            AssertUnaryResponseError(asyncCall, fakeCall, resultTask, StatusCode.Internal);
        }

        [Test]
        public void ClientStreaming_WriteFailureThrowsRpcException3()
        {
            var resultTask = asyncCall.ClientStreamingCallAsync();
            var requestStream = new ClientRequestStream<string, string>(asyncCall);

            var writeTask = requestStream.WriteAsync("request1");
            fakeCall.SendCompletionCallback.OnSendCompletion(false);

            // Until the delayed write completion has been triggered,
            // we still act as if there was an active write.
            Assert.Throws(typeof(InvalidOperationException), () => requestStream.WriteAsync("request2"));

            fakeCall.UnaryResponseClientCallback.OnUnaryResponseClient(true,
                CreateClientSideStatus(StatusCode.Internal),
                CreateNullResponse(),
                new Metadata());

            var ex = Assert.ThrowsAsync<RpcException>(async () => await writeTask);
            Assert.AreEqual(StatusCode.Internal, ex.Status.StatusCode);

            // Following attempts to write keep delivering the same status
            var ex2 = Assert.ThrowsAsync<RpcException>(async () => await requestStream.WriteAsync("after call has finished"));
            Assert.AreEqual(StatusCode.Internal, ex2.Status.StatusCode);

            AssertUnaryResponseError(asyncCall, fakeCall, resultTask, StatusCode.Internal);
        }

        [Test]
        public void ClientStreaming_WriteAfterReceivingStatusThrowsRpcException()
        {
            var resultTask = asyncCall.ClientStreamingCallAsync();
            var requestStream = new ClientRequestStream<string, string>(asyncCall);

            fakeCall.UnaryResponseClientCallback.OnUnaryResponseClient(true,
                new ClientSideStatus(Status.DefaultSuccess, new Metadata()),
                CreateResponsePayload(),
                new Metadata());

            AssertUnaryResponseSuccess(asyncCall, fakeCall, resultTask);

            var writeTask = requestStream.WriteAsync("request1");
            var ex = Assert.ThrowsAsync<RpcException>(async () => await writeTask);
            Assert.AreEqual(Status.DefaultSuccess, ex.Status);
        }

        [Test]
        public void ClientStreaming_WriteAfterReceivingStatusThrowsRpcException2()
        {
            var resultTask = asyncCall.ClientStreamingCallAsync();
            var requestStream = new ClientRequestStream<string, string>(asyncCall);

            fakeCall.UnaryResponseClientCallback.OnUnaryResponseClient(true,
                new ClientSideStatus(new Status(StatusCode.OutOfRange, ""), new Metadata()),
                CreateResponsePayload(),
                new Metadata());

            AssertUnaryResponseError(asyncCall, fakeCall, resultTask, StatusCode.OutOfRange);

            var writeTask = requestStream.WriteAsync("request1");
            var ex = Assert.ThrowsAsync<RpcException>(async () => await writeTask);
            Assert.AreEqual(StatusCode.OutOfRange, ex.Status.StatusCode);
        }

        [Test]
        public void ClientStreaming_WriteAfterCompleteThrowsInvalidOperationException()
        {
            var resultTask = asyncCall.ClientStreamingCallAsync();
            var requestStream = new ClientRequestStream<string, string>(asyncCall);

            requestStream.CompleteAsync();

            Assert.Throws(typeof(InvalidOperationException), () => requestStream.WriteAsync("request1"));

            fakeCall.SendCompletionCallback.OnSendCompletion(true);

            fakeCall.UnaryResponseClientCallback.OnUnaryResponseClient(true,
                new ClientSideStatus(Status.DefaultSuccess, new Metadata()),
                CreateResponsePayload(),
                new Metadata());

            AssertUnaryResponseSuccess(asyncCall, fakeCall, resultTask);
        }

        [Test]
        public void ClientStreaming_CompleteAfterReceivingStatusSucceeds()
        {
            var resultTask = asyncCall.ClientStreamingCallAsync();
            var requestStream = new ClientRequestStream<string, string>(asyncCall);

            fakeCall.UnaryResponseClientCallback.OnUnaryResponseClient(true,
                new ClientSideStatus(Status.DefaultSuccess, new Metadata()),
                CreateResponsePayload(),
                new Metadata());

            AssertUnaryResponseSuccess(asyncCall, fakeCall, resultTask);
            Assert.DoesNotThrowAsync(async () => await requestStream.CompleteAsync());
        }

        [Test]
        public void ClientStreaming_WriteAfterCancellationRequestThrowsTaskCanceledException()
        {
            var resultTask = asyncCall.ClientStreamingCallAsync();
            var requestStream = new ClientRequestStream<string, string>(asyncCall);

            asyncCall.Cancel();
            Assert.IsTrue(fakeCall.IsCancelled);

            var writeTask = requestStream.WriteAsync("request1");
            Assert.ThrowsAsync(typeof(TaskCanceledException), async () => await writeTask);

            fakeCall.UnaryResponseClientCallback.OnUnaryResponseClient(true,
                CreateClientSideStatus(StatusCode.Cancelled),
                CreateNullResponse(),
                new Metadata());

            AssertUnaryResponseError(asyncCall, fakeCall, resultTask, StatusCode.Cancelled);
        }

        [Test]
        public void ClientStreaming_StartCallFailureDoesntLeakResources()
        {
            fakeCall.MakeStartCallFail();
            Assert.Throws(typeof(InvalidOperationException), () => asyncCall.ClientStreamingCallAsync());
            Assert.AreEqual(0, channel.GetCallReferenceCount());
            Assert.IsTrue(fakeCall.IsDisposed);
        }

        [Test]
        public void ServerStreaming_StreamingSendNotAllowed()
        {
            asyncCall.StartServerStreamingCall("request1");
            Assert.Throws(typeof(InvalidOperationException),
                () => asyncCall.SendMessageAsync("abc", new WriteFlags()));
        }

        [Test]
        public void ServerStreaming_NoResponse_Success1()
        {
            asyncCall.StartServerStreamingCall("request1");
            var responseStream = new ClientResponseStream<string, string>(asyncCall);
            var readTask = responseStream.MoveNext();

            fakeCall.ReceivedResponseHeadersCallback.OnReceivedResponseHeaders(true, new Metadata());
            Assert.AreEqual(0, asyncCall.ResponseHeadersAsync.Result.Count);

            fakeCall.ReceivedMessageCallback.OnReceivedMessage(true, CreateNullResponse());
            fakeCall.ReceivedStatusOnClientCallback.OnReceivedStatusOnClient(true, new ClientSideStatus(Status.DefaultSuccess, new Metadata()));

            AssertStreamingResponseSuccess(asyncCall, fakeCall, readTask);
        }

        [Test]
        public void ServerStreaming_NoResponse_Success2()
        {
            asyncCall.StartServerStreamingCall("request1");
            var responseStream = new ClientResponseStream<string, string>(asyncCall);
            var readTask = responseStream.MoveNext();

            // try alternative order of completions
            fakeCall.ReceivedStatusOnClientCallback.OnReceivedStatusOnClient(true, new ClientSideStatus(Status.DefaultSuccess, new Metadata()));
            fakeCall.ReceivedMessageCallback.OnReceivedMessage(true, CreateNullResponse());
            fakeCall.ReceivedResponseHeadersCallback.OnReceivedResponseHeaders(true, new Metadata());

            AssertStreamingResponseSuccess(asyncCall, fakeCall, readTask);
        }

        [Test]
        public void ServerStreaming_NoResponse_Success3()
        {
            asyncCall.StartServerStreamingCall("request1");
            var responseStream = new ClientResponseStream<string, string>(asyncCall);
            var readTask = responseStream.MoveNext();

            // try alternative order of completions
            fakeCall.ReceivedStatusOnClientCallback.OnReceivedStatusOnClient(true, new ClientSideStatus(Status.DefaultSuccess, new Metadata()));
            fakeCall.ReceivedResponseHeadersCallback.OnReceivedResponseHeaders(true, new Metadata());
            fakeCall.ReceivedMessageCallback.OnReceivedMessage(true, CreateNullResponse());

            AssertStreamingResponseSuccess(asyncCall, fakeCall, readTask);
        }

        [Test]
        public void ServerStreaming_NoResponse_ReadFailure()
        {
            asyncCall.StartServerStreamingCall("request1");
            var responseStream = new ClientResponseStream<string, string>(asyncCall);
            var readTask = responseStream.MoveNext();

            fakeCall.ReceivedResponseHeadersCallback.OnReceivedResponseHeaders(true, new Metadata());
            Assert.AreEqual(0, asyncCall.ResponseHeadersAsync.Result.Count);

            fakeCall.ReceivedMessageCallback.OnReceivedMessage(false, CreateNullResponse());  // after a failed read, we rely on C core to deliver appropriate status code.
            fakeCall.ReceivedStatusOnClientCallback.OnReceivedStatusOnClient(true, CreateClientSideStatus(StatusCode.Internal));

            AssertStreamingResponseError(asyncCall, fakeCall, readTask, StatusCode.Internal);
        }

        [Test]
        public void ServerStreaming_MoreResponses_Success()
        {
            asyncCall.StartServerStreamingCall("request1");
            var responseStream = new ClientResponseStream<string, string>(asyncCall);

            fakeCall.ReceivedResponseHeadersCallback.OnReceivedResponseHeaders(true, new Metadata());
            Assert.AreEqual(0, asyncCall.ResponseHeadersAsync.Result.Count);

            var readTask1 = responseStream.MoveNext();
            fakeCall.ReceivedMessageCallback.OnReceivedMessage(true, CreateResponsePayload());
            Assert.IsTrue(readTask1.Result);
            Assert.AreEqual("response1", responseStream.Current);

            var readTask2 = responseStream.MoveNext();
            fakeCall.ReceivedMessageCallback.OnReceivedMessage(true, CreateResponsePayload());
            Assert.IsTrue(readTask2.Result);
            Assert.AreEqual("response1", responseStream.Current);

            var readTask3 = responseStream.MoveNext();
            fakeCall.ReceivedStatusOnClientCallback.OnReceivedStatusOnClient(true, new ClientSideStatus(Status.DefaultSuccess, new Metadata()));
            fakeCall.ReceivedMessageCallback.OnReceivedMessage(true, CreateNullResponse());

            AssertStreamingResponseSuccess(asyncCall, fakeCall, readTask3);
        }

        [Test]
        public void ServerStreaming_CancelCall_Issue8451_1()
        {
            // Test client cancelling the call before the reading the server stream to the end.
            // Test for resource leak https://github.com/grpc/grpc/issues/8451

            // The callbacks OnReceivedResponseHeaders, OnReceivedStatusOnClient and OnReceivedMessage may
            // occur in any order together with calls to MoveNext. We need to test that the resources
            // are released correctly no matter the order of the calls, and that when the call is cancelled
            // or there is an error (such as server unavailable) that MoveNext throws the correct exception.

            // This test is testing the order:
            // OnReceivedResponseHeaders
            // MoveNext
            // OnReceivedMessage
            // Cancel (which could be because of a Dispose or explicit cancel)
            // OnReceivedStatusOnClient

            asyncCall.StartServerStreamingCall("request1");
            var responseStream = new ClientResponseStream<string, string>(asyncCall);

            // Check that the client has a reference to exactly one call
            Assert.AreEqual(1, channel.GetCallReferenceCount());
            // Check that the call is not yet disposed
            Assert.IsFalse(fakeCall.IsDisposed);

            fakeCall.ReceivedResponseHeadersCallback.OnReceivedResponseHeaders(true, new Metadata());
            Assert.AreEqual(0, asyncCall.ResponseHeadersAsync.Result.Count);

            // read some data
            var readTask1 = responseStream.MoveNext();
            fakeCall.ReceivedMessageCallback.OnReceivedMessage(true, CreateResponsePayload());
            Assert.IsTrue(readTask1.Result);
            Assert.AreEqual("response1", responseStream.Current);

            // Cancel the call. This will trigger ReceivedStatusOnClientCallback to be called with a Cancelled status.
            asyncCall.Cancel();
            fakeCall.ReceivedStatusOnClientCallback.OnReceivedStatusOnClient(true, new ClientSideStatus(Status.DefaultCancelled, new Metadata()));

            // Check that the resources have been released and that the client no longer references the call
            Assert.AreEqual(0, channel.GetCallReferenceCount());
            Assert.IsTrue(fakeCall.IsDisposed);
        }

        [Test]
        public void ServerStreaming_CancelCall_Issue8451_2()
        {
            // Test client cancelling the call before the reading the server stream to the end.
            // Test for resource leak https://github.com/grpc/grpc/issues/8451

            // The callbacks OnReceivedResponseHeaders, OnReceivedStatusOnClient and OnReceivedMessage may
            // occur in any order together with calls to MoveNext. We need to test that the resources
            // are released correctly no matter the order of the calls, and that when the call is cancelled
            // or there is an error (such as server unavailable) that MoveNext throws the correct exception.

            // This test is testing the order:
            // Cancel (which could be because of a Dispose or explicit cancel)
            // OnReceivedStatusOnClient
            // OnReceivedResponseHeaders
            // MoveNext - fails with Exception

            asyncCall.StartServerStreamingCall("request1");
            var responseStream = new ClientResponseStream<string, string>(asyncCall);

            // Check that the client has a reference to exactly one call
            Assert.AreEqual(1, channel.GetCallReferenceCount());
            // Check that the call is not yet disposed
            Assert.IsFalse(fakeCall.IsDisposed);

            // Cancel the call. This will trigger ReceivedStatusOnClientCallback to be called with a Cancelled status.
            asyncCall.Cancel();
            fakeCall.ReceivedStatusOnClientCallback.OnReceivedStatusOnClient(true, new ClientSideStatus(Status.DefaultCancelled, new Metadata()));

            // OnReceivedResponseHeaders with have success == false as call is cancelled
            fakeCall.ReceivedResponseHeadersCallback.OnReceivedResponseHeaders(false, new Metadata());
            Assert.AreEqual(0, asyncCall.ResponseHeadersAsync.Result.Count);

            // read some data - this should fail as the call has been cancelled
            var ex = Assert.ThrowsAsync<RpcException>(async () => await responseStream.MoveNext());
            Assert.AreEqual(Status.DefaultCancelled, ex.Status);

            // Check that the resources have been released and that the client no longer references the call
            Assert.AreEqual(0, channel.GetCallReferenceCount());
            Assert.IsTrue(fakeCall.IsDisposed);
        }

        [Test]
        public void ServerStreaming_CancelCall_Issue8451_3()
        {
            // Test client cancelling the call before the reading the server stream to the end.
            // Test for resource leak https://github.com/grpc/grpc/issues/8451

            // The callbacks OnReceivedResponseHeaders, OnReceivedStatusOnClient and OnReceivedMessage may
            // occur in any order together with calls to MoveNext. We need to test that the resources
            // are released correctly no matter the order of the calls, and that when the call is cancelled
            // or there is an error (such as server unavailable) that MoveNext throws the correct exception.

            // This test is testing the order:
            // OnReceivedResponseHeaders
            // Cancel (which could be because of a Dispose or explicit cancel)
            // MoveNext - OK as cancel status not yet picked up in callback
            // OnReceivedMessage
            // OnReceivedStatusOnClient - delayed callback from the Cancel
            // MoveNext - fails with Exception

            asyncCall.StartServerStreamingCall("request1");
            var responseStream = new ClientResponseStream<string, string>(asyncCall);

            // Check that the client has a reference to exactly one call
            Assert.AreEqual(1, channel.GetCallReferenceCount());
            // Check that the call is not yet disposed
            Assert.IsFalse(fakeCall.IsDisposed);

            fakeCall.ReceivedResponseHeadersCallback.OnReceivedResponseHeaders(true, new Metadata());
            Assert.AreEqual(0, asyncCall.ResponseHeadersAsync.Result.Count);

            // cancel the call, but delay the callback with the cancelled status until later
            asyncCall.Cancel();
            // Not yet disposed
            Assert.IsFalse(fakeCall.IsDisposed);

            // read some data
            var readTask1 = responseStream.MoveNext();
            fakeCall.ReceivedMessageCallback.OnReceivedMessage(true, CreateResponsePayload());
            Assert.IsTrue(readTask1.Result);
            Assert.AreEqual("response1", responseStream.Current);

            // Trigger ReceivedStatusOnClientCallback to be called with a Cancelled status.
            fakeCall.ReceivedStatusOnClientCallback.OnReceivedStatusOnClient(true, new ClientSideStatus(Status.DefaultCancelled, new Metadata()));

            // read some more data - this should fail as the call has been cancelled
            var ex = Assert.ThrowsAsync<RpcException>(async () => await responseStream.MoveNext());
            Assert.AreEqual(Status.DefaultCancelled, ex.Status);

            // Check that the resources have been released and that the client no longer references the call
            Assert.AreEqual(0, channel.GetCallReferenceCount());
            Assert.IsTrue(fakeCall.IsDisposed);
        }

        [Test]
        public void ServerStreaming_CancelCall_Issue8451_4()
        {
            // Test client cancelling the call before the reading the server stream to the end.
            // Test for resource leak https://github.com/grpc/grpc/issues/8451

            // The callbacks OnReceivedResponseHeaders, OnReceivedStatusOnClient and OnReceivedMessage may
            // occur in any order together with calls to MoveNext. We need to test that the resources
            // are released correctly no matter the order of the calls, and that when the call is cancelled
            // or there is an error (such as server unavailable) that MoveNext throws the correct exception.

            // This case tests the case when OnReceivedStatusOnClient occurs before the OnReceivedMessage 
            // for data already inflight.

            // This test is testing the order:
            // OnReceivedResponseHeaders
            // MoveNext task started
            // Cancel
            // OnReceivedStatusOnClient
            // OnReceivedMessage
            // MoveNext task completes OK
            // another MoveNext - fails with Exception

            asyncCall.StartServerStreamingCall("request1");
            var responseStream = new ClientResponseStream<string, string>(asyncCall);

            // Check that the client has a reference to exactly one call
            Assert.AreEqual(1, channel.GetCallReferenceCount());
            // Check that the call is not yet disposed
            Assert.IsFalse(fakeCall.IsDisposed);

            fakeCall.ReceivedResponseHeadersCallback.OnReceivedResponseHeaders(true, new Metadata());
            Assert.AreEqual(0, asyncCall.ResponseHeadersAsync.Result.Count);

            // Prime for a read - this sets the OnReceivedMessage callback handler in fakeCall
            // indicating that we are waiting to receive some data
            var readTask1 = responseStream.MoveNext();

            // Cancel the call
            asyncCall.Cancel();
            // Not yet disposed
            Assert.IsFalse(fakeCall.IsDisposed);

            // Trigger ReceivedStatusOnClientCallback to be called with a Cancelled status.
            fakeCall.ReceivedStatusOnClientCallback.OnReceivedStatusOnClient(true, new ClientSideStatus(Status.DefaultCancelled, new Metadata()));

            // Check not yet disposed the call as there is data inflight
            Assert.IsFalse(fakeCall.IsDisposed);

            // Read some data. Although we've already received this cancel status this will succeed as the 
            // the data was already inflight. Note: called with "success = true"
            fakeCall.ReceivedMessageCallback.OnReceivedMessage(true, CreateResponsePayload());
            Assert.IsTrue(readTask1.Result);
            Assert.AreEqual("response1", responseStream.Current);

            // check has disposed the call as there is no more data inflight and we previously received an error status
            Assert.IsTrue(fakeCall.IsDisposed);

            // Read some more data - this should fail as the call has been cancelled
            var ex = Assert.ThrowsAsync<RpcException>(async () => await responseStream.MoveNext());
            Assert.AreEqual(Status.DefaultCancelled, ex.Status);

            // Check that the resources have been released and that the client no longer references the call
            Assert.AreEqual(0, channel.GetCallReferenceCount());
            Assert.IsTrue(fakeCall.IsDisposed);
        }

        [Test]
        public void ServerStreaming_CancelCall_Issue8451_5()
        {
            // Test client cancelling the call before the reading the server stream to the end.
            // Test for resource leak https://github.com/grpc/grpc/issues/8451

            // The callbacks OnReceivedResponseHeaders, OnReceivedStatusOnClient and OnReceivedMessage may
            // occur in any order together with calls to MoveNext. We need to test that the resources
            // are released correctly no matter the order of the calls, and that when the call is cancelled
            // or there is an error (such as server unavailable) that MoveNext throws the correct exception.

            // This case tests the case when OnReceivedStatusOnClient occurs before the OnReceivedMessage 
            // for data already inflight.

            // Same as ServerStreaming_CancelCall_Issue8451_4() but with "success = false" in the 
            // OnReceivedMessage callback

            // This test is testing the order:
            // OnReceivedResponseHeaders
            // MoveNext task started
            // Cancel
            // OnReceivedStatusOnClient
            // OnReceivedMessage (with success = false)
            // MoveNext task completes with error

            asyncCall.StartServerStreamingCall("request1");
            var responseStream = new ClientResponseStream<string, string>(asyncCall);

            // Check that the client has a reference to exactly one call
            Assert.AreEqual(1, channel.GetCallReferenceCount());
            // Check that the call is not yet disposed
            Assert.IsFalse(fakeCall.IsDisposed);

            fakeCall.ReceivedResponseHeadersCallback.OnReceivedResponseHeaders(true, new Metadata());
            Assert.AreEqual(0, asyncCall.ResponseHeadersAsync.Result.Count);

            // Prime for a read - this sets the OnReceivedMessage callback handler in fakeCall
            // indicating that we are waiting to receive some data
            var readTask1 = responseStream.MoveNext();

            // Cancel the call
            asyncCall.Cancel();
            // Not yet disposed
            Assert.IsFalse(fakeCall.IsDisposed);

            // Trigger ReceivedStatusOnClientCallback to be called with a Cancelled status.
            fakeCall.ReceivedStatusOnClientCallback.OnReceivedStatusOnClient(true, new ClientSideStatus(Status.DefaultCancelled, new Metadata()));

            // Check not yet disposed the call as there is data inflight
            Assert.IsFalse(fakeCall.IsDisposed);

            // Read some data. 
            // Note: being called with "success = false" - this may occur if the native code
            // completes the callback indicating that there has already been an error
            fakeCall.ReceivedMessageCallback.OnReceivedMessage(false, CreateResponsePayload());
            var ex = Assert.ThrowsAsync<RpcException>(async () => await readTask1);
            Assert.AreEqual(Status.DefaultCancelled, ex.Status);

            // Check that the resources have been released and that the client no longer references the call
            Assert.AreEqual(0, channel.GetCallReferenceCount());
            Assert.IsTrue(fakeCall.IsDisposed);
        }

        [Test]
        public void ServerStreaming_RequestSerializationExceptionDoesntLeakResources()
        {
            string nullRequest = null;  // will throw when serializing
            Assert.Throws(typeof(ArgumentNullException), () => asyncCall.StartServerStreamingCall(nullRequest));
            Assert.AreEqual(0, channel.GetCallReferenceCount());
            Assert.IsTrue(fakeCall.IsDisposed);

            var responseStream = new ClientResponseStream<string, string>(asyncCall);
            var readTask = responseStream.MoveNext();
        }

        [Test]
        public void ServerStreaming_StartCallFailureDoesntLeakResources()
        {
            fakeCall.MakeStartCallFail();
            Assert.Throws(typeof(InvalidOperationException), () => asyncCall.StartServerStreamingCall("request1"));
            Assert.AreEqual(0, channel.GetCallReferenceCount());
            Assert.IsTrue(fakeCall.IsDisposed);
        }

        [Test]
        public void ServerStreaming_ServerUnavailableDoesntLeakResources1()
        {
            // Test for https://github.com/grpc/grpc/issues/28153
            // Memory leak if resources not released when server unavailable
            asyncCall.StartServerStreamingCall("request1");

            // error reading response headers - possibly because server unavailable
            fakeCall.ReceivedResponseHeadersCallback.OnReceivedResponseHeaders(false, new Metadata());
            fakeCall.ReceivedStatusOnClientCallback.OnReceivedStatusOnClient(true, new ClientSideStatus(new Status(StatusCode.Unavailable, ""), new Metadata()));

            Assert.IsTrue(fakeCall.IsDisposed);
            Assert.AreEqual(0, channel.GetCallReferenceCount());
        }

        [Test]
        public void ServerStreaming_ServerUnavailableDoesntLeakResources2()
        {
            // Test for https://github.com/grpc/grpc/issues/28153
            // Memory leak if resources not released when server unavailable
            asyncCall.StartServerStreamingCall("request1");

            // Test OnReceivedStatusOnClient before OnReceivedResponseHeaders
            fakeCall.ReceivedStatusOnClientCallback.OnReceivedStatusOnClient(true, new ClientSideStatus(new Status(StatusCode.Unavailable, ""), new Metadata()));
            fakeCall.ReceivedResponseHeadersCallback.OnReceivedResponseHeaders(false, new Metadata());
            
            Assert.IsTrue(fakeCall.IsDisposed);
            Assert.AreEqual(0, channel.GetCallReferenceCount());
        }

        [Test]
        public void ServerStreaming_ServerUnavailableDoesntLeakResources3()
        {
            // Test for https://github.com/grpc/grpc/issues/28153
            // Memory leak if resources not released when server unavailable
            asyncCall.StartServerStreamingCall("request1");

            // error reading response headers - possibly because server unavailable
            fakeCall.ReceivedResponseHeadersCallback.OnReceivedResponseHeaders(false, new Metadata());
            fakeCall.ReceivedStatusOnClientCallback.OnReceivedStatusOnClient(true, new ClientSideStatus(new Status(StatusCode.Unavailable, ""), new Metadata()));

            Assert.IsTrue(fakeCall.IsDisposed);

            // Test a cancel/dispose is OK even though the call has already been disposed
            asyncCall.Cancel();

            Assert.IsTrue(fakeCall.IsDisposed);
            Assert.AreEqual(0, channel.GetCallReferenceCount());
        }

        [Test]
        public void DuplexStreaming_NoRequestNoResponse_Success1()
        {
            asyncCall.StartDuplexStreamingCall();
            var requestStream = new ClientRequestStream<string, string>(asyncCall);
            var responseStream = new ClientResponseStream<string, string>(asyncCall);

            fakeCall.ReceivedResponseHeadersCallback.OnReceivedResponseHeaders(true, new Metadata());
            Assert.AreEqual(0, asyncCall.ResponseHeadersAsync.Result.Count);

            var writeTask1 = requestStream.CompleteAsync();
            fakeCall.SendCompletionCallback.OnSendCompletion(true);
            Assert.DoesNotThrowAsync(async () => await writeTask1);

            var readTask = responseStream.MoveNext();
            fakeCall.ReceivedMessageCallback.OnReceivedMessage(true, CreateNullResponse());
            fakeCall.ReceivedStatusOnClientCallback.OnReceivedStatusOnClient(true, new ClientSideStatus(Status.DefaultSuccess, new Metadata()));

            AssertStreamingResponseSuccess(asyncCall, fakeCall, readTask);
        }

        [Test]
        public void DuplexStreaming_NoRequestNoResponse_Success2()
        {
            asyncCall.StartDuplexStreamingCall();
            var requestStream = new ClientRequestStream<string, string>(asyncCall);
            var responseStream = new ClientResponseStream<string, string>(asyncCall);

            var writeTask1 = requestStream.CompleteAsync();
            fakeCall.SendCompletionCallback.OnSendCompletion(true);
            Assert.DoesNotThrowAsync(async () => await writeTask1);

            var readTask = responseStream.MoveNext();
            fakeCall.ReceivedMessageCallback.OnReceivedMessage(true, CreateNullResponse());
            fakeCall.ReceivedStatusOnClientCallback.OnReceivedStatusOnClient(true, new ClientSideStatus(Status.DefaultSuccess, new Metadata()));

            fakeCall.ReceivedResponseHeadersCallback.OnReceivedResponseHeaders(true, new Metadata());
            Assert.AreEqual(0, asyncCall.ResponseHeadersAsync.Result.Count);

            AssertStreamingResponseSuccess(asyncCall, fakeCall, readTask);
        }

        [Test]
        public void DuplexStreaming_WriteAfterReceivingStatusThrowsRpcException()
        {
            asyncCall.StartDuplexStreamingCall();
            var requestStream = new ClientRequestStream<string, string>(asyncCall);
            var responseStream = new ClientResponseStream<string, string>(asyncCall);

            fakeCall.ReceivedResponseHeadersCallback.OnReceivedResponseHeaders(true, new Metadata());
            Assert.AreEqual(0, asyncCall.ResponseHeadersAsync.Result.Count);

            var readTask = responseStream.MoveNext();
            fakeCall.ReceivedMessageCallback.OnReceivedMessage(true, CreateNullResponse());
            fakeCall.ReceivedStatusOnClientCallback.OnReceivedStatusOnClient(true, new ClientSideStatus(Status.DefaultSuccess, new Metadata()));

            AssertStreamingResponseSuccess(asyncCall, fakeCall, readTask);

            var writeTask = requestStream.WriteAsync("request1");
            var ex = Assert.ThrowsAsync<RpcException>(async () => await writeTask);
            Assert.AreEqual(Status.DefaultSuccess, ex.Status);
        }

        [Test]
        public void DuplexStreaming_CompleteAfterReceivingStatusSuceeds()
        {
            asyncCall.StartDuplexStreamingCall();
            var requestStream = new ClientRequestStream<string, string>(asyncCall);
            var responseStream = new ClientResponseStream<string, string>(asyncCall);

            fakeCall.ReceivedResponseHeadersCallback.OnReceivedResponseHeaders(true, new Metadata());
            Assert.AreEqual(0, asyncCall.ResponseHeadersAsync.Result.Count);

            var readTask = responseStream.MoveNext();
            fakeCall.ReceivedMessageCallback.OnReceivedMessage(true, CreateNullResponse());
            fakeCall.ReceivedStatusOnClientCallback.OnReceivedStatusOnClient(true, new ClientSideStatus(Status.DefaultSuccess, new Metadata()));

            AssertStreamingResponseSuccess(asyncCall, fakeCall, readTask);

            Assert.DoesNotThrowAsync(async () => await requestStream.CompleteAsync());
        }

        [Test]
        public void DuplexStreaming_WriteFailureThrowsRpcException()
        {
            asyncCall.StartDuplexStreamingCall();
            var requestStream = new ClientRequestStream<string, string>(asyncCall);
            var responseStream = new ClientResponseStream<string, string>(asyncCall);

            fakeCall.ReceivedResponseHeadersCallback.OnReceivedResponseHeaders(true, new Metadata());
            Assert.AreEqual(0, asyncCall.ResponseHeadersAsync.Result.Count);

            var writeTask = requestStream.WriteAsync("request1");
            fakeCall.SendCompletionCallback.OnSendCompletion(false);

            // The write will wait for call to finish to receive the status code.
            Assert.IsFalse(writeTask.IsCompleted);

            var readTask = responseStream.MoveNext();
            fakeCall.ReceivedMessageCallback.OnReceivedMessage(true, CreateNullResponse());
            fakeCall.ReceivedStatusOnClientCallback.OnReceivedStatusOnClient(true, CreateClientSideStatus(StatusCode.PermissionDenied));

            var ex = Assert.ThrowsAsync<RpcException>(async () => await writeTask);
            Assert.AreEqual(StatusCode.PermissionDenied, ex.Status.StatusCode);

            AssertStreamingResponseError(asyncCall, fakeCall, readTask, StatusCode.PermissionDenied);
        }

        [Test]
        public void DuplexStreaming_WriteFailureThrowsRpcException2()
        {
            asyncCall.StartDuplexStreamingCall();
            var requestStream = new ClientRequestStream<string, string>(asyncCall);
            var responseStream = new ClientResponseStream<string, string>(asyncCall);

            fakeCall.ReceivedResponseHeadersCallback.OnReceivedResponseHeaders(true, new Metadata());
            Assert.AreEqual(0, asyncCall.ResponseHeadersAsync.Result.Count);

            var writeTask = requestStream.WriteAsync("request1");

            var readTask = responseStream.MoveNext();
            fakeCall.ReceivedMessageCallback.OnReceivedMessage(true, CreateNullResponse());
            fakeCall.ReceivedStatusOnClientCallback.OnReceivedStatusOnClient(true, CreateClientSideStatus(StatusCode.PermissionDenied));
            fakeCall.SendCompletionCallback.OnSendCompletion(false);

            var ex = Assert.ThrowsAsync<RpcException>(async () => await writeTask);
            Assert.AreEqual(StatusCode.PermissionDenied, ex.Status.StatusCode);

            AssertStreamingResponseError(asyncCall, fakeCall, readTask, StatusCode.PermissionDenied);
        }

        [Test]
        public void DuplexStreaming_WriteAfterCancellationRequestThrowsTaskCanceledException()
        {
            asyncCall.StartDuplexStreamingCall();
            var requestStream = new ClientRequestStream<string, string>(asyncCall);
            var responseStream = new ClientResponseStream<string, string>(asyncCall);

            fakeCall.ReceivedResponseHeadersCallback.OnReceivedResponseHeaders(true, new Metadata());
            Assert.AreEqual(0, asyncCall.ResponseHeadersAsync.Result.Count);

            asyncCall.Cancel();
            Assert.IsTrue(fakeCall.IsCancelled);

            var writeTask = requestStream.WriteAsync("request1");
            Assert.ThrowsAsync(typeof(TaskCanceledException), async () => await writeTask);

            var readTask = responseStream.MoveNext();
            fakeCall.ReceivedMessageCallback.OnReceivedMessage(true, CreateNullResponse());
            fakeCall.ReceivedStatusOnClientCallback.OnReceivedStatusOnClient(true, CreateClientSideStatus(StatusCode.Cancelled));

            AssertStreamingResponseError(asyncCall, fakeCall, readTask, StatusCode.Cancelled);
        }

        [Test]
        public void DuplexStreaming_ReadAfterCancellationRequestCanSucceed()
        {
            asyncCall.StartDuplexStreamingCall();
            var responseStream = new ClientResponseStream<string, string>(asyncCall);

            fakeCall.ReceivedResponseHeadersCallback.OnReceivedResponseHeaders(true, new Metadata());
            Assert.AreEqual(0, asyncCall.ResponseHeadersAsync.Result.Count);

            asyncCall.Cancel();
            Assert.IsTrue(fakeCall.IsCancelled);

            var readTask1 = responseStream.MoveNext();
            fakeCall.ReceivedMessageCallback.OnReceivedMessage(true, CreateResponsePayload());
            Assert.IsTrue(readTask1.Result);
            Assert.AreEqual("response1", responseStream.Current);

            var readTask2 = responseStream.MoveNext();
            fakeCall.ReceivedMessageCallback.OnReceivedMessage(true, CreateNullResponse());
            fakeCall.ReceivedStatusOnClientCallback.OnReceivedStatusOnClient(true, CreateClientSideStatus(StatusCode.Cancelled));

            AssertStreamingResponseError(asyncCall, fakeCall, readTask2, StatusCode.Cancelled);
        }

        [Test]
        public void DuplexStreaming_ReadStartedBeforeCancellationRequestCanSucceed()
        {
            asyncCall.StartDuplexStreamingCall();
            var responseStream = new ClientResponseStream<string, string>(asyncCall);

            fakeCall.ReceivedResponseHeadersCallback.OnReceivedResponseHeaders(true, new Metadata());
            Assert.AreEqual(0, asyncCall.ResponseHeadersAsync.Result.Count);

            var readTask1 = responseStream.MoveNext();  // initiate the read before cancel request
            asyncCall.Cancel();
            Assert.IsTrue(fakeCall.IsCancelled);

            fakeCall.ReceivedMessageCallback.OnReceivedMessage(true, CreateResponsePayload());
            Assert.IsTrue(readTask1.Result);
            Assert.AreEqual("response1", responseStream.Current);

            var readTask2 = responseStream.MoveNext();
            fakeCall.ReceivedMessageCallback.OnReceivedMessage(true, CreateNullResponse());
            fakeCall.ReceivedStatusOnClientCallback.OnReceivedStatusOnClient(true, CreateClientSideStatus(StatusCode.Cancelled));

            AssertStreamingResponseError(asyncCall, fakeCall, readTask2, StatusCode.Cancelled);
        }

        [Test]
        public void DuplexStreaming_StartCallFailureDoesntLeakResources()
        {
            fakeCall.MakeStartCallFail();
            Assert.Throws(typeof(InvalidOperationException), () => asyncCall.StartDuplexStreamingCall());
            Assert.AreEqual(0, channel.GetCallReferenceCount());
            Assert.IsTrue(fakeCall.IsDisposed);
        }

        ClientSideStatus CreateClientSideStatus(StatusCode statusCode)
        {
            return new ClientSideStatus(new Status(statusCode, ""), new Metadata());
        }

        IBufferReader CreateResponsePayload()
        {
            return fakeBufferReaderManager.CreateSingleSegmentBufferReader(Marshallers.StringMarshaller.Serializer("response1"));
        }

        IBufferReader CreateNullResponse()
        {
            return fakeBufferReaderManager.CreateNullPayloadBufferReader();
        }

        static void AssertUnaryResponseSuccess(AsyncCall<string, string> asyncCall, FakeNativeCall fakeCall, Task<string> resultTask)
        {
            Assert.IsTrue(resultTask.IsCompleted);
            Assert.IsTrue(fakeCall.IsDisposed);

            Assert.AreEqual(Status.DefaultSuccess, asyncCall.GetStatus());
            Assert.AreEqual(0, asyncCall.ResponseHeadersAsync.Result.Count);
            Assert.AreEqual(0, asyncCall.GetTrailers().Count);
            Assert.AreEqual("response1", resultTask.Result);
        }

        static void AssertStreamingResponseSuccess(AsyncCall<string, string> asyncCall, FakeNativeCall fakeCall, Task<bool> moveNextTask)
        {
            Assert.IsTrue(moveNextTask.IsCompleted);
            Assert.IsTrue(fakeCall.IsDisposed);

            Assert.IsFalse(moveNextTask.Result);
            Assert.AreEqual(Status.DefaultSuccess, asyncCall.GetStatus());
            Assert.AreEqual(0, asyncCall.GetTrailers().Count);
        }

        static void AssertUnaryResponseError(AsyncCall<string, string> asyncCall, FakeNativeCall fakeCall, Task<string> resultTask, StatusCode expectedStatusCode)
        {
            Assert.IsTrue(resultTask.IsCompleted);
            Assert.IsTrue(fakeCall.IsDisposed);

            Assert.AreEqual(expectedStatusCode, asyncCall.GetStatus().StatusCode);
            var ex = Assert.ThrowsAsync<RpcException>(async () => await resultTask);
            Assert.AreEqual(expectedStatusCode, ex.Status.StatusCode);
            Assert.AreEqual(0, asyncCall.ResponseHeadersAsync.Result.Count);
            Assert.AreEqual(0, asyncCall.GetTrailers().Count);
        }

        static void AssertStreamingResponseError(AsyncCall<string, string> asyncCall, FakeNativeCall fakeCall, Task<bool> moveNextTask, StatusCode expectedStatusCode)
        {
            Assert.IsTrue(moveNextTask.IsCompleted);
            Assert.IsTrue(fakeCall.IsDisposed);

            var ex = Assert.ThrowsAsync<RpcException>(async () => await moveNextTask);
            Assert.AreEqual(expectedStatusCode, ex.Status.StatusCode);
            Assert.AreEqual(expectedStatusCode, asyncCall.GetStatus().StatusCode);
            Assert.AreEqual(0, asyncCall.GetTrailers().Count);
        }
    }
}

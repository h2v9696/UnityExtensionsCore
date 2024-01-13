using System;
using TinyMessenger;
using UnityEngine;

namespace H2V.ExtensionsCore.Events.ActionDispatch
{
    public static class ActionDispatcher
    {
        private static readonly ITinyMessengerHub _messengerHub =
            new TinyMessengerHub(new LogErrorHandler());

        public static void Dispatch<T>(T action) where T : ActionBase => _messengerHub.Publish(action);

        public static TinyMessageSubscriptionToken Bind<T>(Action<T> callback) where T : ActionBase =>
            _messengerHub.Subscribe<T>(callback);

        public static void Unbind(TinyMessageSubscriptionToken token) => _messengerHub.Unsubscribe(token);
    }

    public class LogErrorHandler : ISubscriberErrorHandler
    {
        public class ErrorAction : ActionBase
        {
            public Exception Exception { get; }

            public ErrorAction(Exception exception)
            {
                Exception = exception;
            }
        }
        public void Handle(ITinyMessage message, Exception exception)
        {
            Debug.LogWarning($"{message} {exception}");
            ActionDispatcher.Dispatch(new ErrorAction(exception));
        }
    }

    public abstract class ActionBase : ITinyMessage
    {
        public object Sender { get; } = null;
    }
}
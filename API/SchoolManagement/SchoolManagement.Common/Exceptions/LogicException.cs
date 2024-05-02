
using SchoolManagement.Common.Enum;

namespace SchoolManagement.Common.Exceptions
{
    public sealed class LogicException : Exception
#pragma warning restore S3925 // "ISerializable" should be implemented correctly
    {
        public MessageType MessageType { get; init; }
        public NotifactionType NotifactionType { get; init; }
        public string LogMessage { get; init; } = null!;
        public object? ErrorData { get; init; }
        public LogicException(string? message = null, string? logMessage = null,
            MessageType? messageType = null, NotifactionType? notifactionType = null, object? errorData = null,
            Exception? inner = null) : base(message, inner)
        {
            LogMessage = logMessage ?? message ?? "";
            MessageType = messageType ?? MessageType.Error;
            NotifactionType = notifactionType ?? NotifactionType.Modal;
            ErrorData = errorData;
        }

        public static LogicException CommonError(string? logMessage = null, object? errorData = null, Exception? inner = null)
        {
            return new LogicException(message: "", logMessage: logMessage,
                messageType: MessageType.Error, notifactionType: NotifactionType.Modal,
                errorData: errorData, inner: inner);
        }

        public static LogicException JsonConvertError(string? json, string typeName)
        {
            return new LogicException(message: $"Cannot cast [{json}] to type [{typeName}]", messageType: MessageType.Error);
        }
    }
}

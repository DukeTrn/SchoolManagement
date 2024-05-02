using SchoolManagement.Common.Enum;

namespace SchoolManagement.Common.Exceptions
{
#pragma warning disable S3925 // "ISerializable" should be implemented correctly
    public sealed class ExistRecordException : Exception
#pragma warning restore S3925 // "ISerializable" should be implemented correctly
    {
        public MessageType MessageType { get; init; }
        public NotifactionType NotifactionType { get; init; }
        public string LogMessage { get; init; } = null!;
        public object? ErrorData { get; init; }
        public ExistRecordException(string? message = null, string? logMessage = null,
            MessageType? messageType = null, NotifactionType? notifactionType = null, object? errorData = null,
            Exception? inner = null) : base(message, inner)
        {
            LogMessage = logMessage ?? message ?? "";
            MessageType = messageType ?? MessageType.Error;
            NotifactionType = notifactionType ?? NotifactionType.Modal;
            ErrorData = errorData;
        }

        public static ExistRecordException CommonError(string? logMessage = null, object? errorData = null, Exception? inner = null)
        {
            return new ExistRecordException(message: "", logMessage: logMessage,
                messageType: MessageType.Error, notifactionType: NotifactionType.Modal,
                errorData: errorData, inner: inner);
        }

        public static ExistRecordException ExistsRecord()
        {
            return new ExistRecordException(message: $"A record already exists", messageType: MessageType.Error);
        }

        public static ExistRecordException ExistsRecord(string message)
        {
            return new ExistRecordException(message: message, messageType: MessageType.Duplicated);
        }
    }
}


namespace Grpc.Client.Models
{
    public record Response(string Message, Status Status)
    {

        internal static Response FromProtobufResponse(Sending.SendResponse response)
            => new(response.Message, ConvertToStatus(response.Status));

        private static Status ConvertToStatus(Sending.SendingStatus status)
            => status switch
            {
                Sending.SendingStatus.Ok => Status.Success,
                Sending.SendingStatus.Error => Status.Error,
                _ => Status.Error,
            };
    }

    public enum Status
    {
        Success,
        Error

    }
}

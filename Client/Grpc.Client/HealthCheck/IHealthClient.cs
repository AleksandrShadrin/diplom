namespace Grpc.Client.HealthCheck
{
    public interface IHealthClient
    {
        Task<bool> ServerIsServing();
    }
}

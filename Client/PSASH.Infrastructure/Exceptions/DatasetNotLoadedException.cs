namespace PSASH.Infrastructure.Exceptions
{
    public class DatasetNotLoadedException : InfrastructureException
    {
        public DatasetNotLoadedException()
            : base("Dataset don't loaded correctly")
        { }
    }
}

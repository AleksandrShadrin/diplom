namespace PSASH.Infrastructure.Exceptions
{
    public class PathDontExistException : InfrastructureException
    {
        public string Path { get; init; } = string.Empty;

        public PathDontExistException(string path)
            : base($"Path: {path} don't exist.")
        { }
    }
}

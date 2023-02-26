namespace PSASH.Infrastructure.Exceptions
{
    public class DatasetStructureInvalidException : InfrastructureException
    {
        public string DatasetPath { get; set; } = string.Empty;

        public DatasetStructureInvalidException(string datasetPath)
            : base($"Dataset at path: {datasetPath} have invalid structure.")
        { }
    }
}

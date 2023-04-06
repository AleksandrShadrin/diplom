namespace PSASH.Infrastructure.Exceptions
{
    internal class CantConvertToMonoFileException : InfrastructureException
    {
        public CantConvertToMonoFileException(string path) 
            : base($"Данный файл не возможно конвертировать в одиночный временной ряд")
        {
        }
    }
}

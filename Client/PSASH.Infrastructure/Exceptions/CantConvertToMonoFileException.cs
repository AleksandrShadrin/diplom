namespace PSASH.Infrastructure.Exceptions
{
    internal class CantConvertToMonoFileException : InfrastructureException
    {
        public CantConvertToMonoFileException(string path) 
            : base($"Данный файл не вохможно конвертировать в одиночный временной ряд")
        {
        }
    }
}

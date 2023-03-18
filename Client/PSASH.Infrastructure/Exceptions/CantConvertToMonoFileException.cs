namespace PSASH.Infrastructure.Exceptions
{
    internal class CantConvertToMonoFileException : InfrastructureException
    {
        public CantConvertToMonoFileException() 
            : base($"Данный файл не вохможно конвертировать в одиночный временной ряд")
        {
        }
    }
}

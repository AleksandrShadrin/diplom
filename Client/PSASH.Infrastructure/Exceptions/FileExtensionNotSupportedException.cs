namespace PSASH.Infrastructure.Exceptions
{
    internal class FileExtensionNotSupportedException : InfrastructureException
    {
        public FileExtensionNotSupportedException()
            : base("Расширение файла не поддерживается")
        {
        }
    }
}

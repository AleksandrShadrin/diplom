namespace PSASH.Infrastructure.Exceptions
{
    public class ThisFileWasNotFound : InfrastructureException
    {
        public ThisFileWasNotFound()
            : base($"Данный файл не найден")
        {
        }
    }
}

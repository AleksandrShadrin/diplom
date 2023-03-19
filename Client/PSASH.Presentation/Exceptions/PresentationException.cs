namespace PSASH.Presentation.Exceptions
{
    public abstract class PresentationException : Exception
    {
        protected PresentationException(string message) :
            base(message)
        { }
    }
}

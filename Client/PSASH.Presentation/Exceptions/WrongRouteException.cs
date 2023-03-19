namespace PSASH.Presentation.Exceptions
{
    public class WrongRouteException : PresentationException
    {
        public WrongRouteException(string route) : base($"route: {route} is wrong")
        {
            Route = route;
        }

        public string Route { get; set; }
    }
}

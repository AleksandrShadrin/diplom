namespace PSASH.Infrastructure.Models
{
    public class Result
    {
        private bool isOk = true;
        private string message = string.Empty;

        private Result(string message, bool isOk)
        {
            this.isOk = isOk;
        }

        /// <summary>
        /// Возвращает true, когда Result Ok
        /// </summary>
        /// <returns>
        /// Возвращает bool значение
        /// </returns>
        public bool Ok()
            => isOk;

        public static Result Ok(string message)
            => new Result(message, true);

        public static Result Error(string message)
            => new Result(message, false);
    }
}

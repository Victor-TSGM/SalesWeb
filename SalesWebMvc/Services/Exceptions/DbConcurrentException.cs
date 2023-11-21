namespace SalesWebMvc.Services.Exceptions
{
    public class DbConcurrentException : ApplicationException
    {
        public DbConcurrentException(string message)  : base(message) { }
    }
}

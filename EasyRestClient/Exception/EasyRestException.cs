namespace EasyRestClient.Exception
{
    public class EasyRestException : System.Exception
    {
        public EasyRestException(string message, System.Exception ex = null)
            : base(message, ex)
        {
        }
    }
}
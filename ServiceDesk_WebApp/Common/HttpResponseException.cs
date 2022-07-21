namespace ServiceDesk_WebApp.Common
{
    public class HttpResponseException:Exception
    {
        public HttpResponseException(int status)
        {
            Status = status;
        }

        /// <summary>
        ///     Status
        /// </summary>
        public int Status { get; set; }
    }
}

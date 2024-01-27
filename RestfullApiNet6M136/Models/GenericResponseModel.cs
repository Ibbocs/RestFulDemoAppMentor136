namespace RestfullApiNet6M136.Models
{
    public class GenericResponseModel<T>
    {
        public T Data { get; set; }
        public int StatusCode { get; set; }
    }
}

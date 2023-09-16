namespace PRUV_WebApp.Models
{
    public class RequestImage
    {
        public int ImageId { get; set; }
        public int? RequestId { get; set; }
        public byte[]? Image { get; set; }

        public RequestImage() { }
    }
}

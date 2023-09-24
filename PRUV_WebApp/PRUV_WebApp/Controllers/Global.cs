namespace PRUV_WebApp.Controllers
{
    public static class Global
    {
        public static int empNum = 0;
        public static int empLoc = 0;


        public static byte[] ConvertImageFile(IFormFile imageFile)
        {
            byte[]? image = null;

            
            if (imageFile.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    imageFile.CopyTo(ms);
                    image = ms.ToArray();

                }
                string filePath = "C:\\Users\\timeg\\PRUV\\PRUV_prototype\\PRUV_WebApp\\PRUV_WebApp\\Views\\UserRequests\\test.jpeg";
                using var stream = System.IO.File.Create(filePath);
                stream.Write(image, 0, image.Length);

            }



            return image;

        }

    }
}

using System.Web;

namespace RentYourCar_PWEB.Models.VeiculosView
{
    public class ImageViewModel
    {
        public string Title { get; set; }
        public HttpPostedFileWrapper ImageFile { get; set; }
    }
}
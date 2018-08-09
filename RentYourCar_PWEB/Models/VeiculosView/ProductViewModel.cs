using System;
using System.Web;

namespace RentYourCar_PWEB.Models.VeiculosView
{
    public class ProductViewModel
    {
        public string Title { get; set; }
        public HttpPostedFileWrapper ImageFile { get; set; }
    }
}
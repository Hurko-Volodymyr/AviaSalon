using System.ComponentModel.DataAnnotations;

namespace AviationSalon.WebUI.Models
{
    public class CustomerInfoModel
    {
        [Required(ErrorMessage = "Secret code is required")]
        public string UserSecret { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Contact information is required")]
        public string ContactInformation { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;


namespace BLL.DTO
{
    internal class PositioniCreateDTO
    {
        public int PositioniID { get; set; }

        [Required(ErrorMessage = "PositioniName is required")]
        public string PositioniName { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Infinion.Core.DTOs
{
    public class ResetPasswordDto
    {
        

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

    }
}

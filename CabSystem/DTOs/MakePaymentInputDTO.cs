using System.ComponentModel.DataAnnotations;

public class MakePaymentInputDTO
{
    [Required]
    public string Method { get; set; } = null!;
}
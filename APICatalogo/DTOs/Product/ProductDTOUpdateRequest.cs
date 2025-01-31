using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTOs;

public class ProductDTOUpdateRequest : IValidatableObject
{
    [Range(1, 9999, ErrorMessage = "O campo Estoque deve ser entre 1 e 9999.")]
    public float Stock { get; init; }
    public DateTime RegistrationDate { get; init; }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (RegistrationDate.Date <= DateTime.Now.Date)
        {
            yield return new ValidationResult("A data deve ser maior que a data atual.",
                new[] { nameof(RegistrationDate) });
        }
    }
}
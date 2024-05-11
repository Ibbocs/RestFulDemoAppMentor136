using FluentValidation;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RestfullApiNet6M136.DTOs.SchoolDTOs;

namespace RestfullApiNet6M136.Validations;

public class SchoolCreateDTOValidator : AbstractValidator<SchoolCreateDTO>
{
    public SchoolCreateDTOValidator()
    {
        //gpt
        RuleFor(x => x.Number)
           .NotEmpty().WithMessage("Okul numarası boş olamaz.")
           .GreaterThan(0).WithMessage("Okul numarası 0'dan büyük olmalıdır.")
           .WithName("School Number");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Okul adı boş olamaz.")
            .MaximumLength(100).WithMessage("Okul adı en fazla 100 karakter olmalıdır.")
            .Must(name => name != "user").WithMessage("Okul adı 'user' olamaz.")
            .WithName("School Name");
    }

}

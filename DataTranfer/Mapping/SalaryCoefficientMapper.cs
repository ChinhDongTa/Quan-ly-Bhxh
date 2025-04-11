using DataServices.Entities.Human;
using Dtos;

namespace DataTranfer.Mapping;

public static class SalaryCoefficientMapper {

    public static SalaryCoefficientDto ToDto(this SalaryCoefficient salaryCoefficient)
    {
        return new()
        {
            Id = salaryCoefficient.Id,
            Name = salaryCoefficient.Name,
            Description = salaryCoefficient.Description
        };
    }

    public static SalaryCoefficient ToEntity(this SalaryCoefficientDto dto, SalaryCoefficient? salaryCoefficient = null)
    {
        if (salaryCoefficient is null)
        {
            return new()
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description
            };
        }
        salaryCoefficient.Name = dto.Name;
        salaryCoefficient.Description = dto.Description;
        return salaryCoefficient;
    }
}
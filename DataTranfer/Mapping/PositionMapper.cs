using DataServices.Entities.Human;
using Dtos.Human;

namespace DataTranfer.Mapping;

public static class PositionMapper {

    public static PositionDto ToDto(this Position position)
    {
        return new()
        {
            Id = position.Id,
            Name = position.Name,
            ShortName = position.ShortName
        };
    }

    public static Position ToEntity(this PositionDto dto, Position? position = null)
    {
        if (position is null)
        {
            return new()
            {
                Id = dto.Id,
                Name = dto.Name,
                ShortName = dto.ShortName
            };
        }
        position.Name = dto.Name;
        position.ShortName = dto.ShortName;
        return position;
    }
}
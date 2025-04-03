using DataServices.Entities.Human;
using DataTranfer.Dtos;

namespace DataTranfer.Mapping;

public static class RewardMapper {

    public static RewardDto ToDto(this Reward reward)
    {
        return new()
        {
            RewardId = reward.RewardId,
            Name = reward.Name,
            ShortName = reward.ShortName,
            Classify = reward.Classify,
            Type = reward.Type
        };
    }
}

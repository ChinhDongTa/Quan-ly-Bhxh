﻿namespace Dtos.Human;

public record UserIdRoleName {
    public string UserId { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
}
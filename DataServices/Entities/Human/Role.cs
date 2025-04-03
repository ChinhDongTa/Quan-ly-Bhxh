﻿namespace DataServices.Entities.Human;

public class Role {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public virtual ICollection<User>? Users { get; set; }
}
﻿using Microsoft.AspNetCore.Identity;

namespace DataServices.Entities.Human;

public class ApiUser : IdentityUser
{
    public int EmployeeId { get; set; }
   
    public virtual Employee? Employee { get; set; }
}
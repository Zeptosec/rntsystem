﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentSystem.Core.Enums;

namespace RentSystem.API.Extensions
{
    public class AuthorizeRole : AuthorizeAttribute
    {
        public AuthorizeRole(params Role [] roles)
        {
            var allowedRolesAsStrings = roles.Select(x => Enum.GetName(typeof(Role), x));
            Roles = string.Join(",", allowedRolesAsStrings);
        }
    }
}

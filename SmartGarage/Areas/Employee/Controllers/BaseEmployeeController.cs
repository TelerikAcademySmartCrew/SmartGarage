﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static SmartGarage.Common.GeneralApplicationConstants.Roles;

namespace SmartGarage.Areas.Employee.Controllers
{
    [Area(EmployeeAreaName)]
    [Authorize(Roles = EmployeeRoleName)]
    public class BaseEmployeeController : Controller
    {
        protected void InitializeUserName()
        {
            var userName = User.Identity.Name.Substring(0, User.Identity.Name.IndexOf("@"));
            this.ViewData["CurrentUser"] = userName;
        }
    }
}

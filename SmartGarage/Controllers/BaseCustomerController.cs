﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static SmartGarage.Common.GeneralApplicationConstants.Roles;

namespace SmartGarage.Controllers
{
    [Authorize(Roles = CustomerRoleName)]
    public class BaseCustomerController : Controller
    {
        public BaseCustomerController()
        {
        }
    }
}

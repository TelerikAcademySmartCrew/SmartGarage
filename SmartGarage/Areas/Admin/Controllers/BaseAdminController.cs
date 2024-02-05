using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using static SmartGarage.Common.GeneralApplicationConstants.Admin;

namespace SmartGarage.Areas.Admin.Controllers
{
    [Area(AdminAreaName)]
    [Authorize(Roles = AdminRoleName)]
    public class BaseAdminController : Controller
    {
    }
}

using Microsoft.AspNetCore.Mvc;
using SmartGarage.Common.Exceptions;
using SmartGarage.Data.Models.QueryParameters;
using SmartGarage.Services.Contracts;
using SmartGarage.Utilities.Mappers.Contracts;

namespace SmartGarage.Controllers
{
    public class VisitsController : Controller
    {
        private readonly IUsersService usersService;
        private readonly IVisitService visitService;
        private readonly IVisitMapper visitMapper;
        private readonly IUserMapper userMapper;

        public VisitsController(IUsersService usersService,
            IVisitService visitService,
            IVisitMapper visitMapper,
            IUserMapper userMapper)
        {
            this.usersService = usersService;
            this.visitService = visitService;
            this.visitMapper = visitMapper;
            this.userMapper = userMapper;
        }

        [HttpGet]
        public async Task<IActionResult> DisplayAll(CancellationToken cancellationToken)
        {
            try
            {
                var visitQueryParameters = new VisitsQueryParameters();
                var visits = await visitService.GetAll(visitQueryParameters, cancellationToken);

                var user = await usersService.GetUserAsync(User);
                var userViewModel = userMapper.Map(user);

                return View(userViewModel);
            }
            catch (EntityNotFoundException ex)
            {
                return RedirectToAction("Login", "Auth");
            }
        }

        [HttpGet]
        public async Task<IActionResult> DisplayVisit(Guid visitId, CancellationToken cancellationToken)
        {
            try
            {
                var visit = await visitService.GetByIdAsync(visitId, cancellationToken);

                var visitViewModel = this.visitMapper.ToViewModel(visit);
                
                return View(visitViewModel);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}

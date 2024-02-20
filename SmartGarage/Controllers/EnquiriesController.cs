using Microsoft.AspNetCore.Mvc;
using SmartGarage.Data.Models;
using SmartGarage.Services.Contracts;
using SmartGarage.Utilities.Mappers.Contracts;

namespace SmartGarage.Controllers
{
    public class EnquiriesController : BaseCustomerController
    {
        private readonly IUsersService usersService;
        private readonly IUserMapper userMapper;
        private readonly IEnquiryService enquiryService;

        public EnquiriesController(IUsersService usersService,
            IUserMapper userMapper,
            IEnquiryService enquiryService)
        {
            this.usersService = usersService;
            this.userMapper = userMapper;
            this.enquiryService = enquiryService;
        }

        [HttpGet]
        public async Task<IActionResult> PostEnquiry()
        {

            var user = await this.usersService.GetUserAsync(User);
            var userViewModel = this.userMapper.Map(user);

            return View(userViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> PostEnquiry(string content, CancellationToken cancellationToken)
        {
            try
            {
                var user = await this.usersService.GetUserAsync(this.User);

                var enquiry = new Enquiry()
                {
                    Id = Guid.NewGuid(),
                    Email = User.Identity!.Name!,
                    Content = content,
                    DateCreated = DateTime.Now,
                    PhoneNumber = user.PhoneNumber ?? ""
                };

                _ = await this.enquiryService.CreateAsync(enquiry, cancellationToken);
                return RedirectToAction("NotifyEnquiryPosted");
            }
            catch (Exception)
            {
                return RedirectToAction("PostEnquiry");
            }
        }

        [HttpGet]
        public async Task<IActionResult> NotifyEnquiryPosted()
        {
            var user = await this.usersService.GetUserAsync(this.User);
            var userViewModel = this.userMapper.Map(user);

            return View(userViewModel);
        }
    }
}

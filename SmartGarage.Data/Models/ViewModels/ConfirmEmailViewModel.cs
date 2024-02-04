namespace SmartGarage.Data.Models.ViewModels
{
    public class ConfirmEmailViewModel
    {
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string EmailConfirmToken { get; set; }
    }
}

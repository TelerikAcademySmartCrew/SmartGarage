namespace SmartGarage.Common.Exceptions
{
    public static class ExceptionMessages
    {
        public static class RepairActivity
        {
            public const string TypeNotFound = "Repair activity type not found!";
            public const string TypeAlreadyExists = "Repair activity type already exists!";
        }

        public static class Vehicle
        {
            public const string InvalidYear = "Year should a number smaller or equal to the current year.";
        }

        public static class Visit
        {
            public const string VisitNotFound = "Visit not found!";
        }

        public static class User
        {
            public const string UserNotFound = "User not found!";
        }

        public static class VehicleBrand
        {
            public const string BrandNotFound = "Brand not found!";
            public const string BrandAlreadyExists = "Brand already exists!";
        }

        public static class VehicleModel
        {
            public const string ModelNotFound = "Model not found!";
            public const string ModelAlreadyExists = "Model already exists!";
        }

        public static class Status
        {
            public const string CannotUpdateStatus = "Status cannot be updated any further!";
        }

        public static class Enquiry
        {
            public const string EnquiryNotFound = "Enquiry not found!";
        }
    }
}

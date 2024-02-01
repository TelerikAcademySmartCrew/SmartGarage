namespace SmartGarage.Common.Exceptions
{
    public static class ExceptionMessages
    {
        public static class RepairActivity
        {
            public const string TypeNotFound = "Repair activity type not found!";
            public const string AlreadyExists = "Repair activity type already exists!";
        }

        public static class Vehicle
        {
            public const string InvalidYear = "Year should a number smaller or equal to the current year.";
        }
    }
}

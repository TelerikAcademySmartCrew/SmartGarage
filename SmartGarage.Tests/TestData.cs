using SmartGarage.Data.Models;

namespace SmartGarage.Tests
{
    public static class TestData
    {
        public static Visit GetTestVisit()
        {
            var repairActivityType = new RepairActivityType
            {
                Id = Guid.NewGuid(),
                Name = "Test Name"
            };

            var visit = new Visit()
            {
                Id = Guid.NewGuid()
            };

            var repairActivity = new RepairActivity
            {
                Id = Guid.NewGuid(),
                RepairActivityTypeId = repairActivityType.Id,
                RepairActivityType = repairActivityType,
                Price = 10,
                VisitId = visit.Id
            };

            visit.RepairActivities = new List<RepairActivity> { repairActivity };

            return visit;
        }
        
        public static RepairActivityType GetTestRepairActivityType()
        {
            return new RepairActivityType()
            {
                Id = Guid.NewGuid(),
                Name = "Test Name"
            };
        }

        public static RepairActivity GetTestRepairActivity()
        {
            var repairActivityType = new RepairActivityType()
            {
                Id = Guid.NewGuid(),
                Name = "Test Name"
            };

            return new RepairActivity
            {
                Id = Guid.NewGuid(),
                RepairActivityTypeId = repairActivityType.Id,
                RepairActivityType = repairActivityType,
                Price = 10,
                VisitId = Guid.NewGuid()
            };
        }
    }
}

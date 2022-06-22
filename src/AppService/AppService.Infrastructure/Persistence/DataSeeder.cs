using System;
using System.Threading.Tasks;
using AppService.Domain.Entities;

namespace AppService.Infrastructure.Persistence
{
    public static class DataSeeder
    {
        public static async Task SeedDataAsync(this AccessControlContext accessControlContext)
        {
            //await accessControlContext.Database.EnsureDeletedAsync();

            if (await accessControlContext.Database.EnsureCreatedAsync())
            {
                var identity = new Identity()
                {
                    Id = Guid.NewGuid(),
                    Name = "Bob",
                };

                accessControlContext.Identitiets.Add(identity);

                var accessPoint = new AccessPoint("AccessPoint1", "192.168.1.182");

                accessPoint.AccessList = new AccessList("AccessPoint1_AccessList");

                accessPoint.AccessList.AddMember(identity);

                accessControlContext.AccessPoints.Add(accessPoint);

                await accessControlContext.SaveChangesAsync();
            }
        }
    }
}

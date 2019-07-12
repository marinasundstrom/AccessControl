using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace Foobiq.AccessControl.AppService.Persistence
{
    public class AccessControlContextFactory : IDesignTimeDbContextFactory<AccessControlContext>
    {
        public AccessControlContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AccessControlContext>();
            optionsBuilder.UseSqlite("Data Source=accesscontrol.db");

            return new AccessControlContext(optionsBuilder.Options);
        }
    }
}

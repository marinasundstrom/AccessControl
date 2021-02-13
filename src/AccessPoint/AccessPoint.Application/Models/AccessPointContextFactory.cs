using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccessPoint.Application.Models
{
    public class AccessControlContextFactory : IDesignTimeDbContextFactory<AccessPointContext>
    {
        public AccessPointContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AccessPointContext>();
            optionsBuilder.UseSqlite("Data Source=accesspoint.db");

            return new AccessPointContext(optionsBuilder.Options);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Database
{
    public class AppIdentityDbContextFactory : DesignTimeDbContextFactory<AppIdentityDbContext>
    {
        protected override AppIdentityDbContext CreateNewInstance(DbContextOptions<AppIdentityDbContext> options)
        {
            return new AppIdentityDbContext(options);
        }
    }
}

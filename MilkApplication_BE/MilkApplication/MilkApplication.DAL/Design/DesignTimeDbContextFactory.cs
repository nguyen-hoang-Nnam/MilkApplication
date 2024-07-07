using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using MilkApplication.DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.DAL.Design
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer("server=(local);database=MilkApplication_DB;uid=sa;pwd=123456;TrustServerCertificate=True;");

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}

namespace MVCAPIWithBearerToken.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MVCAPIWithBearerToken.Models.AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MVCAPIWithBearerToken.Models.AppDbContext context)
        {

            //context.Products.AddOrUpdate(p => p.ProductId,
            //  new Models.Product() { ProductId = 1, ProductName = "Laptop" },
            //  new Models.Product() { ProductId = 2, ProductName = "Mobile" }
            //  );
            //context.Users.AddOrUpdate(u => u.UserId,
            //    new Models.User() { UserId = 1, UserName = "safrina", Password = "12345", Email = "98safrina@gmail.com", Roles = "Admin,User" },
            //    new Models.User() { UserId = 2, UserName = "nishi", Password = "12345", Email = "98nishi@gmail.com", Roles = "User" }
            //    );
        }
    }
}

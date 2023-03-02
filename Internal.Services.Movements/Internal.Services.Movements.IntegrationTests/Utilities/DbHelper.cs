using Internal.Services.Movements.Data.Contexts;
using Internal.Services.Movements.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Internal.Services.Movements.IntegrationTests.Utilities
{
  public class DbHelper
  {
    private readonly MovementsDataContext _movementsDb;

    public DbHelper(MovementsDataContext movementsDb)
    {
      _movementsDb = movementsDb;
    }

    public void InitializeDbForTests()
    {
      if (!_movementsDb.Products.Any())
      {
        var products = new List<Product>
        {
          new Product
          {
            ProductId =1,
            ExternalAccount=AccountHelper.CustomerAccount,
            ProductType=Data.Models.Enums.EnumProductType.SavingsRetirement,
          }
        };

        _movementsDb.Products.AddRange(products);
        _movementsDb.SaveChanges();
      }

      if (!_movementsDb.ProductsCustomers.Any())
      {
        var productCustomers = new List<ProductCustomer>
        { new ProductCustomer
          {
            ProductCustomerId =1,
            ProductId=1,
            CustomerId=1
           }
        };

        _movementsDb.ProductsCustomers.AddRange(productCustomers);
        _movementsDb.SaveChanges();
      }
    }
    
    public static MovementsDataContext PrepareDbForTest()
    {
      var builder = new DbContextOptionsBuilder<MovementsDataContext>().UseInMemoryDatabase("InMemoryDbForTesting");
      MovementsDataContext database = new MovementsDataContext(builder.Options);
      database.Database.EnsureCreated();

      return database;
    }
  }
}

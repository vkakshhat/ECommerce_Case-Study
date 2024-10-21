using DAO_Library;
using Microsoft.Extensions.Configuration;

namespace Unit_Testing { 
public abstract class TestBase
{
    protected string _connectionString;
    protected IOrderProcessorRepository _orderProcessorRepository;

        [SetUp]
        public void Setup()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            _connectionString = configuration.GetConnectionString("dbCn");
            _orderProcessorRepository = new OrderProcessorRepositoryImpl(_connectionString);
        }
    }
}
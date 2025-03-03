using APICatalogo.Context;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogoTest.UnitTests;

public class ProductsUnitTestController
{
    public IUnitOfWork repository;
    public IMapper mapper;
    public static DbContextOptions<AppDbContext> options { get; }

    public static string connectionString = "Server=localhost;DataBase=CatalogDB;Uid=root;Pwd=";

    static ProductsUnitTestController()
    {
        options = new DbContextOptionsBuilder<AppDbContext>()
            .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
            .Options;
    }

    public ProductsUnitTestController()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile((new ProductDtoMappingProfile()));
        });
        
        mapper = config.CreateMapper();
        var context = new AppDbContext(options);
        repository = new UnitOfWork(context);
    }
}
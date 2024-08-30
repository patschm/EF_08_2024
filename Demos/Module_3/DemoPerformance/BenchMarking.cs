using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore;


namespace DemoPerformance;

[MemoryDiagnoser]
//[MaxIterationCount(200)]
public class BenchMarking
{
    public static string connectionString = Program.connectionString;
    private DbContextOptions optionsCompiled;
    private DbContextOptions options;
    private ProductContext contextCompiled;
    private ProductContext context;

    [GlobalSetup]
    public void Setup()
    {
        var optionsBuilder = new DbContextOptionsBuilder<ProductContext>();
        optionsBuilder.UseSqlServer(connectionString);
        options = optionsBuilder.Options;
        context = new ProductContext(options);
        contextCompiled = new ProductContext(options);

        optionsBuilder = new DbContextOptionsBuilder<ProductContext>();
        optionsBuilder.UseSqlServer(connectionString);
        optionsBuilder.UseModel(ProductContextModel.Instance);
        optionsCompiled = optionsBuilder.Options;
    }

    [Benchmark]
    public ProductContext NormalInit()
    {
        var context = new ProductContext(options);
        context.ProductGroups.First();
        return context;
    }

    [Benchmark]
    public ProductContext NormalCompiledModelInit()
    {
        var context = new ProductContext(optionsCompiled);
        context.ProductGroups.First();
        return context;
    }

    [Benchmark]
    public List<ProductGroup> NormalQuery()
    {
        var query = context.ProductGroups
           .Include(pg => pg.Products)
               .ThenInclude(p => p.Brand)
           .Include(pg => pg.Products);
           return query.ToList();
        
    }

    private static Func<ProductContext, IEnumerable<ProductGroup>> _compiled =
       EF.CompileQuery((ProductContext ctx) => ctx.ProductGroups
          .Include(pg => pg.Products)
              .ThenInclude(p => p.Brand)
          .Include(pg => pg.Products));

    [Benchmark]
    public List<ProductGroup> CompiledQuery()
    {
        return _compiled(contextCompiled).ToList();
    }
}

// using Microsoft.EntityFrameworkCore;
// using RefactorMe.Dal;
//
//
// namespace RefactorMe.MsSql.Dal;
//
// public class MsSqlDbContext: AppDbContext
// {
//     public MsSqlDbContext(DbContextOptions<AppDbContext> dbOptions) : base(GetDbOptions(dbOptions))
//     {
    //         Database.EnsureCreated();
//     }
//     
//     private static DbContextOptions<AppDbContext> GetDbOptions(DbContextOptions<AppDbContext> dbOptions)
//     {
//         return dbOptions;
//         // var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
//         // optionsBuilder.UseSqlServer(dbOptions.Extensions.Where(x => x.));
//         // return optionsBuilder.Options;
//     }
// }
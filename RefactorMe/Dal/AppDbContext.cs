using Microsoft.EntityFrameworkCore;
using RefactorMe.Dal.Models;

namespace RefactorMe.Dal;

public class AppDbContext: DbContext
{
    public AppDbContext(){}
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public void EnsureDatabaseCreated()
    {
        Database.EnsureCreated();        
    }
    
    public void EnsureDatabaseDeleted()
    {
        Database.EnsureDeleted();   
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Survey> Surveys { get; set; }
    public DbSet<SurveyQuestion> SurveyQuestions { get; set; }
    public DbSet<SurveyResult> SurveyResults { get; set; }
}
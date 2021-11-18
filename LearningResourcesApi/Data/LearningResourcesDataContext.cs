using Microsoft.EntityFrameworkCore;

namespace LearningResourcesApi.Data
{
    public class LearningResourcesDataContext : DbContext
    {
        public LearningResourcesDataContext(DbContextOptions<LearningResourcesDataContext> options) : 
            base(options)
        { }


        public DbSet<LearningResource> LearningResources { get; set; }


    }
}

// Entity means "An object that is kept in collections that are tracker by an ID"
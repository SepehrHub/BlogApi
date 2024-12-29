// Namespace for Data Layer, defining the BlogContext class
using Microsoft.EntityFrameworkCore;
using BlogAPI.Models;

namespace BlogApi.Data
{
    // BlogContext class to interact with the in-memory database using Entity Framework
    public class BlogContext(DbContextOptions<BlogContext> options) : DbContext(options)
    {

        // Define the BlogPosts DbSet, which will represent the table of blog posts in the database
        public DbSet<BlogPost> BlogPosts { get; set; } = null!;
    }
}



// Importing necessary libraries for unit testing
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;
using BlogAPI.Models;
using BlogApi.Controllers;
using BlogApi.Data;

namespace BlogApi.Tests
{
    public class BlogPostsControllerTests
    {
        private readonly BlogPostsController _controller;
        private readonly BlogContext _context;

        // Constructor for setting up the in-memory database and controller
        public BlogPostsControllerTests()
        {
            var options = new DbContextOptionsBuilder<BlogContext>()
                .UseInMemoryDatabase(databaseName: "TestBlogDb")  // Using an in-memory database for testing
                .Options;

            _context = new BlogContext(options);
            _context.Database.EnsureCreated();  // Ensuring the database is created
            _controller = new BlogPostsController(_context);

            InitializeDatabase();
        }

        // Initialize the database with some test data
        private void InitializeDatabase()
        {
            if (!_context.BlogPosts.Any())  // If no blog posts exist, add an initial one
            {
                _context.BlogPosts.Add(new BlogPost
                {
                    Title = "Initial Post",
                    Content = "This is an initial test post.",
                    Author = "Author 1"
                });
                _context.SaveChanges();
            }
        }

        // Test case for posting a valid blog post (should return 201 Created)
        [Fact]
        public async Task PostBlogPost_ShouldReturn201_WhenBlogPostIsValid()
        {
            var newPost = new BlogPost
            {
                Title = "Test Title",
                Content = "Test Content",
                Author = "Test Author"
            };

            var result = await _controller.PostBlogPost(newPost);
            var createdResult = result.Result as CreatedAtActionResult;
            Assert.NotNull(createdResult);
            Assert.Equal(201, createdResult.StatusCode);  // Assert that status code is 201 Created
        }

        // Test case for getting a non-existent blog post (should return 404 Not Found)
        [Fact]
        public async Task GetBlogPost_ShouldReturn404_WhenBlogPostDoesNotExist()
        {
            var result = await _controller.GetBlogPost(999);  // Try to get a blog post with an invalid ID
            Assert.IsType<NotFoundResult>(result.Result);  // Assert that the result is a 404 Not Found response
        }
    }
}


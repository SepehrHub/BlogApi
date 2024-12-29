// Importing necessary libraries for handling the web API and Entity Framework Core
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlogAPI.Models;  // Importing BlogPost model class
using BlogApi.Data;    // Importing BlogContext class

namespace BlogApi.Controllers
{
    // Controller for handling requests related to Blog Posts
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController(BlogContext context) : ControllerBase
    {
        // Injecting BlogContext to interact with the database
        private readonly BlogContext _context = context;

        // 1. Create a new blog post (POST /api/posts)
        // This method handles the creation of a new blog post.
        [HttpPost]
        public async Task<ActionResult<BlogPost>> PostBlogPost(BlogPost blogPost)
        {
            // Validation to ensure title and content are not empty
            if (string.IsNullOrEmpty(blogPost.Title) || string.IsNullOrEmpty(blogPost.Content))
            {
                return BadRequest("Title and Content are required.");  // If validation fails, return BadRequest with error message
            }

            // Add the new blog post to the database
            _context.BlogPosts.Add(blogPost);
            await _context.SaveChangesAsync();  // Save changes to the database

            // Return the created blog post along with a 201 Created response
            return CreatedAtAction(nameof(GetBlogPost), new { id = blogPost.Id }, blogPost);
        }

        // 2. Retrieve all blog posts (GET /api/posts)
        // This method retrieves all blog posts with pagination (pageNumber and pageSize)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BlogPost>>> GetBlogPosts(int pageNumber = 1, int pageSize = 10)
        {
            // Using Skip and Take for pagination
            var blogPosts = await _context.BlogPosts
                .Skip((pageNumber - 1) * pageSize)  // Skip the records for the previous pages
                .Take(pageSize)                     // Take only the number of records as specified by pageSize
                .ToListAsync();                     // Execute the query and return the results as a list

            return Ok(blogPosts);  // Return the list of blog posts with an HTTP 200 OK response
        }

        // 3. Retrieve a single blog post (GET /api/posts/{id})
        // This method retrieves a specific blog post by its ID
        [HttpGet("{id}")]
        public async Task<ActionResult<BlogPost>> GetBlogPost(int id)
        {
            var blogPost = await _context.BlogPosts.FindAsync(id);  // Find the blog post by its ID

            // If the blog post doesn't exist, return 404 Not Found
            if (blogPost == null)
            {
                return NotFound();
            }

            return blogPost;  // Return the found blog post
        }

        // 4. Update an existing blog post (PUT /api/posts/{id})
        // This method updates an existing blog post by its ID
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBlogPost(int id, BlogPost blogPost)
        {
            // If the ID in the URL doesn't match the ID in the body, return BadRequest
            if (id != blogPost.Id)
            {
                return BadRequest();
            }

            // Validate title and content to ensure they are not empty
            if (string.IsNullOrEmpty(blogPost.Title) || string.IsNullOrEmpty(blogPost.Content))
            {
                return BadRequest("Title and Content are required.");
            }

            // Mark the blog post as modified
            _context.Entry(blogPost).State = EntityState.Modified;

            try
            {
                // Save changes to the database
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle concurrency exception if the blog post is not found in the database
                if (!_context.BlogPosts.Any(e => e.Id == id))
                {
                    return NotFound();  // Return 404 Not Found if the post doesn't exist
                }
                else
                {
                    throw;  // If any other exception occurs, rethrow the error
                }
            }

            return NoContent();  // Return 204 No Content response to indicate the update was successful
        }

        // 5. Delete a blog post (DELETE /api/posts/{id})
        // This method deletes a specific blog post by its ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlogPost(int id)
        {
            // Find the blog post by its ID
            var blogPost = await _context.BlogPosts.FindAsync(id);
            if (blogPost == null)
            {
                return NotFound();  // Return 404 Not Found if the blog post doesn't exist
            }

            // Remove the blog post from the database
            _context.BlogPosts.Remove(blogPost);
            await _context.SaveChangesAsync();  // Save changes to the database

            return NoContent();  // Return 204 No Content to indicate the deletion was successful
        }
    }
}










namespace BlogAPI.Models
{
    // BlogPost model class representing a blog post entity
    public class BlogPost
    {
        public int Id { get; set; }  // Unique identifier for the blog post
        public string Title { get; set; } = string.Empty;  // Title of the blog post
        public string Content { get; set; } = string.Empty;  // Content of the blog post
        public string Author { get; set; } = string.Empty;  // Author of the blog post
    }
}




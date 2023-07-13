using System.ComponentModel.DataAnnotations;

namespace Elasticsearch.Blog.Web.ViewModels;

public record BlogCreateViewModel
{
    [Required]
    public string Title { get; set; }
    
    [Required]
    public string Content { get; set; }
    
    public List<string> Tags { get; set; } = new();
}
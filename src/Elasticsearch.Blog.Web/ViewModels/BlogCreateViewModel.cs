using System.ComponentModel.DataAnnotations;

namespace Elasticsearch.Blog.Web.ViewModels;

public record BlogCreateViewModel
{
    [Required] [Display(Name = "Title")] public string Title { get; set; } = null!;

    [Required] [Display(Name = "Content")] public string Content { get; set; } = null!;

    [Required] [Display(Name = "Tags")] public string Tags { get; set; } = null!;
}
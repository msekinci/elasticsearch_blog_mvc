using Elasticsearch.Blog.Web.Repositories;
using Elasticsearch.Blog.Web.ViewModels;

namespace Elasticsearch.Blog.Web.Services;

public class BlogService
{
    private readonly BlogRepository _blogRepository;

    public BlogService(BlogRepository blogRepository)
    {
        _blogRepository = blogRepository;
    }

    public async Task<bool> SaveAsync(BlogCreateViewModel model)
    {
        Models.Blog newBlog = new()
        {
            Title = model.Title,
            Content = model.Content,
            Tags = model.Tags.ToArray(),
            UserId = Guid.NewGuid(),
            Created = DateTime.Now
        };

        var result = await _blogRepository.SaveAsync(newBlog);
        if (result == null)
        {
            return false;
        }

        return true;
    }
    
}
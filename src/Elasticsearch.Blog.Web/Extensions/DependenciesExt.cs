using Elasticsearch.Blog.Web.Repositories;
using Elasticsearch.Blog.Web.Services;

namespace Elasticsearch.Blog.Web.Extensions;

public static class DependenciesExt
{
    public static void AddDependencies(this IServiceCollection service)
    {
        service.AddScoped<BlogRepository>();
        service.AddScoped<BlogService>();
    }
}
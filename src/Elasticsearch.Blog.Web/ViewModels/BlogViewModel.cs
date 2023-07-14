namespace Elasticsearch.Blog.Web.ViewModels;

public record BlogViewModel(string Id, string Title, string Content, string Tags, Guid UserId, DateTime created);
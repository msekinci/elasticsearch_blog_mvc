using System.Collections.Immutable;
using Elastic.Clients.Elasticsearch;
using Elasticsearch.Blog.Web.Extensions;

namespace Elasticsearch.Blog.Web.Repositories;

public class BlogRepository
{
    private readonly ElasticsearchClient _elasticsearchClient;
    private const string indexName = "blog";

    public BlogRepository(ElasticsearchClient elasticsearchClient)
    {
        _elasticsearchClient = elasticsearchClient;
    }

    public async Task<Models.Blog?> SaveAsync(Models.Blog newBlog)
    {
        newBlog.Created = DateTime.Now;

        var response = await _elasticsearchClient
            .IndexAsync(newBlog,
                x => x.Index(indexName));

        if (!response.IsValidResponse) return null;

        newBlog.Id = response.Id;
        return newBlog;
    }

    public async Task<ImmutableList<Models.Blog>> SearchAsync(string searchText)
    {
        /*
        var result = await _elasticsearchClient
            .SearchAsync<Models.Blog>(s => s
                .Index(indexName)
                .Size(100)
                .Query(q => q
                    .Bool(b => b
                        .Should(s => s
                            .Match(m => m
                                .Field(f => f.Title)
                                .Query(searchText))
                            .MatchBoolPrefix(m => m
                                .Field(f => f.Content)
                                .Query(searchText))))));
                                */
        
        //Eğer yukarıdaki gibi should içindeki ifadeler birbiriylr . ile bağlanırsa AND görevi görür. .MatchBool gibi
        
        var result = await _elasticsearchClient
            .SearchAsync<Models.Blog>(s => s
                .Index(indexName)
                .Size(100)
                .Query(q => q
                    .Bool(b => b
                        .Should(s => s
                            .Match(m => m
                                .Field(f => f.Title)
                                .Query(searchText)),
                            s => s
                                .MatchBoolPrefix(m => m
                                    .Field(f => f.Content)
                                    .Query(searchText))))));

        return result.ConvertImmutableListWithId();
    }
}
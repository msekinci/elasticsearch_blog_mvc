using System.Collections.Immutable;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
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

        List<Action<QueryDescriptor<Models.Blog>>> listQueries = new();

        if (string.IsNullOrWhiteSpace(searchText))
        {
            Action<QueryDescriptor<Models.Blog>> matchAll = (q) => q.MatchAll();
            listQueries.Add(matchAll);
        }
        else
        {
            Action<QueryDescriptor<Models.Blog>> matchTitle = (q) => q.MatchBoolPrefix(m => m.Field(f => f.Title).Query(searchText));
            Action<QueryDescriptor<Models.Blog>> matchContent = (q) => q.Match(m => m.Field(f => f.Content).Query(searchText));
            Action<QueryDescriptor<Models.Blog>> termTags = (q) => q.Term(m => m.Field(f => f.Tags).CaseInsensitive(true).Value(searchText));
            listQueries.Add(matchTitle);
            listQueries.Add(matchContent);
            listQueries.Add(termTags);
        }
        
        var result = await _elasticsearchClient
            .SearchAsync<Models.Blog>(s => s
                .Index(indexName)
                .Size(100)
                .Query(q => q
                    .Bool(b => b
                        .Should(listQueries.ToArray()))));

        return result.ConvertImmutableListWithId();
    }
}
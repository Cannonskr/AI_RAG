using APIRagSoleItem.Models;
using APIRagSoleItem.Repository;

namespace APIRagSoleItem.Services;

public class SoldItemService
{
    private readonly EmbeddingService _embed;
    private readonly SoldItemRepository _repo;

    public SoldItemService(EmbeddingService embed, SoldItemRepository repo)
    {
        _embed = embed;
        _repo = repo;
    }

    public async Task InsertWithEmbeddingAsync(SoldItem item)
    {
        string text = $"{item.ProductName} {item.Category} {item.Note}";
        var embedding = await _embed.GetEmbeddingAsync(text);

        await _repo.InsertAsync(item, embedding);
    }

    public async Task<List<SoldItemDto>> SearchAsync(string query)
    {
        var embedding = await _embed.GetEmbeddingAsync(query);
        return await _repo.SearchAsync(embedding);
    }
}
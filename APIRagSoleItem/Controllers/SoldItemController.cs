using APIRagSoleItem.Models;
using APIRagSoleItem.Services;
using Microsoft.AspNetCore.Mvc;
namespace APIRagSoleItem.Controllers;
[ApiController]
[Route("api/sold-items")]
public class SoldItemController : ControllerBase
{
    private readonly SoldItemService _service;

    public SoldItemController(SoldItemService service)
    {
        _service = service;
    }

    // INSERT
    [HttpPost("insert")]
    public async Task<IActionResult> Insert(SoldItem item)
    {
        await _service.InsertWithEmbeddingAsync(item);
        return Ok("Inserted with vector embedding!");
    }

    // VECTOR SEARCH
    [HttpPost("search")]
    public async Task<IActionResult> Search(SearchRequest req)
    {
        var items = await _service.SearchAsync(req.Query);
        return Ok(items);
    }
}
using System.Net;
using Catalog.Host.Data.Entities;
using Catalog.Host.Models.Response;
using Catalog.Host.Services.Interfaces;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Host.Controllers;

[ApiController]
[Route(ComponentDefaults.DefaultRoute)]
public class CatalogBrandController : ControllerBase
{
    private readonly ILogger<CatalogBrandController> _logger;
    private readonly IBaseCatalogService<CatalogBrand> _catalogBrandService;

    public CatalogBrandController(ILogger<CatalogBrandController> logger, IBaseCatalogService<CatalogBrand> catalogBrandService)
    {
        _logger = logger;
        _catalogBrandService = catalogBrandService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(AddItemResponse<int?>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Add(string name)
    {
        var result = await _catalogBrandService.Add(name, Repositories.Enums.EntityType.CatalogBrand);
        return Ok(new AddItemResponse<int?>() { Id = result });
    }

    [HttpPut]
    [ProducesResponseType(typeof(AddItemResponse<int?>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Update(int id, string name)
    {
        try
        {
            var result = await _catalogBrandService.Update(id, name, Repositories.Enums.EntityType.CatalogBrand);
            return Ok(new AddItemResponse<int?>() { Id = result });
        }
       catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _catalogBrandService.Delete(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
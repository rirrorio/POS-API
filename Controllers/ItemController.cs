using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using POS_API.DTOs;
using POS_API.Services;

namespace POS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly ItemService _itemService;

        public ItemController(ItemService itemService)
        {
            _itemService = itemService;
        }

        // GET: api/items
        [HttpGet]
        public async Task<IActionResult> GetItems()
        {
            var items = await _itemService.GetItemsAsync();
            return Ok(items);
        }

        // GET: api/items/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetItemById(int id)
        {
            var result = await _itemService.GetItemByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        // POST: api/items
        [HttpPost]
        public async Task<IActionResult> CreateItem([FromForm] ItemCreateDTO itemCreateDto)
        {
            if (
                (itemCreateDto.Name == null || itemCreateDto.Name.Length == 0) ||
                (itemCreateDto.Price == null) ||
                (itemCreateDto.Stock == null) ||
                (itemCreateDto.Category == null || itemCreateDto.Category.Length == 0) ||
                (itemCreateDto.Image == null || itemCreateDto.Image.Length == 0)
                )
            {
                return BadRequest("Bad Request : check your input");
            }

            var item = await _itemService.CreateItemAsync(itemCreateDto);
            return Ok(item);
        }

        // PUT: api/items/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(int id, [FromForm] ItemCreateDTO itemCreateDto)
        {
            string result = await _itemService.UpdateItemAsync(id, itemCreateDto);
            if (result.Contains("not found"))
            {
                return NotFound();
            }
            return Ok(result);
        }

        // DELETE: api/items/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            string result = await _itemService.DeleteItemAsync(id);
            if (result.Contains("not found"))
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}

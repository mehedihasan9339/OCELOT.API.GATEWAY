using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace OCELOT.ProjectThree.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // Apply [Authorize] here to require authentication for all actions in this controller
    [Authorize]
    public class ProjectThreeController : ControllerBase
    {
        // In-memory data store (acting like a database)
        private static List<Item> _items = new List<Item>
        {
            new Item { Id = 1, Name = "Item One" },
            new Item { Id = 2, Name = "Item Two" },
            new Item { Id = 3, Name = "Item Three" }
        };

        // GET: api/Item
        [HttpGet]
        public async Task<IActionResult> GetAllItems()
        {
            return Ok(await Task.FromResult(_items));
        }

        // GET: api/Item/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetItemById(int id)
        {
            var item = _items.FirstOrDefault(i => i.Id == id);
            if (item == null)
                return NotFound();
            return Ok(await Task.FromResult(item));
        }

        // POST: api/Item
        [HttpPost]
        public async Task<IActionResult> CreateItem([FromBody] Item model)
        {
            if (model == null)
                return BadRequest("Invalid data.");

            // Simple ID generator
            model.Id = _items.Max(i => i.Id) + 1;
            _items.Add(model);

            return CreatedAtAction(nameof(GetItemById), new { id = model.Id }, model);
        }

        // PUT: api/Item/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(int id, [FromBody] Item model)
        {
            var existingItem = _items.FirstOrDefault(i => i.Id == id);
            if (existingItem == null)
                return NotFound();

            existingItem.Name = model.Name;
            return Ok(await Task.FromResult(existingItem));
        }

        // DELETE: api/Item/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = _items.FirstOrDefault(i => i.Id == id);
            if (item == null)
                return NotFound();

            _items.Remove(item);
            return NoContent();
        }
    }

    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}

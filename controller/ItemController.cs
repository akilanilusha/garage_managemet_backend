using garage_managemet_backend_api.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace garage_managemet_backend_api.controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ItemController(AppDbContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item>>> GetItems()
        {
            var items = await _context.Items
                .Where(i => !i.IsDelete)
                .ToListAsync();
            return Ok(items);
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<Item>> GetItem(int id)
        {
            var item = await _context.Items.FindAsync(id);

            if (item == null || item.IsDelete)
                return NotFound(new { message = "Item not found" });

            return Ok(item);
        }

        
        [HttpPost]
        public async Task<ActionResult<Item>> CreateItem(Item item)
        {
            _context.Items.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetItem), new { id = item.ItemID }, item);
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(int id, Item updatedItem)
        {
            if (id != updatedItem.ItemID)
                return BadRequest(new { message = "Item ID mismatch" });

            var existingItem = await _context.Items.FindAsync(id);
            if (existingItem == null)
                return NotFound(new { message = "Item not found" });

            existingItem.ItemName = updatedItem.ItemName;
            existingItem.ItemCode = updatedItem.ItemCode;
            existingItem.Qty = updatedItem.Qty;
            existingItem.MesureUnit = updatedItem.MesureUnit;
            existingItem.UnitPrice = updatedItem.UnitPrice;
            existingItem.Description = updatedItem.Description;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Item updated successfully" });
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null)
                return NotFound(new { message = "Item not found" });

            
            item.IsDelete = true;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Item deleted successfully" });
        }

        
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Item>>> SearchItemByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest(new { message = "Search term cannot be empty" });

            var items = await _context.Items
                .Where(i => !i.IsDelete && EF.Functions.Like(i.ItemName, $"%{name}%"))
                .ToListAsync();

            if (items.Count == 0)
                return NotFound(new { message = "No matching items found" });

            return Ok(items);
        }

    }
}

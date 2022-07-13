using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlazorAPI.Models;

namespace BlazorAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly NewTestDateBaseContext _context;

        public CategoriesController(NewTestDateBaseContext context)
        {
            _context = context;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ObjectResult>>> GetCategory()
        {
            if (_context.Category == null)
            {
                 return NotFound();
            }
            List<Category> categorys = await _context.Category.Include(c => c.IdO).ToListAsync();
            List<ObjectResult> categoryResult = new List<ObjectResult>();
            foreach (Category _category in categorys)
            {
                foreach (Objects _objects in _category.IdO)
                {
                    categoryResult.Add(new ObjectResult(_objects, _category));
                }
            }
            return categoryResult;
        }
        // GET: api/Categories/5
        [HttpGet("{id}")] 
        public async Task<ActionResult<List<ObjectResult>>> GetCategory(string id)
        {
            List<ObjectResult> categoryResult = new List<ObjectResult>();
            List<Category> categorys;

            if (_context.Category == null)
            {
                return NotFound();
            }

            if ( Int32.TryParse(id, out int l))
            {
                categorys = await _context.Category.Where(d => (d.Id == l)).Include(c => c.IdO).ToListAsync();
            }
            else 
            {
                categorys = await _context.Category.Where(d => d.Name == id).Include(c => c.IdO).ToListAsync();
            }
            if (categorys == null)
            {
                return NotFound();
            }
            foreach (Category _category in categorys)
            { 
                foreach(Objects _objects in _category.IdO)
                {
                    categoryResult.Add(new ObjectResult(_objects, _category));
                }
            }   
            return categoryResult;
        }

        // PUT: api/Categories/5
        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, Category category)
        {
            if (id != category.Id)
            {
                return BadRequest();
            }

            _context.Entry(category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
          if (_context.Category == null)
          {
              return Problem("Entity set 'NewTestDateBaseContext.Category'  is null.");
          }
            _context.Category.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategory", new { id = category.Id }, category);
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            if (_context.Category == null)
            {
                return NotFound();
            }
            var category = await _context.Category.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Category.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CategoryExists(int id)
        {
            return (_context.Category?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
    public class ObjectResult
    {
        public string Category { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public ObjectResult()
        {
            
        }
        public ObjectResult(Objects obj, Category cat)
        {
            Category = cat.Name;
            Name = obj.Name;
            Color = obj.Color;
            Size = obj.Size;
        }
    }
}

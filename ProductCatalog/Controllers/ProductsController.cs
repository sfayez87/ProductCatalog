using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Data;
using ProductCatalog.Models;
using ProductCatalog.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalog.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> ProductsManagement()
        {
            return View("ProductsManagement", await _context.Products.Include(a=>a.Category).ToListAsync());
        }

        // GET: Products
        public async Task<IActionResult> Index(int? categoryId)
        {
            ViewBag.CategoriesList = _context.Categories.ToList();
            if (categoryId==null)
            {
                return View(await _context.Products.Where(p => DateTime.Now >= p.StartDate && DateTime.Now <= p.StartDate.AddDays(p.Duration)).Include(a => a.Category).ToListAsync());
            }
            return View(await _context.Products.Where(p=>DateTime.Now>=p.StartDate && DateTime.Now <=p.StartDate.AddDays(p.Duration) && p.CategoryId== categoryId).Include(a => a.Category).ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            var productVM = new ProductViewModel
            {
                Categories = await _context.Categories.ToListAsync()
            };
            return View(productVM);
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,StartDate,Duration,Price,CategoryId")] ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                Product product = new Product
                {
                    CreatedByUserId=User.Identity.Name,
                    Name = model.Name,
                    StartDate = model.StartDate,
                    Duration = model.Duration,
                    Price = model.Price,
                    CategoryId = model.CategoryId,
                };
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ProductsManagement));
            }
            return View(model);
        }

        // GET: Products/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            var productVM = new ProductViewModel
            {
                Id=product.Id,
                Name=product.Name,
                StartDate=product.StartDate,
                Duration=product.Duration,
                Price=product.Price,
                CategoryId = product.CategoryId,
                Categories = await _context.Categories.ToListAsync()
            };
            return View(productVM);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,StartDate,Duration,Price,CategoryId")] ProductViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Product product = new Product
                    {
                        Id=model.Id,
                        CreatedByUserId = User.Identity.Name,
                        Name = model.Name,
                        StartDate = model.StartDate,
                        Duration = model.Duration,
                        Price = model.Price,
                        CategoryId = model.CategoryId,
                    };
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ProductsManagement));
            }
            return View(model);
        }

        // GET: Products/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ProductsManagement));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}

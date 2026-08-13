using Microsoft.AspNetCore.Mvc;
using ShopVanPhongPham.Helpers;
using ShopVanPhongPham.Models.Interfaces;
using ShopVanPhongPham.Data; 

namespace ShopVanPhongPham.Controllers;

public class ProductController : Controller
{
    private readonly IProductRepository _productRepo;
    private readonly AppDbContext _context;

    public ProductController(IProductRepository productRepo, AppDbContext context)
    {
        _productRepo = productRepo;
        _context = context;
    }
    public IActionResult Shop(string? search, string? category)
    {
        var allProducts = _productRepo.GetAllProducts();

        var categories = _context.Categories
            .OrderBy(c => c.Name)
            .Select(c => c.Name)
            .ToList();  

        var products = allProducts.AsEnumerable();

        if (!string.IsNullOrEmpty(search))
        {
            var keyword = StringHelper.RemoveDiacritics(search);
            products = products.Where(p =>
                StringHelper.RemoveDiacritics(p.Name).Contains(keyword));
        }

        if (!string.IsNullOrEmpty(category))
            products = products.Where(p => p.Category != null && p.Category.Name == category);

        ViewBag.Search = search;
        ViewBag.Category = category;
        ViewBag.Categories = categories;

        return View(products.ToList());
    }

    public IActionResult Detail(int id)
    {
        var product = _productRepo.GetProductById(id);
        if (product == null) return NotFound();
        return View(product);
    }
}
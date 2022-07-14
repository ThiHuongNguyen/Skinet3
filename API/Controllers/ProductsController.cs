using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]                 // Atribute
    [Route("api/[controller]")]     // Route
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repo;

        public ProductsController(IProductRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]       // HTTP Method
        public async Task<ActionResult<List<Product>>> GetProducts() //ActionResult from ControllerBase
        {
            var products = await _repo.GetProductsAsync();

            return Ok(products);
        }

        [HttpGet("{id}")]    // id is HTTP parameter, HTTP get method: products/id
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            return await _repo.GetProductByIdAsync(id);
        }      

        [HttpGet("brands")]       // HTTP get method: products/brands
        public async Task<ActionResult<List<ProductBrand>>> GetProductBrands() //ActionResult from ControllerBase
        {
            return Ok(await _repo.GetProductBrandsAsync());
        }  

        [HttpGet("types")]       // HTTP get method: products/types
        public async Task<ActionResult<List<ProductType>>> GetProductTypes() //ActionResult from ControllerBase
        {
            return Ok(await _repo.GetProductTypesAsync());
        }  
    }
}
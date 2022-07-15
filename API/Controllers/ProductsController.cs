using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]                 // Atribute
    [Route("api/[controller]")]     // Route
    public class ProductsController : ControllerBase
    {
        
        private readonly IGenericRepository<Product> _productsRepo;
        private readonly IGenericRepository<ProductBrand> _ProductBrandsRepo;
        private readonly IGenericRepository<ProductType> _productTypesRepo;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> productsRepo, 
            IGenericRepository<ProductBrand> ProductBrandsRepo, 
            IGenericRepository<ProductType> productTypesRepo, IMapper mapper)
        {
            _mapper = mapper;
            _productTypesRepo = productTypesRepo;
            _ProductBrandsRepo = ProductBrandsRepo;
            _productsRepo = productsRepo;
        }

        [HttpGet]       // HTTP Method
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts() //ActionResult from ControllerBase
        {
            var spec = new ProductsWithTypesAndBrandsSpecification();

            var products = await _productsRepo.ListAsync(spec);

            return Ok(_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products));
        }

        [HttpGet("{id}")]    // id is HTTP parameter, HTTP get method: products/id
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);

            var product = await _productsRepo.GetEntityWithSpec(spec);

            return _mapper.Map<Product, ProductToReturnDto>(product);
        }      

        [HttpGet("brands")]       // HTTP get method: products/brands
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands() //ActionResult from ControllerBase
        {
            return Ok(await _ProductBrandsRepo.ListAllAsync());
        }  

        [HttpGet("types")]       // HTTP get method: products/types
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes() //ActionResult from ControllerBase
        {
            return Ok(await _productTypesRepo.ListAllAsync());
        }  
    }
}
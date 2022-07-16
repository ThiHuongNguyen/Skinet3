using API.Dtos;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
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
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts(
            [FromQuery]ProductSpecParams productParams) //ActionResult from ControllerBase  // FromQuery: Look up params value from Query
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(productParams);

            var countSpec = new ProductWithFiltersForCountSpecification(productParams);

            var totalItems = await _productsRepo.CountAsync(countSpec);

            var products = await _productsRepo.ListAsync(spec);

            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

            return Ok(new Pagination<ProductToReturnDto>(productParams.PageIndex, productParams.PageSize, totalItems, data));
        }

        [HttpGet("{id}")]    // id is HTTP parameter, HTTP get method: products/id
        [ProducesResponseType(StatusCodes.Status200OK)] // Add more information for swagger
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)] // typeof(ApiResponse): customise return data type for swagger
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);

            var product = await _productsRepo.GetEntityWithSpec(spec);

            if (product == null) return NotFound(new ApiResponse(404));

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
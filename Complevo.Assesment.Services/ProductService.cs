using AutoMapper;
using Complevo.Assesment.Data;
using Complevo.Assesment.Data.Entities;
using Complevo.Assesment.Services.BusinessException;
using Complevo.Assesment.Services.Dto;
using Microsoft.EntityFrameworkCore;

namespace Complevo.Assesment.Services
{
    public class ProductService
    {
        private readonly IMapper _mapper;
        private readonly ApplicationContext _productContext;

        public ProductService(IMapper mapper, ApplicationContext productContext)
        {
            _mapper = mapper;
            _productContext = productContext;
        }

        public async Task<ProductDto> GetProduct(long id)
        {
            var product = _productContext.Products.FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                throw new ProductNotFoundException($"Product with Id = {id} does not exist");
            }
            var dto = _mapper.Map<ProductDto>(product);
            return dto;
        }

        public async Task<IEnumerable<ProductDto>> GetProducts(int? limit, int? offset)
        {
            IQueryable<Product> products = _productContext.Products
                .OrderBy(x => x.Id);
            if (offset.HasValue)
            {
                products = products.Skip(offset.Value);
            }
            if (limit.HasValue)
            {
                products = products.Take(limit.Value);
            }            
            var prodEntities = await products.ToListAsync();
            var prodDtos = _mapper.Map<IEnumerable<ProductDto>>(prodEntities);
            return prodDtos;
        }

        public async Task<ProductDto> CreateProduct(ProductDto productDto)
        {
            var entity = _mapper.Map<Product>(productDto);
            if (entity.Id != 0)
            {
                var existingWithid = await _productContext.Products.FirstOrDefaultAsync(x => x.Id == entity.Id);
                if (existingWithid != null)
                {
                    throw new ProductAlreadyExistsException($"Product with Id={entity.Id} already exists");
                }
            }
            var existingProduct = await _productContext.Products.FirstOrDefaultAsync(x => x.Name.ToLower() == productDto.Name.ToLower());
            if (existingProduct != null)
            {
                throw new ProductAlreadyExistsException($"Product with Name {productDto.Name} already exists");
            }
            _productContext.Products.Add(entity);
            var ops = await _productContext.SaveChangesAsync();

            var resultDto = _mapper.Map<ProductDto>(entity);
            return resultDto;

        }

        public async Task<ProductDto> UpdateProduct(ProductDto product)
        {
            if (!product.Id.HasValue || product.Id == 0)
            {
                throw new ProductIdCannotBeZeroException();
            };
            var existingProduct = await _productContext.Products.FirstOrDefaultAsync(x => x.Id == product.Id);
            if (existingProduct is null)
            {
                //todo todo proper exception
                throw new ProductNotFoundException($"Product with Id = {product.Id} does not exist");
            }
            _mapper.Map(product, existingProduct);
            var ops = await _productContext.SaveChangesAsync();

            var resultDto = _mapper.Map<ProductDto>(product);
            return resultDto;
        }

        public async Task DeleteProduct(long id)
        {
            if (id == 0)
            {
                throw new ProductIdCannotBeZeroException();
            };
            var existingProduct = await _productContext.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (existingProduct is null)
            {
                throw new ProductNotFoundException($"Product with Id = {id} does not exist");
            }
            _productContext.Products.Remove(existingProduct);
            var ops = await _productContext.SaveChangesAsync();
        }




    }
}

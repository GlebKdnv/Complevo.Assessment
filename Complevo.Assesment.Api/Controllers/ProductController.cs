using Complevo.Assesment.Services;
using Complevo.Assesment.Services.BusinessException;
using Complevo.Assesment.Services.Dto;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Complevo.Assesment.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }


        /// <summary>
        /// List all available products
        /// </summary>
        /// <param name="limit">number of rows to retrieve</param>
        /// <param name="offset">number of rows to skip</param>
        /// <returns></returns>
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<ProductDto>>> Get(int? limit, int? offset)
        {
            if (!ModelState.IsValid || limit <= 0 || offset <= 0)
            {
                return BadRequest();
            }
            try
            {
                var result = await _productService.GetProducts(limit, offset);
                return Ok(result);
            }
            catch (ApplicationException ex)
            {
                return SetError<IEnumerable<ProductDto>>(ex);
            }

        }

        /// <summary>
        /// Retrieves product by Id
        /// </summary>
        /// <param name="id">Unique identifier</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> Get(long id)
        {
            if (id < 0)
            {
                return BadRequest();
            }

            try
            {
                var result = await _productService.GetProduct(id);
                return Ok(result);
            }
            catch (ApplicationException ex)
            {
                return SetError<ProductDto>(ex);
            }

        }

        /// <summary>
        /// Create new product
        /// </summary>
        /// <param name="product">product model to create</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ProductDto>> Post([FromBody] ProductDto product)
        {
            if (!ModelState.IsValid)
            {
                return this.UnprocessableEntity(ModelState);
            }
            try
            {
                var result = await _productService.CreateProduct(product);
                Response.StatusCode = StatusCodes.Status201Created;
                return Created($"api/Products/{result.Id}",result);
            }
            catch (ApplicationException ex)
            {
                return SetError<ProductDto>(ex);
            }

        }

        /// <summary>
        /// Update existing product
        /// </summary>
        /// <param name="id">id of product to update</param>
        /// <param name="value">product model</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] ProductDto value)
        {
            if (!ModelState.IsValid)
            {
                return this.UnprocessableEntity(ModelState);
            }
            if (id != value.Id)
            {
                return BadRequest("Incompatible Ids");
            }
            try
            {
                var result = await _productService.UpdateProduct(value);
            }
            catch (ApplicationException ex)
            {
                return SetError(ex);
            }

            return Ok();
        }

        /// <summary>
        /// Delete existing product
        /// </summary>
        /// <param name="id">Id of product to delete</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await _productService.DeleteProduct(id);
                return Ok();
            }
            catch(ApplicationException ex)
            {
                return SetError(ex);
            }
        }

        #region Error handling
        private ActionResult<T> SetError<T>(ApplicationException appExc)
        {
            return SetRespStatus(appExc);
        }
        private IActionResult SetError(ApplicationException appExc)
        {
            return SetRespStatus(appExc);
        }
        private ActionResult SetRespStatus(ApplicationException appExc)
        {
            switch (appExc)
            {
                case ProductNotFoundException ex:
                    return NotFound(ex.Message);
                case ProductAlreadyExistsException ex:
                    return Conflict(ex.Message);
                case ProductIdCannotBeZeroException ex:
                    return BadRequest(ex.Message);
                default:
                    Response.StatusCode = StatusCodes.Status500InternalServerError;
                    return Problem(appExc.Message);

            }
        }
        #endregion
    }
}

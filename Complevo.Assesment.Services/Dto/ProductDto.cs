using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Complevo.Assesment.Services.Dto
{
    /// <summary>
    /// Product Data Transfer Object
    /// </summary>
    public class ProductDto
    {
        /// <summary>
        /// Unique Id of Product
        /// </summary>
        public long? Id { get; set; }
        /// <summary>
        /// Name of Product
        /// </summary>
        /// <remarks>Shoould Be unique among all products</remarks>
        public string Name { get; set; }
        /// <summary>
        /// Text description of product
        /// </summary>
        public string Description { get; set; }
    }
}

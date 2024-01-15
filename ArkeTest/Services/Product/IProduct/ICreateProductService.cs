using ArkeTest.DTO;
using ArkeTest.DTO.Product;

namespace ArkeTest.Services.Product.IProduct
{
    public interface ICreateProductService
    {
        Task<ReturnDTO> CreateProduct(CreateProductDTO dto);
    }
}

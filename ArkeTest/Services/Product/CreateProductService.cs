using ArkeTest.Data;
using ArkeTest.DTO;
using ArkeTest.DTO.Product;
using ArkeTest.Models;
using ArkeTest.Services.Jwt.IJwt;
using ArkeTest.Services.Product.IProduct;
using ArkeTest.Services.User;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ArkeTest.Services.Product
{
    public class CreateProductService : ICreateProductService
    {
        private readonly MyDbContext _db;
        private readonly IJwtService _jwtService;
        private readonly ILogger<CreateProductService> _logger;

        public CreateProductService(MyDbContext db,
                                 IJwtService jwtService,
                                 ILogger<CreateProductService> logger)
        {
            _db = db;
            _jwtService = jwtService;
            _logger = logger;
        }

        public async Task<ReturnDTO> CreateProduct(CreateProductDTO dto){
            try
            {
                string? id = _jwtService.GetAndDecodeJwtToken();
                if (id == null)
                {
                    ReturnDTO returnDTO = new()
                    {
                        Message = "User not logged in",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                    _logger.LogInformation("User not logged in");

                    return returnDTO;
                }
                else
                {
                    ApplicationUser? login = await _db.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == id);
                    if (login == null)
                    {
                        ReturnDTO returnDTO = new()
                        {
                            Message = "Login not found",
                            StatusCode = HttpStatusCode.NotFound
                        };
                        _logger.LogInformation("Login not found");

                        return returnDTO;
                    }
                    else
                    {
                        Users? existingUser = await _db.UserInformations.FirstOrDefaultAsync(x => x.LoginId == login.Id);
                        if (existingUser == null)
                        {
                            ReturnDTO returnDTO = new()
                            {
                                Message = "User not found",
                                StatusCode = HttpStatusCode.NotFound
                            };
                            _logger.LogInformation("User not found");

                            return returnDTO;
                        }
                        else
                        {
                            Products? existingProduct = await _db.Products.FirstOrDefaultAsync(x => x.Name == dto.Name && x.Description == x.Description && x.IsActive == true);
                            if(existingProduct != null){
                                ReturnDTO returnDTO = new()
                                {
                                    Message = "Product already exists",
                                    StatusCode = HttpStatusCode.Conflict
                                };
                                _logger.LogInformation("Product already exists");

                                return returnDTO;
                            }
                            else{
                                Products product = new(dto.Name, dto.Description, dto.Price, dto.Quantity, existingUser.Id);
                                await _db.Products.AddAsync(product);
                                await _db.SaveChangesAsync();

                                ReturnDTO returnDTO = new()
                                {
                                    Message = "Product created",
                                    StatusCode = HttpStatusCode.Created
                                };
                                _logger.LogInformation("Product created");

                                return returnDTO;
                            }
                        }
                    }
                }
            }
            // If there is an exception, return a 500
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal error creating or updating user");

                ReturnDTO returnDTO = new()
                {
                    Message = "Internal error creating or updating user",
                    StatusCode = HttpStatusCode.InternalServerError
                };

                return returnDTO;
            }
        }
    }
}

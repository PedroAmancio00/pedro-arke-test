using ArkeTest.Data;
using ArkeTest.DTO;
using ArkeTest.DTO.User;
using ArkeTest.Models;
using ArkeTest.Services.Login;
using ArkeTest.Services.Login.ILogin;
using ArkeTest.Services.User.IUser;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Xml.Linq;

namespace ArkeTest.Services.User
{
    public class CreateOrUpdateUserService : ICreateUser
    {
        private readonly MyDbContext _db;
        private readonly IJwtService _jwtService;
        private readonly ILogger<CreateOrUpdateUserService> _logger;

        public CreateOrUpdateUserService(MyDbContext db,
                                 IJwtService jwtService,
                                 ILogger<CreateOrUpdateUserService> logger)
        {
            _db = db;
            _jwtService = jwtService;
            _logger = logger;
        }

        public async Task<ReturnDTO> CreateOrUpdateUser(CreateUserDTO dto){
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
            else{
                var login = await _db.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == id);
                if (login == null)
                {
                    ReturnDTO returnDTO = new()
                    {
                        Message = "Login not found",
                        StatusCode = HttpStatusCode.NotFound
                    };
                    _logger.LogInformation("User not logged in");

                    return returnDTO;
                }
                else
                {
                    var existingUser = await _db.UserInformations.FirstOrDefaultAsync(x => x.LoginId == login.Id);
                    
                    if (existingUser != null)
                    {
                        existingUser.Name = dto.Name;
                        existingUser.AddressLine1 = dto.AddressLine1 ?? null;
                        existingUser.AddressLine2 = dto.AddressLine2 ?? null;
                        existingUser.UpdateModifiedAt();
                        await _db.SaveChangesAsync();
                        ReturnDTO returnDTO = new()
                        {
                            Message = "User updated",
                            StatusCode = HttpStatusCode.OK
                        };
                        _logger.LogInformation("User already exists");

                        return returnDTO;
                    }
                    else
                    {
                        UserInformation newUser = new(dto.Name, id, dto.AddressLine1 ?? null, dto.AddressLine2 ?? null);

                        await _db.UserInformations.AddAsync(newUser);

                        await _db.SaveChangesAsync();

                        ReturnDTO returnDTO = new()
                        {
                            Message = "User created",
                            StatusCode = HttpStatusCode.Created
                        };
                        _logger.LogInformation("User created");

                        return returnDTO;
                    }
                }
            }
        }


        /*
        public async Task<ReturnDTO> CreateUser(CreateUserDTO dto)
        {
            try
            {
                // Check if the user already exists
                ApplicationUser? user = await _db.ApplicationUsers.FirstOrDefaultAsync(x => x.Email == dto.Email);

                // If the user already exists, return a 400
                if (user != null)
                {
                    ReturnDTO returnDTO = new()
                    {
                        Message = "User already exists",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                    _logger.LogInformation("User already exists");

                    return returnDTO;
                }

                // Create a new user
                ApplicationUser newUser = new()
                {
                    Email = dto.Email,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Password = dto.Password,
                    UserName = dto.Email
                };

                // Add the user to the database
                await _db.ApplicationUsers.AddAsync(newUser);
                await _db.SaveChangesAsync();

                ReturnDTO returnDTO = new()
                {
                    Message = "User created successfully",
                    StatusCode = HttpStatusCode.OK
                };
                _logger.LogInformation("User created successfully");

                return returnDTO;
            }
            // If an error occurs, return a 500
            catch (Exception ex)
            {
                _logger.LogError(ex, "User creation failed");
                ReturnDTO returnDTO = new()
                {
                    Message = "Error creating user",
                    StatusCode = HttpStatusCode.InternalServerError
                };

                return returnDTO;
            }
        }   
        */

    }
}

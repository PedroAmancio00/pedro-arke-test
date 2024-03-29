﻿using ArkeTest.Data;
using ArkeTest.DTO;
using ArkeTest.DTO.User;
using ArkeTest.Models;
using ArkeTest.Services.Jwt.IJwt;
using ArkeTest.Services.User.IUser;
using Microsoft.EntityFrameworkCore;
using System.Net;

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

        public async Task<ReturnDTO> CreateOrUpdateUser(CreateUserDTO dto)
        {
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
                        _logger.LogInformation("User not logged in");

                        return returnDTO;
                    }
                    else
                    {
                        UserInformation? existingUser = await _db.UserInformations.FirstOrDefaultAsync(x => x.LoginId == login.Id);

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
                            _logger.LogInformation("User updated");

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

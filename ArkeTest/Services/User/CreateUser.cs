using ArkeTest.Data;
using ArkeTest.DTO;
using ArkeTest.DTO.User;
using ArkeTest.Services.Login;
using ArkeTest.Services.Login.ILogin;
using ArkeTest.Services.User.IUser;

namespace ArkeTest.Services.User
{
    public class CreateUser : ICreateUser
    {
        private readonly MyDbContext _db;
        private readonly ILogger<CreateUser> _logger;

        public CreateUser(MyDbContext db, ILogger<CreateUser> logger)
        {
            _db = db;
            _logger = logger;
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

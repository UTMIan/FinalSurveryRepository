using AutoMapper;
using FinalSurveyPractice.Data;
using FinalSurveyPractice.DTOs.AuthUser;
using FinalSurveyPractice.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FinalSurveyPractice.Services
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public AuthService(DataContext context, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
        }
//----------------------------------------------------------------------------------------
        public async Task<ServiceResponse<string>> Login(string username, string password)
        {
            var resp = new ServiceResponse<string>();
            var user = await _context.User
                .Include(r => r.Role)
                .FirstOrDefaultAsync(c => c.Name.ToLower().Equals(username.ToLower()));

            if (username == null)
            {
                resp.Success = false;
                resp.Message = "User not found";
            }
            else if (!verifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                resp.Success = false;
                resp.Message = "Wrong password";
            }
            else
            {
                resp.Data = CreateToken(user);
            }

            return resp;
        }
    

        public async Task<ServiceResponse<int>> Register(User user, string password)
        {
            ServiceResponse<int> resp = new ServiceResponse<int>();
            if (await UserExist(user.Name))
            {
                resp.Success = false;
                resp.Message = "El usuario ya existe";
                return resp;
            }

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _context.User.Add(user);

            await _context.SaveChangesAsync();
            resp.Data = user.IdUser;

            return resp;
        }

        public async Task<ServiceResponse<GetUserDto>> UpdateIdUser(User user, string password, int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UserExist(string username)
        {
            if (await _context.User.AnyAsync(c => c.Name.ToLower() == username.ToLower()))
            {
                return true;
            }
            return false;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA256())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool verifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA256(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                return computeHash.SequenceEqual(passwordHash);
            }
        }

        private string CreateToken(User user)
        {
            //Listado de Parametros que tendrà el Json Token
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.IdUser.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Surname, user.FirstSurname),
                //new Claim(ClaimTypes.Role, consumer.Role)
            };

            foreach (var role in user.Role)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8
                .GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public async Task<ServiceResponse<GetUserDto>> UpdateUser(User user, string password, int id)
        {
            ServiceResponse<GetUserDto> response = new ServiceResponse<GetUserDto>();

            try
            {
                if (await UserIdExist(id))
                {
                    CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                    user.IdUser = id;

                    _context.Entry(user).State = EntityState.Modified;

                    await _context.SaveChangesAsync();

                    response.Data = _mapper.Map<GetUserDto>(user);
                }
                else
                {
                    response.Success = false;
                    response.Message = "Usuario no Encontrado";
                }
            }
            catch (DbUpdateException ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
            //throw new NotImplementedException();
        }

        public async Task<bool> UserIdExist(int id)
        {
            if (await _context.User.AnyAsync(u => u.IdUser.Equals(id)))
            {
                return true;
            }

            return false;
        }
    }
}
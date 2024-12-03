using AutoMapper;
using Doantour.DTO;
using Doantour.Models;
using Doantour.Repository;
using Doantour.Request;
using Doantour.Respository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Doantour.Service
{
    public class AccountService
    {
        private readonly RoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AccountService(IMapper mapper, IServiceProvider serviceProvider, UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager, IConfiguration configuration)
        {
            var _repositoryFactory = serviceProvider.GetService<RepositoryFactory>();
            _roleRepository = _repositoryFactory.RoleRepository;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task<IdentityResult> Register(RegisterModel account)
        {

            var user = new AppUser()
            {
                FullName = account.Fullname,
                Phone = account.Phone,
                PhoneNumber = account.Phone,
                RoleId = account.RoleId,
                UserName = account.Email,
                Email = account.Email,
                PasswordHash = account.Password,
            };

            var result = await _userManager.CreateAsync(user, user.PasswordHash!);

            return result;
        }

        public async Task<AuthResponseDTO> Login(string email, string password)
        {
            var signInResult = await _signInManager.PasswordSignInAsync(
                userName: email!,
                password: password!,
                isPersistent: false,
                lockoutOnFailure: false
            );
            if (signInResult.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(email);
                var role = await _roleRepository.SearchAsync(r => r.Id == user.RoleId);
                // Tạo claims cho token
                var claims = new[]
                {
                    new Claim("Email", email),
                    new Claim("UserFullName", user.FullName),
                    new Claim("Id", user.Id),
                    new Claim("Role", role[0].RoleName),
                };

                // Lấy cấu hình JWT từ appsettings.json
                var jwtConfig = _configuration.GetSection("JwtConfig");
                var secretKeyString = jwtConfig["SecretKey"];
                var issuer = jwtConfig["Issuer"];
                var audience = jwtConfig["Audience"];
                var tokenExpiryInDays = Convert.ToDouble(jwtConfig["TokenExpiryInDays"]);

                // Sử dụng secretKeyString trong mã của bạn
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKeyString));

                // Tạo credentials từ key
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                // Tạo token
                var securityToken = new JwtSecurityToken(
                    issuer: issuer,
                    audience: audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddDays(tokenExpiryInDays),
                    signingCredentials: creds
                );

                var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

                // Trả về token dưới dạng chuỗi
                return new AuthResponseDTO
                {
                    Token = token,
                    UserId = user.Id,
                    UserFullName = user.FullName,
                    Email = email,
                    Role = role[0].RoleName
                };
            }
            return null;
        }

        public async Task<bool> ChangePassword(string accountId, string oldPassword, string newPassword)
        {

            if (accountId != "" || accountId != null)
            {
                var user = await _userManager.FindByIdAsync(accountId);
                if (user == null)
                {
                    return false;
                }

                var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);

                if (result.Succeeded)
                {
                    return true;
                }

                return false;
            }

            return false;
        }

        public async Task<string> Logout()
        {
            await _signInManager.SignOutAsync();
            return "Đã đăng xuất thành công!";
        }

        public List<AccountDTO> GetAllAccount()
        {
            var users = _userManager.Users.ToList();
            var usersDTO = _mapper.Map<List<AccountDTO>>(users);
            return usersDTO;
        }

        public List<AccountDTO> GetAccountsByName(string name)
        {
            var users = _userManager.Users.Where(x => x.FullName.Contains(name)).ToList();
            var usersDTO = _mapper.Map<List<AccountDTO>>(users);
            return usersDTO;
        }

        public async Task<AccountDTO> GetAccountByEmail(string email)
        {
            var users = await _userManager.FindByEmailAsync(email);

            var userDTO = _mapper.Map<AccountDTO>(users);
            return userDTO;
        }

        public async Task<AccountDTO> GetAccountById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var userDTO = _mapper.Map<AccountDTO>(user);
            return userDTO;
        }

        public async Task<string?> GetAccountId(string email)
        {
            var userByEmail = await _userManager.FindByEmailAsync(email);
            if (userByEmail == null)
            {
                return "";
            }

            var userId = await _userManager.GetUserIdAsync(userByEmail);

            return userId;
        }

        public async Task<IdentityResult> UpdateAccount(string id, EditAccountRequest account)
        {
            try
            {
                if (account.Id == null || !account.Id.Equals(id))
                {
                    return IdentityResult.Failed();
                }

                var user = await _userManager.FindByIdAsync(id);

                if (user == null)
                {
                    return IdentityResult.Failed();
                }

                // Use AutoMapper to map properties from EditAccountRequest to AppUser
                _mapper.Map(account, user);

                var result = await _userManager.UpdateAsync(user);

                return result;
            }
            catch (DbUpdateConcurrencyException)
            {
                return IdentityResult.Failed();
            }
        }

        public async Task<IdentityResult> editPassWord(string id, string password)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);

                if (user == null)
                {
                    return IdentityResult.Failed();
                }

                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);


                var result = await _userManager.ResetPasswordAsync(user, resetToken, password);

                return result;
            }
            catch (DbUpdateConcurrencyException)
            {
                return IdentityResult.Failed();
            }
        }

        public async Task<IdentityResult> DeleteAccount(string id)
        {
            var account = await _userManager.FindByIdAsync(id);
            if (account == null)
            {
                return IdentityResult.Failed();
            }

            var result = await _userManager.DeleteAsync(account);

            return result;
        }


    }
}

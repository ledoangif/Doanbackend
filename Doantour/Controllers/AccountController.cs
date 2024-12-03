using Doantour.Models;
using Doantour.Request;
using Doantour.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Services.Users;
using NetTopologySuite.Triangulate.Tri;
using System.Net;
using System.Text.RegularExpressions;
using static Microsoft.VisualStudio.Services.Graph.GraphResourceIds;

namespace Doantour.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _service;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(ServiceFactory serviceFactory, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _service = serviceFactory.AccountService;
            _userManager = userManager;
            _signInManager = signInManager; ;
        }
        //Thêm tài khoản , role (1: admin,2: user)
        [HttpPost("register")]
        public async Task<ActionResult<ResponseFormat>> Register(RegisterModel account)
        {
            if (string.IsNullOrEmpty(account.Fullname) || string.IsNullOrEmpty(account.Phone)|| string.IsNullOrEmpty(account.Email)|| string.IsNullOrEmpty(account.Password))
            {
                return BadRequest("Thông tin này không được để trống");
            }
            if (!Regex.IsMatch(account.Fullname, @"^[A-Za-zÀ-ỹ\s]+$"))
            {
                return BadRequest("Họ tên chỉ được chứa ký tự chữ cái.");
            }
            if (!Regex.IsMatch(account.Phone, @"^((03|05|07|08|09|01[2|6|8|9])\d{8}|\+84\d{9})$")) 
            {
                return BadRequest("Số điện thoại không đúng");
            }
            // Kiểm tra nếu email bị thiếu hoặc không hợp lệ
            if (!Regex.IsMatch(account.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                return BadRequest("Vui lòng nhập một email hợp lệ");
            }
            // Kiểm tra nếu mật khẩu bị thiếu hoặc không đủ điều kiện
            if (account.Password.Length < 5)
            {
                return BadRequest("Mật khẩu phải có ít nhất 5 ký tự");
            }
            if (!Regex.IsMatch(account.Password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{5,}$"))
            {
                return BadRequest("Mật khẩu phải có ít nhất 5 ký tự gồm chữ hoa, chữ thường và số");
            }
            // Kiểm tra tài khoản đã tồn tại trong cơ sở dữ liệu chưa
            var existingUser = await _userManager.FindByEmailAsync(account.Email);
            if (existingUser != null)
            {
                return BadRequest("Email này đã tồn tại.");
            }
            var result = await _service.Register(account);
            if (result.Succeeded)
            {
                return new ResponseFormat(HttpStatusCode.OK, "Thêm tài khoản thành công!", null);
            }
            return BadRequest("Thêm tài khoản thất bại");
        }
        // Đăng nhập
        [HttpPost("login")]
        public async Task<ActionResult<ResponseFormat>> Login([FromBody] AccountLogin loginModel)
        {
            if (string.IsNullOrEmpty(loginModel.Email))
            {
                return BadRequest("Email không được để trống");
            }

            if (string.IsNullOrEmpty(loginModel.Password))
            {
                return BadRequest("Mật khẩu không được để trống");
            }

            if (!Regex.IsMatch(loginModel.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                return BadRequest("Vui lòng nhập email hợp lệ");
            }

            if (loginModel.Password.Length < 5)
            {
                return BadRequest("Mật khẩu phải có ít nhất 5 ký tự");
            }

            if (!Regex.IsMatch(loginModel.Password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{5,}$"))
            {
                return BadRequest("Mật khẩu phải có ít nhất 5 ký tự gồm chữ hoa, chữ thường và số");
            }

            // Tìm tài khoản trong hệ thống
            var user = await _userManager.FindByEmailAsync(loginModel.Email);
            if (user == null)
            {
                //return NotFound(new { Message = "Tài khoản không tồn tại!" });
                return null;
            }

            // Kiểm tra mật khẩu có khớp không
            var signInResult = await _signInManager.PasswordSignInAsync(
            userName: user.UserName,
                password: loginModel.Password,
                isPersistent: false,
                lockoutOnFailure: false
            );

            if (!signInResult.Succeeded)
            {
                return Unauthorized(new { Message = "Tên đăng nhập hoặc mật khẩu không đúng" });
            }

            var result = await _service.Login(loginModel.Email, loginModel.Password);

            if (result == null)
            {
                return NotFound("Đăng nhập thất bại");
            }

            return new ResponseFormat(HttpStatusCode.OK, "Đăng nhập thành công", new
            {
                accessToken = result.Token,
                userId = result.UserId,
                userFullName = result.UserFullName,
                email = result.Email,
                role = result.Role
            });
        }
        //Đăng xuất
        [HttpPost("logout")]
        public async Task<ActionResult<ResponseFormat>> Logout()
        {
            var result = await _service.Logout();
            return new ResponseFormat(HttpStatusCode.NoContent, "Đã đăng xuất!", result);
        }
        // Đổi mật khẩu
        // Trường AccountId là được truyền qua từ props.accountId bên fontend
        [HttpPut("changePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] AccountChangePassword changePassword)
        {
            // Kiểm tra tài khoản ID
            if (string.IsNullOrWhiteSpace(changePassword.AccountId))
            { // Không cần check , bên fonend mình chuyển thẳng id tk qua pops, viết vào để tránh lỗi khi k nhập đúng id bên backend.
                return BadRequest(new { Message = "ID tài khoản không được để trống!" }); 
            }
            // Kiểm tra mật khẩu cũ
            if (string.IsNullOrWhiteSpace(changePassword.OldPassword))
            {
                return BadRequest(new { Message = "Mật khẩu cũ không được để trống!" });
            }
            // Kiểm tra mật khẩu mới
            if (string.IsNullOrWhiteSpace(changePassword.NewPassword))
            {
                return BadRequest(new { Message = "Mật khẩu mới không được để trống!" });
            }
            // Kiểm tra độ dài mật khẩu mới
            if (changePassword.NewPassword.Length < 5)
            {
                return BadRequest(new { Message = "Mật khẩu mới phải chứa ít nhất 5 ký tự." });
            }
            // Kiểm tra mật khẩu mới không được giống mật khẩu cũ
            if (changePassword.OldPassword == changePassword.NewPassword)
            {
                return BadRequest(new { Message = "Mật khẩu mới không được trùng với mật khẩu hiện tại." });
            }
            // Kiểm tra mật khẩu cũ với cơ sở dữ liệu
            var user = await _userManager.FindByIdAsync(changePassword.AccountId);
            if (user == null)
            {// Không cần check , bên fonend mình chuyển thẳng id tk qua pops, viết vào để tránh lỗi khi k nhập đúng id bên backend.
                return NotFound(new { Message = "Tài khoản không tồn tại!" }); 
            }
            // Kiểm tra mật khẩu cũ với mật khẩu đã lưu trong cơ sở dữ liệu
            var passwordValid = await _userManager.CheckPasswordAsync(user, changePassword.OldPassword);
            if (!passwordValid)
            {
                return BadRequest(new { Message = "Mật khẩu hiện tại không chính xác." });
            }
            // Thực hiện thay đổi mật khẩu
            var result = await _userManager.ChangePasswordAsync(user, changePassword.OldPassword, changePassword.NewPassword);
            if (result.Succeeded)
            {
                return Ok(new { StatusCode = 200, Message = "Đổi mật khẩu thành công!" });
            }
            else
            {
                return BadRequest(new { Message = "Đổi mật khẩu thất bại!" });
            }
        }
        // Đổi mật khẩu dành cho admin
        [HttpPut("editPassword")]
        public async Task<ActionResult<ResponseFormat>> editPassword([FromBody] AccounteditPassword editPassword)
        {
            // Kiểm tra ID tài khoản
            if (string.IsNullOrWhiteSpace(editPassword.Id))
            {
                return BadRequest(new { Message = "ID tài khoản không được để trống!" });
            }
            // Kiểm tra mật khẩu mới
            if (string.IsNullOrWhiteSpace(editPassword.Password))
            {
                return BadRequest(new { Message = "Mật khẩu mới không được để trống!" });
            }
            // Kiểm tra độ dài mật khẩu mới
            if (editPassword.Password.Length < 5)
            {
                return BadRequest(new { Message = "Mật khẩu mới phải chứa ít nhất 5 ký tự." });
            }
            // Kiểm tra mật khẩu phải chứa ít nhất 1 chữ hoa, 1 chữ thường và 1 số
            if (!Regex.IsMatch(editPassword.Password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{5,}$"))
            {
                return BadRequest(new { Message = "Mật khẩu phải có ít nhất 5 ký tự gồm chữ hoa, chữ thường và số" });
            }
            var result = await _service.editPassWord(editPassword.Id, editPassword.Password);
            if (!result.Succeeded)
            {
                return NotFound("Đổi mật khẩu thất bại");
            }
            return new ResponseFormat(HttpStatusCode.OK, "Đổi mật khẩu thành công!", result);
        }
        //Xóa tài khoản
        // DELETE: api/Account/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseFormat>> DeleteAccount(string id)
        {
            var result = await _service.DeleteAccount(id);
            if (!result.Succeeded)
            {
                return new ResponseFormat(HttpStatusCode.NoContent, "Xóa thất bại!", 0);
            }

            return new ResponseFormat(HttpStatusCode.NoContent, "Xóa thành công!", 1);
        }
        // GET: api/Account
        [HttpGet("All")]
        public ActionResult<ResponseFormat> GetAccount()
        {
            var users = _service.GetAllAccount();
            return new ResponseFormat(HttpStatusCode.OK, "List", users);
        }

        [HttpGet("searchByName/{name}")]
        public ActionResult<ResponseFormat> GetAccountsByName(string name)
        {
            var users = _service.GetAccountsByName(name);
            return new ResponseFormat(HttpStatusCode.OK, "List", users);
        }
        //API Tìm kiếm tài khoản bằng email
        [HttpGet("{findbyemail}")]
        public async Task<ActionResult<ResponseFormat>> GetAccountByEmail(string email)
        {
            var users = await _service.GetAccountByEmail(email);
            return new ResponseFormat(HttpStatusCode.OK, "Find Account By Email!", users);
        }
        [HttpGet("findbyid/{id}")]
        public async Task<ActionResult<ResponseFormat>> GetAccountById(string id)
        {
            var user = await _service.GetAccountById(id);
            return new ResponseFormat(HttpStatusCode.OK, "Find Account By Id!", user);
        }
        [HttpGet("getId/{email}")]
        public async Task<ActionResult<ResponseFormat>> GetAccountId(string email)
        {
            var userId = await _service.GetAccountId(email);

            return new ResponseFormat(HttpStatusCode.OK, "Get User Id!", userId);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseFormat>> PutAccount(string id, EditAccountRequest account)
        {
            var result = await _service.UpdateAccount(id, account);
            if (!result.Succeeded)
            {
                return NotFound("Cập nhật thất bại!");
            }

            return new ResponseFormat(HttpStatusCode.NoContent, "Cập nhật thành công!", result);
        }
    }
}

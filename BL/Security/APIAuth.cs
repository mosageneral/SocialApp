using BL.Infrastructure;
using BL.Security;
using DL.DTO;
using DL.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Model;
using Model.ApiModels;

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace BL.Security
{
    public interface IAuthenticateService
    {
        User AuthenticateUser(ApiLoginModelDTO request, out string token);
    }

    public class TokenAuthenticationService : IAuthenticateService
    {
        private readonly IUserManagementService _userManagementService;
        private readonly TokenManagement _tokenManagement;

        public TokenAuthenticationService(IUserManagementService service, IOptions<TokenManagement> tokenManagement)
        {
            _userManagementService = service;
            _tokenManagement = tokenManagement.Value;
        }

        public User AuthenticateUser(ApiLoginModelDTO request, out string token)
        {
            token = string.Empty;
            var user = _userManagementService.IsValidUser(request.UserName, request.Password);
            if (user != null)
            {
                var claims = new[] {new Claim(ClaimTypes.Name, request.UserName)};
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenManagement.Secret));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
                var expireDate = DateTime.Now.AddDays(_tokenManagement.AccessExpiration);
                var tokenDiscriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = expireDate,
                    SigningCredentials = credentials
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenObj = tokenHandler.CreateToken(tokenDiscriptor);
                token = tokenHandler.WriteToken(tokenObj);
            }
            return user;
        }
    }

    public interface IUserManagementService
    {
        User IsValidUser(string username, string password);
    }

    public class UserManagementService : IUserManagementService
    {
        private readonly IUnitOfWork _uow;
        public UserManagementService(IUnitOfWork uow) { _uow = uow; }

        public User IsValidUser(string userName, string password)
        {
            var user = _uow.UserRepository.GetMany(ent => ent.UserName.ToLower() == userName.ToLower().Trim() &&
            ent.Password == EncryptANDDecrypt.EncryptText(password) ).ToHashSet();
            return user.Count() == 1 ? user.FirstOrDefault() : null;
        }
    }
}

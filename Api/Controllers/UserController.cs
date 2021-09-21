using AutoMapper;
using BL.Infrastructure;
using BL.Security;
using DL.DTO;
using Helper;
using Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Model.ApiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUnitOfWork _uow;

        private readonly IAuthenticateService _authService;

        private readonly IHostingEnvironment _hostingEnvironment;

        private readonly IMapper _mapper;
        public UserController(IMapper mapper, IHostingEnvironment hostingEnvironment,IUnitOfWork uow, IAuthenticateService authService, IOptions<TokenManagement> tokenManagement, IHostingEnvironment env)
        {
            _uow = uow;
            _authService = authService;
            _hostingEnvironment = _hostingEnvironment;
            _mapper = mapper;
        }
        /// <summary>
        /// Log in user
        /// </summary>
        /// <remarks></remarks>
        /// <response code="200"> user object and token </response>
        /// <response code="400">invalid user name or password</response>
        /// <response code="401">Unauthorized</response>
        [AllowAnonymous]
        [HttpPost, Route("LogIn")]
        public IActionResult LogIn([FromBody] ApiLoginModelDTO request)
        {
            if (!ModelState.IsValid)
            { return BadRequest(ModelState); }
            var user = _authService.AuthenticateUser(request, out string token);
            if (user != null)
            {
                user.Password = null;
                return Ok(new
                {
                    user,
                    token
                });
            }
            return BadRequest("Invalid Username or Password");
        }
        [AllowAnonymous]
        [HttpPost, Route("Register")]
        public IActionResult Register([FromForm] UserRegisterDTO request)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var ProfileImage = FileHelper.FileUpload(request.UserPic, _hostingEnvironment, Constants.UserUploadFolder);
                    var CovorImage = FileHelper.FileUpload(request.CovorPic, _hostingEnvironment, Constants.UserUploadFolder);
                    var User = _mapper.Map<DL.Entities.User>(request);
                    User.UserPic = ProfileImage;
                    User.CovorPic = CovorImage;
                    User.Password = EncryptANDDecrypt.EncryptText(request.Password);
                    _uow.UserRepository.Add(User);
                    _uow.Save();
                    return Ok(User);
                }
                catch (Exception ex)
                {

                    return BadRequest(ex.ToString());
                }
             
            }
            return BadRequest("Invalid Username or Password");
        }
    }
}

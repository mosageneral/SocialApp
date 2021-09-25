using AutoMapper;
using BL.Infrastructure;
using BL.Security;
using DL.DTO;
using DL.MailModels;
using Helper;
using Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MimeKit;
using Model.ApiModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("MyPolicy")]
    public class UserController : BaseController
    {
        private readonly IUnitOfWork _uow;

        private readonly IAuthenticateService _authService;

        private readonly IHostingEnvironment _hostingEnvironment;

        private readonly IMapper _mapper;

        private readonly IMailService _mailService;
        public UserController(IMailService mailService,IMapper mapper, IHostingEnvironment hostingEnvironment,IUnitOfWork uow, IAuthenticateService authService, IOptions<TokenManagement> tokenManagement, IHostingEnvironment env)
        {
            _uow = uow;
            _authService = authService;
            _hostingEnvironment = _hostingEnvironment;
            _mapper = mapper;
            _mailService = mailService;
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
        public IActionResult LogIn( ApiLoginModelDTO request)
        {
            if (!ModelState.IsValid)
            { return BadRequest(ModelState); }
            var user = _authService.AuthenticateUser(request, out string token);
            if (user==null)
            {
                return BadRequest("البريد الالكتروني او كلمة السر خطأ");

            }
            if (!user.IsActive)
            {
                return BadRequest("الحساب غير مفعل توجه لبريدك الالكتروني للتفعيل");
            }
            if (user != null)
            {
                user.Password = null;
                return Ok(new
                {
                    user,
                    token
                });
            }
            return BadRequest("البريد الالكتروني او كلمة السر خطأ");

        }
        [AllowAnonymous]
        [HttpPost, Route("Register")]
        public IActionResult Register([FromForm] UserRegisterDTO request)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (CheckEmail(request.Email))
                    {
                        return BadRequest("Email Already Exists");
                    }
                    var ProfileImage = FileHelper.FileUpload(request.UserPic, _hostingEnvironment, Constants.UserUploadFolder);
                    var CovorImage = FileHelper.FileUpload(request.CovorPic, _hostingEnvironment, Constants.UserUploadFolder);
                    var User = _mapper.Map<DL.Entities.User>(request);
                    User.UserPic = ProfileImage;
                    User.CovorPic = CovorImage;
                    User.Password = EncryptANDDecrypt.EncryptText(request.Password);
                    _uow.UserRepository.Add(User);
                    _uow.Save();
                   var email =  _mailService.SendWelcomeEmailAsync(new WelcomeRequest { ToEmail = User.Bio, UserName = User.Email });
                    return Ok(User);
                }
                catch (Exception ex)
                {

                    return BadRequest(ex.ToString());
                }
             
            }
            return BadRequest("Invalid Username or Password");
        }
        [NonAction]
        public bool CheckEmail(string Email)
        {
           var IsEmailExist =  _uow.UserRepository.GetMany(a => a.Email.ToLower() == Email.ToLower()).FirstOrDefault();
            if (IsEmailExist!=null )
            {
                return true;
            }
            return false;
        }
        [AllowAnonymous]
        [HttpPost, Route("ActivateAccount")]
        public IActionResult ActivateAccount([FromBody] int UserId)
        {
            try
            {
                var User = _uow.UserRepository.GetById(UserId);
                User.IsActive = true;
                _uow.UserRepository.Update(User);
                _uow.Save();
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.ToString());
            }
            
           
        }
        [AllowAnonymous]
        [HttpPost, Route("ActivateEmailAccount")]
        public IActionResult ActivateEmailAccount(string Email)
        {
            try
            {
                var User = _uow.UserRepository.GetAll().Where(a=>a.Email.ToLower()==Email.ToLower()).FirstOrDefault();
                User.IsActive = true;
                _uow.UserRepository.Update(User);
                _uow.Save();
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.ToString());
            }


        }

    }
}

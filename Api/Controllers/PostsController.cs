using AutoMapper;
using BL.Infrastructure;
using BL.Security;
using DL.DTO;
using DL.Entities;
using DL.MailModels;
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
using System.Net;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : BaseController
    {
        private readonly IUnitOfWork _uow;

        private readonly IAuthenticateService _authService;

        private readonly IHostingEnvironment _hostingEnvironment;

        private readonly IMapper _mapper;

        private readonly IMailService _mailService;
        public PostsController(IMailService mailService, IMapper mapper, IHostingEnvironment hostingEnvironment, IUnitOfWork uow, IAuthenticateService authService, IOptions<TokenManagement> tokenManagement, IHostingEnvironment env)
        {
            _uow = uow;
            _authService = authService;
            _hostingEnvironment = _hostingEnvironment;
            _mapper = mapper;
            _mailService = mailService;
        }
        [HttpPost, Route("CreatePost")]
        [Authorize]
        public IActionResult CreatePost(PostDTO post)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var Attachment = FileHelper.FileUpload(post.Attachment, _hostingEnvironment, Constants.UserUploadFolder);
                    var Post = _mapper.Map<Post>(post);
                    Post.Attachment = Attachment;
                    _uow.PostRepository.Add(Post);
                    _uow.Save();
                    return Ok(Post);
                }
                catch (Exception ex)
                {

                    return BadRequest(ex.ToString());
                }
                
            }
            return BadRequest(ModelState);
        }


        [HttpPost, Route("Comment")]
        [Authorize]
        public IActionResult Comment(CommentDTO commentDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var Attachment = FileHelper.FileUpload(commentDTO.Attachment, _hostingEnvironment, Constants.UserUploadFolder);
                    var Comment = _mapper.Map<Comment>(commentDTO);
                    Comment.Attachment = Attachment;
                    _uow.CommentRepository.Add(Comment);
                    _uow.Save();
                    return Ok(Comment);
                }
                catch (Exception ex)
                {

                    return BadRequest(ex.ToString());
                }

            }
            return BadRequest(ModelState);
        }

        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Unauthorized)]
        [HttpPost, Route("Like")]
        [Authorize]
        public IActionResult Like(Like like)
        {
            if (ModelState.IsValid)
            {
                _uow.LikeRepository.Add(like);
                _uow.Save();
                return Ok(like) ;
            }
            return BadRequest(ModelState);
        }

    }
}

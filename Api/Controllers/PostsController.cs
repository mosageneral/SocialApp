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
using Microsoft.EntityFrameworkCore;
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
        public IActionResult CreatePost([FromForm]PostDTO post)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var Post = _mapper.Map<Post>(post);

                    if (post.Attachment != null)
                    {
                        var Attachments = FileHelper.FileUpload(post.Attachment, _hostingEnvironment, Constants.UserUploadFolder);
                        Post.Attachment = Attachments;

                    }
                    else
                    {
                        Post.Attachment = "Null";
                    }
                    var Attachment = FileHelper.FileUpload(post.Attachment, _hostingEnvironment, Constants.UserUploadFolder);

                    Post.Date = DateTime.Now;

                    Post.Attachment = Attachment;
                    _uow.PostRepository.Add(Post);
                    _uow.Save();
                    Post = _uow.PostRepository.GetMany(a=>a.Id==Post.Id).Include(a=>a.User).FirstOrDefault();
                    return Ok(Post);
                }
                catch (Exception ex)
                {

                    return BadRequest(ex.ToString());
                }
                
            }
            return BadRequest(ModelState);
        }


        [HttpPost, Route("DeletePost")]
        [Authorize]
        public IActionResult DeletePost(int PostId)
        {
            List<string> DeletedFiles = new  List<string>();
            var Post = _uow.PostRepository.GetById(PostId);
            DeletedFiles.Add(Post.Attachment);
          var Comments =  _uow.CommentRepository.GetAll().Where(a => a.PostId == PostId);
          var likes =  _uow.LikeRepository.GetAll().Where(a => a.PostId == PostId);
            foreach (var item in Comments)
            {
                DeletedFiles.Add(item.Attachment);
                _uow.CommentRepository.Delete(item.Id);
            }
            foreach (var item in likes)
            {
               
                _uow.CommentRepository.Delete(item.Id);
            }
            _uow.PostRepository.Delete(PostId);
            _uow.Save();
            foreach (var item in DeletedFiles)
            {
               var Deleted =  FileHelper.DeleteFile(item, Constants.UserUploadFolder);
            }

            return Ok(PostId);
        }


        [HttpGet, Route("(")]
        [Authorize]
        public IActionResult DeleteAllPosts()
        {
            
            var posts = _uow.PostRepository.GetAll();
            foreach (var item in posts)
            {
                _uow.CommentRepository.Delete(item.Id);
                _uow.LikeRepository.Delete(item.Id);
                _uow.PostRepository.Delete(item.Id);

            }
            _uow.Save();
            return Ok();
        }

        [HttpGet, Route("TimeLine")]
        [Authorize]
        public IActionResult TimeLine(int UserId)
        {
            List<PostDTOback> UserPosts = new List<PostDTOback>();
            var Posts = _uow.PostRepository.GetAll().Include(a => a.Comments).Include(a => a.Likes).Include(a => a.User).OrderByDescending(a => a.Date).ToHashSet();
            foreach (var item in Posts)
            {
                var liked = _uow.LikeRepository.GetMany(a => a.PostId == item.Id && a.UserId == UserId).FirstOrDefault();
                var Post = _mapper.Map<PostDTOback>(item);

                if (liked == null)
                {
                    Post.Liked = false;

                }
                else
                {
                    Post.Liked = true;

                }
                UserPosts.Add(Post);
            }
            return Ok(UserPosts);
        }

            [HttpPost, Route("Comment")]
        [Authorize]
        public IActionResult Comment([FromForm] CommentDTO commentDTO)
        {
            if (ModelState.IsValid)
            {
               
                try
                {
                   
                    var Comment = _mapper.Map<Comment>(commentDTO);
                    if (commentDTO.Attachment != null)
                    {
                        var Attachment = FileHelper.FileUpload(commentDTO.Attachment, _hostingEnvironment, Constants.UserUploadFolder);
                        Comment.Attachment = Attachment;

                    }
                    else
                    {
                        Comment.Attachment = "Null";
                    }
                    Comment.Date = DateTime.Now;
                    _uow.CommentRepository.Add(Comment);
                    _uow.Save();
                    var C = _uow.CommentRepository.GetById(Comment.Id);

                    return Ok(C);
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
        public IActionResult Like(LikeDTO like)
        {
            if (ModelState.IsValid)
            {
                if (!IsLikedBefore(like))
                {
                    var Like = _mapper.Map<Like>(like);
                    Like.Date = DateTime.Now;
                    _uow.LikeRepository.Add(Like);
                    _uow.Save();
                    return Ok(new { Liked = true });

                }
                else
                {
                    var Like = _uow.LikeRepository.GetMany(a => a.PostId == like.PostId && a.UserId == like.UserId).FirstOrDefault();
                    _uow.LikeRepository.Delete(Like.Id);
                    _uow.Save();
                    return Ok(new {Liked= false});
                }
                   
            }
            return BadRequest(ModelState);
        }
        [HttpGet, Route("GetUserPost")]
        [Authorize]
        public IActionResult GetUserPost(int UserId)
        {
            List<PostDTOback> UserPosts = new List<PostDTOback>();
            var Posts = _uow.PostRepository.GetMany(a => a.UserId == UserId).Include(a=>a.Comments).Include(a => a.Likes).Include(a=>a.User).OrderByDescending(a=>a.Date).ToHashSet();
            foreach (var item in Posts)
            {
                var liked = _uow.LikeRepository.GetMany(a => a.PostId == item.Id && a.UserId == UserId).FirstOrDefault();
                    var Post = _mapper.Map<PostDTOback>(item);

                if (liked==null)
                {
                    Post.Liked = false;

                }
                else
                {
                    Post.Liked = true;

                }
                UserPosts.Add(Post);
            }
            return Ok(UserPosts);
        }

        [HttpGet, Route("GetPostComments")]
        [Authorize]
        public IActionResult GetPostComments(int PostId)
        {
            var Comments = _uow.CommentRepository.GetMany(a => a.PostId == PostId).Include(a=>a.User);
            return Ok(Comments);
        }


        [HttpGet, Route("GetPost")]
        [Authorize]
        public IActionResult GetPost(int PostId)
        {
            var Post = _uow.PostRepository.GetMany(a => a.Id == PostId).Include(a => a.User).Include(a=>a.Likes).Include(a=>a.Comments.OrderByDescending(a=>a.Date)).OrderByDescending(a=>a.Id).ToHashSet();
            foreach (var item in Post)
            {
                foreach (var Comment in item.Comments)
                {
                    Comment.User = _uow.UserRepository.GetById(Comment.UserId);
                }
            }
            return Ok(Post);
        }



        [HttpPost, Route("checkLikedPost")]
        [Authorize]
        public IActionResult checkLikedPost(int UserId,int PostId)
        {

            var Liked = _uow.LikeRepository.GetMany(a => a.UserId == UserId&&a.PostId==PostId).FirstOrDefault();
            if (Liked!=null)
            {
                return Ok(new {Liked=true });

            }
            return Ok(new { Liked = false });

        }



        [NonAction]
        public bool IsLikedBefore(LikeDTO Like)
        {
            var like = _uow.LikeRepository.GetMany(a => a.PostId == Like.PostId && a.UserId == Like.UserId).FirstOrDefault();
            if (like!=null)
            {
                return true;
            }
            return false;
        }

    }
}

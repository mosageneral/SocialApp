using AutoMapper;
using DL.DTO;
using DL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.Mapping
{
 
    public class MappingConfigration : Profile
    {
        public MappingConfigration()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<User, UserRegisterDTO>().ForMember(x => x.CovorPic, opt => opt.Ignore()).ForMember(x => x.UserPic, opt => opt.Ignore()).ReverseMap();
            CreateMap<Post, PostDTO>().ForMember(x => x.Attachment, opt => opt.Ignore()).ReverseMap();
            CreateMap<Comment, CommentDTO>().ForMember(x => x.Attachment, opt => opt.Ignore()).ReverseMap();

        }
    }
}

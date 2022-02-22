using AutoMapper;
using Domain.VO;
using Domain.VO.Request;
using Domain.Model;
using System;
using System.Collections.Generic;

namespace Application.Utils
{
    public class Profile: AutoMapper.Profile
    {
        public Profile()
        {
            CreateMap<User, UserVO>().ReverseMap();
            CreateMap<UserVO, UserCreateVO>().ReverseMap();
            CreateMap<UserVO, UserUpdateVO>().ReverseMap();
            CreateMap<User, UserCreateVO>().ReverseMap();
            CreateMap<User, UserUpdateVO>().ReverseMap();

            CreateMap<UserRole, UserRoleVO>().ReverseMap();

            CreateMap<Example, ExampleVO>().ReverseMap();
            CreateMap<ExampleRequestVO, Example>().ReverseMap();
        }
    }

    public static class Mapping
    {
        public readonly static Dictionary<Type, Type> toModel = new()
        {
            { typeof(ExampleVO), typeof(Example) },
            { typeof(ExampleRequestVO), typeof(Example) },
        };

        public readonly static Dictionary<Type, Type> toVO = new()
        {
            { typeof(Example), typeof(ExampleVO) },
            { typeof(ExampleRequestVO), typeof(ExampleVO) },

        };
    }
}

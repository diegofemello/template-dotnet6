using Application.DTO;
using Application.DTO.Request;
using Domain.Model;
using System;
using System.Collections.Generic;

namespace Application.Utils
{
    public class Profile: AutoMapper.Profile
    {
        public Profile()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<UserDTO, UserCreateDTO>().ReverseMap();
            CreateMap<UserDTO, UserUpdateDTO>().ReverseMap();
            CreateMap<User, UserCreateDTO>().ReverseMap();
            CreateMap<User, UserUpdateDTO>().ReverseMap();
        }
    }

    public static class Mapping
    {
        public readonly static Dictionary<Type, Type> toModel = new()
        {
            { typeof(ExampleDTO), typeof(Example) },
            { typeof(ExampleRequestDTO), typeof(Example) },

            { typeof(UserDTO), typeof(User) },
            { typeof(UserCreateDTO), typeof(User) },
            { typeof(UserUpdateDTO), typeof(User) },
        };

        public readonly static Dictionary<Type, Type> toDTO = new()
        {
            { typeof(Example), typeof(ExampleDTO) },
            { typeof(ExampleRequestDTO), typeof(ExampleDTO) },

            { typeof(User), typeof(UserDTO) },
            { typeof(UserCreateDTO), typeof(UserDTO) },
            { typeof(UserUpdateDTO), typeof(UserDTO) },
        };
    }
}

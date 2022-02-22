using AutoMapper;
using Domain.Data.VO;
using Domain.Model;

namespace Domain.Utils
{
    public class Profile: Profile
    {
        public Profile()
        {
            CreateMap<Sitio, SitioVO>().ReverseMap();
            CreateMap<Equipment, EquipmentVO>().ReverseMap();
            CreateMap<EquipmentModel, EquipmentModelVO>().ReverseMap();
            CreateMap<EquipmentModelBusiness, EquipmentModelBusinessVO>().ReverseMap();
            CreateMap<EquipmentModelCategory, EquipmentModelCategoryVO>().ReverseMap();
            CreateMap<User, UserVO>().ReverseMap();
            CreateMap<UserCpf, UserCpfVO>().ReverseMap();
            CreateMap<UserCnpj, UserCnpjVO>().ReverseMap();
            CreateMap<UserAdmin, UserAdminVO>().ReverseMap();
        }
    }
}

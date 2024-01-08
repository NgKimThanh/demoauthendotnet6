using AutoMapper;
using DotNet6Authen.Entities;
using DotNet6Authen.DTO;

namespace DemoWebApiDotNet6.Helper
{
    public class HelperMapper : Profile
    {
        public HelperMapper()
        {
            CreateMap<GroupOfProduct, GroupOfProductDTO>().ReverseMap();
        }
    }
}

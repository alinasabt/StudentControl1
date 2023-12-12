using AutoMapper;
using StudentControl.Domain.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentControl.DTO
{
    public class MapperProfile : Profile
    {
        public MapperProfile() 
        {
            CreateMap<Student, StudentDTO>().ReverseMap();
            CreateMap<Order, OrderDTO>().ReverseMap();
            CreateMap<Group, GroupDTO>().ReverseMap();
        }

    }
}

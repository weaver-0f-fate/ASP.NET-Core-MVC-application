using AutoMapper;
using Core.Models;
using Core.ModelsDTO;

namespace Task9.AutoMapperProfiles {
    public class UserProfile : Profile {

        public UserProfile() {
            CreateMap<Course, CourseDTO>();
            CreateMap<Group, GroupDTO>()
                .ForMember(x => x.CourseName, 
                    y => y.MapFrom(src => src.Course.CourseName));
            CreateMap<Student, StudentDTO>()
                .ForMember(x => x.GroupName,
                    y => y.MapFrom(src => src.Group.GroupName))
                .ForMember(x => x.CourseName,
                y => y.MapFrom(src => src.Group.Course.CourseName)); ;
        }
    }
}
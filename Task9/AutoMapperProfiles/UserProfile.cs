using AutoMapper;
using Core.Models;
using Services.ModelsDTO;

namespace Task9.AutoMapperProfiles {
    public class UserProfile : Profile {

        public UserProfile() {
            CreateMap<Course, CourseDto>();
            CreateMap<Group, GroupDto>()
                .ForMember(x => x.CourseName, 
                    y => y.MapFrom(src => src.Course.CourseName));
            CreateMap<Student, StudentDto>()
                .ForMember(x => x.GroupName,
                    y => y.MapFrom(src => src.Group.GroupName))
                .ForMember(x => x.CourseName,
                y => y.MapFrom(src => src.Group.Course.CourseName));

            CreateMap<CourseDto, Course>();
            CreateMap<GroupDto, Group>();
            CreateMap<StudentDto, Student>();
        }
    }
}
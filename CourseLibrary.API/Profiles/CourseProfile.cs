using AutoMapper;

namespace CourseLibrary.API.Profiles
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<Entities.Course, Models.CourseDto>();

            CreateMap<Models.CourseForCreationDto, Entities.Course>();
        }
    }
}

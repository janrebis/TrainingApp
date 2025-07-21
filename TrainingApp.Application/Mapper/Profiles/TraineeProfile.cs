using AutoMapper;
using TrainingApp.Application.DTO;
using TrainingApp.Core.Entities;
namespace TrainingApp.Application.Mapper.Profiles
{
    public class TraineeProfile : Profile
    {
        public TraineeProfile() {
            CreateMap<Trainee, TraineeDTO>();        
        }
    }
}

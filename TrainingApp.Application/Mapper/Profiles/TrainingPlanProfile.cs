using AutoMapper;
using TrainingApp.Application.DTO;
using TrainingApp.Core.Entities.AggregateRoots;

namespace TrainingApp.Application.Mapper.Profiles
{
    public class TrainingPlanProfile : Profile
    {
        public TrainingPlanProfile() {
            CreateMap<TrainingPlanDTO, TrainingPlan>();
            CreateMap<TrainingPlan, TrainingPlanDTO>();
        }
    }
}

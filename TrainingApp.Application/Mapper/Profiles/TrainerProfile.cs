using AutoMapper;
using TrainingApp.Api.DTO;
using TrainingApp.Core.Entities.AggregateRoots;

namespace TrainingApp.Application.Mapper.Profiles
{
    public class TrainingPlanProfile : Profile
    {
        public TrainingPlanProfile() {
            CreateMap<TrainingPlan, TrainingPlanDTO>();
        }
    }
}

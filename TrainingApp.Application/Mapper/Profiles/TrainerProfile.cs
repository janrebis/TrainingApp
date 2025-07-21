using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TrainingApp.Application.DTO;
using TrainingApp.Core.Entities.AggregateRoots;

namespace TrainingApp.Application.Mapper.Profiles
{
    public class TrainerProfile : Profile
    {
        public TrainerProfile() {

            CreateMap<TrainerDTO, Trainer>();
        } 
    }
}

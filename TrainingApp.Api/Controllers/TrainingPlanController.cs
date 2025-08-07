using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TrainingApp.Application.DTO;
using TrainingApp.Core.Entities.AggregateRoots;
using TrainingApp.Core.Interfaces.Services;
using TrainingApp.Core.ValueObjects;




namespace TrainingApp.Api.Controllers
{
    [Route("[controller]/[action]")]

    public class TrainingPlanController : Controller
    {
        private readonly ITrainingPlanService _trainingPlanService;
        private readonly IMapper _mapper;
        public TrainingPlanController(ITrainingPlanService trainingPlanService, IMapper mapper)
        {
            _trainingPlanService = trainingPlanService;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> AllTrainings(Guid traineeId)
        {
            var trainings = await _trainingPlanService.GetTrainingPlans<TrainingPlanDTO>(traineeId);
            ViewBag.Trainings = trainings;
            ViewBag.TraineeId = traineeId;
            return View();
        }

        [HttpGet]
        public IActionResult AddTraining(Guid traineeId)
        {
            var model = new TrainingPlanDTO
            {
                TraineeId = traineeId
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> AddTraining(TrainingPlanDTO trainingPlanDTO)
        {
            if (ModelState.IsValid)
            {
                var trainingPlanData = new TrainingPlanData(
                    trainingPlanDTO.Name,
                    trainingPlanDTO.TraineeId,
                    trainingPlanDTO.TrainingType,
                    trainingPlanDTO.ScheduledDate,
                    trainingPlanDTO.Notes
                    );
                await _trainingPlanService.AddTrainingPlan(trainingPlanData);
                return RedirectToAction(nameof(AllTrainings), "TrainingPlan");
            }

            else
            {
                throw new Exception("Nie udało się utworzyć planu");
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditTraining(Guid trainingPlanId)
        {
            var trainingPlanDTO = await _trainingPlanService.FindTrainingPlanById<TrainingPlanDTO>(trainingPlanId);
            return View(trainingPlanDTO);
        }
        

        [HttpPost]
        public async Task<IActionResult> EditTraining(TrainingPlanDTO trainingPlanDTO)
        {
            if (ModelState.IsValid)
            {

                await _trainingPlanService.EditTrainingPlan(_mapper.Map<TrainingPlan>(trainingPlanDTO));
                return RedirectToAction(nameof(AllTrainings), "TrainingPlan", new { traineeId = trainingPlanDTO.TraineeId});
            }

            else
            {
                throw new Exception("Nie udało się edytować planu");
            }
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using TrainingApp.Application.DTO;
using TrainingApp.Core.Interfaces.Services;
using TrainingApp.Core.ValueObjects;


/*
 TODO: Dodać porządną walidację, uporządkować kod
 */
namespace TrainingApp.Api.Controllers
{
    [Route("[controller]/[action]")]

    public class TrainingPlanController : Controller
    {
        private readonly ITrainingPlanService _trainingPlanService;
        public TrainingPlanController(ITrainingPlanService trainingPlanService)
        {
            _trainingPlanService = trainingPlanService;
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
        public IActionResult EditTraining(Guid traineeId)
        {
            var model = new TrainingPlanDTO
            {
                TraineeId = traineeId
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> EditTraining(TrainingPlanDTO trainingPlanDTO)
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
                throw new Exception("Nie udało się edytować planu");
            }
        }
    }
}
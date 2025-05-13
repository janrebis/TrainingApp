using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TrainingApp.Application.Services;
using TrainingApp.Core.DTO;
using TrainingApp.Core.Entities.AggregateRoots;

namespace TrainingApp.Api.Controllers
{
    [Route("[controller]/[action]")]
    public class TraineeController : Controller
    {
        private readonly TrainerService _trainerService;
        public TraineeController(TrainerService trainerService)
        {
            _trainerService = trainerService;
        }

        [HttpGet]
        public async Task<IActionResult> AllTrainees()
        {
            var trainerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            Trainer trainer = await _trainerService.FindTrainerAsync(trainerId);
            var trainees = await _trainerService.GetAllTraineesAsync(trainerId);
            ViewBag.TraineesList = trainer.Trainees.ToList().Count;
            return View(trainees);
        }

        [HttpGet]
        public IActionResult AddTrainee()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> AddTrainee(TraineeDTO traineeDto)
        {
            if (ModelState.IsValid)
            {
                var trainerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var name = traineeDto.Name;
                int age = traineeDto.Age ?? 0;

                try
                {
                    await _trainerService.AddTraineeAsync(trainerId, name, age);
                    return RedirectToAction(nameof(AllTrainees), "Trainee");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    foreach (var entry in ex.Entries)
                    {
                        if (entry.Entity is Trainer)
                        {
                            var databaseEntry = entry.GetDatabaseValues();
                            throw new Exception("Dane zostały zmodyfikowane przez innego użytkownika.");
                        }
                    }
                }

            }
            return View(traineeDto);
        }

        [HttpGet]
        public async Task<IActionResult> EditTrainee(Guid traineeId)
        {
            var trainerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            Trainer trainer = await _trainerService.FindTrainerAsync(trainerId);
            var traineeDTO = trainer.GetTraineeData(traineeId);

            if (traineeDTO == null)
            {
                return NotFound();
            }

            return View(traineeDTO);
        }

        [HttpPost]
        public async Task<IActionResult> EditTrainee(TraineeDTO traineeDto)
        {
            if (ModelState.IsValid)
            {
                var trainerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var traineeId = traineeDto.TraineeId;
                var name = traineeDto.Name;
                var age = traineeDto.Age;

                try
                {
                    await _trainerService.UpdateTraineeAsync(trainerId, traineeId, name, age);
                    return RedirectToAction(nameof(AllTrainees), "Trainee");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    foreach (var entry in ex.Entries)
                    {
                        if (entry.Entity is Trainer)
                        {
                            var databaseEntry = entry.GetDatabaseValues();
                            throw new Exception("Dane zostały zmodyfikowane przez innego użytkownika.");
                        }
                    }
                }

            }

            return RedirectToAction(nameof(TraineeController.AllTrainees), "Trainee");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveTrainee(Guid traineeId)
        {
                var trainerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                await _trainerService.RemoveTraineeAsync(trainerId, traineeId);
                return RedirectToAction(nameof(AllTrainees));

        }
    }
}

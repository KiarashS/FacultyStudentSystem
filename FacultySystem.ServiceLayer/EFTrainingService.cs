using ContentManagementSystem.ServiceLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentManagementSystem.Models.ViewModels;
using ContentManagementSystem.DataLayer.Context;
using System.Data.Entity;
using ContentManagementSystem.DomainClasses;
using EFSecondLevelCache;

namespace ContentManagementSystem.ServiceLayer
{
    public class EFTrainingService : ITrainingRecordService
    {
        IUnitOfWork _uow;
        readonly IDbSet<TrainingRecord> _trainings;
        public EFTrainingService(IUnitOfWork uow)
        {
            _uow = uow;
            _trainings = _uow.Set<TrainingRecord>();
        }

        public IEnumerable<TrainingViewModel> GetTrainings(int userId)
        {
            var trainingList = new List<TrainingViewModel>();
            var trainings = _trainings
                .Where(t => t.ProfessorId == userId)
                .OrderByDescending(t => t.Order)
                .ThenBy(t => t.Id)
                .Cacheable()
                .ToList();

            foreach (var training in trainings)
            {
                trainingList.Add(new TrainingViewModel
                {
                    Id = training.Id,
                    Title = training.Title,
                    Place = training.Place,
                    Time = training.Time,
                    FromTime = training.FromTime,
                    ToTime = training.ToTime,
                    Teacher = training.Teacher,
                    Participant = training.Participant,
                    Secretary = training.Secretary,
                    Link = training.Link,
                    Order = training.Order
                });
            }

            return trainingList;
        }

        public TrainingRecord CreateTraining(int userId, TrainingViewModel training)
        {
            var newTraining = _trainings.Add(new TrainingRecord
            {
                ProfessorId = userId,
                Title = training.Title,
                Place = training.Place,
                Time = training.Time,
                FromTime = training.FromTime,
                ToTime = training.ToTime,
                Teacher = training.Teacher,
                Participant = training.Participant,
                Secretary = training.Secretary,
                Link = training.Link,
                Order = training.Order
            });

            return newTraining;
        }

        public void UpdateTraining(int userId, TrainingViewModel newTraining)
        {
            var training = _trainings.Single(t => t.ProfessorId == userId && t.Id == newTraining.Id);

            training.Title = newTraining.Title;
            training.Place = newTraining.Place;
            training.Time = newTraining.Time;
            training.FromTime = newTraining.FromTime;
            training.ToTime = newTraining.ToTime;
            training.Teacher = newTraining.Teacher;
            training.Participant = newTraining.Participant;
            training.Secretary = newTraining.Secretary;
            training.Link = newTraining.Link;
            training.Order = newTraining.Order;
        }

        public void DeleteTraining(int userId, long id)
        {
            var training = _trainings.Single(t => t.ProfessorId == userId && t.Id == id);
            _trainings.Remove(training);
        }
    }
}

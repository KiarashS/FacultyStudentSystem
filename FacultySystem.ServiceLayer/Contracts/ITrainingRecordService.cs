using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.ServiceLayer.Contracts
{
    public interface ITrainingRecordService
    {
        IEnumerable<TrainingViewModel> GetTrainings(int userId);
        TrainingRecord CreateTraining(int userId, TrainingViewModel training);
        void UpdateTraining(int userId, TrainingViewModel newTraining);
        void DeleteTraining(int userId, long id);
    }
}

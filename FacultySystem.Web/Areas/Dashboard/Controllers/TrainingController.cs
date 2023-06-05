using ContentManagementSystem.Commons.Web.Attributes;
using ContentManagementSystem.DataLayer.Context;
using ContentManagementSystem.Models.ViewModels;
using ContentManagementSystem.ServiceLayer.Contracts;
using ContentManagementSystem.Web.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ContentManagementSystem.Web.Areas.Dashboard.Controllers
{
    [SiteAuthorize(Roles = ConstantsUtil.ProfessorRole)]
    public partial class TrainingController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ITrainingRecordService _trainingService;
        private readonly IProfessorService _professorService;

        public TrainingController(IUnitOfWork uow, ITrainingRecordService trainingService, IProfessorService professorService)
        {
            _uow = uow;
            _trainingService = trainingService;
            _professorService = professorService;
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult List()
        {
            var trainings = _trainingService.GetTrainings(CurrentUserId);
            return Json(new { Result = "OK", Records = trainings });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Create(TrainingViewModel training)
        {
            _professorService.UpdateLastUpdateTime(CurrentUserId);
            var newTraining = _trainingService.CreateTraining(CurrentUserId, training);
            _uow.SaveAllChanges();

            training.Id = newTraining.Id;

            return Json(new { Result = "OK", Record = training });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Update(TrainingViewModel training)
        {
            _professorService.UpdateLastUpdateTime(CurrentUserId);
            _trainingService.UpdateTraining(CurrentUserId, training);
            _uow.SaveAllChanges();

            return Json(new { Result = "OK" });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Delete(long id)
        {
            _professorService.UpdateLastUpdateTime(CurrentUserId);
            _trainingService.DeleteTraining(CurrentUserId, id);
            _uow.SaveAllChanges();

            return Json(new { Result = "OK" });
        }
    }
}
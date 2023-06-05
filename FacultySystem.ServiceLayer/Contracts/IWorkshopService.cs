using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.ServiceLayer.Contracts
{
    public interface IWorkshopService
    {
        IEnumerable<WorkshopViewModel> GetWorkshops(int userId);
        CourseAndWorkshop CreateWorkshop(int userId, WorkshopViewModel workshop);
        void UpdateWorkshop(int userId, WorkshopViewModel newWorkshop);
        void DeleteWorkshop(int userId, long id);
    }
}

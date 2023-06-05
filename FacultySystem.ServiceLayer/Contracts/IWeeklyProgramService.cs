using System;
using System.Collections.Generic;
using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.Models.ViewModels;

namespace ContentManagementSystem.ServiceLayer.Contracts
{
    public interface IWeeklyProgramService
    {
        IEnumerable<WeeklyProgramViewModel> GetListPrograms(int userId, byte dayOfWeek = 0);
        WeeklyProgram CreateProgram(int userId, WeeklyProgramViewModel program);
        void UpdateProgram(int userId, WeeklyProgramViewModel newProgram);
        void DeleteProgram(int userId, long id);
    }
}

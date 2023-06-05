using ContentManagementSystem.ServiceLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.Models.ViewModels;
using ContentManagementSystem.DataLayer.Context;
using System.Data.Entity;
using EFSecondLevelCache;

namespace ContentManagementSystem.ServiceLayer
{
    public class EFWeeklyProgramService : IWeeklyProgramService
    {
        IUnitOfWork _uow;
        readonly IDbSet<WeeklyProgram> _programs;
        //private readonly Lazy<Professor> _professorService;
        public EFWeeklyProgramService(IUnitOfWork uow)
        {
            _uow = uow;
            _programs = _uow.Set<WeeklyProgram>();
        }

        public IEnumerable<WeeklyProgramViewModel> GetListPrograms(int userId, byte dayOfWeek = 0)
        {
            var programList = new List<WeeklyProgramViewModel>();
            var dayNumber = (dayOfWeek >= 0 && dayOfWeek <= 7) ? dayOfWeek : 0;
            var query = _programs.AsQueryable();

            if (dayOfWeek > 0)
            {
                var day = (DayOfProgram)dayNumber;
                query = query.Where(wp => wp.ProfessorId == userId && wp.DayOfProgram == day);
            }
            else
            {
                query = query.Where(wp => wp.ProfessorId == userId);
            }

            var programs = query
                .OrderBy(wp => wp.DayOfProgram)
                .ThenBy(wp => wp.StartTime)
                .ThenBy(wp => wp.EndTime)
                .Cacheable()
                .ToList();

            foreach (var program in programs)
            {
                programList.Add(new WeeklyProgramViewModel
                {
                    Id = program.Id,
                    DayOfProgram = program.DayOfProgram,
                    StartTime = program.StartTime,
                    EndTime = program.EndTime,
                    Description = program.Description
                });
            }

            return programList;
        }

        public WeeklyProgram CreateProgram(int userId, WeeklyProgramViewModel program)
        {
            var newProgram = _programs.Add(new WeeklyProgram
            {
                ProfessorId = userId,
                DayOfProgram = program.DayOfProgram,
                StartTime = program.StartTime,
                EndTime = program.EndTime,
                Description = program.Description
            });

            return newProgram;
        }

        public void UpdateProgram(int userId, WeeklyProgramViewModel newProgram)
        {
            var program = _programs.Single(wp => wp.ProfessorId == userId && wp.Id == newProgram.Id);

            program.DayOfProgram = newProgram.DayOfProgram;
            program.StartTime = newProgram.StartTime;
            program.EndTime = newProgram.EndTime;
            program.Description = newProgram.Description;
        }

        public void DeleteProgram(int userId, long id)
        {
            var program = _programs.Single(wp => wp.ProfessorId == userId && wp.Id == id);
            _programs.Remove(program);
        }
    }
}

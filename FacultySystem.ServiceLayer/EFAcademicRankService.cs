using ContentManagementSystem.DataLayer.Context;
using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.Models.ViewModels;
using ContentManagementSystem.ServiceLayer.Contracts;
using EFSecondLevelCache;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.ServiceLayer
{
    public class EFAcademicRankService: IAcademicRankService
    {
        IUnitOfWork _uow;
        readonly IDbSet<AcademicRank> _academicRanks;
        private readonly Lazy<IProfessorService> _professorService;
        public EFAcademicRankService(IUnitOfWork uow, Lazy<IProfessorService> professorService)
        {
            _uow = uow;
            _academicRanks = _uow.Set<AcademicRank>();
            _professorService = professorService;
        }

        public IEnumerable<AcademicRankViewModel> GetListAcademicRanks()
        {
            var list = _academicRanks
                .Where(ar => ar.Name != "--")
                .Select(ar => new { ar.Id, ar.Name, ar.Order, count = ar.Professors.Count() })
                .OrderByDescending(ar => ar.Order)
                .ThenBy(ar => ar.Id)
                .Cacheable()
                .ToList();
            var ranks = new List<AcademicRankViewModel>();

            foreach (var item in list)
            {
                ranks.Add(new AcademicRankViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Order = item.Order,
                    ProfessorCount = item.count
                });
            }

            return ranks;
        }

        public IEnumerable<AcademicRankViewModel> GetAcademicRanks()
        {
            var list = _academicRanks
                .Where(ar => ar.Name == "--")
                .Union(_academicRanks
                .Where(ar => ar.Name != "--"))
                .OrderByDescending(ar => ar.Order)
                .ThenBy(ar => ar.Id)
                .Cacheable()
                .ToList();
            var ranks = new List<AcademicRankViewModel>();

            foreach (var item in list)
            {
                ranks.Add(new AcademicRankViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Order = item.Order
                });
            }

            return ranks;
        }

        public AcademicRank CreateAcademicRank(AcademicRankViewModel academicRank)
        {
            var rank = new AcademicRank
            {
                Name = academicRank.Name,
                Order = academicRank.Order
            };

            _academicRanks.Add(rank);

            return rank;
        }

        public void UpdateAcademicRank(AcademicRankViewModel newAcademicRank)
        {
            var rank = _academicRanks.Single(ar => ar.Id == newAcademicRank.Id);

            rank.Name = newAcademicRank.Name;
            rank.Order = newAcademicRank.Order;
        }

        public bool DeleteAcademicRank(int id)
        {
            var rank = _academicRanks.Single(ar => ar.Id == id);

            if (rank.Professors.Any())
            {
                var defaultAcademicRankId = _academicRanks.Where(eg => eg.Name == "--").Select(ed => ed.Id).Single();
                _professorService.Value.UpdateAcademicRankToDefault(rank.Id, defaultAcademicRankId);
            }

            _academicRanks.Remove(rank);
            return true;
        }

        public bool ExistName(int id, string name)
        {
            return _academicRanks.Any(ar => ar.Id != id && ar.Name == name.Trim());
        }

        public int GetIdByName(string name)
        {
            return _academicRanks.Where(ar => ar.Name == name.Trim()).Select(ar => ar.Id).SingleOrDefault();
        }
    }
}

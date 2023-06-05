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
    public class EFLanguageService : ILanguageService
    {
        IUnitOfWork _uow;
        readonly IDbSet<Language> _languages;
        //private readonly Lazy<Professor> _professorService;
        public EFLanguageService(IUnitOfWork uow)
        {
            _uow = uow;
            _languages = _uow.Set<Language>();
        }

        public IEnumerable<LanguageViewModel> GetLanguages(int userId)
        {
            var languageList = new List<LanguageViewModel>();
            var languages = _languages
                .Where(l => l.ProfessorId == userId)
                .OrderByDescending(l => l.Order)
                .ThenBy(l => l.Id)
                .Cacheable()
                .ToList();

            foreach (var language in languages)
            {
                languageList.Add(new LanguageViewModel
                {
                    Id = language.Id,
                    Name = language.Name,
                    Level = language.Level,
                    Description = language.Description,
                    Link = language.Link,
                    Order = language.Order
                });
            }

            return languageList;
        }

        public Language CreateLanguage(int userId, LanguageViewModel language)
        {
            var newLanguage = _languages.Add(new Language
            {
                ProfessorId = userId,
                Name = language.Name,
                Level = language.Level,
                Description = language.Description,
                Link = language.Link,
                Order = language.Order
            });

            return newLanguage;
        }

        public void UpdateLanguage(int userId, LanguageViewModel newLanguage)
        {
            var language = _languages.Single(l => l.ProfessorId == userId && l.Id == newLanguage.Id);

            language.Name = newLanguage.Name;
            language.Level = newLanguage.Level;
            language.Description = newLanguage.Description;
            language.Link = newLanguage.Link;
            language.Order = newLanguage.Order;
        }

        public void DeleteLanguage(int userId, long id)
        {
            var language = _languages.Single(l => l.ProfessorId == userId && l.Id == id);
            _languages.Remove(language);
        }
    }
}

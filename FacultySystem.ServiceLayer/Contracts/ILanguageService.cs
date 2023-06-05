using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.ServiceLayer.Contracts
{
    public interface ILanguageService
    {
        IEnumerable<LanguageViewModel> GetLanguages(int userId);
        Language CreateLanguage(int userId, LanguageViewModel language);
        void UpdateLanguage(int userId, LanguageViewModel newLanguage);
        void DeleteLanguage(int userId, long id);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.Models.ViewModels
{
    public class EducationalGroupViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Order { get; set; }
        public int ProfessorCount { get; set; }
    }
}

using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.Models.Utils;
using System;
using System.Globalization;

namespace ContentManagementSystem.Models.ViewModels
{
    public class UserSettingsViewModel
    {
        public bool IsActiveBio { get; set; }
        public bool IsActiveFreePage { get; set; }
        public bool ShowScopusDocumentsCitationChart { get; set; }
        public bool ShowGoogleDocumentsCitationChart { get; set; }
        public bool ShowHIndexSection { get; set; }
        public bool IsActiveWeeklyProgram { get; set; }
    }
}

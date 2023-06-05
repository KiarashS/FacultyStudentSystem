using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.Models.Utils;

namespace ContentManagementSystem.Models.ViewModels
{
    public class ThesisViewModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public ThesisState ThesisState { get; set; }
        public ThesisPost ThesisPost { get; set; }
        public string Executers { get; set; }
        public ThesisGrade ThesisGrade { get; set; }
        public ThesisType ThesisType { get; set; }
        public string University { get; set; }
        public string Description { get; set; }
        public int? Time { get; set; }
        public string Link { get; set; }
        public int? Order { get; set; }

        public string ThesisPostText
        {
            get
            {
                return EnumExtensions.GetDescription(ThesisPost);
            }
        }

        public string ThesisStateText
        {
            get
            {
                return EnumExtensions.GetDescription(ThesisState);
            }
        }

        public string ThesisGradeText
        {
            get
            {
                return EnumExtensions.GetDescription(ThesisGrade);
            }
        }

        public string ThesisTypeText
        {
            get
            {
                return EnumExtensions.GetDescription(ThesisType);
            }
        }

        public string FullTime
        {
            get
            {
                if (Time != null)
                {
                    return $"سال {Time}";
                }

                return null;
            }
        }
    }
}

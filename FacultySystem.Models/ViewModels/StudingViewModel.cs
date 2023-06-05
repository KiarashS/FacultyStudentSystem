namespace ContentManagementSystem.Models.ViewModels
{
    public class StudingViewModel
    {
        public long Id { get; set; }
        public string Grade { get; set; }
        public string Field { get; set; }
        public string Trend { get; set; }
        public string University { get; set; }
        public int? StartTime { get; set; }
        public int? EndTime { get; set; }
        public string ThesisTitle { get; set; }
        public string ThesisSupervisors { get; set; }
        public string ThesisAdvisors { get; set; }
        public string Link { get; set; }
        public int? Order { get; set; }

        public string FullTime
        {
            get
            {
                if (StartTime != null && EndTime == null)
                {
                    return $"از سال {StartTime}";
                }

                if (StartTime == null && EndTime != null)
                {
                    return $"تا سال {EndTime}";
                }

                if (StartTime != null && EndTime != null)
                {
                    return $"از سال {StartTime} تا سال {EndTime}";
                }

                return null;
            }
        }
    }
}

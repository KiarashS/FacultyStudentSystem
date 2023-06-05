namespace ContentManagementSystem.Models.ViewModels
{
    public class ProfessorMembershipViewModel
    {
        public long Id { get; set; }
        public string CommitteeTitle { get; set; }
        public string Post { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public int? Order { get; set; }

        public string FullTime
        {
            get
            {
                if (StartTime != null && EndTime == null)
                {
                    return $"از {StartTime}";
                }

                if (StartTime == null && EndTime != null)
                {
                    return $"تا {EndTime}";
                }

                if (StartTime != null && EndTime != null)
                {
                    return $"از {StartTime} تا {EndTime}";
                }

                return null;
            }
        }
    }
}

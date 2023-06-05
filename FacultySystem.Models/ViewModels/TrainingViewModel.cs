namespace ContentManagementSystem.Models.ViewModels
{
    public class TrainingViewModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Place { get; set; }
        public int? Time { get; set; } // Year
        public int? FromTime { get; set; } // Year
        public int? ToTime { get; set; } // Year
        public string Teacher { get; set; }
        public string Participant { get; set; }
        public string Secretary { get; set; }
        public string Link { get; set; }
        public int? Order { get; set; }

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

        public string FullDetailedTime
        {
            get
            {
                if (FromTime != null && ToTime != null)
                {
                    return $"(از سال {FromTime} تا سال {ToTime})";
                }

                if (FromTime != null && ToTime == null)
                {
                    return $"(از سال {FromTime})";
                }

                if (FromTime == null && ToTime != null)
                {
                    return $"(تا سال {ToTime})";
                }

                return null;
            }
        }
    }
}

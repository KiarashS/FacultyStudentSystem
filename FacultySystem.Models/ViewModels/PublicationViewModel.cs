namespace ContentManagementSystem.Models.ViewModels
{
    public class PublicationViewModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Publisher { get; set; }
        public int? Time { get; set; }
        public string Description { get; set; }
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
    }
}

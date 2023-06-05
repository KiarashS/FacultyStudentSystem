using System.Data.Entity.ModelConfiguration;
using ContentManagementSystem.DomainClasses;

namespace ContentManagementSystem.DataLayer.Mappings
{
    public class ProfessorMap : EntityTypeConfiguration<Professor>
    {
        public ProfessorMap()
        {
            //public int Id { get; set; }
            //public string CommonAuthorPaperName { get; set; }
            //public string AcademicRank { get; set; }
            //public string EducationalDegree { get; set; }
            //public string EducationalGroup { get; set; }
            //public string EducationalRecords { get; set; }
            //public string TrainingRecords { get; set; }
            //public string AdministraticesRecords { get; set; }
            //public string InternalResearchesRecords { get; set; }
            //public string ExternalResearchesRecords { get; set; }
            //public string InternalSeminarsRecords { get; set; }
            //public string ExternalSeminarsRecords { get; set; }
            //public string Thesis { get; set; }
            //public string CoursesAndWorkshops { get; set; }
            //public string Honors { get; set; }
            //public string Mobile { get; set; }
            //public string PhoneNumber { get; set; }
            //public string ResearchFields { get; set; }
            //public string Interests { get; set; }
            //public string PersonalWebPage { get; set; }
            //public string ResumeFileName { get; set; }
            //public string ScopusPage { get; set; }
            //public string OrcidId { get; set; }
            //public string ResearchGate { get; set; }
            //public string GoogleScholar { get; set; }
            //public string Bio { get; set; }
            //public string BirthPlace { get; set; }
            //public string Fax { get; set; }
            //public bool IsBanned { get; set; }
            //public DateTime? LastPageUpdateDate { get; set; }
            //public bool IsApproved { get; set; }
            //public DateTime? BannedDate { get; set; }
            //public string BannedReason { get; set; }
            //public string Avatar { get; set; }
            //public DateTime? BirthDate { get; set; }
            //HasKey(m => m.Id);

            //Property(m => m.Name)
            //    .IsRequired()
            //    .HasMaxLength(30);

            //HasRequired(m => m.UserDetails).
            //    WithOptional(u => u.ProfessorProfile);

            Property(u => u.PageId).
                IsRequired();

            Property(u => u.PageId).
                HasMaxLength(450);

            HasMany(p => p.LessonClassInfos).
                WithRequired(l => l.ProfessorDetails).
                WillCascadeOnDelete(false);

            HasMany(p => p.LessonFiles).
                WithRequired(l => l.ProfessorDetails).
                WillCascadeOnDelete(false);

            HasMany(p => p.LessonNews).
                WithRequired(l => l.ProfessorDetails).
                WillCascadeOnDelete(false);

            HasMany(p => p.LessonPractices).
                WithRequired(l => l.ProfessorDetails).
                WillCascadeOnDelete(false);

            HasMany(p => p.LessonScores).
                WithRequired(l => l.ProfessorDetails).
                WillCascadeOnDelete(false);

            HasMany(p => p.ImportantDates).
                WithRequired(l => l.ProfessorDetails).
                WillCascadeOnDelete(false);

            HasMany(p => p.PracticeClassInfos).
                WithRequired(l => l.ProfessorDetails).
                WillCascadeOnDelete(false);

            HasMany(p => p.Galleries).
                WithRequired(g => g.ProfessorDetails).
                WillCascadeOnDelete(false);

            HasMany(p => p.GalleryItems).
                WithRequired(g => g.ProfessorDetails).
                WillCascadeOnDelete(false);
        }
    }
}

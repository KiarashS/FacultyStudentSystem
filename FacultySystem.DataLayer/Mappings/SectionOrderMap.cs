using System.Data.Entity.ModelConfiguration;
using ContentManagementSystem.DomainClasses;

namespace ContentManagementSystem.DataLayer.Mappings
{
    public class SectionOrderMap : EntityTypeConfiguration<SectionOrder>
    {
        public SectionOrderMap()
        {
            HasKey(so => so.Id);
            //HasKey(so => new { so.ProfessorId, so.SectionName });
        }
    }
}

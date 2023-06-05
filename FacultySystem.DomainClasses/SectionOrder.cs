using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.DomainClasses
{
    public class SectionOrder : DomainClassBase
    {
        public long Id { get; set; }
        [Index("IX_ProfessorIdAndSectionName", 1, IsUnique = true)]
        public int ProfessorId { get; set; }
        [Index("IX_ProfessorIdAndSectionName", 2, IsUnique = true)]
        public SectionName SectionName { get; set; }
        public int Order { get; set; }

        #region Navigations
        [ForeignKey("ProfessorId")]
        public virtual Professor Professor { get; set; }
        #endregion
    }


    // this enum also include default orders
    public enum SectionName : byte
    {
        [Description("شاخص ها و نمودار ها")]
        IndexAndCharts = 1,
        [Description("بیوگرافی")]
        Bio = 2,
        [Description("آدرس ها")]
        Addresses = 3,
        [Description("سوابق تحصیلی")]
        StudingRecords = 4,
        [Description("سوابق آموزشی")]
        TrainingRecords = 5,
        [Description("عضویت ها")]
        Memberships = 6,
        [Description("سوابق پژوهشی")]
        //[DefaultOrder(2)] Or [DefaultOrder(127)]
        ResearchRecords = 7,
        [Description("مقالات چاپ شده در مجلات معتبر خارجی")]
        ExternalResearchRecords = 8,
        [Description("مقالات چاپ شده در مجلات معتبر داخلی")]
        InternalResearchRecords = 9,
        [Description("مقالات ارائه شده در سمینارها و کنگره های خارجی")]
        ExternalSeminarRecords = 10,
        [Description("مقالات ارائه شده در سمینارها و کنگره های داخلی")]
        InternalSeminarRecords = 11,
        [Description("پایان نامه ها")]
        Theses = 12,
        [Description("تدوین ها و تالیفات")]
        Publications = 13,
        [Description("سوابق اجرایی و مدیریتی")]
        AdministrationRecords = 14,
        [Description("جوایز و افتخارات")]
        Honors = 15,
        [Description("دوره های آموزشی و کارگاه‌ها")]
        Workshops = 16,
        [Description("زبان ها")]
        Languages = 17,
    }

    [AttributeUsageAttribute(AttributeTargets.All)]
    public class DefaultOrderAttribute : Attribute
    {
        public int Order { get; private set; }

        public DefaultOrderAttribute(int order)
        {
            this.Order = order;
        }
    }

    public static partial class DomainEnumExtensions
    {
        public static int GetDefaultOrder(this Enum enu)
        {
            Type type = enu.GetType();

            MemberInfo[] memInfo = type.GetMember(enu.ToString());

            if (memInfo != null && memInfo.Length > 0)
            {

                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DefaultOrderAttribute), false);

                if (attrs != null && attrs.Length > 0)
                    return ((DefaultOrderAttribute)attrs[0]).Order;
            }

            return Convert.ToInt32(enu);
        }
    }
}

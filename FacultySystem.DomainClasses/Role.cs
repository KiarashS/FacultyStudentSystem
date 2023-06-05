using System.Collections.Generic;

namespace ContentManagementSystem.DomainClasses
{
    public class Role : DomainClassBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}

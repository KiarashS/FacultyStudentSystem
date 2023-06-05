using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContentManagementSystem.DomainClasses
{
    public class User : DomainClassBase
    {
        public User()
        {
            RegisterDate = DateTime.UtcNow;
            Roles = new List<Role>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string LastIp { get; set; }
        public string Note { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public DateTime RegisterDate { get; set; }
        //public byte[] RowVersion { get; set; }

        #region Navigations
        public virtual Professor ProfessorProfile { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
        #endregion
    }
}

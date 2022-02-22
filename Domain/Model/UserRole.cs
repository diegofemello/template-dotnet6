using Domain.Model.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Model
{
    [Table("UserRoles")]
    public class UserRole
    {
        public UserRole() {
            Users = new HashSet<User>();
        }

        [Key]
        public string Name { get; set; }

        public ICollection<User> Users { get; set; }
    }
}

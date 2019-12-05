using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B32.Model
{
    [Table("tb_m_user")]
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ChangePassword { get; set; }
        public Role Role { get; set; }
        public DateTimeOffset CreateDate { get; set; }

        public User() { }

        public User(string name, string email, string password, Role type)
        {
            this.Name = name;
            this.Email = email;
            this.Password = password;
            this.Role = type;
            this.CreateDate = DateTimeOffset.Now.LocalDateTime;
        }
    }
}

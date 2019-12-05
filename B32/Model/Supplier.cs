using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B32.Model
{
    [Table("tb_m_supplier")]
    public class Supplier
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public DateTimeOffset CreateDate { get; set; }

        //public int SupplierId { get; set; }
        //public string SupplierEmail { get; set; }

        //public ICollection<Item> Items { get; set; }
        //public ICollection<Item> SupplierEmail { get; set; }

        public Supplier() { }
        public Supplier(string name, string email) {
            this.Name = name;
            this.Email = email;
            this.CreateDate = DateTimeOffset.Now.LocalDateTime;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B32.Model
{
    [Table("tb_m_transaction")]
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        public int Total { get; set; }
        public int Pay { get; set; }
        public DateTimeOffset OrderDate { get; set; }

        public Transaction() { }
        public Transaction(int total, int pay)
        {
            this.Total = total;
            this.Pay = pay;
            this.OrderDate = DateTimeOffset.Now.LocalDateTime;
        }
    }
}

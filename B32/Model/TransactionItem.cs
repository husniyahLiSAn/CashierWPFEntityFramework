using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B32.Model
{
    [Table("tb_m_transactionitem")]
    public class TransactionItem
    {
        [Key]
        public int Id { get; set; }
        public Transaction TransactionFK { get; set; }
        public Item ItemFK { get; set; }
        public int Quantity { get; set; }
        public int SubTotal { get; set; }

        public TransactionItem() { }
        public TransactionItem(Transaction transactionFK, Item itemFK, int quantity, int price)
        {
            this.TransactionFK = transactionFK;
            this.ItemFK = itemFK;
            this.Quantity = quantity;
            this.SubTotal = price;
        }
    }
}

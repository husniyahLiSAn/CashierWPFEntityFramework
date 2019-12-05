using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B32.Model
{
    [Table("tb_m_item")]
    public class Item
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Stock { get; set; }
        public int Price { get; set; }

        //[ForeignKey("SupplierFK")]
        //public int SupplierId { get; set; }
        public Supplier SupplierFK { get; set; }

        public Item() { }
        //public Item (string name, int stock, int price)
        //{
        //    this.Name = name;
        //    this.Stock = stock;
        //    this.Price = price;
        //}
        public Item(string name, int stock, int price, Supplier supplierFK)
        {
            this.Name = name;
            this.Stock = stock;
            this.Price = price;
            this.SupplierFK = supplierFK;
            //this.SupplierEmail = supplierEmail;
        }
    }
}

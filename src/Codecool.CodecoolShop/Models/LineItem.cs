using System;

namespace Codecool.CodecoolShop.Models
{
    public class LineItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime DateCreated { get; set; }
    }
}

using Orders.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Core.Models
{
    public class OrderModel
    {
        public int OrderId { get; set; }
        public int? UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderItemModel>? OrderItemsModel { get; set; }
        public List<OrderStatusModel>? OrderStatusesModel { get; set; }
    }

    public class OrderItemModel
    {
        public int OrderItemId { get; set; }
        public int? OrderId { get; set; }

        [Required(ErrorMessage = "ProductId is required")]
        public required int ProductId { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]

        public required int Quantity { get; set; }
        [Required(ErrorMessage = "Price is required")]

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public required decimal Price { get; set; }
        public decimal? TotalCost { get; set; }

    }

    public class OrderStatusModel
    {
        public int StatusId { get; set; }
        public int? OrderId { get; set; }
        public required string StatusName { get; set; }
    }
}

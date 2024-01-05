using AutoMapper;
using Orders.Core.Entities;
using Orders.Core.Models;
using Orders.Data;

namespace Orders.Service
{
    public interface IOrderService
    {
        Task<OrderModel>? GetOrderByIdAsync(int orderId);
        Task<List<OrderModel>> GetOrdersByUserIdAsync(int userId);
        Task<List<OrderModel>> GetAllOrdersAsync();
        Task<OrderModel> AddOrderAsync(OrderModel orderModel);
        Task<OrderModel> UpdateOrderAsync(OrderModel orderModel);
        Task<bool> DeleteOrderAsync(int orderId);
        Task<bool> CancelOrderAsync(int orderId);
    }

    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<OrderModel>? GetOrderByIdAsync(int orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            return order == null ? null : MapOrderToOrderModel(order);
        }

        public async Task<List<OrderModel>> GetOrdersByUserIdAsync(int userId)
        {
            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);
            return MapOrderListToOrderModelList(orders);
        }

        public async Task<List<OrderModel>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllOrdersAsync();
            return MapOrderListToOrderModelList(orders);
        }

        public async Task<OrderModel> AddOrderAsync(OrderModel orderModel)
        {
            var order = MapOrderModelToOrder(orderModel);
            order.OrderStatuses = new List<OrderStatus>() { new OrderStatus() { StatusName = "Order Placed" } };
            var addedOrder = await _orderRepository.AddOrderAsync(order);
            return MapOrderToOrderModel(addedOrder);
        }

        public async Task<OrderModel> UpdateOrderAsync(OrderModel orderModel)
        {
            var order = MapOrderModelToOrder(orderModel);
            var updatedOrder = await _orderRepository.UpdateOrderAsync(order);
            return MapOrderToOrderModel(updatedOrder);
        }

        public async Task<bool> DeleteOrderAsync(int orderId)
        {
            return await _orderRepository.DeleteOrderAsync(orderId);
        }

        public Task<bool> CancelOrderAsync(int orderId)
        {
            return _orderRepository.CancelOrderAsync(orderId);
        }

        // Manual mapping methods
        private OrderModel MapOrderToOrderModel(Order order)
        {
            return new OrderModel
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                OrderItemsModel = MapOrderItemListToOrderItemModelList(order.OrderItems.ToList()),
                OrderStatusesModel = MapOrderStatusListToOrderStatusModel(order.OrderStatuses.ToList())
            };
        }

        private List<OrderModel> MapOrderListToOrderModelList(List<Order> orders)
        {
            return orders.Select(MapOrderToOrderModel).ToList();
        }

        private OrderItemModel MapOrderItemToOrderItemModel(OrderItem orderItem)
        {
            return new OrderItemModel
            {
                OrderItemId = orderItem.OrderItemId,
                OrderId = orderItem.OrderId,
                ProductId = orderItem.ProductId ?? 0, // Check for nullability
                Quantity = orderItem.Quantity,
                Price = orderItem.Price,
                TotalCost = orderItem.TotalCost
            };
        }

        private List<OrderItemModel> MapOrderItemListToOrderItemModelList(List<OrderItem> orderItems)
        {
            return orderItems.Select(MapOrderItemToOrderItemModel).ToList();
        }

        private OrderStatusModel MapOrderStatusToOrderStatusModel(OrderStatus orderStatus)
        {
            return new OrderStatusModel
            {
                StatusId = orderStatus.StatusId,
                OrderId = orderStatus.OrderId,
                StatusName = orderStatus.StatusName
            };
        }

        private List<OrderStatusModel> MapOrderStatusListToOrderStatusModel(List<OrderStatus> orderStatuses)
        {
            return orderStatuses.Select(MapOrderStatusToOrderStatusModel).ToList();
        }

        private Order MapOrderModelToOrder(OrderModel orderModel)
        {
            return new Order
            {
                OrderId = orderModel.OrderId,
                UserId = orderModel.UserId,
                OrderDate = orderModel.OrderDate,
                TotalAmount = orderModel.TotalAmount,
                OrderItems = MapOrderItemModelListToOrderItemList(orderModel.OrderItemsModel.ToList()),
                OrderStatuses = MapOrderStatusModelToOrderStatus(
                    orderModel.OrderStatusesModel==null?null:orderModel.OrderStatusesModel.ToList())
            };
        }

        private List<OrderItem> MapOrderItemModelListToOrderItemList(List<OrderItemModel>? orderItemModels)
        {
            return orderItemModels?.Select(MapOrderItemModelToOrderItem).ToList() ?? new List<OrderItem>();
        }

        private OrderItem MapOrderItemModelToOrderItem(OrderItemModel orderItemModel)
        {
            return new OrderItem
            {
                OrderItemId = orderItemModel.OrderItemId,
                OrderId = orderItemModel.OrderId,
                ProductId = orderItemModel.ProductId,
                Quantity = orderItemModel.Quantity,
                Price = orderItemModel.Price,
                TotalCost = orderItemModel.TotalCost ?? 0
            };
        }

        private List<OrderStatus> MapOrderStatusModelToOrderStatus(List<OrderStatusModel> orderStatusModel)
        {
            return orderStatusModel != null
                ? orderStatusModel.Select(s => new OrderStatus
                {
                    StatusId = s.StatusId,
                    OrderId = s.OrderId,
                    StatusName = s.StatusName
                }).ToList()
                : new List<OrderStatus>();
        }
    }


}

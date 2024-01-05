using Microsoft.EntityFrameworkCore;
using Orders.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Data
{
    public interface IOrderRepository
    {
        Task<Order>? GetOrderByIdAsync(int orderId);
        Task<List<Order>> GetOrdersByUserIdAsync(int userId);
        Task<List<Order>> GetAllOrdersAsync();
        Task<Order> AddOrderAsync(Order order);
        Task<Order> UpdateOrderAsync(Order order);
        Task<bool> DeleteOrderAsync(int orderId);
        Task<bool> CancelOrderAsync(int orderId);
    }
    public class OrderRepository : IOrderRepository
    {
        private readonly LearnSmartDbContext _dbContext;

        public OrderRepository(LearnSmartDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public  Task<Order>? GetOrderByIdAsync(int orderId)
        {
            return _dbContext.Orders
                .Include(o => o.OrderItems)
                .Include(o => o.OrderStatuses)
                //.Include(o => o.PaymentInformations)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        public async Task<List<Order>> GetOrdersByUserIdAsync(int userId)
        {
            return await _dbContext.Orders
                .Include(o => o.OrderItems)
                .Include(o => o.OrderStatuses)
                .Include(o => o.PaymentInformations)
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _dbContext.Orders
                .Include(o => o.OrderItems)
                .Include(o => o.OrderStatuses)
                .Include(o => o.PaymentInformations)
                .ToListAsync();
        }

        public async Task<Order> AddOrderAsync(Order order)
        {
            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();
            return order;
        }

        public async Task<Order> UpdateOrderAsync(Order order)
        {
            _dbContext.Entry(order).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return order;
        }

        public async Task<bool> DeleteOrderAsync(int orderId)
        {
            var order = await _dbContext.Orders.FindAsync(orderId);
            if (order != null)
            {
                _dbContext.Orders.Remove(order);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> CancelOrderAsync(int orderId)
        {
            var order = await _dbContext.OrderStatuses.FirstOrDefaultAsync(o=>o.OrderId==orderId);
            if (order != null)
            {
                order.StatusName = "Cancelled";
                _dbContext.OrderStatuses.Update(order);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }

}

using HotelManagement.WebApi.Models;

namespace HotelManagement.WebApi.Data
{
    public interface IOrderService
    {
        List<OrderMaster> ReadOrders();
        void WriteOrder(OrderMaster order);
        void WriteAllOrders(List<OrderMaster> orders);
        void UpdateOrder(OrderMaster updatedOrder);
        void DeleteOrder(int orderId);
    }
}

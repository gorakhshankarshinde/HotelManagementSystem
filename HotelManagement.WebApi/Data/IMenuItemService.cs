using HotelManagement.WebApi.Models;

namespace HotelManagement.WebApi.Data
{
    public interface IMenuItemService
    {
        List<MenuItem> ReadMenuItems();
        void WriteMenuItem(MenuItem item);
        void UpdateMenuItem(MenuItem item);
        void DeleteMenuItem(int id);
    }
}

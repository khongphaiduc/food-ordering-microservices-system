using food_service.Models;
using food_service.ProductService.Application.DTOs.Internals;
using food_service.ProductService.Application.Interface;

namespace food_service.ProductService.Infastructure.Repositories
{
    public class OutBoxPatternProduct : IOutBoxPatternProduct
    {
        private readonly FoodProductsDbContext _db;

        public OutBoxPatternProduct(FoodProductsDbContext foodProductsDbContext)
        {
            _db = foodProductsDbContext;
        }

        // tạo message mới trong OutBox table 
        public async Task CreateNewMessage(OutboxMessageDTO message)
        {

            await _db.OutBoxMessages.AddAsync(new OutBoxMessage
            {
                Id = message.Id,
                Type = message.Type,
                Payload = message.PayLoad,
                IsProcessd = false,
                CreateAt = DateTime.Now,
            });


        }
    }
}

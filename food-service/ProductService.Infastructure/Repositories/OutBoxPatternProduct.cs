
using food_service.ProductService.Application.DTOs.Internals;
using food_service.ProductService.Application.Interface;
using food_service.ProductService.Infastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;
using System.Text.Json;

namespace food_service.ProductService.Infastructure.Repositories
{
    public class OutBoxPatternProduct : IOutBoxPatternProduct
    {
        private readonly FoodProductsDbContext _db;

        public OutBoxPatternProduct(FoodProductsDbContext foodProductsDbContext)
        {
            _db = foodProductsDbContext;
        }

        // đánh dấu đã gửi tới message broker 
        public async Task MarkAsProcessed(Guid Id)
        {
            var OutBoxMessage = await _db.OutBoxMessages.FirstOrDefaultAsync(x => x.Id == Id);

            if (OutBoxMessage != null)
            {
                OutBoxMessage.IsProcessd = true;
            }
            await _db.SaveChangesAsync();
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
                CreateAt = DateTime.UtcNow,
            });


        }

        public async Task<OutboxMessageDTO> GetMessageOutBoxMessage()
        {

            var message = await _db.OutBoxMessages.Where(s => s.IsProcessd == false).FirstOrDefaultAsync();
            if (message != null)
            {
                return new OutboxMessageDTO
                {
                    Id = message.Id,
                    Type = message.Type,
                    PayLoad = message.Payload,
                    IsProcesced = message.IsProcessd,
                    CreateAt = message.CreateAt,

                };

            }
            else
            {
                return null;
            }

        }
    }
}

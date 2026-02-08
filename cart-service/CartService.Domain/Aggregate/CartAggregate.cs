using cart_service.CartService.Domain.Entities;
using cart_service.CartService.Domain.ValueObject;

namespace cart_service.CartService.Domain.Aggregate
{
    public class CartAggregate
    {
        public Guid Id { get; private set; }

        public Guid UserId { get; private set; }

        public string Status { get; private set; }

        public Price TotalPrice { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public DateTime UpdatedAt { get; private set; }

        private readonly List<CartItemEntity> _cartItemEntities = new();
        public IReadOnlyList<CartItemEntity> CartItemList => _cartItemEntities;


        private readonly List<CartDiscountEntity> _cartDiscountEntities = new();

        public IReadOnlyList<CartDiscountEntity> CartDiscountList => _cartDiscountEntities;

        internal CartAggregate(Guid id, Guid userId, string status, Price totalPrice, DateTime createdAt, DateTime updatedAt, List<CartItemEntity> cartItemEntities, List<CartDiscountEntity> cartDiscountEntities)
        {
            Id = id;
            UserId = userId;
            Status = status;
            TotalPrice = totalPrice;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            _cartItemEntities = cartItemEntities;
            _cartDiscountEntities = cartDiscountEntities;
        }

        public CartAggregate()
        {
        }

        public static CartAggregate CreateNewCart(Guid IdUser)
        {
            return new CartAggregate
            {
                Id = Guid.NewGuid(),
                UserId = IdUser,
                Status = "ACTIVE",
                TotalPrice = new Price(0),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow

            };
        }


        public void ChangeStatus(string status)
        {
            Status = status;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateTotalPrice(decimal totalPrice)
        {
            TotalPrice = new Price(totalPrice);
            UpdatedAt = DateTime.UtcNow;
        }


        public void AddCartItem(CartItemEntity item)
        {
            _cartItemEntities.Add(item);
        }

        public void RemoveCartItem(CartItemEntity item)
        {
            _cartItemEntities.Remove(item);
        }

        public void AddCartDiscount(CartDiscountEntity discount)
        {
            _cartDiscountEntities.Add(discount);
        }



    }
}

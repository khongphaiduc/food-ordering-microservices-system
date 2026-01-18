using food_service.productservice.infastructure.Models;
using food_service.productservice.infastructure.ProductDbContexts;
using food_service.ProductService.API.GlobalExceptions;
using food_service.ProductService.Domain.Aggragate;
using food_service.ProductService.Domain.Interface;
using food_service.ProductService.Domain.ValueOject;
using Microsoft.EntityFrameworkCore;

namespace food_service.ProductService.Infastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly FoodProductsDbContext _db;

        public CategoryRepository(FoodProductsDbContext foodProductsDbContext)
        {
            _db = foodProductsDbContext;
        }

        public async Task<bool> AddNewCategory(CategoryAggregate NewCategoty)
        {
            var CategoryModel = new Category
            {
                Id = NewCategoty.Id,
                Name = NewCategoty.Name.Value,
                Description = NewCategoty.Description,
                IsActive = NewCategoty.IsActive,
                CreatedAt = NewCategoty.CreateAt,
                UpdatedAt = NewCategoty.UpdateAt,
            };
            await _db.Categories.AddAsync(CategoryModel);
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<CategoryAggregate> GetCategoryById(Guid Id)
        {
            var CategoryOrigin = await _db.Categories.Where(s => s.Id == Id).FirstOrDefaultAsync();

            if (CategoryOrigin != null)
            {
                return new CategoryAggregate(CategoryOrigin.Id, new Name(CategoryOrigin.Name), CategoryOrigin.Description, CategoryOrigin.IsActive, CategoryOrigin.CreatedAt, CategoryOrigin.UpdatedAt);
            }
            throw new NotFoundCategoryException($"Not found Category Id : {Id}");
        }


        public async Task<bool> UpdateCategory(CategoryAggregate updateCategoty)
        {
            var category = await _db.Categories.Where(s => s.Id == updateCategoty.Id).FirstOrDefaultAsync();

            if (category != null)
            {  category.Name = updateCategoty.Name.Value;
                category.Description = updateCategoty.Description;
                category.IsActive = updateCategoty.IsActive;
              
            }
            return await _db.SaveChangesAsync() > 0;
        }

    }
}

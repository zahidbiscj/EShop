using System.Collections.Generic;

namespace EShop.Core.Dto
{
    public class CategoryModel
    {
        public CategoryModel()
        {
            SubCategories = new List<CategoryModel>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? ParentCategoryId { get; set; }

        public List<CategoryModel> SubCategories { get; set; }
    }
}

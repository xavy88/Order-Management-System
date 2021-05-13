using Microsoft.AspNetCore.Mvc.Rendering;
using OMS.DataAccess.Data.Repository.IRepository;
using OMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS.DataAccess.Data.Repository
{
    public class CategoryRepository:Repository<Category>,ICategoryRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }

        public IEnumerable<SelectListItem> GetCategoryListForDropDown()
        {
            return _db.Category.Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
        }
        public void Update(Category category)
        {
                var objFromDb = _db.Category.FirstOrDefault(c => c.Id == category.Id);
                objFromDb.Name = category.Name;
                objFromDb.Status = category.Status;

                _db.SaveChanges();

        }
    }
}

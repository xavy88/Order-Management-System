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
    public class ServiceRepository : Repository<Service>,IServiceRepository
    {
        private readonly ApplicationDbContext _db;

        public ServiceRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }

        public void Update(Service service)
        {
                var objFromDb = _db.Service.FirstOrDefault(c => c.Id == service.Id);
                objFromDb.Name = service.Name;
                objFromDb.Description = service.Description;
                objFromDb.Price = service.Price;
                objFromDb.ImageUrl = service.ImageUrl;
                objFromDb.CategoryId = service.CategoryId;
                objFromDb.Status = service.Status;

                _db.SaveChanges();

        }
    }
}

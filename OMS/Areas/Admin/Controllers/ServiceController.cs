using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OMS.DataAccess.Data.Repository.IRepository;
using OMS.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OMS.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ServiceController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        public ServiceController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            ServiceVM serVM = new ServiceVM()
            {
                Service = new Models.Service(),
                CategoryList= _unitOfWork.Category.GetCategoryListForDropDown(),
            };

            if (id!=null)
            {
                serVM.Service = _unitOfWork.Service.Get(id.GetValueOrDefault());
            }

            return View(serVM);
        }

        #region API Calls
        public IActionResult GetAll()
        {
            return Json(new { data = _unitOfWork.Service.GetAll(incluideProperties:"Category") });
        }

        #endregion


    }
}

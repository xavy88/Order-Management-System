using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OMS.DataAccess.Data.Repository.IRepository;
using OMS.Models.ViewModel;
using OMS.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OMS.Areas.Admin.Controllers
{
    [Authorize(Roles =SD.Admin + ","+SD.Manager)]
    [Area("Admin")]
    public class ServiceController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        
        [BindProperty]
        public ServiceVM SerVM { get; set; }
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
             SerVM = new ServiceVM()
            {
                Service = new Models.Service(),
                CategoryList= _unitOfWork.Category.GetCategoryListForDropDown(),
            };

            if (id!=null)
            {
                SerVM.Service = _unitOfWork.Service.Get(id.GetValueOrDefault());
            }

            return View(SerVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Upsert()
        {
            if (ModelState.IsValid)
            {
                string webRootPath = _hostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if (SerVM.Service.Id==0)
                {
                    //New Service
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"images\product");
                    var extension = Path.GetExtension(files[0].FileName);

                    using(var fileStreams = new FileStream(Path.Combine(uploads,fileName+extension),FileMode.Create))
                    {
                        files[0].CopyTo(fileStreams);
                    }

                    SerVM.Service.ImageUrl = @"\images\product\" + fileName + extension;

                    _unitOfWork.Service.Add(SerVM.Service);
                }
                else
                {
                    //Edit Services
                    var serviceFromDb = _unitOfWork.Service.Get(SerVM.Service.Id);
                    if (files.Count>0)
                    {
                        string fileName = Guid.NewGuid().ToString();
                        var uploads = Path.Combine(webRootPath, @"images\product");
                        var extension_new = Path.GetExtension(files[0].FileName);

                        var imagePath = Path.Combine(webRootPath, serviceFromDb.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                        using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension_new), FileMode.Create))
                        {
                            files[0].CopyTo(fileStreams);
                        }

                        SerVM.Service.ImageUrl = @"\images\product\" + fileName + extension_new;
                    }
                    else
                    {
                        SerVM.Service.ImageUrl = serviceFromDb.ImageUrl;
                    }

                    _unitOfWork.Service.Update(SerVM.Service);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                SerVM.CategoryList = _unitOfWork.Category.GetCategoryListForDropDown();
                return View(SerVM);
            }
        }

        #region API Calls
        public IActionResult GetAll()
        {
            return Json(new { data = _unitOfWork.Service.GetAll(incluideProperties:"Category") });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var serviceFromDb = _unitOfWork.Service.Get(id);
            string webRootPath = _hostEnvironment.WebRootPath;
            var imagePath = Path.Combine(webRootPath, serviceFromDb.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            if (serviceFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting." });
            }

            _unitOfWork.Service.Remove(serviceFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Deleted Successfully." });
        }

        #endregion


    }
}

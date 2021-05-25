using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OMS.DataAccess.Data.Repository.IRepository;
using OMS.Extensions;
using OMS.Models;
using OMS.Models.ViewModel;
using OMS.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace OMS.Controllers
{
    
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private HomeVM HomeVM;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger,IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            
        }

        public IActionResult Index()
        {
            HomeVM = new HomeVM()
            {
                CategoryList = _unitOfWork.Category.GetAll(),
                ServiceList = _unitOfWork.Service.GetAll()
            };
            return View(HomeVM);
        }

        public IActionResult Details(int id)
        {
            var servicesFromDB = _unitOfWork.Service.GetFirstOrDefault(includeProperties: "Category",filter:c=>c.Id==id);
            return View(servicesFromDB);
        }

        public IActionResult AddToCart(int serviceId)
        {
            List<int> sessionList = new List<int>();
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(SD.SessionCart)))
            {
                sessionList.Add(serviceId);
                HttpContext.Session.SetObject(SD.SessionCart, sessionList);
            }
            else
            {
                sessionList = HttpContext.Session.GetObject<List<int>>(SD.SessionCart);
                if (!sessionList.Contains(serviceId))
                {
                    sessionList.Add(serviceId);
                    HttpContext.Session.SetObject(SD.SessionCart, sessionList);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

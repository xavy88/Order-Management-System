using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OMS.DataAccess.Data.Repository.IRepository;
using OMS.Models;
using OMS.Models.ViewModel;
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

        public HomeController(ILogger<HomeController> logger,IUnitOfWork unitOfWork, HomeVM homeVM)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            HomeVM = homeVM;
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

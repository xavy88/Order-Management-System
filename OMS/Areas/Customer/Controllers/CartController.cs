using Microsoft.AspNetCore.Mvc;
using OMS.DataAccess.Data.Repository;
using OMS.DataAccess.Data.Repository.IRepository;
using OMS.Extensions;
using OMS.Models;
using OMS.Models.ViewModel;
using OMS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OMS.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public CartVM CartVM { get; set; }

        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            CartVM = new CartVM()
            {
                OrderHeader = new Models.OrderHeader(),
                ServicesList = new List<Service>()
            };
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetObject<List<int>>(SD.SessionCart)!=null)
            {
                List<int> sessionList = new List<int>();
                sessionList = HttpContext.Session.GetObject<List<int>>(SD.SessionCart);
                foreach (int serviceId in sessionList)
                {
                    CartVM.ServicesList.Add(_unitOfWork.Service.GetFirstOrDefault(u => u.Id == serviceId, includeProperties:"Category"));
                }
            }
            return View(CartVM);
        }

        public IActionResult Remove (int serviceId)
        {
            List<int> sessionList = new List<int>();
            sessionList = HttpContext.Session.GetObject<List<int>>(SD.SessionCart);
            sessionList.Remove(serviceId);
            HttpContext.Session.SetObject(SD.SessionCart, sessionList);

            return RedirectToAction(nameof(Index));
        }
    }
}

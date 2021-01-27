using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using shift_dashboard.Data;
using shift_dashboard.Models;
using shift_dashboard.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace shift_dashboard.Controllers
{
    public class HomeController : Controller
    {
        private DashboardConfig _dashboardOptions;
        private readonly ILogger<HomeController> _logger;
        private DashboardContext _dbcontext;
        private IApiService _apiService;

        public HomeController(ILogger<HomeController> logger, DashboardConfig dashboardconfig, DashboardContext dbcontext, IApiService apiService)
        {
            _dashboardOptions = dashboardconfig;
            _logger = logger;
            _dbcontext = dbcontext;
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            return View( await _apiService.GetDelegatesFromDb());
        }


        [HttpGet]
        public JsonResult GetDelegates()
        {
            if (!_dbcontext.Delegates.Any())
            {
                return null;
            }
            else
            {
                return Json(_dbcontext.Delegates.OrderBy(y => y.Username).Select(x => x.Username.TrimEnd().TrimStart()).ToList());
            }
        }

        public IActionResult About()
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

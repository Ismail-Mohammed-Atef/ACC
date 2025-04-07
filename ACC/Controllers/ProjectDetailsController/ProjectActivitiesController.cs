 using ACC.ViewModels;
using ACC.ViewModels.ProjectActivityVM;
using BusinessLogic.Repository.RepositoryClasses;
using BusinessLogic.Repository.RepositoryInterfaces;
using DataLayer.Models.Enums;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;
using NuGet.Protocol.Core.Types;
using DataLayer.Models.Enums.ProjectActivity;
using Helpers;

namespace ACC.Controllers.ProjectDetailsController
{
    public class ProjectActivitiesController : Controller
    {
        private readonly IProjectActivityRepository activityRepository;

        public ProjectActivitiesController(IProjectActivityRepository _activityRepository)
        {
            activityRepository = _activityRepository;
        }



        public IActionResult Index(int page = 1, int pageSize = 4, string activityType = null, DateTime? startDate = null, DateTime? endDate = null)
        
        {
            // استرجاع جميع الأنشطة
            var query = activityRepository.GetAll();

            // تطبيق الفلاتر
            if (!string.IsNullOrEmpty(activityType))
            {
                query = query.Where(a => a.ActivityType == activityType).ToList();
            }

            if (startDate.HasValue)
            {
                query = query.Where(a => a.Date >= startDate.Value).ToList();
            }

            if (endDate.HasValue)
            {
                query = query.Where(a => a.Date <= endDate.Value).ToList();
            }

            // حساب إجمالي السجلات بعد الفلترة
            int totalRecords = query.Count();

            // تقسيم البيانات إلى صفحات
            var ActivitiesListModel = query
                .Select(a => new ProjectActivityVM
                {
                    Id = a.Id,
                    Date = a.Date,
                    ActivityType = a.ActivityType,
                    ActivityDetail = a.ActivityDetail,
                })
                .Skip((page - 1) * pageSize)  // تخطي العناصر بناءً على الصفحة الحالية
                .Take(pageSize)  // أخذ العناصر بناءً على حجم الصفحة
                .ToList();

            ViewBag.TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            ViewBag.CurrentPage = page;
            ViewBag.ActivityTypeFilter = activityType;
            ViewBag.StartDateFilter = startDate;
            ViewBag.EndDateFilter = endDate;
            ViewBag.ActivitTypeList = Enum_Helper.GetEnumSelectListWithDisplayNames<ActivityType>();

            return View("Index", ActivitiesListModel);
        }






    }
}

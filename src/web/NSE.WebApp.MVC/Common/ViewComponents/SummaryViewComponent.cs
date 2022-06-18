using Microsoft.AspNetCore.Mvc;

namespace NSE.WebApp.MVC.Common.ViewComponents
{
    public class SummaryViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}

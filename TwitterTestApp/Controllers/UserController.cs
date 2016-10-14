using System.Web.Mvc;
using TwitterTestApp.Models.TwitterFactory;

namespace TwitterTestApp.Controllers
{
    public class UserController : Controller
    {
        public ActionResult Index(string username)
        {
            var twitterViewModelFactory = new TwitterViewModelFactory();

            var model = twitterViewModelFactory.CreateViewModel(string.IsNullOrEmpty(username) ? "" : username);

            return View(model);
        }

    }
}

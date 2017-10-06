using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;
using PairingTest.Web.Models;
using PairingTest.Web.Wrappers.Interfaces;

namespace PairingTest.Web.Controllers
{
    public class QuestionnaireController : Controller
    {
        private readonly IHttpClientWrapper _httpClientWrapper;

        public QuestionnaireController(IHttpClientWrapper httpClientWrapper)
        {
            _httpClientWrapper = httpClientWrapper;
            _httpClientWrapper.Initialise(ConfigurationManager.AppSettings["QuestionnaireServiceUri"], "QuestionServiceWebApi");
        }
        
        public async Task<ViewResult> Index()
        {
            return await Task.Run(() => View(_httpClientWrapper.GetAsync<QuestionnaireViewModel>("Questions/Get")));
        }
    }
}

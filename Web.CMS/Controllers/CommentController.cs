using Azure.Core;
using Caching.Elasticsearch;
using Entities.ViewModels;
using Entities.ViewModels.Comment;
using HuloToys_Service.ElasticSearch.NewEs;
using Microsoft.AspNetCore.Mvc;
using Repositories.IRepositories;
using Repositories.Repositories;

namespace WEB.CMS.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentRepository _IcommentRepository;
        private readonly ClientESService _ClientESService;
        public CommentController(ICommentRepository icommentRepository,IConfiguration configuration)
        {
            _IcommentRepository = icommentRepository;
            _ClientESService = new ClientESService(configuration["DataBaseConfig:Elastic:Host"], configuration);

        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ListComment(CommentParamRequest request)
        {
            var lst = await _IcommentRepository.GetAllComment(request);
            return View(lst);
        }

        public async Task<GenericViewModel<CommentViewModel>> GetAllComment(CommentParamRequest request) 
        {
            return await _IcommentRepository.GetAllComment(request);
        }

        public async Task<IActionResult> Client(string phoneOrName) 
        {
            try
            {
                if (!string.IsNullOrEmpty(phoneOrName))
                    return Ok(new
                    {
                        is_success = true,
                        data = _ClientESService.GetClientByNameOrPhone(phoneOrName)
                    });
            }
            catch
            {

            }
            return Ok(new
            {
                is_success = false
            });
        }
    }
}

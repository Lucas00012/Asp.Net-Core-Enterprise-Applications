using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Models;
using System.Diagnostics;

namespace NSE.WebApp.MVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Route("sistema-indisponivel")]
        public IActionResult SistemaIndisponivel()
        {
            var modelErro = new ErrorViewModel
            {
                Mensagem = "O sistema está temporariamente indisponível, isto pode ocorrer em momentos de sobrecarga de usuários.",
                Titulo = "Sistema indisponível.",
                ErroCode = 500
            };

            return View("Error", modelErro);
        }

        [Route("erro/{id:length(3,3)}")]
        public IActionResult Error(int id)
        {
            return id switch
            {
                500 => View("Error", new ErrorViewModel
                {
                    Mensagem = "Ocorreu um erro! Tente novamente mais tarde ou contate nosso suporte.",
                    Titulo = "Ocorreu um erro!",
                    ErroCode = id,
                }),

                404 => View("Error", new ErrorViewModel
                {
                    Mensagem = "A página que está procurando não existe! <br />Em caso de dúvidas entre em contato com nosso suporte",
                    Titulo = "Ops! Página não encontrada.",
                    ErroCode = id,
                }),

                403 => View("Error", new ErrorViewModel
                {
                    Mensagem = "Você não tem permissão para fazer isto.",
                    Titulo = "Acesso Negado",
                    ErroCode = id,
                }),

                _ => StatusCode(404)
            };
        }
    }
}
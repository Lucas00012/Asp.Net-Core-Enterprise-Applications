using Microsoft.AspNetCore.Mvc;

namespace NSE.Identidade.API.Controllers
{
    [ApiController]
    public abstract class MainController : ControllerBase
    {
        private List<string> Erros = new List<string>();

        protected IActionResult CustomResponse(object result = null)
        {
            if (OperacaoValida())
            {
                return Ok(result);
            }

            return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
            {
                { "Mensagens", Erros.ToArray() }
            }));
        }

        protected bool OperacaoValida()
        {
            return !Erros.Any();
        }

        protected void AdicionarErro(string erro)
        {
            Erros.Add(erro);
        }

        protected void AdicionarErros(IEnumerable<string> erros)
        {
            Erros.AddRange(erros);
        }

        protected void LimparErros()
        {
            Erros.Clear();
        }
    }
}

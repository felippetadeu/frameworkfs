using Framework.Attributes;
using Framework.Authorize;
using Framework.BO;
using Framework.CustomException;
using Framework.Filters;
using Framework.HttpResult;
using Framework.Model;
using System;
using System.Net;
using System.Web.Http;

namespace Framework.Controller
{
    [AuthorizeControllerActions(checkList: false, checkFind: false)]
    public class UsuarioController : AbstractCRUDController<Usuario>
    {
        protected override AbstractBO<Usuario> RetornaBO()
        {
            if (BO == null)
            {
                BO = new UsuarioBO(ConnectionFactory, UsuarioId, EmpresaId);
            }

            return BO;
        }

        private UsuarioBO UsuarioBO {
            get { return (UsuarioBO)RetornaBO(); }
        }

        [HttpPost]
        [Compress]
        [Authorize]
        public IHttpActionResult retornadadosporusuario()
        {
            try
            {
                var bo = RetornaBO();
                return Ok(new Result(value: UsuarioBO.RetornaDadosUsuario()));
            }
            catch (BrokenRulesException ex)
            {
                return Ok(new Result(message: ex.Message, code: HttpStatusCode.PreconditionFailed));
            }
            catch (Exception ex)
            {
                return Ok(new Result(message: "Ops! Ocorreu um erro ao inserir.", code: HttpStatusCode.InternalServerError, value:UsuarioId, ex: ex));
            }
        }
    }
}

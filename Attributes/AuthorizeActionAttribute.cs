using Framework.Attributes;
using Framework.DAO;
using Framework.DbConnection;
using Framework.Enum;
using Framework.FileManipulations;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Framework.Authorize
{
    public class AuthorizeActionAttribute : AuthorizeAttribute
    {
        private string AuthMethod { get; set; }

        private string[] RelationControllerName { get; set; }

        public AuthorizeActionAttribute(string method = null, string[] controller = null)
        {
            if (!string.IsNullOrEmpty(method))
                AuthMethod = method;

            RelationControllerName = controller;
            if (RelationControllerName == null)
            {
                RelationControllerName = new string[] { };
            }
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var actionName = actionContext.ActionDescriptor.ActionName;
            if (!string.IsNullOrEmpty(AuthMethod))
                actionName = AuthMethod;

            var controllerName = actionContext.ControllerContext.ControllerDescriptor.ControllerName;
            var controllerActionsToCheck = actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AuthorizeControllerActionsAttribute>().FirstOrDefault();

            bool checkAction = true;

            string[] defaultActions = new string[] { "Insert", "IdentityInsert", "Find", "List", "Update", "Delete", "Activate", "Deactivate" };

            if (defaultActions.Contains(actionName) && (
                (actionName == "Insert" && controllerActionsToCheck.CheckInsert)
                    || (actionName == "IdentityInsert" && controllerActionsToCheck.CheckIdentityInsert)
                        || (actionName == "Find" && controllerActionsToCheck.CheckFind)
                            || (actionName == "List" && controllerActionsToCheck.CheckList)
                                || (actionName == "Update" && controllerActionsToCheck.CheckUpdate)
                                    || (actionName == "Delete" && controllerActionsToCheck.CheckDelete)
                                        || (actionName == "Activate" && controllerActionsToCheck.CheckActivate)
                                            || (actionName == "Deactivate" && controllerActionsToCheck.CheckDeactivate)))
                checkAction = true;
            else if (defaultActions.Contains(actionName))
                checkAction = false;

            if (checkAction)
            {
                base.OnAuthorization(actionContext);
                if (!HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    return;
                }
                var identity = HttpContext.Current.User.Identity as System.Security.Claims.ClaimsIdentity;
                int empresaId = Convert.ToInt32(identity.Claims.Single(x => x.Type == "EmpresaId").Value);
                int usuarioId = Convert.ToInt32(identity.Claims.Single(x => x.Type == "Id").Value);

                bool usuarioAutorizado = false;

                string connectionString = WebConfigManipulation.GetConfig("ConnectionString");
                ConnectionEnum connectionType = (ConnectionEnum)Convert.ToInt32(WebConfigManipulation.GetConfig("ConnectionType"));
                using (var connectionFactory = new ConnectionFactory(connectionString, connectionType))
                    usuarioAutorizado = new PermissaoUsuarioDAO(connectionFactory, empresaId).UsuarioPossuiPermissao(usuarioId, controllerName, actionName);

                if (!usuarioAutorizado)
                {
                    HandleUnauthorizedRequest(actionContext);
                }
            }
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
                base.HandleUnauthorizedRequest(actionContext);
            else
            {
                actionContext.Response = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Forbidden,
                    Content = new StringContent("Usuário não possui permissão para executar essa ação")
                };
            }
        }
    }
}

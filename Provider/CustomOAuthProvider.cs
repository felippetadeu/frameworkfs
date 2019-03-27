using Framework.BO;
using Framework.DbConnection;
using Framework.Enum;
using Framework.FileManipulations;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Framework.Provider
{
    public abstract class CustomOAuthProvider : OAuthAuthorizationServerProvider
    {
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override Task TokenEndpointResponse(OAuthTokenEndpointResponseContext context)
        {
            var accessToken = context.AccessToken;
            var identidade = context.Identity;
            string connectionstring = WebConfigManipulation.GetConfig("ConnectionString");
            ConnectionEnum connectionType = (ConnectionEnum)Convert.ToInt32(WebConfigManipulation.GetConfig("ConnectionType"));

            var connectionFactory = new ConnectionFactory(connectionstring, connectionType);

            int id = Convert.ToInt32(identidade.Claims.Single(x => x.Type == "Id").Value);
            int empresaId = Convert.ToInt32(identidade.Claims.Single(x => x.Type == "EmpresaId").Value);
            var userBO = new UsuarioBO(connectionFactory, id, empresaId);
            userBO.AtualizarToken(id, accessToken);
            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            string connectionstring = WebConfigManipulation.GetConfig("ConnectionString");
            ConnectionEnum connectionType = (ConnectionEnum)Convert.ToInt32(WebConfigManipulation.GetConfig("ConnectionType"));

            var connectionFactory = new ConnectionFactory(connectionstring, connectionType);

            var userBO = new UsuarioBO(connectionFactory, 0, 0);

            int empresaId = Convert.ToInt32(System.Web.HttpContext.Current.Request.Params["empresaId"]);

            var loginUser = userBO.Login(context.UserName, context.Password, empresaId);
            if (loginUser == null)
            {
                context.SetError("invalid_grant", "Usuário ou senha incorretos");
                return;
            }

            ClaimsIdentity identity = new ClaimsIdentity(OAuthDefaults.AuthenticationType);
            identity.AddClaim(new Claim("Id", loginUser.UsuarioId.ToString()));
            identity.AddClaim(new Claim("EmpresaId", loginUser.EmpresaId.ToString()));

            var ticket = new AuthenticationTicket(identity, null);
            context.Validated(ticket);
            await Task.FromResult<object>(ticket);
        }
    }
}

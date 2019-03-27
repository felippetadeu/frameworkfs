using Framework.BO;
using Framework.DbConnection;
using Framework.Enum;
using Framework.FileManipulations;
using System;
using System.Linq;
using System.Web.Http;

namespace Framework.Controller
{
    public abstract class AbstractController<T> : ApiController, IDisposable where T : new()
    {
        protected string ConnectionString { get; set; }
        protected ConnectionEnum ConnectionType { get; set; }

        protected AbstractBO<T> BO { get; set; }

        protected abstract AbstractBO<T> RetornaBO();

        protected ConnectionFactory ConnectionFactory { get; set; }

        protected int UsuarioId { get; private set; }

        protected int? EmpresaId { get; private set; }

        public AbstractController()
        {
            string connectionstring = WebConfigManipulation.GetConfig("ConnectionString");
            ConnectionEnum connectionType = (ConnectionEnum)Convert.ToInt32(WebConfigManipulation.GetConfig("ConnectionType"));
            ConnectionString = connectionstring;
            ConnectionType = connectionType;
            ConnectionFactory = new ConnectionFactory(ConnectionString, ConnectionType);

            var identity = User.Identity as System.Security.Claims.ClaimsIdentity;
            int usuarioId = 0;
            int? empresaId = null;

            if (identity.Claims.Count() > 0)
            {
                usuarioId = Convert.ToInt32(identity.Claims.Single(x => x.Type == "Id").Value);
                if (identity.Claims.Any(x => x.Type == "EmpresaId"))
                {
                    empresaId = Convert.ToInt32(identity.Claims.Single(x => x.Type == "EmpresaId").Value);
                }
            }

            UsuarioId = usuarioId;
            EmpresaId = empresaId;
        }

        protected override void Dispose(bool disposing)
        {
            ConnectionFactory.Dispose();
            base.Dispose(disposing);
        }
    }
}

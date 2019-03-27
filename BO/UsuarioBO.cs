using System.Collections.Generic;
using Framework.Criptografia;
using Framework.CustomException;
using Framework.DAO;
using Framework.DbConnection;
using Framework.Enum;
using Framework.Model;

namespace Framework.BO
{
    public class UsuarioBO : AbstractBO<Usuario>
    {
        public UsuarioBO(ConnectionFactory factory, int usuarioId, int? empresaId) : base(factory, usuarioId, empresaId)
        {
        }

        public override AbstractDAO<Usuario> GetDAO()
        {
            if (DAO == null)
            {
                DAO = new UsuarioDAO(ConnectionFactory, EmpresaId);
            }

            return DAO;
        }

        public UsuarioDAO UsuarioDAO
        {
            get
            {
                return (UsuarioDAO)GetDAO();
            }
        }

        public Usuario Login(string user, string pwd, int empresaId)
        {
            return UsuarioDAO.Login(user, CryptManager.StringToMD5(pwd), empresaId);
        }

        public void AtualizarToken(int usuarioId, string token)
        {
            var usuario = UsuarioDAO.Find(usuarioId);
            if (usuario != null)
            {
                usuario.Token = token;
                UsuarioDAO.Update(usuario);
            }
        }

        protected override void BeforeUpdate(Usuario model)
        {
            var usuario = UsuarioDAO.Find(model.UsuarioId);

            if (usuario != null)
            {
                if (string.IsNullOrEmpty(model.Senha))
                {
                    model.Senha = usuario.Senha;
                }
                else
                {
                    model.Senha = CryptManager.StringToMD5(model.Senha);
                }

                model.Token = usuario.Token;
            }
        }

        protected override void BeforeSave(Usuario model, BusinessObjectAcaoEnum acao)
        {
            base.BeforeSave(model, acao);

            var findUsuario = Find(model.UsuarioId);

            if (string.IsNullOrEmpty(model.Nome))
                throw new BrokenRulesException("É necessário informar o nome do usuário");

            if (string.IsNullOrEmpty(model.Email))
                throw new BrokenRulesException("É necessário informar o email do usuário");

            if (acao == BusinessObjectAcaoEnum.IdentityInsert && string.IsNullOrEmpty(model.Senha))
                throw new BrokenRulesException("É necessário informar a senha do usuário");
            
            var findEmail = UsuarioDAO.ValidaEmailExistente(model.Email, model.UsuarioId);

            if (findEmail)
            {
                throw new BrokenRulesException("O email digitado já se está sendo utilizado!");
            }

            if (model.Administrativo != 1)
            {
                model.Administrativo = 0;
            }

            if (!string.IsNullOrEmpty(model.Senha))
            {
                model.Senha = CryptManager.StringToMD5(model.Senha);
            }
            else {
                model.Senha = findUsuario.Senha;
            }

            model.EmpresaId = EmpresaId.Value;
        }

        protected override void AfterList(List<Usuario> list)
        {
            base.AfterList(list);
            foreach (var item in list)
            {
                item.Senha = "";
                item.Token = "";
            }
        }

        public Usuario RetornaDadosUsuario() {
            var retorno = Find(UsuarioId);

            retorno.Senha = "";

            return retorno;
        }
    }
}
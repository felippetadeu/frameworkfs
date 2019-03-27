using Framework.ClassManipulations;
using Framework.CustomException;
using Framework.DAO;
using Framework.DbConnection;
using Framework.Enum;
using Framework.Interfaces;
using Framework.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Framework.BO
{
    public abstract class AbstractBO<T> where T : new()
    {
        protected ConnectionFactory ConnectionFactory { get; private set; }

        protected int UsuarioId { get; private set; } = 0;

        protected int? EmpresaId { get; private set; } = null;

        private LogSistemaDAO LogSistemaDAO { get; set; } = null;

        protected AbstractDAO<T> DAO { get; set; }

        public AbstractBO() { }

        protected AbstractBO(ConnectionFactory factory, int usuarioId, int? empresaId)
        {
            ConnectionFactory = factory;
            UsuarioId = usuarioId;
            EmpresaId = empresaId;
            if (UsuarioId > 0)
            {
                LogSistemaDAO = new LogSistemaDAO(factory, EmpresaId);
            }
        }

        public abstract AbstractDAO<T> GetDAO();

        private void SetDefaultValues(T model)
        {
            if (((model as IChildEmpresaObject) != null) && EmpresaId.HasValue)
            {
                var empresaProp = ClassManipulation.GetColumn<T>("EmpresaId");
                if (empresaProp != null)
                {
                    empresaProp.SetValue(model, EmpresaId.Value);
                }
            }
            if (((model as IChildUsuarioObject) != null) && UsuarioId > 0)
            {
                var usuarioProp = ClassManipulation.GetColumn<T>("UsuarioId");
                if (usuarioProp != null)
                {
                    usuarioProp.SetValue(model, UsuarioId);
                }
            }
        }

        public T Insert(T model)
        {
            ConnectionFactory.BeginTransaction();
            BeforeInsert(model);
            BeforeSave(model, BusinessObjectAcaoEnum.Insert);
            var retorno = GetDAO().Insert(model);
            ConnectionFactory.Commit();
            SaveLog(model, LogSistemaAcaoEnum.Inserir);
            return retorno;
        }

        public T IdentityInsert(T model)
        {
            ConnectionFactory.BeginTransaction();
            BeforeIdentityInsert(model);
            BeforeSave(model, BusinessObjectAcaoEnum.IdentityInsert);
            var retorno = GetDAO().IdentityInsert(model);
            ConnectionFactory.Commit();
            SaveLog(model, LogSistemaAcaoEnum.Inserir);
            return retorno;
        }

        public void IdentityInsertImage(T model, HttpFileCollection imagem)
        {
            ConnectionFactory.BeginTransaction();
            BeforeIdentityInsert(model);
            BeforeSave(model, BusinessObjectAcaoEnum.IdentityInsert);

            foreach (string item in imagem)
            {
                SalvarImagem(model, imagem[item]);
                GetDAO().IdentityInsert(model);
                SaveLog(model, LogSistemaAcaoEnum.Inserir);
            }
            
            ConnectionFactory.Commit();
        }

        public void IdentityInsertMultipleImage(List<T> models, HttpFileCollection imagem)
        {
            if (models.Count == imagem.Count)
            {
                ConnectionFactory.BeginTransaction();

                for (int idx = 0; idx < imagem.Count; idx++)
                {
                    BeforeIdentityInsert(models.ElementAt(idx));
                    BeforeSave(models.ElementAt(idx), BusinessObjectAcaoEnum.IdentityInsert);

                    SalvarImagem(models.ElementAt(idx), imagem[idx]);
                    GetDAO().IdentityInsert(models.ElementAt(idx));
                    SaveLog(models.ElementAt(idx), LogSistemaAcaoEnum.Inserir);
                }

                ConnectionFactory.Commit();
            }
            else
            {
                throw new BrokenRulesException("Não foi possível realizar essa operação!");
            }
        }

        public void Delete(T model)
        {
            ConnectionFactory.BeginTransaction();
            BeforeDelete(model);
            GetDAO().Delete(model);
            ConnectionFactory.Commit();
            SaveLog(model, LogSistemaAcaoEnum.Excluir);
        }

        public T Update(T model)
        {
            ConnectionFactory.BeginTransaction();
            BeforeUpdate(model);
            BeforeSave(model, BusinessObjectAcaoEnum.Update);
            var retorno = GetDAO().Update(model);
            ConnectionFactory.Commit();
            SaveLog(model, LogSistemaAcaoEnum.Editar);
            return retorno;
        }

        public T UpdateImage(T model, HttpPostedFile imagem)
        {
            ConnectionFactory.BeginTransaction();
            BeforeUpdate(model);
            BeforeSave(model, BusinessObjectAcaoEnum.Update);

            if (imagem != null)
            {
                SalvarImagem(model, imagem);
            }
            else
            {
                var find = Find((int)ClassManipulation.GetIdentityProp<T>().GetValue(model));

                if (find != null)
                {
                    if ((model as IFileUploadObject) != null)
                    {
                        (model as IFileUploadObject).Imagem = (find as IFileUploadObject).Imagem;
                    }
                    else
                    {
                        throw new BrokenRulesException("Não foi possível carregar a imagem cadastrada!");
                    }
                }
                else {
                    throw new BrokenRulesException("Não foi possível carregar a imagem cadastrada!");
                }
            }

            var retorno = GetDAO().Update(model);
            ConnectionFactory.Commit();
            SaveLog(model, LogSistemaAcaoEnum.Editar);
            return retorno;
        }

        public T Find(int id)
        {
            var obj = GetDAO().Find(id);
            AfterFind(obj);
            return obj;
        }

        public List<T> List()
        {
            var list = GetDAO().List();
            AfterList(list);
            return list;
        }

        public List<T> List(FilterObject<T> filter)
        {
            return GetDAO().List(filter);
        }

        public T Deactivate(T model)
        {
            if (model is IActivableObject)
            {
                ConnectionFactory.BeginTransaction();
                var retorno = GetDAO().Deactivate(model);
                ConnectionFactory.Commit();
                SaveLog(model, LogSistemaAcaoEnum.Desativar);
                return retorno;
            }
            else
            {
                throw new BrokenRulesException("Objeto não suporta desativação");
            }
        }

        public T Activate(T model)
        {
            if (model is IActivableObject)
            {
                ConnectionFactory.BeginTransaction();
                var retorno = GetDAO().Activate(model);
                ConnectionFactory.Commit();
                SaveLog(model, LogSistemaAcaoEnum.Ativar);
                return retorno;
            }
            else
            {
                throw new BrokenRulesException("Objeto não suporta ativação");
            }
        }

        protected virtual void SaveLog(T model, LogSistemaAcaoEnum action)
        {
            if (UsuarioId > 0)
            {
                LogSistema log = new LogSistema
                {
                    Acao = (int)action,
                    DataHora = DateTime.UtcNow,
                    UsuarioId = UsuarioId,
                    Dados = JsonConvert.SerializeObject(model),
                    EmpresaId = EmpresaId.Value
                };

                LogSistemaDAO.IdentityInsert(log);
            }
        }

        protected virtual void BeforeUpdate(T model)
        {
            SetDefaultValues(model);
        }

        protected virtual void BeforeInsert(T model)
        {
            SetDefaultValues(model);
        }

        protected virtual void BeforeIdentityInsert(T model)
        {
            SetDefaultValues(model);
        }

        protected virtual void BeforeDelete(T model)
        {
            SetDefaultValues(model);
        }

        protected virtual void BeforeSave(T model, BusinessObjectAcaoEnum acao) { }

        protected virtual void AfterList(List<T> list) { }

        protected virtual void AfterFind(T model) { }
        
        private void SalvarImagem(T model, HttpPostedFile imagem)
        {
            bool flag = false;
            string path = "/Uploads/";
            string str = HttpContext.Current.Server.MapPath("~" + path);
            if (imagem != null && imagem.ContentLength > 0)
            {
                string extension = Path.GetExtension(imagem.FileName);
                if (extension != null)
                {
                    string fileExtension = extension.ToLower();
                    string[] source = new string[4]
                    {
                        ".gif",
                        ".png",
                        ".jpeg",
                        ".jpg"
                    };

                    flag = source.Any(t => t == fileExtension);
                }
                if (flag)
                {
                    string text2 = "";
                    text2 = Guid.NewGuid().ToString();
                    string extension2 = Path.GetExtension(imagem.FileName);
                    if (File.Exists(str + text2 + extension2))
                        SalvarImagem(model, imagem);
                    else
                    {
                        imagem.SaveAs(str + text2 + extension2);
                        text2 += extension2;
                        (model as IFileUploadObject).Imagem = path + text2;
                    }
                }
                else
                {
                    throw new BrokenRulesException("Selecione uma imagem válida (.gif, .png, .jpeg ou .jpg)");
                }
            }
        }
    }
}

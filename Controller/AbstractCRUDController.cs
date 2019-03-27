using Framework.Authorize;
using Framework.BO;
using Framework.CustomException;
using Framework.DAO;
using Framework.Filters;
using Framework.HttpResult;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Web.Http;

namespace Framework.Controller
{
    public abstract class AbstractCRUDController<T> : AbstractController<T> where T : new()
    {
        [HttpPost]
        [AuthorizeAction]
        public IHttpActionResult Insert(T model)
        {
            try
            {
                var bo = RetornaBO();
                model = bo.Insert(model);
                return Ok(new Result(value: model));
            }
            catch (BrokenRulesException ex)
            {
                return Ok(new Result(message: ex.Message, code: HttpStatusCode.PreconditionFailed));
            }
            catch (Exception ex)
            {
                return Ok(new Result(message: "Ops! Ocorreu um erro ao inserir.", code: HttpStatusCode.InternalServerError, value: model, ex: ex));
            }
        }

        [HttpPost]
        [AuthorizeAction]
        public IHttpActionResult IdentityInsert(T model)
        {
            try
            {
                var bo = RetornaBO();
                model = bo.IdentityInsert(model);
                return Ok(new Result(value: model));
            }
            catch (BrokenRulesException ex)
            {
                return Ok(new Result(message: ex.Message, code: HttpStatusCode.PreconditionFailed));
            }
            catch (Exception ex)
            {
                return Ok(new Result(message: "Ops! Ocorreu um erro ao inserir.", code: HttpStatusCode.InternalServerError, value: model, ex: ex));
            }
        }

        [HttpPost]
        [AuthorizeAction]
        public IHttpActionResult IdentityInsertImage()
        {
            try
            {
                var bo = RetornaBO();
                var requestJson = HttpContext.Current.Request.Params["model"];
                T model = JsonConvert.DeserializeObject<T>(requestJson);
                bo.IdentityInsertImage(model, HttpContext.Current.Request.Files);
                return Ok(new Result());
            }
            catch (BrokenRulesException ex)
            {
                return Ok(new Result(message: ex.Message, code: HttpStatusCode.PreconditionFailed));
            }
            catch (Exception ex)
            {
                return Ok(new Result(message: "Ops! Ocorreu um erro ao inserir.", code: HttpStatusCode.InternalServerError, ex: ex));
            }
        }

        [HttpPost]
        [AuthorizeAction]
        public IHttpActionResult IdentityInsertMultipleImage()
        {
            try
            {
                var bo = RetornaBO();
                var requestJson = HttpContext.Current.Request.Params["model"];
                List<T> models = JsonConvert.DeserializeObject<List<T>>(requestJson);
                bo.IdentityInsertMultipleImage(models, HttpContext.Current.Request.Files);
                return Ok(new Result());
            }
            catch (BrokenRulesException ex)
            {
                return Ok(new Result(message: ex.Message, code: HttpStatusCode.PreconditionFailed));
            }
            catch (Exception ex)
            {
                return Ok(new Result(message: "Ops! Ocorreu um erro ao inserir.", code: HttpStatusCode.InternalServerError, ex: ex));
            }
        }

        [HttpGet]
        [Compress]
        [AuthorizeAction]
        public IHttpActionResult Find(int id)
        {
            try
            {
                var bo = RetornaBO();
                return Ok(new Result(value: bo.Find(id)));
            }
            catch (BrokenRulesException ex)
            {
                return Ok(new Result(message: ex.Message, code: HttpStatusCode.PreconditionFailed));
            }
            catch (Exception ex)
            {
                return Ok(new Result(message: "Ops! Ocorreu um erro ao inserir.", code: HttpStatusCode.InternalServerError, value: id, ex: ex));
            }
        }

        [HttpGet]
        [Compress]
        [AuthorizeAction]
        public IHttpActionResult List()
        {
            try
            {
                var bo = RetornaBO();
                return Ok(new Result(value: bo.List()));
            }
            catch (BrokenRulesException ex)
            {
                return Ok(new Result(message: ex.Message, code: HttpStatusCode.PreconditionFailed));
            }
            catch (Exception ex)
            {
                return Ok(new Result(message: "Ops! Ocorreu um erro ao inserir.", code: HttpStatusCode.InternalServerError, ex: ex));
            }
        }

        [HttpPost]
        [Compress]
        [AuthorizeAction]
        public IHttpActionResult List(FilterObject<T> filter)
        {
            try
            {
                var bo = RetornaBO();
                return Ok(new Result(value: bo.List(filter)));
            }
            catch (BrokenRulesException ex)
            {
                return Ok(new Result(message: ex.Message, code: HttpStatusCode.PreconditionFailed));
            }
            catch (Exception ex)
            {
                return Ok(new Result(message: "Ops! Ocorreu um erro ao inserir.", code: HttpStatusCode.InternalServerError, value: filter, ex: ex));
            }
        }

        [HttpPost]
        [AuthorizeAction]
        public IHttpActionResult Update(T model)
        {
            try
            {
                var bo = RetornaBO();
                model = bo.Update(model);
                return Ok(new Result(value: model));
            }
            catch (BrokenRulesException ex)
            {
                return Ok(new Result(message: ex.Message, code: HttpStatusCode.PreconditionFailed));
            }
            catch (Exception ex)
            {
                return Ok(new Result(message: "Ops! Ocorreu um erro ao inserir.", code: HttpStatusCode.InternalServerError, value: model, ex: ex));
            }
        }

        [HttpPost]
        [AuthorizeAction]
        public IHttpActionResult UpdateImage()
        {
            try
            {
                var bo = RetornaBO();
                var requestJson = HttpContext.Current.Request.Params["model"];
                T model = JsonConvert.DeserializeObject<T>(requestJson);
                model = bo.UpdateImage(model, HttpContext.Current.Request.Files["UploadedImage"]);
                return Ok(new Result(value: model));
            }
            catch (BrokenRulesException ex)
            {
                return Ok(new Result(message: ex.Message, code: HttpStatusCode.PreconditionFailed));
            }
            catch (Exception ex)
            {
                return Ok(new Result(message: "Ops! Ocorreu um erro ao inserir.", code: HttpStatusCode.InternalServerError, ex: ex));
            }
        }

        [HttpPost]
        [AuthorizeAction]
        public IHttpActionResult Delete(T model)
        {
            try
            {
                var bo = RetornaBO();
                bo.Delete(model);
                return Ok(new Result());
            }
            catch (BrokenRulesException ex)
            {
                return Ok(new Result(message: ex.Message, code: HttpStatusCode.PreconditionFailed));
            }
            catch (Exception ex)
            {
                return Ok(new Result(message: "Ops! Ocorreu um erro ao inserir.", code: HttpStatusCode.InternalServerError, value: model, ex: ex));
            }
        }

        [HttpPost]
        [AuthorizeAction]
        public IHttpActionResult Deactivate(T model)
        {
            try
            {
                var bo = RetornaBO();
                return Ok(new Result(value: bo.Deactivate(model)));
            }
            catch (BrokenRulesException ex)
            {
                return Ok(new Result(message: ex.Message, code: HttpStatusCode.PreconditionFailed));
            }
            catch (Exception ex)
            {
                return Ok(new Result(message: "Ops! Ocorreu um erro ao inserir.", code: HttpStatusCode.InternalServerError, value: model, ex: ex));
            }
        }

        [HttpPost]
        [AuthorizeAction]
        public IHttpActionResult Activate(T model)
        {
            try
            {
                var bo = RetornaBO();
                return Ok(new Result(value: bo.Activate(model)));
            }
            catch (BrokenRulesException ex)
            {
                return Ok(new Result(message: ex.Message, code: HttpStatusCode.PreconditionFailed));
            }
            catch (Exception ex)
            {
                return Ok(new Result(message: "Ops! Ocorreu um erro ao inserir.", code: HttpStatusCode.InternalServerError, value: model, ex: ex));
            }
        }
    }
}

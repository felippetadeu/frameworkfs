using System;
using System.Linq;
using System.Web.Http.Filters;

namespace Framework.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class Compress : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            //base.OnActionExecuted(actionExecutedContext);
            var acceptedEnconding = actionExecutedContext.Response?.RequestMessage.Headers.AcceptEncoding.FirstOrDefault();
            var acceptedValue = acceptedEnconding == null ? "" : acceptedEnconding.Value;
            if (!acceptedValue.Equals("gzip", StringComparison.InvariantCultureIgnoreCase)
                && !acceptedValue.Equals("deflate", StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }

            actionExecutedContext.Response.Content = new CompressedContent(actionExecutedContext.Response.Content, acceptedValue);
        }
    }
}

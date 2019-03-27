using Framework.FileManipulations;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Framework.App_Start
{
    public static class BaseWebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Serviços e configuração da API da Web
            var urls = WebConfigManipulation.GetConfig("AuthorizedUrls");
            var politicas = new EnableCorsAttribute(urls, "*", "GET, PUT, POST, DELETE, OPTIONS");
            config.EnableCors(politicas);
            // Rotas da API da Web
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Formatters.XmlFormatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("multipart/form-data"));
        }
    }
}

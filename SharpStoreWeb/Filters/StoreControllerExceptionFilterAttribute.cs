
using SharpStoreWeb.Exceptions;
using System.Net.Http;
using System.Web.Http.Filters;

namespace SharpStoreWeb.Filters
{
    public class StoreControllerExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext ctx)
        {
            if (ctx.Exception is StoreException)
            {
                ctx.Response = new HttpResponseMessage(System.Net.HttpStatusCode.NotFound)
                {
                    Content = new StringContent(ctx.Exception.Message)
                };
            }

        }
    }
}
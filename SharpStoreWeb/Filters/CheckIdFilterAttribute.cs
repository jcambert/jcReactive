
/*
using SharpStoreWeb.App_Start;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace SharpStoreWeb.Filters
{
    public class CheckIdFilterAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);
            if (actionContext.ActionArguments.ContainsKey("id"))
            {
                var kernel = NinjectWebCommon.Bootstrapper.Kernel;
                var name = actionContext.ActionDescriptor.ControllerDescriptor.ControllerName;
               
            }
        }

    }
}*/
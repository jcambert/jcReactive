

using System.Web.Mvc;

namespace SharpStoreWeb.Filters
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }


        

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
         /*   if (filterContext.ModelState.IsValid == false)
            {
                filterContext.Response = filterContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, filterContext.ModelState);
            }*/
        }
    }
}
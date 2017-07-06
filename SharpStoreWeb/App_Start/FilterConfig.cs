using SharpStoreWeb.Filters;
using System.Web;
using System.Web.Mvc;

namespace SharpStoreWeb
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ValidateModelAttribute());
            filters.Add(new HandleErrorAttribute());
        }
    }
}

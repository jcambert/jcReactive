using jcReactive.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;

namespace SharpStoreWeb.Helpers
{
    public static class DbContextExtensions
    {
        public static bool SaveFromController(this IReactiveDbContext ctx, Func<IHttpActionResult> ok, Func<string, IHttpActionResult> badRequest, out IHttpActionResult result)
        {
            try
            {
                ctx.SaveChanges();
                var errors = (ctx as ReactiveDbContext).GetValidationErrors();
                if (errors.Count() > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    errors.ToList().ForEach(e =>
                    {
                        e.ValidationErrors.ToList().ForEach(v =>
                        {
                            sb.AppendLine(v.ErrorMessage);
                        });
                    });
                    result= badRequest(sb.ToString());
                    return false;
                }
                result= ok();
                return true;
            }catch(DbUpdateException e)
            {
                result= badRequest(e.Message);
                return false;
            }
        }
    }
}
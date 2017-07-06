using AutoMapper;
using SharpStore;
using SharpStoreWeb.Exceptions;
using SharpStoreWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SharpStoreWeb.Controllers
{
    /// <summary>
    /// Manage page
    /// </summary>
    public class PageController : BaseStoreController<UIPage,UIPageDto>
    {
        /// <summary>
        /// Return one TDto by its key
        /// </summary>
        /// <param name="code">Unique code of page (eg: state name)</param>
        /// <returns>the finded dto</returns>
        [ActionName("getByCode")]
        public IHttpActionResult GetByCode([FromUri] string code)
        {
            var model = All().Where(p => p.Code == code).FirstOrDefault();
            if (model == null) throw new StoreException(new List<string>() { "Thee", "CodeNotExist" }, typeof(UIPage).Name, code.ToString());

            return Ok(Mapper.Map<UIPageDto>(model));
        }
    }
}
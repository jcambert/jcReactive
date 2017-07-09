using jcReactive.Common;
using Ninject;
using SharpStore;
using SharpStoreWeb.Filters;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SharpStoreWeb.Helpers;
using SharpStoreWeb.Models;

namespace SharpStoreWeb.Controllers
{
    
    public class ArticleController : BaseStoreController<Article, ArticleDto> 
    {

    }
}
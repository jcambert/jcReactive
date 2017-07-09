using AutoMapper;
using Ninject;
using SharpStore;
using SharpStoreWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SharpStoreWeb
{
    public class MapperConfig
    {
       
        internal static void RegisterMappings(IKernel kernel)
        {

            Mapper.Initialize(cfg =>
            {
                cfg.ConstructServicesUsing(type => kernel.Get(type));
                cfg.CreateMap<Parametre, ParametreDto>().ReverseMap();
                cfg.CreateMap<Article, ArticleDto>().ReverseMap();
                cfg.CreateMap<Chiffrage, ChiffrageDto>().ReverseMap();
                cfg.CreateMap<Menu, MenuDto>().ReverseMap();
                cfg.CreateMap<UIPage,UIPageDto>().ReverseMap();
            });
        }
    }
}
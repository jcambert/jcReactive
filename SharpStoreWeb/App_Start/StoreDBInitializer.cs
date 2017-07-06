using jcReactive.Common;
using Ninject;
using SharpStore;
using SharpStoreWeb.App_Start;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpStoreWeb
{
    public class StoreDBInitializer : CreateDatabaseIfNotExists<StoreContext>
    {
        private readonly IKernel Kernel;

        public StoreDBInitializer()
        {
            this.Kernel = NinjectWebCommon.Bootstrapper.Kernel;
        }

        protected override void Seed(StoreContext context)
        {
            base.Seed(context);

            context.Error.Subscribe(e =>
            {

            });

            context.ValidationError.Subscribe(e =>
            {

            });

            /*   using (var repo = Kernel.Get<IRepository<Parametre>>())
               {
                   var param = repo.Create();
                   param.Societe = "000";
                   param.Numero = 270;
                   param.Code = "CS";
                   param.Description = "Consommable";
                   param.CreatedBy = "Systeme";
                   param.CreatedAt = DateTime.Now;
                   repo.Add(param);

                   param = repo.Create();
                   param.Societe = "000";
                   param.Numero = 270;
                   param.Code = "FG";
                   param.Description = "Frais Generaux";
                   param.CreatedBy = "Systeme";
                   param.CreatedAt = DateTime.Now;
                   repo.Add(param);

                   param = repo.Create();
                   param.Societe = "000";
                   param.Numero = 270;
                   param.Code = "LB";
                   param.Description = "Libelle";
                   param.CreatedBy = "Systeme";
                   param.CreatedAt = DateTime.Now;
                   repo.Add(param);

                   param = repo.Create();
                   param.Societe = "000";
                   param.Numero = 270;
                   param.Code = "MP";
                   param.Description = "Matiere premiere";
                   param.CreatedBy = "Systeme";
                   param.CreatedAt = DateTime.Now;
                   repo.Add(param);
               }*/
            context.SaveChanges();
            using (var repo = Kernel.Get<IRepository<Menu>>())
            {
                var menu = repo.Create();
                menu.Societe = "000";
                menu.Code = "gpao";
                menu.Action = "gpao.main";
                menu.Tooltip = "Gestion de production";
                menu.TooltipPlacement = "right";
                menu.Icon = "fa fa-industry";
                menu.Animation = "";
                repo.Add(menu);

                menu = repo.Create();
                menu.Societe = "000";
                menu.Code = "sale";
                menu.Action = "sale.main";
                menu.Tooltip = "Gestion des ventes";
                menu.TooltipPlacement = "right";
                menu.Icon = "fa fa-truck";
                menu.Animation = "";
                repo.Add(menu);

                menu = repo.Create();
                menu.Societe = "000";
                menu.Code = "purchase";
                menu.Action = "purchase.main";
                menu.Tooltip = "Gestion des achats";
                menu.TooltipPlacement = "right";
                menu.Icon = "fa fa-cart-arrow-down";
                menu.Animation = "faa-tada";
                repo.Add(menu);

                menu = repo.Create();
                menu.Societe = "000";
                menu.Code = "staff";
                menu.Action = "staff.main";
                menu.Tooltip = "Gestion du personnel";
                menu.TooltipPlacement = "right";
                menu.Icon = "fa fa-users";
                menu.Animation = "";
                repo.Add(menu);

                menu = repo.Create();
                menu.Societe = "000";
                menu.Code = "mail";
                menu.Action = "mail.main";
                menu.Tooltip = "Messagerie";
                menu.TooltipPlacement = "right";
                menu.Icon = "fa fa-envelope";
                menu.Animation = "";
                repo.Add(menu);

                menu = repo.Create();
                menu.Societe = "000";
                menu.Code = "tools";
                menu.Action = "tools.main";
                menu.Tooltip = "Paramétrages";
                menu.TooltipPlacement = "right";
                menu.Icon = "fa fa-gears";
                menu.Animation = "";
                repo.Add(menu);

            }
            context.SaveChanges();

    
            using (var repo = Kernel.Get<IRepository<UIPage>>())
            {
                var page = repo.Create();
                page.Societe = "000";
                page.Code = "gpao.main";
                page.Title = "MAIN GPAO";
                page.Navbar = new UINavbar() { Id = "navbar-id", Brand = "WebErp", Classes = "navbar-fixed-top",Icon="stars" };
                

                var cards = new UICards() {/* Id = "cards",*/ Header = new UICardHeader() { Color = "light-grey" }, Width = 3,RepeatOn="card in cards" };
                page.Childs.Add(cards);
                repo.Add(page);
            }

            context.SaveChanges();
        }
    }
}

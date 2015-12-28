using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Dao;
using Dao.IRepository;
using Dao.Migrations;
using Dao.Model;
using Dao.Repository;

namespace Martin
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var builder = new ContainerBuilder();

            builder.RegisterType<AlbumRepository>();
            builder.Register<IAlbumRepository>(x => x.Resolve<AlbumRepository>());

            builder.RegisterType<SongRepository>();
            builder.Register<ISongRepository>(x => x.Resolve<SongRepository>());

            builder.RegisterType<StaticContentRepository>();
            builder.Register<IStaticContentRepository>(x => x.Resolve<StaticContentRepository>());

            builder.RegisterType<ArtistRepository>();
            builder.Register<IArtistRepository>(x => x.Resolve<ArtistRepository>());

            StaticContainer.Container = builder.Build();

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<MartinContext, Configuration>());
        }
    }
}
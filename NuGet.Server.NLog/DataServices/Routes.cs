using System.Data.Services;
using System.ServiceModel.Activation;
using System.Web.Routing;
using NuGet.Server.DataServices;
using NuGet.Server.Publishing;
using RouteMagic;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(NuGet.Server.NLog.NuGetRoutes), "Start")]

namespace NuGet.Server.NLog {
    public static class NuGetRoutes
    {
        public static void Start()
        {
            // Replaced DefaultServiceResolver with NLog implementation.
            var serviceResolver = new NLogServiceResolver();
            ServiceResolver.SetServiceResolver(serviceResolver);

            MapRoutes(RouteTable.Routes);
        }

        private static void MapRoutes(RouteCollection routes)
        {
            // Route to create a new package(http://{root}/nuget)
            routes.MapDelegate("CreatePackageNuGet",
                "nuget",
                new {httpMethod = new HttpMethodConstraint("PUT")},
                context => CreatePackageService().CreatePackage(context.HttpContext));

            // The default route is http://{root}/nuget/Packages
            var factory = new DataServiceHostFactory();
            var serviceRoute = new ServiceRoute("nuget", factory, typeof(Packages))
            {
                Defaults = new RouteValueDictionary {{"serviceType", "odata"}},
                Constraints = new RouteValueDictionary {{"serviceType", "odata"}}
            };
            routes.Add("nuget", serviceRoute);
        }

        private static IPackageService CreatePackageService()
        {
            return ServiceResolver.Resolve<IPackageService>();
        }
    }
}

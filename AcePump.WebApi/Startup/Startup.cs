using System;
using System.Configuration;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.OData.Extensions;
using System.Web.Security;
using AcePump.Domain;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using AcePump.Rdlc.Builder;
using AcePump.Common;

using CustomErrorsMode = System.Web.Configuration.CustomErrorsMode;
using CustomErrorsSection = System.Web.Configuration.CustomErrorsSection;
using Microsoft.Owin.Cors;
using System.Web.Cors;
using AcePump.Common.Storage;
using AcePump.Common.Storage.FileSystem;

[assembly: OwinStartup(typeof(AcePump.WebApi.Startup.Startup))]
namespace AcePump.WebApi.Startup
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app
                .UseAcePumpCors()
                .UseAcePumpDecodeAuthHeader()
                .UseAcePumpResolveTenant()
                .UseAcePumpAuthentication()
                .UseAcePumpWebApi();

            RdlcBuilder.SetPathMapper(x => HostingEnvironment.MapPath("~/bin/Rdlc/" + x));
        }
    }

    internal static class AcePumpMiddleware
    {
        public static IAppBuilder UseAcePumpCors(this IAppBuilder app)
        {
            var policy = new CorsPolicy()
            {
                AllowAnyHeader = true,
                AllowAnyMethod = true,
                AllowAnyOrigin = true,
                SupportsCredentials = true
            };

            policy.addAcePumpUri(AcePumpEnvironment.Environment.Configuration.PtpApi.UriV1);            

            return app.UseCors(new CorsOptions
            {
                PolicyProvider = new CorsPolicyProvider
                {
                    PolicyResolver = context => Task.FromResult(policy)
                }
            });
        }

        private static void addAcePumpUri(this CorsPolicy policy, string uri)
        {           
            uri = uri.TrimEnd(new[] { '/' });
            policy.Origins.Add(uri);
        }

        public static IAppBuilder UseAcePumpDecodeAuthHeader(this IAppBuilder app)
        {
            return app
                .Use(async (context, next) => {
                    const string OAUTH_QSTRING_AUTH_KEY = "oauth_qs_token";

                    if (context.Request.QueryString.HasValue)
                    {
                        var qs = HttpUtility.ParseQueryString(context.Request.QueryString.Value);
                        var authToken = qs.Get(OAUTH_QSTRING_AUTH_KEY);
                        if (!string.IsNullOrEmpty(authToken))
                        {
                            context.Request.Headers.Add("Authorization", new string[] { "Bearer " + authToken });
                        }
                    }

                    if(next != null)
                    {
                        await next.Invoke();
                    }
                })
                .UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }

        public static IAppBuilder UseAcePumpResolveTenant(this IAppBuilder app)
        {
            return app.Use(async (ctx, next) => {
                ClaimsIdentity cI = ctx?.Authentication?.User?.Identity as ClaimsIdentity;

                var tenantContext = new TenantContext();
                var isAppleTenant = (cI != null && cI.HasClaim("tenant", "apple.com"));
                if (isAppleTenant)
                {
                    tenantContext = new TenantContext()
                    {
                        StorageProvider = StorageFactory.GetStorageProvider(VirtualPathMapper.Instance),
                        Db = DataSourceFactory.GetAppleDataSource()
                    };
                }
                else
                {
                    tenantContext = new TenantContext()
                    {
                        StorageProvider = StorageFactory.GetStorageProvider(VirtualPathMapper.Instance),
                        Db = DataSourceFactory.GetAcePumpDataSource()
                    };
                }

                ctx.Set<TenantContext>(tenantContext);
                if (next != null) { await next.Invoke(); }

                tenantContext.Dispose();
            });
        }

        public static IAppBuilder UseAcePumpAuthentication(this IAppBuilder app)
        {
            return app
                .UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions()
                {
                    AllowInsecureHttp = true,
                    TokenEndpointPath = new PathString("/auth/token"),
                    AccessTokenExpireTimeSpan = TimeSpan.FromDays(30),
                    Provider = new OAuthAuthorizationServerProvider()
                    {                    
                        OnValidateClientAuthentication = async (OAuthValidateClientAuthenticationContext context) =>
                        {
                            context.Validated();

                            await Task.FromResult(0);
                        },
                        OnGrantResourceOwnerCredentials = async (OAuthGrantResourceOwnerCredentialsContext context) =>
                        {
                            if (context.UserName.Equals("test@apple.com", StringComparison.InvariantCultureIgnoreCase) && context.Password == "test@apple.com")
                            {
                                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                                identity.AddClaim(new Claim("username", context.UserName));
                                identity.AddClaim(new Claim("tenant", "apple.com"));

                                context.Validated(identity);
                            }
                            else if (Membership.ValidateUser(context.UserName, context.Password))
                            {
                                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                                identity.AddClaim(new Claim("username", context.UserName));
                                identity.AddClaim(new Claim("tenant", "acepumpinc.com"));

                                context.Validated(identity);
                            }
                            else
                            {
                                context.SetError("invalid_grant", "Unknown username or bad password.");
                            }

                            await Task.FromResult(0);
                        }
                    }
                });
        }

        public static IAppBuilder UseAcePumpWebApi(this IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();
            config.LoadWebConfigSettings();
            config.MapHttpAttributeRoutes();

            config.Filters.Add(new ValidateModelAttribute());
            config.Filters.Add(new LogRequestAttribute());
            config.Services.Add(typeof(IExceptionLogger), new BugzScoutExceptionLogger());
            config.Formatters.JsonFormatter.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
            config.EnableDependencyInjection();
            config.Select().Expand().Filter().OrderBy().MaxTop(null).Count();

            config.EnsureInitialized();

            return app
                .Use(async (context, next) =>
                {
                    if (!context.Request.Path.StartsWithSegments(new PathString("/api")))
                    {
                        context.Request.Path = new PathString("/api" + context.Request.Path.ToUriComponent());
                    }

                    if (next != null)
                    {
                        await next.Invoke();
                    }
                })
                .UseWebApi(config);
        }
    }

    internal static class ConfigurationExtensions
    {
        public static void LoadWebConfigSettings(this HttpConfiguration configuration)
        {
            CustomErrorsSection config = (CustomErrorsSection)ConfigurationManager.GetSection("system.web/customErrors");

            IncludeErrorDetailPolicy errorDetailPolicy;
            switch (config.Mode)
            {
                case CustomErrorsMode.RemoteOnly:
                    errorDetailPolicy = IncludeErrorDetailPolicy.LocalOnly;
                    break;

                case CustomErrorsMode.On:
                    errorDetailPolicy = IncludeErrorDetailPolicy.Never;
                    break;

                case CustomErrorsMode.Off:
                    errorDetailPolicy = IncludeErrorDetailPolicy.Always;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            configuration.IncludeErrorDetailPolicy = errorDetailPolicy;
        }
    }
}

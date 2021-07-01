using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using AcePump.Common;
using AcePump.Domain;
using AcePump.Domain.Models;

namespace AcePump.WebApi.Startup
{
    public class LogRequestAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (AcePumpEnvironment.Environment.Configuration.Logging.LogAllRequests)
            {
                using (var db = DataSourceFactory.GetAcePumpDataSource())
                {
                    var request = new Log_HttpRequest {
                        Environment = "webapi",
                        HttpMethod = actionContext.Request.Method.Method,
                        Path = actionContext.Request.RequestUri.AbsolutePath,
                        RequestTime = DateTime.Now,
                        LoggedInUsername = actionContext.RequestContext.Principal.Identity.IsAuthenticated ? actionContext.RequestContext.Principal.Identity.Name : "",
                        Parameters = new List<Log_HttpRequestParam>()
                    };

                    foreach (KeyValuePair<string, string> qstringEntry in actionContext.Request.GetQueryNameValuePairs())
                    {
                        if (qstringEntry.Key.Equals("password", StringComparison.InvariantCultureIgnoreCase)) continue;

                        request.Parameters.Add(new Log_HttpRequestParam
                        {
                            ParamType = "qstring",
                            ParamName = qstringEntry.Key,
                            ParamValue = qstringEntry.Value
                        });
                    }
                    
                    foreach (KeyValuePair<string, object> actionArgument in actionContext.ActionArguments)
                    {
                        if (actionArgument.Key.Equals("password", StringComparison.InvariantCultureIgnoreCase)) continue;

                        request.Parameters.Add(new Log_HttpRequestParam
                        {
                            ParamType = "actionargument",
                            ParamName = actionArgument.Key,
                            ParamValue = actionArgument.Value.ToString()
                        });
                    }

                    foreach (KeyValuePair<string, object> routeDataEntry in actionContext.RequestContext.RouteData.Values)
                    {
                        request.Parameters.Add(new Log_HttpRequestParam {
                            ParamType = "routedata",
                            ParamName = routeDataEntry.Key,
                            ParamValue = routeDataEntry.Value.ToString()
                        });
                    }

                    db.Log_HttpRequests.Add(request);
                    db.SaveChanges();
                }
            }
        }
    }
}
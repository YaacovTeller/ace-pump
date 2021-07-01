using System;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using AcePump.Common;

namespace AcePump.WebApi.Startup
{
    public class BugzScoutExceptionLogger : ExceptionLogger
    {
        public override async Task LogAsync(ExceptionLoggerContext context, CancellationToken cancellationToken)
        {
            BugzScoutClient bugzScoutClient = new BugzScoutClient("https://sorissoftware.fogbugz.com");
            await bugzScoutClient.Submit(
                description: FormatExceptionDescription(context.ExceptionContext),
                additionalInformation: await FormatAdditionalInformation(context.ExceptionContext),
                username: "BugzScout",
                project: "Ace Pump",
                area: "Misc"
            );
        }

        public override bool ShouldLog(ExceptionLoggerContext context)
        {
            return AcePumpEnvironment.Environment.Configuration.Logging.LogErrorsToFogBugz && base.ShouldLog(context);
        }

        private string FormatExceptionDescription(ExceptionContext context)
        {
            return $"{AcePumpEnvironment.Environment.Configuration.Logging.LogEntryPrefix}: {context.Request.Method} {context.Request.RequestUri.AbsolutePath}: {context.Exception.Message}";
        }

        private async Task<string> FormatAdditionalInformation(ExceptionContext context)
        {
            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(context.Request.RequestUri.Query))
            {
                sb.AppendLine("Querystring --");
                sb.AppendLine(context.Request.RequestUri.Query);
                sb.AppendLine();
            }

            string rawRequestBody = await context.Request.Content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(rawRequestBody))
            {
                sb.AppendLine("Raw request body --");
                sb.AppendLine(rawRequestBody);
                sb.AppendLine();
            }

            if (context.RequestContext.RouteData.Values.Count > 0)
            {
                sb.AppendLine("Route data --");
                foreach (var value in context.RequestContext.RouteData.Values)
                {
                    sb.AppendLine($"{value.Key} = {value.Value}");
                }
                sb.AppendLine();
            }

            if (context.Request.Properties.ContainsKey("MS_UserPrincipal"))
            {
                IPrincipal user = context.Request.Properties["MS_UserPrincipal"] as IPrincipal;

                sb.AppendLine("Logged in user: " + user.Identity.Name);
                sb.AppendLine();
            }

            sb.AppendLine("Exception message(s) --");
            Exception current = context.Exception;
            int cnt = 1;
            while (current != null)
            {
                sb.AppendLine($"{cnt}: {current.GetType().Name}: {current.Message}");

                cnt++;
                current = current.InnerException;
            }
            sb.AppendLine();

            sb.AppendLine("Stack trace --");
            sb.AppendLine(context.Exception.StackTrace);

            return sb.ToString();
        }
    }
}
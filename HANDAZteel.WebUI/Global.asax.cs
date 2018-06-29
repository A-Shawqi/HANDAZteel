using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using System.Web.SessionState;

namespace HANDAZ.PEB.WebUI
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            string bin = AppDomain.CurrentDomain.SetupInformation.ShadowCopyDirectories;
            Assembly[] ass = AppDomain.CurrentDomain.GetAssemblies();
            Assembly rhinoAss = ass.FirstOrDefault(c => c.Location.Contains("Rhino3dmIO.dll"));
            GlobalConfiguration.Configure(config =>
            {
                config.MapHttpAttributeRoutes();
                config.Routes.MapHttpRoute(
                    name: "DefaultApi",
                    routeTemplate: "api/{controller}/{id}",
                    defaults: new { id = RouteParameter.Optional }
                );
            });
            if (rhinoAss != null)
            {
                string rhinoDir = Path.GetDirectoryName(rhinoAss.Location);

                string sourcePathx64 = bin + "\\x64";
                string sourcePathx86 = bin + "\\x86";

                string targetPathx64 = rhinoDir + "\\x64";
                string targetPathx86 = rhinoDir + "\\x86";

                if (!System.IO.Directory.Exists(targetPathx64))
                {
                    System.IO.Directory.CreateDirectory(targetPathx64);
                }

                if (!System.IO.Directory.Exists(targetPathx86))
                {
                    System.IO.Directory.CreateDirectory(targetPathx86);
                }

                if (System.IO.Directory.Exists(sourcePathx64))
                {
                    string[] files = System.IO.Directory.GetFiles(sourcePathx64);

                    // Copy the files and overwrite destination files if they already exist.
                    foreach (string s in files)
                    {
                        // Use static Path methods to extract only the file name from the path.
                        string fileName = System.IO.Path.GetFileName(s);
                        string destFile = System.IO.Path.Combine(targetPathx64, fileName);
                        System.IO.File.Copy(s, destFile, true);
                    }
                }

                if (System.IO.Directory.Exists(sourcePathx86))
                {
                    string[] files = System.IO.Directory.GetFiles(sourcePathx86);

                    // Copy the files and overwrite destination files if they already exist.
                    foreach (string s in files)
                    {
                        // Use static Path methods to extract only the file name from the path.
                        string fileName = System.IO.Path.GetFileName(s);
                        string destFile = System.IO.Path.Combine(targetPathx86, fileName);
                        System.IO.File.Copy(s, destFile, true);
                    }
                }
            }
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}
using System;
using Topshelf;
using Topshelf.Autofac;

namespace TopShelfQuartzAutofac
{
    public class Program
    {
        public static void Main()
        {
            var container = Bootstrapper.BuildContainer();

            var rc = HostFactory.Run(x =>                                   
            {
                x.UseAutofacContainer(container);
                x.Service<SchedulerService>(s =>                                   
                {
                    s.ConstructUsingAutofacContainer();              
                    s.WhenStarted(tc => tc.Start());                         
                    s.WhenStopped(tc => tc.Stop());                          
                });
                x.RunAsLocalSystem();                                       

                x.SetDescription("Sample Topshelf Host");                   
                x.SetDisplayName("Stuff");                                  
                x.SetServiceName("Stuff");                                  
            });                                                             

            var exitCode = (int)Convert.ChangeType(rc, rc.GetTypeCode());  
            Environment.ExitCode = exitCode;
        }
    }
}

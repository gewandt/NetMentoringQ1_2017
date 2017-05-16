using Topshelf;

namespace ScanService.UI
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service(() => new ScanService());
                x.SetServiceName("Scan Service");
                x.StartAutomaticallyDelayed();
                x.RunAsLocalSystem();
            });
        }
    }
}

using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(BlazorInvoice.Client.Areas.Identity.IdentityHostingStartup))]
namespace BlazorInvoice.Client.Areas.Identity
{
	public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}
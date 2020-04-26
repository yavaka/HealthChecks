using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace HealthCheck
{
    public class ICMPHealthCheck : IHealthCheck
    {
		private string Host = "www.doesssnotexist.com";
		private int Timeout = 300;
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
			try
			{
				using (var ping = new Ping())
				{
					var reply = await ping.SendPingAsync(Host);
					switch (reply.Status)
					{
						case IPStatus.Success:
							return (reply.RoundtripTime > Timeout)
								? HealthCheckResult.Degraded()
								: HealthCheckResult.Healthy();
						
								default:
                            return HealthCheckResult.Unhealthy();
					}
				}
			}
			catch (Exception)
			{
                return HealthCheckResult.Unhealthy();
			}
        }
    }
}

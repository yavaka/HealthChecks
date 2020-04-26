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
		private string Host { get; set; }
		private int Timeout { get; set; }

		public ICMPHealthCheck(string host, int timeout)
		{
			this.Host = host;
			this.Timeout = timeout;
		}

		public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
			try
			{
				using (var ping = new Ping())
				{
					var reply = await ping.SendPingAsync(this.Host);
					switch (reply.Status)
					{
						case IPStatus.Success:
							//outcome message
							var msg = $"ICMP to {this.Host} took {reply.RoundtripTime} ms.";
							return (reply.RoundtripTime > Timeout)
								? HealthCheckResult.Degraded(msg)
								: HealthCheckResult.Healthy(msg);
						
								default:
							//outcome message
							var err = $"ICMP to {this.Host} failed: {reply.Status}";
                            return HealthCheckResult.Unhealthy(err);
					}
				}
			}
			catch (Exception e)
			{
				//outcome message
				var err = $"ICMP to {this.Host} failed: {e.Message}";
                return HealthCheckResult.Unhealthy(err);
			}
        }
    }
}

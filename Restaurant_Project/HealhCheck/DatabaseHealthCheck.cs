// ✅ أضف دول
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Villa_API_Project.DataAccess.Data;
namespace Restaurant.API.HealhCheck
{
    public class DatabaseHealthCheck : IHealthCheck
    {
        private readonly Context _context;
        private readonly ILogger<DatabaseHealthCheck> _logger;

        public DatabaseHealthCheck(Context context, ILogger<DatabaseHealthCheck> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            try
            {
              
                var canConnect = await _context.Database.CanConnectAsync(cancellationToken);

                if (!canConnect)
                    return HealthCheckResult.Unhealthy("Cannot connect to SQL Server ❌");

     
                var pendingMigrations = await _context.Database
                    .GetPendingMigrationsAsync(cancellationToken);

                if (pendingMigrations.Any())
                    return HealthCheckResult.Degraded(
                        $"Pending migrations: {string.Join(", ", pendingMigrations)} ⚠️");

                return HealthCheckResult.Healthy("SQL Server connected ✅");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database health check failed");
                return HealthCheckResult.Unhealthy("SQL Server error ❌", ex);
            }
        }
    }
}

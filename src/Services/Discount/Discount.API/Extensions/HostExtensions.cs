using Npgsql;

namespace Discount.API.Extensions
{
    public static class HostExtensions
    {
        public async static Task<IHost> MigrateDatabaseAsync<TContext>(this IHost host, int? retry = 0)
        {
            int retryForAvailability = retry ?? 0;

            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var configuration = services.GetRequiredService<IConfiguration>();
            var logger = services.GetRequiredService<ILogger<TContext>>();

            try
            {
                logger.LogInformation("Migrating postgresql database.");

                using var connection = new NpgsqlConnection
                (configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

                await connection.OpenAsync();

                using var command = connection.CreateCommand();

                command.CommandText = "DROP TABLE IF EXISTS Coupon";
                await command.ExecuteNonQueryAsync();

                command.CommandText = @"CREATE TABLE Coupon(
                                            ID SERIAL PRIMARY KEY NOT NULL,
                                            ProductName VARCHAR(24) NOT NULL,
                                            Description TEXT,
	                                        Amount INT
                                        ); ";
                await command.ExecuteNonQueryAsync();

                command.CommandText = @"INSERT INTO Coupon(ProductName,Description,Amount) VALUES ('IPhone X', 'IPhone Discount', 150);";
                await command.ExecuteNonQueryAsync();

                command.CommandText = @"INSERT INTO Coupon(ProductName,Description,Amount) VALUES ('Samsung 10', 'Samsung Discount', 100);";
                await command.ExecuteNonQueryAsync();

                logger.LogInformation("Migrated postgresql database.");
            }
            catch (NpgsqlException exception)
            {
                logger.LogError(exception, "An error occured while migrating the postgresql database.");

                if (retryForAvailability < 50)
                {
                    retryForAvailability++;
                    await Task.Delay(2000);
                    host = await MigrateDatabaseAsync<TContext>(host, retryForAvailability);
                }
            }

            return host;
        }
    }
}

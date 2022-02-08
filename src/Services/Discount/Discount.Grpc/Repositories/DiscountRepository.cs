using Dapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Repositories.Interfaces;
using Npgsql;

namespace Discount.Grpc.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration configuration;

        public DiscountRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<Coupon> Get(string productName)
        {
            using var connection = new NpgsqlConnection
                (configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>
                ("SELECT * FROM public.Coupon WHERE ProductName = @productName", new { productName });

            if (coupon == null)
            {
                return Coupon.Default();
            }

            return coupon;
        }

        public async Task<bool> Create(Coupon coupon)
        {
            using var connection = new NpgsqlConnection
                 (configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected = 
                await connection.ExecuteAsync
                    ("INSERT INTO public.Coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)", coupon);

            if (affected == 0)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> Delete(string productName)
        {
            using var connection = new NpgsqlConnection
                 (configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected =
                await connection.ExecuteAsync
                    ("DELETE FROM public.Coupon WHERE ProductName = @productName", new { productName });

            if (affected == 0)
            {
                return false;
            }

            return true;
        }


        public async Task<bool> Update(Coupon coupon)
        {
            using var connection = new NpgsqlConnection
                (configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected = await connection.ExecuteAsync
                    ("UPDATE Coupon SET ProductName=@ProductName, Description = @Description, Amount = @Amount WHERE Id = @Id",coupon);

            if (affected == 0)
                return false;

            return true;
        }
    }
}

using MediatR;

namespace Ordering.Application.Features.Orders.Queries.GetOrderList
{
    public class GetOrdersListQuery : IRequest<List<OrdersViewModel>>
    {
        public string UserName { get; set; }

        public GetOrdersListQuery(string userName)
        {
            UserName = userName ?? throw new ArgumentNullException(nameof(userName));
        }
    }
}

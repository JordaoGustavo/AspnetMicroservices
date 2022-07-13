using Ordering.Application.Models;

namespace Ordering.Application.Contracts.Infrastruture
{
    public interface IEmailService
    {
        Task<bool> SendEmail(Email email);
    }
}

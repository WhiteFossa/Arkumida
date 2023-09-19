using webapi.Services.Abstract;

namespace webapi.Services.Implementations;

public class PrivateMessagesService : IPrivateMessagesService
{
    public async Task<int> GetUnreadPrivateMessagesCountAsync(Guid creatureId)
    {
        return 5; // TODO: Add DAO call
    }
}
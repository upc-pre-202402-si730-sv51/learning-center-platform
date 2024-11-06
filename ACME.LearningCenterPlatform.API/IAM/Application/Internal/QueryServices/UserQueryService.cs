using ACME.LearningCenterPlatform.API.IAM.Domain.Model.Aggregates;
using ACME.LearningCenterPlatform.API.IAM.Domain.Model.Queries;
using ACME.LearningCenterPlatform.API.IAM.Domain.Repositories;
using ACME.LearningCenterPlatform.API.IAM.Domain.Services;

namespace ACME.LearningCenterPlatform.API.IAM.Application.Internal.QueryServices;

/// <summary>
/// User query service. 
/// </summary>
/// <param name="userRepository">
/// The <see cref="IUserRepository"/> instance.
/// </param>
public class UserQueryService(IUserRepository userRepository) : IUserQueryService
{
    // <inheritdoc/>
    public async Task<User?> Handle(GetUserByIdQuery query)
    {
        return await userRepository.FindByIdAsync(query.Id);
    }

    // <inheritdoc/>
    public async Task<IEnumerable<User>> Handle(GetAllUsersQuery query)
    {
        return await userRepository.ListAsync();
    }

    // <inheritdoc/>
    public async Task<User?> Handle(GetUserByUsernameQuery query)
    {
        return await userRepository.FindByUsernameAsync(query.Username);
    }
}
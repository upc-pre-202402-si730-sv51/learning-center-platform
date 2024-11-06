using ACME.LearningCenterPlatform.API.IAM.Domain.Model.Aggregates;
using ACME.LearningCenterPlatform.API.IAM.Interfaces.REST.Resources;

namespace ACME.LearningCenterPlatform.API.IAM.Interfaces.REST.Transform;

/// <summary>
/// Represents the assembler for the user resource from entity. 
/// </summary>
public static class UserResourceFromEntityAssembler
{
    /// <summary>
    /// Converts the user entity to the user resource. 
    /// </summary>
    /// <param name="user">
    /// The <see cref="User"/> entity.
    /// </param>
    /// <returns>
    /// The <see cref="UserResource"/> resource.
    /// </returns>
    public static UserResource ToResourceFromEntity(User user)
    {
        return new UserResource(user.Id, user.Username);
    }
}
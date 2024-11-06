using ACME.LearningCenterPlatform.API.IAM.Domain.Model.Commands;
using ACME.LearningCenterPlatform.API.IAM.Interfaces.REST.Resources;

namespace ACME.LearningCenterPlatform.API.IAM.Interfaces.REST.Transform;

/// <summary>
/// Represents the assembler for the sign-up command from resource. 
/// </summary>
public static class SignUpCommandFromResourceAssembler
{
    /// <summary>
    /// Converts the sign-up resource to the sign-up command. 
    /// </summary>
    /// <param name="resource">
    /// The <see cref="SignUpResource"/> resource.
    /// </param>
    /// <returns>
    /// The <see cref="SignUpCommand"/> command.
    /// </returns>
    public static SignUpCommand ToCommandFromResource(SignUpResource resource)
    {
        return new SignUpCommand(resource.Username, resource.Password);
    }
}
namespace ACME.LearningCenterPlatform.API.Shared.Domain.Repositories;

/// <summary>
/// Unit of work interface for all repositories 
/// </summary>
public interface IUnitOfWOrk
{
    /// <summary>
    /// Save changes to the repository 
    /// </summary>
    /// <returns></returns>
    Task CompleteAsync();
}
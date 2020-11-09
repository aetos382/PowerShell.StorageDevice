using System.Threading;
using System.Threading.Tasks;

namespace PSAsync
{
    public interface IAsyncDynamicPropertyCmdletProvider :
        IAsyncPropertyCmdletProvider
    {
        Task CopyPropertyAsync(
            string sourcePath,
            string sourceProperty,
            string destinationPath,
            string destinationProperty,
            CancellationToken cancellationToken);

        Task<object> CopyPropertyDynamicParametersAsync(
            string sourcePath,
            string sourceProperty,
            string destinationPath,
            string destinationProperty,
            CancellationToken cancellationToken);

        Task MovePropertyAsync(
            string sourcePath,
            string sourceProperty,
            string destinationPath,
            string destinationProperty,
            CancellationToken cancellationToken);

        Task<object> MovePropertyDynamicParametersAsync(
            string sourcePath,
            string sourceProperty,
            string destinationPath,
            string destinationProperty,
            CancellationToken cancellationToken);

        Task NewPropertyAsync(
            string path,
            string propertyName,
            string propertyTypeName,
            object value,
            CancellationToken cancellationToken);

        Task<object> NewPropertyDynamicParametersAsync(
            string path,
            string propertyName,
            string propertyTypeName,
            object value,
            CancellationToken cancellationToken);
        
        Task RemovePropertyAsync(
            string path,
            string propertyName,
            CancellationToken cancellationToken);
        
        Task<object> RemovePropertyDynamicParametersAsync(
            string path,
            string propertyName,
            CancellationToken cancellationToken);
        
        Task RenamePropertyAsync(
            string path,
            string sourceProperty,
            string destinationProperty,
            CancellationToken cancellationToken);
        
        Task<object> RenamePropertyDynamicParameterAsync(
            string path,
            string sourceProperty,
            string destinationProperty,
            CancellationToken cancellationToken);
    }
}

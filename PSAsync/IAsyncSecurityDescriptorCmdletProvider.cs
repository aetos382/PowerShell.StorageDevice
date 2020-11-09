using System.Security.AccessControl;
using System.Threading;
using System.Threading.Tasks;

namespace PSAsync
{
    public interface IAsyncSecurityDescriptorCmdletProvider
    {
        Task GetSecurityDescriptorAsync(
            string path,
            AccessControlSections includeSections,
            CancellationToken cancellationToken);

        Task<ObjectSecurity> NewSecurityDescriptorFromPathAsync(
            string path,
            AccessControlSections includeSections,
            CancellationToken cancellationToken);

        Task<ObjectSecurity> NewSecurityDescriptorOfTypeAsync(
            string type,
            AccessControlSections includeSections,
            CancellationToken cancellationToken);

        Task SetSecurityDescriptorAsync(
            string path,
            ObjectSecurity securityDescriptor,
            CancellationToken cancellationToken);
    }
}

using System.Management.Automation;

namespace PSAsync
{
    public interface ICmdlet
    {
        void WriteError(
            ErrorRecord errorRecord);

        void WriteWarning(
            string text);

        void WriteVerbose(
            string text);

        void WriteInformation(
            InformationRecord informationRecord);

        void WriteInformation(
            object messageData,
            string[] tags);

        void WriteProgress(
            ProgressRecord progressRecord);

        bool ShouldProcess(
            string target);

        bool ShouldProcess(
            string target,
            string action);

        bool ShouldProcess(
            string verboseDescription,
            string verboseWarning,
            string caption);

        bool ShouldProcess(
            string verboseDescription,
            string verboseWarning,
            string caption,
            out ShouldProcessReason reason);

        bool ShouldContinue(
            string query,
            string caption);

        bool ShouldContinue(
            string query,
            string caption,
            ref bool yesToAll,
            ref bool noToAll);
    }
}

namespace PSAsync.Command
{
    public interface ICmdletOutput :
        ICommonOutput
    {
        bool ShouldContinue(
            string query,
            string caption,
            bool hasSecurityImpact,
            ref bool yesToAll,
            ref bool noToAll);
    }
}

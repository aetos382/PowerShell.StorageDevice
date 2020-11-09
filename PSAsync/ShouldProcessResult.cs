using System;
using System.Diagnostics;
using System.Management.Automation;

namespace PSAsync
{
    public readonly struct ShouldProcessResult :
        IEquatable<ShouldProcessResult>
    {
        public ShouldProcessResult(
            bool result,
            ShouldProcessReason reason)
        {
            this.Result = result;
            this.Reason = reason;
        }

        public bool Result
        {
            [DebuggerStepThrough]
            get;
        }

        public ShouldProcessReason Reason
        {
            [DebuggerStepThrough]
            get;
        }

        public void Deconstruct(
            out bool result,
            out ShouldProcessReason reason)
        {
            result = this.Result;
            reason = this.Reason;
        }

        public bool Equals(
            ShouldProcessResult other)
        {
            return
                this.Result == other.Result &&
                this.Reason == other.Reason;
        }

        /// <inheritdoc />
        public override bool Equals(
            object? obj)
        {
            return (obj is ShouldProcessResult other) && Equals(other);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return HashCode.Combine(this.Result, (int) this.Reason);
        }

        public static bool operator ==(
            ShouldProcessResult left,
            ShouldProcessResult right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(
            ShouldProcessResult left,
            ShouldProcessResult right)
        {
            return !(left == right);
        }
    }
}

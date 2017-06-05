using System;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace PowerShellEngine
{
    public class PowerShellErrorEventArg
    {
        public PowerShellErrorEventArg(Exception ex)
        {
            this.PowerShellException = ex;
        }

        public Exception PowerShellException;
    }

    public class PipelineErrorEventArg
    {
        public PipelineErrorEventArg(string error)
        {
            Error = error;
        }

        public string Error;
    }

    public class PipelineOutputEventArg
    {
        public PipelineOutputEventArg(PSObject output)
        {
            PSObjectOutput = output;
        }

        public PSObject PSObjectOutput;
    }

    public class PipelineStateEventArg
    {
        public PipelineStateEventArg(PipelineState state)
        {
            State = state;
        }

        public PipelineState State;
    }

    public class RunspaceStateEventArg
    {
        public RunspaceStateEventArg(RunspaceState state)
        {
            State = state;
        }

        public RunspaceState State;
    }
}

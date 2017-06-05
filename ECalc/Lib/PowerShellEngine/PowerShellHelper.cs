using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Management.Automation;
using System.Threading;

namespace PowerShellEngine
{
    public class PowerShellHelper
    {
        private PowerShell powerShell;

        private Pipeline pipeline;

        private Runspace runspace;

        /// <summary>
        /// Thread script execution
        /// </summary>
        /// <param name="script"></param>
        public void ExecuteScript(string script)
        {
            InvokeScriptStarted(new EventArgs());
            var starter = new ParameterizedThreadStart(Execute);
            new Thread(starter).Start(script);
        }

        /// <summary>
        /// Called by the ExecuteScript method, open the pipeline and execute code
        /// </summary>
        /// <param name="scriptObject"></param>
        private void Execute(object scriptObject)
        {
            runspace = RunspaceFactory.CreateRunspace();
            runspace.StateChanged -= new EventHandler<RunspaceStateEventArgs>(Runspace_StateChanged);
            runspace.StateChanged += new EventHandler<RunspaceStateEventArgs>(Runspace_StateChanged);
            runspace.Open();

            powerShell = PowerShell.Create();
            powerShell.Runspace = runspace;

            try
            {
                var script = scriptObject.ToString();
                pipeline = powerShell.Runspace.CreatePipeline();
                
                // Register events (state changes, output data ready in the pipeline
                pipeline.StateChanged -= new EventHandler<PipelineStateEventArgs>(pipeline_StateChanged);
                pipeline.Output.DataReady -= new EventHandler(Output_DataReady);
                pipeline.StateChanged += new EventHandler<PipelineStateEventArgs>(pipeline_StateChanged);
                pipeline.Output.DataReady += new EventHandler(Output_DataReady);
                

                pipeline.Commands.AddScript(script);
                
                try
                {
                    pipeline.InvokeAsync();
                }
                catch (Exception ex)
                {   
                    InvokePowerShellErrorRaised(new PowerShellErrorEventArg(ex));
                }
            }
            catch (Exception ex)
            {
                InvokePowerShellErrorRaised(new PowerShellErrorEventArg(ex));
            }
        }

        /// <summary>
        /// Triggered when runspace state change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Runspace_StateChanged(object sender, RunspaceStateEventArgs e)
        {
            InvokeRunspaceStateChangedReceived(new RunspaceStateEventArg(e.RunspaceStateInfo.State));
        }

        /// <summary>
        /// Triggered when pipeline state change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pipeline_StateChanged(object sender, PipelineStateEventArgs e)
        {
            InvokePipelineStateChangedReceived(new PipelineStateEventArg(e.PipelineStateInfo.State));

            if (e.PipelineStateInfo.State.ToString() == "Failed" && e.PipelineStateInfo.Reason != null)
            {
                InvokePipelineErrorRaised(new PipelineErrorEventArg(e.PipelineStateInfo.Reason.Message + " (" + e.PipelineStateInfo.Reason.InnerException + ")"));
            }

            // Close runspace if pipeline failed/stopped/completed
            var state = e.PipelineStateInfo.State;
            if (state == PipelineState.Failed || state == PipelineState.Stopped || state == PipelineState.Completed)
            {
                runspace.CloseAsync();
            }
        }

        /// <summary>
        /// Triggered when data arrives in the pipeline
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Output_DataReady(object sender, EventArgs e)
        {
            var errors = pipeline.Error;
            var outputs = pipeline.Output.NonBlockingRead();

            // Error check
            if (errors != null && errors.Count > 0)
            {
                InvokePipelineErrorRaised(new PipelineErrorEventArg(errors.ToString()));
            }

            // Outputs Check
            if (outputs.Count <= 0) return;

            foreach (var output in outputs.Where(output => output != null))
            {
                InvokePipelineOutputReceived(new PipelineOutputEventArg(output));
            }
        }

        #region Events

        // PowerShell Engine Error Handler
        public delegate void PowerShellErrorEventHandler(object sender, PowerShellErrorEventArg arg);
        public event PowerShellErrorEventHandler PowerShellErrorRaised;
        public void InvokePowerShellErrorRaised(PowerShellErrorEventArg e)
        {
            PowerShellErrorEventHandler handler = PowerShellErrorRaised;
            if (handler != null) handler(null, e);
        }

        // PowerShell Pipeline Error Handler
        public delegate void PipelineErrorEventHandler(object sender, PipelineErrorEventArg arg);
        public event PipelineErrorEventHandler PipelineErrorRaised;
        public void InvokePipelineErrorRaised(PipelineErrorEventArg e)
        {
            PipelineErrorEventHandler handler = PipelineErrorRaised;
            if (handler != null) handler(null, e);
        }

        // PowerShell Pipeline Output Handler
        public delegate void PipelineOutputEventHandler(object sender, PipelineOutputEventArg arg);
        public event PipelineOutputEventHandler PipelineOutputReceived;
        public void InvokePipelineOutputReceived(PipelineOutputEventArg e)
        {
            PipelineOutputEventHandler handler = PipelineOutputReceived;
            if (handler != null) handler(null, e);
        }

        // Pipeline State Changed
        public delegate void PipelineStateChangedEventHandler(object sender, PipelineStateEventArg arg);
        public event PipelineStateChangedEventHandler PipelineStateChangedReceived;
        public void InvokePipelineStateChangedReceived(PipelineStateEventArg e)
        {
            PipelineStateChangedEventHandler handler = PipelineStateChangedReceived;
            if (handler != null) handler(null, e);
        }

        // Runspace State Changed
        public delegate void RunspaceStateChangedEventHandler(object sender, RunspaceStateEventArg arg);
        public event RunspaceStateChangedEventHandler RunspaceStateChangedReceived;
        public void InvokeRunspaceStateChangedReceived(RunspaceStateEventArg e)
        {
            RunspaceStateChangedEventHandler handler = RunspaceStateChangedReceived;
            if (handler != null) handler(null, e);
        }

        /// <summary>
        /// Script started event
        /// </summary>
        public event EventHandler ScriptStarted;
        public void InvokeScriptStarted(EventArgs e){
            EventHandler handler = ScriptStarted;
            if (handler != null) handler(this, e);
        }

        #endregion
    }
}

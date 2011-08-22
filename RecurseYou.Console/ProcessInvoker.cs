using System.Diagnostics;

namespace RecurseYou
{
    public class ProcessInvoker : IInvokeProcess
    {
        #region IInvokeProcess Members

        public void Invoke(ProcessStartInfo process)
        {
            Process.Start(process);
        }

        #endregion
    }
}
using System.Diagnostics;

namespace RecurseYou
{
    public interface IInvokeProcess
    {
        void Invoke(ProcessStartInfo process);
    }
}
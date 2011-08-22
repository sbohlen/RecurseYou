namespace RecuseYou
{
    public class ProcessInvoker : IInvokeProcess
    {
        public void Invoke(string process)
        {
            System.Diagnostics.Process.Start(process);
        }
    }
}
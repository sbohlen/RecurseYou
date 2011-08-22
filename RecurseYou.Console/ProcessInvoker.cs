namespace RecuseYou
{
    public class ProcessInvoker
    {
        public void Invoke(string process)
        {
            System.Diagnostics.Process.Start(process);
        }
    }
}
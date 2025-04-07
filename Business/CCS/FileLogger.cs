namespace Business.CCS
{
    public class FileLogger : ILogger
    {
        public void Log()
        {
            Console.WriteLine("Dosyaya Loglama Sistemi devre dışı!");
        }
    }
}

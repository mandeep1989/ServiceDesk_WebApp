namespace ServiceDesk_WebApp.Common
{
    public class LogService
    {
        private readonly string _filePath;
        public LogService()
        {
             _filePath = "./Log/textfile.txt";
            if (!Directory.Exists(Path.GetDirectoryName(_filePath)))
                Directory.CreateDirectory(_filePath);
        }


        public void AddLogInfo(string info)
        {
            File.AppendAllText(_filePath, $"\n{DateTime.Now} - [INFO] : {info}");
        }

        public void AddLogError(string error)
        {
            File.AppendAllText(_filePath, $"\n{DateTime.Now} - [Error]: {error}");

        }

    }
}

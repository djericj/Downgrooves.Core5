using Newtonsoft.Json;

namespace Downgrooves.Data
{
    public abstract class BaseDao
    {
        protected string _filePath;
        protected string _fileContent;

        public string FilePath { get => _filePath; set => _filePath = value; }
        public string FileContent { get => _fileContent; set => _fileContent = value; }

        public BaseDao(string filePath)
        {
            _filePath = filePath;
            _fileContent = ReadFile(_filePath);
        }

        protected T? GetData<T>()
        {
            return GetData<T>(ReadFile(_filePath));
        }

        protected static T? GetData<T>(string fileContent)
        {
            if (!string.IsNullOrEmpty(fileContent))
                return JsonConvert.DeserializeObject<T>(fileContent);
            return default;
        }

        private static string ReadFile(string filePath)
        {
            return File.ReadAllText(filePath);
        }
    }
}

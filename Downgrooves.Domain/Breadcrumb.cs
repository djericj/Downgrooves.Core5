namespace Downgrooves.Domain
{
    public class Breadcrumb
    {
        public Breadcrumb(string url, string name)
        {
            Name = name;
            Url = url;
        }

        public string Name { get; }
        public string Url { get; }
    }
}
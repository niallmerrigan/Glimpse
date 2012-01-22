namespace Glimpse.Core2.Framework
{
    public class TabResult
    {
        public object Data { get; set; }
        public string Name { get; set; }

        public TabResult(string name, object data)
        {
            Data = data;
            Name = name;
        }
    }
}
namespace ConfigurationDemo
{
    public class AppConfig
    {
        public bool Verbose { get; set; }
        public int Depth { get; set; } = 1;
        public bool Silent { get; set; }
        public string Output { get; set; }
    }
}

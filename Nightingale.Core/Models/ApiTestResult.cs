namespace Nightingale.Core.Models
{
    public class ApiTestResult
    {
        public TestResult TestResult { get; set; }

        public string Name { get; set; }
    }

    public enum TestResult
    {
        Unstarted,
        Pass,
        Fail,
        Error
    }
}

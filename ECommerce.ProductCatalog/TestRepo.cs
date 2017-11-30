namespace ECommerce.ProductCatalog
{
    public interface ITestRepo
    {
        string GetSomething();
    }

    public class TestRepo : ITestRepo
    {
        public string GetSomething()
        {
            return "something";
        }
    }
}

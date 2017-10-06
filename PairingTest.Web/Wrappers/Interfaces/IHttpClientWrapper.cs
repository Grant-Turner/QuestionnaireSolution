namespace PairingTest.Web.Wrappers.Interfaces
{
    public interface IHttpClientWrapper
    {
        void Initialise(string url, string apiName);

        string GetUrl();

        T GetAsync<T>(string apiMethod);

        T PutAsync<T>(string apiMethod, object data);

        T DeleteAsync<T>(string apiMethod);
    }
}

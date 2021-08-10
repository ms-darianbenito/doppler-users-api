namespace Doppler.UsersApi.Services
{
    public interface IEncryptionService
    {
        public string DecryptAES256(string input);
        void Dispose();
        public string EncryptAES256(string input);
    }
}

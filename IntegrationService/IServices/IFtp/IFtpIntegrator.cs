namespace IntegrationService.IServices.Ftp
{
    public interface IFtpIntegrator
    {
        //bool Connect(string host, string username, string password, int port);
        Stream GetFileStream();
        void UploadFileStream();
    }
}
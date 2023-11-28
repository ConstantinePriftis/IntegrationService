using FluentFTP;
using IntegrationService.Configuration;
using IntegrationService.IServices.Ftp;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.VisualBasic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace IntegrationService.ConcreteServices.Ftp
{
    public class FtpIntegrator : IFtpIntegrator
    {
        readonly AsyncFtpClient _client;
        readonly Configurations _configuration;

        //May be used if we dont go async
        //public FtpClient _ftpClient { get; set; }
        public FtpIntegrator(Configurations config)
        {

            _configuration = config;
            _client = new AsyncFtpClient(
                _configuration.FtpHost,
                _configuration.FtpUsername,
                _configuration.FtpPassword);
        }

        private bool Connected()
        {
            return _client.IsConnected;
        }
        public Stream GetFileStream()
        {
            //max value for byte is 2130702268
            byte[] buffer = new byte[2130702268];
            MemoryStream stream = new MemoryStream();
            using (_client)
            {
                _client.AutoConnect();
                if (Connected())
                {
                    FtpClient client = new FtpClient();
                    client.Host = _configuration.FtpHost;
                    //client.Encoding = Encoding.GetEncoding("ANSI");
                    client.Credentials = new NetworkCredential(_configuration.FtpUsername, _configuration.FtpPassword);
                    try
                    {
                        client.Connect();
                        client.DownloadBytes(out buffer, "/gk/" + _configuration.FtpFile);

                        //string content = reader.ReadToEnd();
                        //string Content = reader.ReadToEnd();
                        //Encoding.UTF8.GetString(buffer);
                        Encoding[] encodings = { Encoding.UTF8, Encoding.GetEncoding(1253), Encoding.GetEncoding(1252) };

                        var encodingNeeded = Encoding.GetEncoding(1253);
                        StreamReader reader = new StreamReader(new MemoryStream(buffer), encodingNeeded);
                        var ContentToReturn = reader.BaseStream;
                        var stringContent = reader.ReadToEnd();
                        var StreamToReturn = encodingNeeded.GetBytes(stringContent);
                        
                        _client.Disconnect();
                        return new MemoryStream(StreamToReturn);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            return new MemoryStream(buffer);
        }

        //Export Mechanism to Ftp server
        public void UploadFileStream()
        {
            using (_client)
            {
                _client.Connect();
                try
                {
                    var path = _client.UploadFile(@"C:\" + _configuration.FtpFile, "*/in/" + _configuration.FtpFile);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                _client.Disconnect();
            }
        }
    }
}

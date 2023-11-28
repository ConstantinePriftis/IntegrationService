using IntegrationService.Quartz;
using System.Security.Cryptography.X509Certificates;

namespace IntegrationService.Configuration
{
    public class Configurations
    {
        private readonly IConfiguration _configs;

        public Configurations(IConfiguration configs)
        {
            _configs = configs;
        }
        public int MyProperty { get; set; }

        public string? DatabaseString => _configs?.GetValue<string>("IntegrationService");
        public bool? EnabledQuartz => _configs?.GetValue<bool>("Quartz:Enabled");
        public List<JobModel> Jobs => _configs.GetSection("Quartz:Jobs").Get<List<JobModel>>();
        public string FtpHost => _configs.GetValue<string>("Ftphost");
        public string FtpUsername => _configs.GetValue<string>("FtpUsername");
        public string FtpPassword => _configs.GetValue<string>("FtpPassword");
        public string FtpFile => _configs.GetValue<string>("FtpFile");

    }
}

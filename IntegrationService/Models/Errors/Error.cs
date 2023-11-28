using System.Runtime.InteropServices;

namespace IntegrationService.Models.Errors
{
    public class Error
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string ErrorMessage { get; set; }
        public bool HasRetried { get; set; } = false;
        public int NumberOfRetries { get; set; } = 0;
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Error()
        {
        }
        private Error(string errorMessage, bool hasRetried, int numberOfRetries, DateTime createdOn, DateTime modifiedOn)
        {
            ErrorMessage = errorMessage;
            HasRetried = hasRetried;
            NumberOfRetries = numberOfRetries;
            CreatedOn = createdOn;
            ModifiedOn = modifiedOn;
        }
        public static Error Create(string errorMessage)
        {
            var error = new Error();
            error.Id = Guid.NewGuid();
            error.ErrorMessage = errorMessage;
            error.CreatedOn = DateTime.Now;
            error.ModifiedOn = DateTime.Now;
            error.HasRetried = false;
            return error;
        }
        public  void Update(int numberOfRetries)
        {           
            this.NumberOfRetries = numberOfRetries;
            this.ModifiedOn = DateTime.Now;            
        }
    }
}

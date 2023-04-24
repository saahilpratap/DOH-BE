using System.ComponentModel.DataAnnotations;

namespace LockthreatCompliance.Authorization.Accounts.Dto
{
    public class SendEmailActivationLinkInput
    {
        [Required]
        public string EmailAddress { get; set; }
    }
}
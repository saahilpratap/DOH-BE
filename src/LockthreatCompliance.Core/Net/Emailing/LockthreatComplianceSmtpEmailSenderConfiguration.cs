using Abp.Configuration;
using Abp.Net.Mail;
using Abp.Net.Mail.Smtp;
using Abp.Runtime.Security;

namespace LockthreatCompliance.Net.Emailing
{
    public class LockthreatComplianceSmtpEmailSenderConfiguration : SmtpEmailSenderConfiguration
    {
        public LockthreatComplianceSmtpEmailSenderConfiguration(ISettingManager settingManager) : base(settingManager)
        {

        }

        public override string Password => SimpleStringCipher.Instance.Decrypt(GetNotEmptySettingValue(EmailSettingNames.Smtp.Password));
    }
}
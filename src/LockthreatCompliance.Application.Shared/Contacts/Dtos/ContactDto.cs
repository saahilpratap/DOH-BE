
using System;
using Abp.Application.Services.Dto;

namespace LockthreatCompliance.Contacts.Dtos
{
    public class ContactDto : EntityDto
    {
        public string Code { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName { get; set; }

        public string JobTitle { get; set; }

        public string Mobile { get; set; }

        public string DirectPhone { get; set; }

        public string CompanyName { get; set; }

        public string Email { get; set; }

        public int BusinessEntityId { get; set; }

        public string ContactType { get; set; }

    }
}
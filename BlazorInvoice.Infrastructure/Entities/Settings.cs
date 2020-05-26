using System.ComponentModel.DataAnnotations;

namespace BlazorInvoice.Infrastructure.Entities
{
    public class Settings
	{
        [Key]
        public int Id { get; set; }

        public string CompanyName { get; set; }
        public string Website { get; set; }
        public string Phone { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }
        public string SMTP { get; set; }
        public int Port { get; set; }

        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        public string BankAccount { get; set; }
        public string Bank { get; set; }

        public string BusinessNumber { get; set; }
        public string VAT { get; set; }

        public string InvoicePrefix { get; set; }

        public string Logo { get; set; }
        public bool ShowLogo { get; set; }
        public bool ShowLogoInPDF { get; set; }
        public string Color { get; set; }

        public Settings()
        {

        }

        public Settings(int id, string companyName, string website, string phone, string email, string password, string sMTP, int port, string address, string postalCode, string city, string country, string bankAccount, string bank, string businessNumber, string vAT, string invoicePrefix, string logo, bool showLogo, bool showLogoInPDF, string color)
        {
            Id = id;
            CompanyName = companyName;
            Website = website;
            Phone = phone;
            Email = email;
            Password = password;
            SMTP = sMTP;
            Port = port;
            Address = address;
            PostalCode = postalCode;
            City = city;
            Country = country;
            BankAccount = bankAccount;
            Bank = bank;
            BusinessNumber = businessNumber;
            VAT = vAT;
            InvoicePrefix = invoicePrefix;
            Logo = logo;
            ShowLogo = showLogo;
            ShowLogoInPDF = showLogoInPDF;
            Color = color;
        }
    }
}

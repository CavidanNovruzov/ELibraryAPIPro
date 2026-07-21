using System;
using System.Collections.Generic;
using System.Text;

namespace ELibraryAPI.Infrastructure.Options;

public class SmtpOptions
{
    public const string SectionName = "Email";
    public bool SendEmails { get; set; }
    public string SmtpHost { get; set; } = null!;
    public int SmtpPort { get; set; } 
    public bool UseSsl { get; set; }
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string FromEmail { get; set; } = null!;
    public string FromName { get; set; } = null!;
    public string ConfirmEmailBaseUrl { get; set; } = null!;
    public string ResetPasswordBaseUrl { get; init; }=null!;
}

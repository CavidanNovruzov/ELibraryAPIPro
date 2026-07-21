

namespace ELibraryAPI.Application.Abstractions.Services;

public interface IEmailSender
{
    /// <summary>
    /// E-poçt göndərmək üçün əsas metod.
        /// </summary>
        /// <param name="to">Alıcının e-poçt ünvanı</param>
        /// <param name="subject">E-poçtun mövzusu</param>
        /// <param name="htmlBody">HTML formatında məzmun</param>
        /// <param name="plainBody">Sadə mətn formatında məzmun (Optional - SPAM riskini azaltmaq üçün)</param>
        /// <param name="ct">Asinxron əməliyyatı ləğv etmək üçün token</param>
        Task SendEmailAsync(
            string to,
            string subject,
            string htmlBody,
            string? plainBody = null,
            CancellationToken ct = default);
    
}

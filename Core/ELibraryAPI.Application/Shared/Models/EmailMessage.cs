

namespace ELibraryAPI.Application.Shared.Models;

public record EmailMessage(string To, string Subject, string HtmlBody, string? PlainBody = null);

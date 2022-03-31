using TinyExchange.RazorPages.Models.AuthModels;

namespace TinyExchange.RazorPages.Infrastructure.Authentication;

public static class KycClaimSettings
{
    public const string PolicyName = "KycRequired";
    public const string ClaimType = "KycStatus";
    public static string ConfirmedKycClaimValue => KycState.Confirmed.ToString(); 
}
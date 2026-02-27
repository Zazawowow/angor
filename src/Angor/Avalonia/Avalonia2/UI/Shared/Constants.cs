namespace Avalonia2.UI.Shared;

/// <summary>
/// Shared constants replacing magic numbers and strings scattered across ViewModels.
/// </summary>
public static class Constants
{
    /// <summary>
    /// Vue threshold: investments below this amount (in BTC) are auto-approved.
    /// Vue ref: handleInvestment() in App.vue — amount &lt; 0.01 → auto-approved.
    /// </summary>
    public const double AutoApprovalThreshold = 0.01;

    /// <summary>Stubbed miner fee in BTC for transaction calculations.</summary>
    public const double MinerFee = 0.00000391;

    /// <summary>Angor fee rate (1%).</summary>
    public const double AngorFeeRate = 0.01;

    /// <summary>Formatted miner fee string for display.</summary>
    public const string MinerFeeDisplay = "0.00000391 BTC";

    /// <summary>Angor fee percentage for display.</summary>
    public const string AngorFeeDisplay = "1%";

    /// <summary>Stubbed Lightning invoice string shared across invest and deploy flows.</summary>
    public const string InvoiceString =
        "lnbc100n1pjk4x0spp5qe7m2wlr8kg3xm2h4f6n7y9t3v5w8k2j6p4r1s0d9f8g7h6j5qdzz2dpkx2ctvd5shjmnpd36x2mmpwvhsyg3pp8qctvd4ek2unnv5shjg3pd3skjmn0d36x7eqw3hjqar0ypxhxgrfducqzzsxqyz5vqsp5";

    /// <summary>Minimum investment amount in BTC.</summary>
    public const double MinInvestmentAmount = 0.001;
}

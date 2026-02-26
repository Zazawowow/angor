using System.Collections.ObjectModel;

namespace Avalonia2.UI.Sections.Portfolio;

/// <summary>
/// A stage in an investment's release schedule.
/// </summary>
public class InvestmentStageViewModel
{
    public int StageNumber { get; set; }
    public string Percentage { get; set; } = "0%";
    public string ReleaseDate { get; set; } = "";
    public string Amount { get; set; } = "0.00000000";
    public string Status { get; set; } = "Pending"; // Pending, Released, Available, Not Spent
    /// <summary>Stage label prefix: "Stage" for invest, "Payment" for fund/subscription</summary>
    public string StagePrefix { get; set; } = "Stage";

    // Status visibility helpers for per-status badge coloring
    public bool IsStatusPending => Status == "Pending";
    public bool IsStatusReleased => Status == "Released";
    public bool IsStatusNotSpent => Status == "Not Spent";
    public bool IsStatusRecovered => Status == "Recovered";
}

/// <summary>
/// A funded project investment shown in the "Your Investments" panel.
/// Data from Vue reference — Hope With Bitcoin investment.
/// Vue: investment-card in App.vue (desktop Investments page) and HubInvestments.vue
/// </summary>
public class InvestmentViewModel
{
    public string ProjectName { get; set; } = "";
    public string ShortDescription { get; set; } = "";
    public string TotalInvested { get; set; } = "0.00000000";
    public string AvailableToClaim { get; set; } = "0.00000000";
    public string Spent { get; set; } = "0.00000000";
    public double Progress { get; set; }
    /// <summary>Progress as a fraction 0.0–1.0 for ScaleTransform binding</summary>
    public double ProgressFraction => Progress / 100.0;
    public string Status { get; set; } = "Active";
    /// <summary>Funding amount displayed on the investment card</summary>
    public string FundingAmount { get; set; } = "0.0000";
    /// <summary>Date string displayed on the investment card</summary>
    public string FundingDate { get; set; } = "";
    /// <summary>Number of completed payment segments</summary>
    public int PaymentSegmentsCompleted { get; set; }
    /// <summary>Total number of payment segments</summary>
    public int PaymentSegmentsTotal { get; set; } = 3;
    /// <summary>Whether segment 1 is completed (for green fill in UI)</summary>
    public bool IsSegment1Complete => PaymentSegmentsCompleted >= 1;
    /// <summary>Whether segment 2 is completed</summary>
    public bool IsSegment2Complete => PaymentSegmentsCompleted >= 2;
    /// <summary>Whether segment 3 is completed</summary>
    public bool IsSegment3Complete => PaymentSegmentsCompleted >= 3;
    /// <summary>Banner image URL</summary>
    public string? BannerUrl { get; set; }
    /// <summary>Avatar/logo image URL</summary>
    public string? AvatarUrl { get; set; }

    // ── Type and Status pills (Vue: .investment-pills, .investment-type-pill, .stage-status) ──
    /// <summary>Type pill label: Investment, Funding, Subscription</summary>
    public string TypeLabel { get; set; } = "Investment";
    /// <summary>Status pill text: Awaiting Approval, Transaction signed, Investment Active, Funds recovered</summary>
    public string StatusText { get; set; } = "Transaction signed";
    /// <summary>Status class: waiting, signed, active, recovered</summary>
    public string StatusClass { get; set; } = "signed";

    // Status visibility helpers for XAML binding
    public bool IsStatusWaiting => StatusClass == "waiting";
    public bool IsStatusSigned => StatusClass == "signed";
    public bool IsStatusActive => StatusClass == "active";
    public bool IsStatusRecovered => StatusClass == "recovered";

    // Type visibility helpers for XAML binding (invest=blue, fund=amber, subscription=purple)
    public bool IsTypeInvest => ProjectType == "invest";
    public bool IsTypeFund => ProjectType == "fund";
    public bool IsTypeSubscription => ProjectType == "subscription";

    /// <summary>Whether this is a subscription/fund type with payment plan progress</summary>
    public bool HasPaymentPlan { get; set; }

    // Legacy pill properties — kept for InvestmentDetailView compatibility
    public string StatusPill1 { get; set; } = "Funding";
    public string StatusPill2 { get; set; } = "Transaction signed";
    /// <summary>Project type: invest, fund, subscription</summary>
    public string ProjectType { get; set; } = "invest";
    /// <summary>Investment step: 1=waiting, 2=preview, 3=active</summary>
    public int Step { get; set; } = 3;
    /// <summary>Target/Goal amount for the project</summary>
    public string TargetAmount { get; set; } = "0.0000";
    /// <summary>Total raised across all investors</summary>
    public string TotalRaised { get; set; } = "0.0000";
    /// <summary>Total investor count</summary>
    public int TotalInvestors { get; set; }
    /// <summary>Start date of the investment</summary>
    public string StartDate { get; set; } = "";
    /// <summary>End date of the investment</summary>
    public string EndDate { get; set; } = "";
    /// <summary>Transaction date</summary>
    public string TransactionDate { get; set; } = "";
    /// <summary>Approval status text</summary>
    public string ApprovalStatus { get; set; } = "Approved";
    public ObservableCollection<InvestmentStageViewModel> Stages { get; set; } = new();

    // ── Step visibility helpers (for XAML binding without converters) ──
    public bool IsStep1 => Step == 1;
    public bool IsStep2 => Step == 2;
    public bool IsStep3 => Step == 3;
    public bool IsStepAtLeast2 => Step >= 2;
    public bool IsStepAtLeast3 => Step >= 3;

    // ── Type-specific terminology (Vue: invest/fund/subscription maps) ──
    /// <summary>Action verb: "Invest" / "Fund" / "Subscribe"</summary>
    public string ActionVerb => ProjectType switch
    {
        "fund" => "Fund",
        "subscription" => "Subscribe",
        _ => "Invest"
    };

    /// <summary>Amount noun: "Investment" / "Funding" / "Subscription"</summary>
    public string AmountNoun => ProjectType switch
    {
        "fund" => "Funding",
        "subscription" => "Subscription",
        _ => "Investment"
    };

    /// <summary>Stage label: "Stage" / "Payment"</summary>
    public string StageLabel => ProjectType switch
    {
        "fund" => "Payment",
        "subscription" => "Payment",
        _ => "Stage"
    };

    /// <summary>Schedule title: "Release Schedule" / "Payment Schedule"</summary>
    public string ScheduleTitle => ProjectType switch
    {
        "fund" => "Payment Schedule",
        "subscription" => "Payment Schedule",
        _ => "Release Schedule"
    };

    /// <summary>Progress label: "Funding Progress" / "Subscription Progress"</summary>
    public string ProgressLabel => ProjectType switch
    {
        "subscription" => "Subscription Progress",
        _ => "Funding Progress"
    };

    /// <summary>Investor noun: "Investors" / "Funders" / "Subscribers"</summary>
    public string InvestorNoun => ProjectType switch
    {
        "fund" => "Funders",
        "subscription" => "Subscribers",
        _ => "Investors"
    };

    /// <summary>Target label: "Target Amount" / "Goal Amount"</summary>
    public string TargetLabel => ProjectType switch
    {
        "fund" => "Goal Amount",
        "subscription" => "Goal Amount",
        _ => "Target Amount"
    };

    /// <summary>Total raised label: "Total Raised" / "Total Funded" / "Total Subscribed"</summary>
    public string TotalRaisedLabel => ProjectType switch
    {
        "fund" => "Total Funded",
        "subscription" => "Total Subscribed",
        _ => "Total Raised"
    };

    /// <summary>Your amount label: "Your Investment" / "Your Funding" / "Your Subscription"</summary>
    public string YourAmountLabel => ProjectType switch
    {
        "fund" => "Your Funding",
        "subscription" => "Your Subscription",
        _ => "Your Investment"
    };
}

/// <summary>
/// Portfolio/Funded ViewModel — visual layer only.
/// Vue reference shows two-column layout:
/// - Left: Portfolio summary with Angor logo, stats, Refresh/Penalties buttons
/// - Right: Your Investments with investment cards
/// Data from Vue reference JS bundle.
/// </summary>
public partial class PortfolioViewModel : ReactiveObject
{
    /// <summary>
    /// When true, shows the empty state ("You haven't funded any projects yet.")
    /// When false, shows the portfolio with investments.
    /// Default false (showing populated state as demo).
    /// Toggle by setting HasInvestments = false.
    /// </summary>
    [Reactive] private bool hasInvestments = true;

    /// <summary>
    /// When set, the detail view for this investment is shown instead of the portfolio list.
    /// </summary>
    [Reactive] private InvestmentViewModel? selectedInvestment;

    // ── Left panel stats (reflect all sample investments) ──
    public int FundedProjects { get; } = 4;
    public string TotalInvested { get; } = "2.3500";
    public string RecoveredToPenalty { get; } = "0.2500";
    public int ProjectsInRecovery { get; } = 1;

    /// <summary>Total available to claim across all projects</summary>
    public string TotalAvailable { get; } = "0.08544254";

    // ── Right panel investments ──
    public ObservableCollection<InvestmentViewModel> Investments { get; } = new()
    {
        // ── Card 1: Investment type, signed status, payment plan ──
        new InvestmentViewModel
        {
            ProjectName = "Hope With \u20bfitcoin",
            ShortDescription = "A humanitarian initiative in Benin feeding vulnerable people, supporting orphanages.",
            FundingAmount = "0.5 BTC",
            FundingDate = "2/25/2026",
            TypeLabel = "Investment",
            StatusText = "Transaction signed",
            StatusClass = "signed",
            StatusPill1 = "Funding",
            StatusPill2 = "Transaction signed",
            HasPaymentPlan = true,
            PaymentSegmentsCompleted = 0,
            PaymentSegmentsTotal = 3,
            BannerUrl = "https://angor.tx1138.com/projects/hope-with-bitcoin-banner.webp",
            AvatarUrl = "https://angor.tx1138.com/projects/hope-with-bitcoin-logo.webp",
            TotalInvested = "0.50000000",
            AvailableToClaim = "0.00000000",
            Spent = "0.00000000",
            Progress = 0,
            Status = "Active",
            ProjectType = "invest",
            Step = 3,
            TargetAmount = "21.0000",
            TotalRaised = "4.5000",
            TotalInvestors = 12,
            StartDate = "Feb 25, 2026",
            EndDate = "Nov 25, 2026",
            TransactionDate = "Feb 25, 2026",
            ApprovalStatus = "Approved",
            Stages = new ObservableCollection<InvestmentStageViewModel>
            {
                new()
                {
                    StageNumber = 1,
                    Percentage = "33%",
                    ReleaseDate = "25 May 2026",
                    Amount = "0.16666667",
                    Status = "Pending"
                },
                new()
                {
                    StageNumber = 2,
                    Percentage = "33%",
                    ReleaseDate = "25 Aug 2026",
                    Amount = "0.16666667",
                    Status = "Pending"
                },
                new()
                {
                    StageNumber = 3,
                    Percentage = "34%",
                    ReleaseDate = "25 Nov 2026",
                    Amount = "0.16666666",
                    Status = "Pending"
                },
            }
        },

        // ── Card 2: Funding type, active status, payment plan with 2/3 paid ──
        new InvestmentViewModel
        {
            ProjectName = "Bitcoin Education Africa",
            ShortDescription = "Bringing Bitcoin literacy to schools across sub-Saharan Africa through workshops.",
            FundingAmount = "1.0 BTC",
            FundingDate = "1/10/2026",
            TypeLabel = "Funding",
            StatusText = "Investment Active",
            StatusClass = "active",
            StatusPill1 = "Funding",
            StatusPill2 = "Investment Active",
            HasPaymentPlan = true,
            PaymentSegmentsCompleted = 2,
            PaymentSegmentsTotal = 3,
            BannerUrl = null,
            AvatarUrl = null,
            TotalInvested = "1.00000000",
            AvailableToClaim = "0.05000000",
            Spent = "0.66666667",
            Progress = 67,
            Status = "Active",
            ProjectType = "fund",
            Step = 3,
            TargetAmount = "10.0000",
            TotalRaised = "7.2000",
            TotalInvestors = 24,
            StartDate = "Jan 10, 2026",
            EndDate = "Oct 10, 2026",
            TransactionDate = "Jan 10, 2026",
            ApprovalStatus = "Approved",
            Stages = new ObservableCollection<InvestmentStageViewModel>
            {
                new() { StageNumber = 1, StagePrefix = "Payment", Percentage = "33%", ReleaseDate = "10 Apr 2026", Amount = "0.33333333", Status = "Released" },
                new() { StageNumber = 2, StagePrefix = "Payment", Percentage = "33%", ReleaseDate = "10 Jul 2026", Amount = "0.33333333", Status = "Released" },
                new() { StageNumber = 3, StagePrefix = "Payment", Percentage = "34%", ReleaseDate = "10 Oct 2026", Amount = "0.33333334", Status = "Pending" },
            }
        },

        // ── Card 3: Subscription type, waiting status, no payment plan (progress bar) ──
        new InvestmentViewModel
        {
            ProjectName = "Nostr Relay Infrastructure",
            ShortDescription = "Decentralized relay infrastructure for censorship-resistant communication.",
            FundingAmount = "0.6 BTC",
            FundingDate = "2/01/2026",
            TypeLabel = "Subscription",
            StatusText = "Awaiting Approval",
            StatusClass = "waiting",
            StatusPill1 = "Subscription",
            StatusPill2 = "Awaiting Approval",
            HasPaymentPlan = false,
            PaymentSegmentsCompleted = 0,
            PaymentSegmentsTotal = 3,
            BannerUrl = null,
            AvatarUrl = null,
            TotalInvested = "0.60000000",
            AvailableToClaim = "0.00000000",
            Spent = "0.00000000",
            Progress = 35,
            Status = "Pending",
            ProjectType = "subscription",
            Step = 1,
            TargetAmount = "5.0000",
            TotalRaised = "1.7500",
            TotalInvestors = 8,
            StartDate = "Feb 01, 2026",
            EndDate = "Aug 01, 2026",
            TransactionDate = "Feb 01, 2026",
            ApprovalStatus = "Pending",
            Stages = new ObservableCollection<InvestmentStageViewModel>
            {
                new() { StageNumber = 1, StagePrefix = "Payment", Percentage = "50%", ReleaseDate = "01 May 2026", Amount = "0.30000000", Status = "Pending" },
                new() { StageNumber = 2, StagePrefix = "Payment", Percentage = "50%", ReleaseDate = "01 Aug 2026", Amount = "0.30000000", Status = "Pending" },
            }
        },

        // ── Card 4: Investment type, recovered status, no payment plan ──
        new InvestmentViewModel
        {
            ProjectName = "Lightning Mesh Network",
            ShortDescription = "Building resilient Lightning Network routing across emerging markets.",
            FundingAmount = "0.25 BTC",
            FundingDate = "11/15/2025",
            TypeLabel = "Investment",
            StatusText = "Funds recovered",
            StatusClass = "recovered",
            StatusPill1 = "Funding",
            StatusPill2 = "Funds recovered",
            HasPaymentPlan = false,
            PaymentSegmentsCompleted = 0,
            PaymentSegmentsTotal = 3,
            BannerUrl = null,
            AvatarUrl = null,
            TotalInvested = "0.25000000",
            AvailableToClaim = "0.03544254",
            Spent = "0.21455746",
            Progress = 86,
            Status = "Recovered",
            ProjectType = "invest",
            Step = 3,
            TargetAmount = "3.0000",
            TotalRaised = "2.8000",
            TotalInvestors = 15,
            StartDate = "Nov 15, 2025",
            EndDate = "May 15, 2026",
            TransactionDate = "Nov 15, 2025",
            ApprovalStatus = "Approved",
            Stages = new ObservableCollection<InvestmentStageViewModel>
            {
                new() { StageNumber = 1, Percentage = "33%", ReleaseDate = "15 Jan 2026", Amount = "0.08333333", Status = "Released" },
                new() { StageNumber = 2, Percentage = "33%", ReleaseDate = "15 Mar 2026", Amount = "0.08333333", Status = "Released" },
                new() { StageNumber = 3, Percentage = "34%", ReleaseDate = "15 May 2026", Amount = "0.08333334", Status = "Not Spent" },
            }
        },
    };

    /// <summary>Navigate to investment detail view</summary>
    public void OpenInvestmentDetail(InvestmentViewModel investment)
    {
        SelectedInvestment = investment;
    }

    /// <summary>Navigate back to portfolio list from investment detail</summary>
    public void CloseInvestmentDetail()
    {
        SelectedInvestment = null;
    }
}

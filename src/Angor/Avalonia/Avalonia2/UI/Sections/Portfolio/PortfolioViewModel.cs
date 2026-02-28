using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using Avalonia.Media.Imaging;
using Avalonia2.UI.Sections.FindProjects;
using Avalonia2.UI.Shared;
using Avalonia2.UI.Shared.Helpers;
using Avalonia2.UI.Shell;

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
/// Implements INotifyPropertyChanged so Step/Status changes propagate to the UI
/// (e.g. when a founder approves a signature in the Funders section).
/// </summary>
public class InvestmentViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

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
    private string? _bannerUrl;
    public string? BannerUrl
    {
        get => _bannerUrl;
        set
        {
            _bannerUrl = value;
            ImageCacheService.LoadBitmapAsync(value, bmp => { BannerBitmap = bmp; OnPropertyChanged(nameof(BannerBitmap)); });
        }
    }
    /// <summary>Avatar/logo image URL</summary>
    private string? _avatarUrl;
    public string? AvatarUrl
    {
        get => _avatarUrl;
        set
        {
            _avatarUrl = value;
            ImageCacheService.LoadBitmapAsync(value, bmp => { AvatarBitmap = bmp; OnPropertyChanged(nameof(AvatarBitmap)); });
        }
    }
    /// <summary>Decoded banner bitmap, loaded from <see cref="BannerUrl"/> via ImageCacheService.</summary>
    public Bitmap? BannerBitmap { get; private set; }
    /// <summary>Decoded avatar bitmap, loaded from <see cref="AvatarUrl"/> via ImageCacheService.</summary>
    public Bitmap? AvatarBitmap { get; private set; }

    // ── Type and Status pills (Vue: .investment-pills, .investment-type-pill, .stage-status) ──
    /// <summary>Type pill label: Investment, Funding, Subscription</summary>
    public string TypeLabel { get; set; } = "Investment";

    // ── Mutable properties that raise change notifications ──
    // These are updated by OnSignatureStatusChanged when a founder approves/rejects.

    private string _statusText = "Transaction signed";
    /// <summary>Status pill text: Awaiting Approval, Transaction signed, Investment Active, Funds recovered</summary>
    public string StatusText
    {
        get => _statusText;
        set
        {
            if (_statusText == value) return;
            _statusText = value;
            OnPropertyChanged();
        }
    }

    private string _statusClass = "signed";
    /// <summary>Status class: waiting, signed, active, recovered, rejected</summary>
    public string StatusClass
    {
        get => _statusClass;
        set
        {
            if (_statusClass == value) return;
            _statusClass = value;
            OnPropertyChanged();
            // Raise dependent computed properties
            OnPropertyChanged(nameof(IsStatusWaiting));
            OnPropertyChanged(nameof(IsStatusSigned));
            OnPropertyChanged(nameof(IsStatusActive));
            OnPropertyChanged(nameof(IsStatusRecovered));
        }
    }

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

    private string _statusPill2 = "Transaction signed";
    public string StatusPill2
    {
        get => _statusPill2;
        set
        {
            if (_statusPill2 == value) return;
            _statusPill2 = value;
            OnPropertyChanged();
        }
    }

    /// <summary>Project type: invest, fund, subscription</summary>
    public string ProjectType { get; set; } = "invest";

    private int _step = 3;
    /// <summary>Investment step: 1=waiting, 2=preview, 3=active</summary>
    public int Step
    {
        get => _step;
        set
        {
            if (_step == value) return;
            _step = value;
            OnPropertyChanged();
            // Raise dependent computed properties used in InvestmentDetailView step pills
            OnPropertyChanged(nameof(IsStep1));
            OnPropertyChanged(nameof(IsStep2));
            OnPropertyChanged(nameof(IsStep3));
            OnPropertyChanged(nameof(IsStepAtLeast2));
            OnPropertyChanged(nameof(IsStepAtLeast3));
        }
    }

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

    private string _approvalStatus = "Approved";
    /// <summary>Approval status text</summary>
    public string ApprovalStatus
    {
        get => _approvalStatus;
        set
        {
            if (_approvalStatus == value) return;
            _approvalStatus = value;
            OnPropertyChanged();
        }
    }

    /// <summary>Cross-reference to SharedSignature.Id for approval flow</summary>
    public int SignatureId { get; set; }
    public ObservableCollection<InvestmentStageViewModel> Stages { get; set; } = new();

    // ── Recovery / Penalty State (Vue: penaltyState in InvestmentDetail.vue) ──
    // State machine: none → pending → canRelease → released

    private string _penaltyState = "none";
    /// <summary>Recovery penalty state: "none", "pending", "canRelease", "released"</summary>
    public string PenaltyState
    {
        get => _penaltyState;
        set
        {
            if (_penaltyState == value) return;
            _penaltyState = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsPenaltyNone));
            OnPropertyChanged(nameof(IsPenaltyPending));
            OnPropertyChanged(nameof(IsPenaltyCanRelease));
            OnPropertyChanged(nameof(PenaltyButtonText));
            OnPropertyChanged(nameof(PenaltyButtonIcon));
            OnPropertyChanged(nameof(ShowRecoverButton));
        }
    }

    // Penalty state visibility helpers
    public bool IsPenaltyNone => PenaltyState == "none";
    public bool IsPenaltyPending => PenaltyState == "pending";
    public bool IsPenaltyCanRelease => PenaltyState == "canRelease";

    /// <summary>Whether the recovery button should be shown (Vue: isApproved && (investmentCompleted || currentStep === 3))</summary>
    public bool ShowRecoverButton => ApprovalStatus == "Approved" && Step == 3 && PenaltyState != "released";

    /// <summary>Dynamic button text per penalty state (Vue: computed penaltyButtonText)</summary>
    public string PenaltyButtonText => PenaltyState switch
    {
        "pending" => "Claim Penalties",
        "canRelease" => "Release Funds",
        _ => "Recover Funds"
    };

    /// <summary>Dynamic button icon per penalty state (Vue: refresh for none, check-circle for others)</summary>
    public string PenaltyButtonIcon => PenaltyState switch
    {
        "none" => "fa-solid fa-arrows-rotate",
        _ => "fa-solid fa-circle-check"
    };

    /// <summary>Number of unreleased stages (Vue: stagesToRecover computed)</summary>
    public int StagesToRecover => Stages.Count(s => s.Status != "Released");

    /// <summary>Sum of unreleased stage amounts (Vue: amountToRecover computed)</summary>
    public string AmountToRecover
    {
        get
        {
            var total = Stages.Where(s => s.Status != "Released")
                .Sum(s => double.TryParse(s.Amount, System.Globalization.NumberStyles.Float,
                    System.Globalization.CultureInfo.InvariantCulture, out var v) ? v : 0);
            return total.ToString("F5", System.Globalization.CultureInfo.InvariantCulture);
        }
    }

    // ── Recovery Modal Visibility State ──

    private bool _showRecoveryModal;
    public bool ShowRecoveryModal
    {
        get => _showRecoveryModal;
        set { if (_showRecoveryModal == value) return; _showRecoveryModal = value; OnPropertyChanged(); }
    }

    private bool _showClaimModal;
    public bool ShowClaimModal
    {
        get => _showClaimModal;
        set { if (_showClaimModal == value) return; _showClaimModal = value; OnPropertyChanged(); }
    }

    private bool _showReleaseModal;
    public bool ShowReleaseModal
    {
        get => _showReleaseModal;
        set { if (_showReleaseModal == value) return; _showReleaseModal = value; OnPropertyChanged(); }
    }

    private bool _showSuccessModal;
    public bool ShowSuccessModal
    {
        get => _showSuccessModal;
        set { if (_showSuccessModal == value) return; _showSuccessModal = value; OnPropertyChanged(); }
    }

    private string _selectedFeePriority = "standard";
    /// <summary>Fee priority: "priority", "standard", "economy"</summary>
    public string SelectedFeePriority
    {
        get => _selectedFeePriority;
        set
        {
            if (_selectedFeePriority == value) return;
            _selectedFeePriority = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsFeePriority));
            OnPropertyChanged(nameof(IsFeeStandard));
            OnPropertyChanged(nameof(IsFeeEconomy));
        }
    }

    public bool IsFeePriority => SelectedFeePriority == "priority";
    public bool IsFeeStandard => SelectedFeePriority == "standard";
    public bool IsFeeEconomy => SelectedFeePriority == "economy";

    private bool _isCustomFeeRate;
    public bool IsCustomFeeRate
    {
        get => _isCustomFeeRate;
        set { if (_isCustomFeeRate == value) return; _isCustomFeeRate = value; OnPropertyChanged(); }
    }

    private bool _isProcessing;
    public bool IsProcessing
    {
        get => _isProcessing;
        set { if (_isProcessing == value) return; _isProcessing = value; OnPropertyChanged(); }
    }

    // ── Recovery stub data (Vue: computed/static values) ──
    public string PenaltyDuration => "90 days";
    public string MinerFee => "0.00000645";
    public string DestinationAddress => "tb1q7fxzwjc97ft53sugz8v7szj5ul02er7wtykjwp";
    public string RecoveryProjectId => "angor1q3pthkftzh3ym4emg0ctcxhmz5u5m7lt5tk69je";
    public string PenaltyAmount => AmountToRecover;
    public int PenaltyDaysRemaining => 67;

    // ── Step visibility helpers (for XAML binding without converters) ──
    public bool IsStep1 => Step == 1;
    public bool IsStep2 => Step == 2;
    public bool IsStep3 => Step == 3;
    public bool IsStepAtLeast2 => Step >= 2;
    public bool IsStepAtLeast3 => Step >= 3;

    // ── Type-specific terminology (via shared ProjectTypeTerminology) ──
    private Shared.ProjectType TypeEnum => ProjectTypeExtensions.FromLowerString(ProjectType);

    /// <summary>Action verb: "Invest" / "Fund" / "Subscribe"</summary>
    public string ActionVerb => ProjectTypeTerminology.ActionVerb(TypeEnum);

    /// <summary>Amount noun: "Investment" / "Funding" / "Subscription"</summary>
    public string AmountNoun => ProjectTypeTerminology.AmountNoun(TypeEnum);

    /// <summary>Stage label: "Stage" / "Payment"</summary>
    public string StageLabel => ProjectTypeTerminology.StageLabel(TypeEnum);

    /// <summary>Schedule title: "Release Schedule" / "Payment Schedule"</summary>
    public string ScheduleTitle => ProjectTypeTerminology.ScheduleTitle(TypeEnum);

    /// <summary>Progress label: "Funding Progress" / "Subscription Progress"</summary>
    public string ProgressLabel => ProjectType switch
    {
        "subscription" => "Subscription Progress",
        _ => "Funding Progress"
    };

    /// <summary>Investor noun: "Investors" / "Funders" / "Subscribers"</summary>
    public string InvestorNoun => ProjectTypeTerminology.InvestorNounPlural(TypeEnum);

    /// <summary>Target label: "Target Amount" / "Goal Amount"</summary>
    public string TargetLabel => ProjectTypeTerminology.TargetNoun(TypeEnum);

    /// <summary>Total raised label: "Total Raised" / "Total Funded" / "Total Subscribed"</summary>
    public string TotalRaisedLabel => ProjectTypeTerminology.RaisedNoun(TypeEnum);

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

    public PortfolioViewModel()
    {
        // Listen for signature status changes to update investment steps.
        // Vue: when a founder approves/rejects, the investor's InvestmentDetail
        // reacts because it reads from the shared signatures array.
        SharedViewModels.Signatures.SignatureStatusChanged += OnSignatureStatusChanged;

        // Load sample data if prototype toggle says populated
        if (SharedViewModels.Prototype.ShowPopulatedApp)
            LoadSampleInvestments();

        // React to prototype toggle changes (populated ↔ empty)
        SharedViewModels.Prototype.WhenAnyValue(x => x.ShowPopulatedApp)
            .Skip(1) // skip initial value (already handled above)
            .Subscribe(showPopulated =>
            {
                if (showPopulated)
                    LoadSampleInvestments();
                else
                    ClearToEmpty();
            });
    }

    /// <summary>
    /// When a signature is approved, advance the matching investment from Step 1 → Step 2.
    /// Vue: currentStep computed in InvestmentDetail.vue (line 1026-1043).
    /// </summary>
    private void OnSignatureStatusChanged(SharedSignature sig)
    {
        var investment = Investments.FirstOrDefault(i => i.SignatureId == sig.Id);
        if (investment == null) return;

        if (sig.IsApproved && investment.Step == 1)
        {
            investment.Step = 2;
            investment.StatusText = "Transaction signed";
            investment.StatusClass = "signed";
            investment.StatusPill2 = "Transaction signed";
            investment.ApprovalStatus = "Approved";
        }
        else if (sig.IsRejected && investment.Step == 1)
        {
            investment.StatusText = "Rejected";
            investment.StatusClass = "rejected";
            investment.StatusPill2 = "Rejected";
            investment.ApprovalStatus = "Rejected";
        }
    }

    // ── Left panel stats (reflect all sample investments) ──
    public int FundedProjects { get; } = 4;
    public string TotalInvested { get; } = "2.3500";
    public string RecoveredToPenalty { get; } = "0.2500";
    public int ProjectsInRecovery { get; } = 1;

    /// <summary>Total available to claim across all projects</summary>
    public string TotalAvailable { get; } = "0.08544254";

    // ── Right panel investments ──
    public ObservableCollection<InvestmentViewModel> Investments { get; } = new();

    /// <summary>
    /// Populate with sample investments for visual testing of the populated state.
    /// Called when ShowPopulatedApp toggle is enabled.
    /// </summary>
    public void LoadSampleInvestments()
    {
        Investments.Clear();

        // ── Card 1: Investment type, signed status, payment plan ──
        Investments.Add(new InvestmentViewModel
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
        });

        // ── Card 2: Funding type, active status, payment plan with 2/3 paid ──
        Investments.Add(new InvestmentViewModel
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
            BannerUrl = "https://angor.tx1138.com/projects/generasi-merdeka-banner.jpg",
            AvatarUrl = "https://angor.tx1138.com/projects/generasi-merdeka-logo.jpg",
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
        });

        // ── Card 3: Subscription type, waiting status, no payment plan (progress bar) ──
        Investments.Add(new InvestmentViewModel
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
            BannerUrl = "https://angor.tx1138.com/projects/nostria-banner.jpg",
            AvatarUrl = "https://angor.tx1138.com/projects/nostria-logo.png",
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
        });

        // ── Card 4: Investment type, recovered status, no payment plan ──
        Investments.Add(new InvestmentViewModel
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
            BannerUrl = "https://angor.tx1138.com/projects/network-effect-banner.png",
            AvatarUrl = "https://angor.tx1138.com/projects/network-effect-logo.jpg",
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
        });

        HasInvestments = true;
    }

    /// <summary>
    /// Clear all investments to show the empty state.
    /// Called when ShowPopulatedApp toggle is disabled.
    /// </summary>
    public void ClearToEmpty()
    {
        SelectedInvestment = null;
        Investments.Clear();
        HasInvestments = false;
    }

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

    /// <summary>
    /// Remove all dynamically-added investments (those created via the invest flow,
    /// identified by SignatureId != 0) and reload sample data. Called by Settings → Reset Data.
    /// </summary>
    public void ResetToSampleData()
    {
        SelectedInvestment = null;
        Investments.Clear();
        LoadSampleInvestments();
    }

    /// <summary>
    /// Create an InvestmentViewModel from a ProjectItemViewModel and an investment amount,
    /// then add it to the Investments collection. Called after a successful invest flow.
    /// This enables the funded project to appear in the "Funded" section.
    /// </summary>
    public void AddInvestmentFromProject(ProjectItemViewModel project, string investmentAmount)
    {
        // Map ProjectType casing: ProjectItemViewModel uses "Invest"/"Fund"/"Subscription",
        // InvestmentViewModel uses lowercase "invest"/"fund"/"subscription"
        var projectType = project.ProjectType.ToLowerInvariant();
        var typeEnum = ProjectTypeExtensions.FromLowerString(projectType);
        var typeLabel = ProjectTypeTerminology.AmountNoun(typeEnum);

        // Build stages from the project's stage data
        var stages = new ObservableCollection<InvestmentStageViewModel>();
        var stagePrefix = ProjectTypeTerminology.StageLabel(typeEnum);
        if (project.Stages.Count > 0)
        {
            foreach (var s in project.Stages)
            {
                stages.Add(new InvestmentStageViewModel
                {
                    StageNumber = s.StageNumber,
                    StagePrefix = stagePrefix,
                    Percentage = s.Percentage,
                    ReleaseDate = s.ReleaseDate,
                    Amount = s.Amount,
                    Status = "Pending"
                });
            }
        }
        else
        {
            // Default 3-stage schedule
            for (var i = 1; i <= 3; i++)
            {
                stages.Add(new InvestmentStageViewModel
                {
                    StageNumber = i,
                    StagePrefix = stagePrefix,
                    Percentage = i < 3 ? "33%" : "34%",
                    ReleaseDate = DateTime.Now.AddMonths(i * 3).ToString("dd MMM yyyy"),
                    Amount = "0.00000000",
                    Status = "Pending"
                });
            }
        }

        // Vue threshold: investments < 0.01 BTC are auto-approved (App.vue line 8756)
        var amountValue = double.TryParse(investmentAmount, System.Globalization.NumberStyles.Float,
            System.Globalization.CultureInfo.InvariantCulture, out var parsedAmt) ? parsedAmt : 0;
        var isAutoApproved = amountValue < Constants.AutoApprovalThreshold;

        var investment = new InvestmentViewModel
        {
            ProjectName = project.ProjectName,
            ShortDescription = project.ShortDescription,
            FundingAmount = $"{investmentAmount} BTC",
            FundingDate = DateTime.Now.ToString("M/dd/yyyy"),
            TypeLabel = typeLabel,
            StatusText = isAutoApproved ? $"{typeLabel} Active" : "Awaiting Approval",
            StatusClass = isAutoApproved ? "active" : "waiting",
            StatusPill1 = typeLabel,
            StatusPill2 = isAutoApproved ? $"{typeLabel} Active" : "Awaiting Approval",
            HasPaymentPlan = true,
            PaymentSegmentsCompleted = 0,
            PaymentSegmentsTotal = stages.Count > 0 ? stages.Count : 3,
            BannerUrl = project.BannerUrl,
            AvatarUrl = project.AvatarUrl,
            TotalInvested = parsedAmt > 0 ? $"{parsedAmt:F8}" : investmentAmount,
            AvailableToClaim = "0.00000000",
            Spent = "0.00000000",
            Progress = 0,
            Status = isAutoApproved ? "Active" : "Pending",
            ProjectType = projectType,
            Step = isAutoApproved ? 3 : 1,
            TargetAmount = project.Target,
            TotalRaised = project.Raised,
            TotalInvestors = project.InvestorCount,
            StartDate = DateTime.Now.ToString("MMM dd, yyyy"),
            EndDate = project.EndDate,
            TransactionDate = DateTime.Now.ToString("MMM dd, yyyy"),
            ApprovalStatus = isAutoApproved ? "Approved" : "Pending",
            Stages = stages
        };

        // Create a signature in the shared store
        // Vue: handleInvestment() creates both investment + signature (App.vue line 8806)
        var sig = SharedViewModels.Signatures.AddSignature(
            project.ProjectName, // Using project name as ID (no real IDs in stub layer)
            project.ProjectName,
            investmentAmount);

        // Store the signature ID on the investment for cross-reference
        investment.SignatureId = sig.Id;

        // Insert at the top of the list so it's immediately visible
        Investments.Insert(0, investment);
        HasInvestments = true;
    }
}

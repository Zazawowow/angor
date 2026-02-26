using System.Collections.ObjectModel;
using Avalonia2.UI.Sections.MyProjects.Deploy;
using Avalonia2.UI.Sections.Portfolio;
using ReactiveUI;

namespace Avalonia2.UI.Sections.FindProjects;

/// <summary>Which screen the invest flow overlay is showing.</summary>
public enum InvestScreen
{
    InvestForm,
    WalletSelector,
    Invoice,
    Success
}

/// <summary>Quick amount option for the investment form.</summary>
public class QuickAmountOption
{
    public double Amount { get; set; }
    public string AmountText { get; set; } = "";
    public string Label { get; set; } = "";
}

/// <summary>Computed stage row for the release schedule column.</summary>
public class InvestStageRow
{
    public int StageNumber { get; set; }
    public string ReleaseDate { get; set; } = "";
    public string Percentage { get; set; } = "";
    public string Amount { get; set; } = "0.00000000";
}

/// <summary>
/// ViewModel for the InvestPage flow.
/// Orchestrates: InvestForm → WalletSelector → Invoice → Success.
/// All data is stubbed — no SDK dependencies.
///
/// Vue ref: InvestPage.vue (2984 lines)
/// </summary>
public partial class InvestPageViewModel : ReactiveObject
{
    // ── Project Reference ──
    public ProjectItemViewModel Project { get; }

    // ── Form State ──
    [Reactive] private string investmentAmount = "";
    [Reactive] private double? selectedQuickAmount;
    [Reactive] private InvestScreen currentScreen = InvestScreen.InvestForm;
    [Reactive] private WalletItem? selectedWallet;
    [Reactive] private bool isProcessing;
    [Reactive] private string paymentStatusText = "Awaiting payment...";
    [Reactive] private bool paymentReceived;

    // ── Derived visibility ──
    public bool IsInvestForm => CurrentScreen == InvestScreen.InvestForm;
    public bool IsWalletSelector => CurrentScreen == InvestScreen.WalletSelector;
    public bool IsInvoice => CurrentScreen == InvestScreen.Invoice;
    public bool IsSuccess => CurrentScreen == InvestScreen.Success;
    public bool HasSelectedWallet => SelectedWallet != null;
    public string PayButtonText => SelectedWallet != null
        ? $"Pay with {SelectedWallet.Name}"
        : "Choose Wallet";

    // ── Quick Amounts ──
    // Vue ref: quickAmounts grid — 4 items
    public ObservableCollection<QuickAmountOption> QuickAmounts { get; } = new()
    {
        new() { Amount = 0.001, AmountText = "0.001", Label = "100K sats" },
        new() { Amount = 0.01, AmountText = "0.01", Label = "1M sats" },
        new() { Amount = 0.1, AmountText = "0.1", Label = "10M sats" },
        new() { Amount = 0.5, AmountText = "0.5", Label = "50M sats" }
    };

    // ── Release Schedule (computed from project stages + amount) ──
    public ObservableCollection<InvestStageRow> Stages { get; } = new();

    // ── Transaction Details (stubbed) ──
    public string MinerFee { get; } = "0.00000391 BTC";
    public string AngorFee { get; } = "1%";
    public string ProjectId => Project.ProjectId;

    // ── Computed totals ──
    public string TotalAmount => ComputeTotal();
    public string FormattedAmount => string.IsNullOrWhiteSpace(InvestmentAmount) ? "0.00000000" : $"{ParseAmount():F8}";
    public string AngorFeeAmount => $"{ParseAmount() * 0.01:F8} BTC";
    public bool CanSubmit => ParseAmount() >= 0.001;

    // ── Success message ──
    public string SuccessTitle => Project.ProjectType switch
    {
        "Fund" => "Funding Pending Approval",
        "Subscription" => "Subscription Pending Approval",
        _ => "Investment Pending Approval"
    };
    public string SuccessDescription => $"Your {Project.ProjectType.ToLower()} of {FormattedAmount} BTC to {Project.ProjectName} has been submitted successfully.";
    public string SuccessButtonText => Project.ProjectType switch
    {
        "Fund" => "View My Fundings",
        "Subscription" => "View My Subscriptions",
        _ => "View My Investments"
    };

    // ── Stub wallets (reuse from DeployFlowOverlay) ──
    public ObservableCollection<WalletItem> Wallets { get; } = new()
    {
        new() { Name = "Main Wallet", Network = "Mainnet", Balance = "0.04821000 BTC" },
        new() { Name = "Angor Wallet", Network = "Signet", Balance = "1.25000000 BTC" },
        new() { Name = "Test Wallet", Network = "Testnet", Balance = "0.50000000 BTC" }
    };

    public string InvoiceString { get; } = "lnbc100n1pjk4x0spp5qe7m2wlr8kg3xm2h4f6n7y9t3v5w8k2j6p4r1s0d9f8g7h6j5qdzz2dpkx2ctvd5shjmnpd36x2mmpwvhsyg3pp8qctvd4ek2unnv5shjg3pd3skjmn0d36x7eqw3hjqar0ypxhxgrfducqzzsxqyz5vqsp5";

    public InvestPageViewModel(ProjectItemViewModel project)
    {
        Project = project;

        // Raise derived property notifications when screen changes
        this.WhenAnyValue(x => x.CurrentScreen)
            .Subscribe(_ =>
            {
                this.RaisePropertyChanged(nameof(IsInvestForm));
                this.RaisePropertyChanged(nameof(IsWalletSelector));
                this.RaisePropertyChanged(nameof(IsInvoice));
                this.RaisePropertyChanged(nameof(IsSuccess));
            });

        this.WhenAnyValue(x => x.SelectedWallet)
            .Subscribe(_ =>
            {
                this.RaisePropertyChanged(nameof(HasSelectedWallet));
                this.RaisePropertyChanged(nameof(PayButtonText));
            });

        // Recompute totals + stages when amount changes
        this.WhenAnyValue(x => x.InvestmentAmount)
            .Subscribe(_ =>
            {
                this.RaisePropertyChanged(nameof(TotalAmount));
                this.RaisePropertyChanged(nameof(FormattedAmount));
                this.RaisePropertyChanged(nameof(AngorFeeAmount));
                this.RaisePropertyChanged(nameof(CanSubmit));
                this.RaisePropertyChanged(nameof(SuccessDescription));
                RecomputeStages();
            });

        // Initialize stages from project
        RecomputeStages();
    }

    private double ParseAmount()
    {
        if (double.TryParse(InvestmentAmount, System.Globalization.NumberStyles.Float,
            System.Globalization.CultureInfo.InvariantCulture, out var val))
            return val;
        return 0;
    }

    private string ComputeTotal()
    {
        var amount = ParseAmount();
        var minerFee = 0.00000391;
        var angorFee = amount * 0.01;
        var total = amount + minerFee + angorFee;
        return $"{total:F8} BTC";
    }

    private void RecomputeStages()
    {
        Stages.Clear();
        var amount = ParseAmount();

        if (Project.Stages.Count > 0)
        {
            foreach (var s in Project.Stages)
            {
                // Parse percentage from string like "25%"
                var pctStr = s.Percentage.Replace("%", "");
                double.TryParse(pctStr, System.Globalization.NumberStyles.Float,
                    System.Globalization.CultureInfo.InvariantCulture, out var pct);
                var stageAmount = amount * (pct / 100.0);

                Stages.Add(new InvestStageRow
                {
                    StageNumber = s.StageNumber,
                    ReleaseDate = s.ReleaseDate,
                    Percentage = s.Percentage,
                    Amount = $"{stageAmount:F8}"
                });
            }
        }
        else
        {
            // Default 4 equal stages if project has none
            for (var i = 1; i <= 4; i++)
            {
                Stages.Add(new InvestStageRow
                {
                    StageNumber = i,
                    ReleaseDate = $"Stage {i}",
                    Percentage = "25%",
                    Amount = $"{amount * 0.25:F8}"
                });
            }
        }
    }

    // ── Actions ──

    /// <summary>Select a quick amount and set it as the investment amount.</summary>
    public void SelectQuickAmount(double amount)
    {
        SelectedQuickAmount = amount;
        InvestmentAmount = amount.ToString(System.Globalization.CultureInfo.InvariantCulture);
    }

    /// <summary>Submit the invest form → show wallet selector.
    /// Vue ref: proceedToPayment() checks balances, opens wallet modal.</summary>
    public void Submit()
    {
        if (!CanSubmit) return;
        CurrentScreen = InvestScreen.WalletSelector;
    }

    /// <summary>Select a wallet from the list.</summary>
    public void SelectWallet(WalletItem wallet)
    {
        foreach (var w in Wallets) w.IsSelected = false;
        wallet.IsSelected = true;
        SelectedWallet = wallet;
    }

    /// <summary>Pay with selected wallet → simulate processing → success.
    /// Vue ref: payWithWallet() → 800ms spinner → "received" → 1500ms → success.</summary>
    public async void PayWithWallet()
    {
        if (SelectedWallet == null) return;
        IsProcessing = true;
        PaymentStatusText = "Processing payment...";
        await Task.Delay(2500);
        CurrentScreen = InvestScreen.Success;
        IsProcessing = false;
    }

    /// <summary>Switch to invoice payment screen.
    /// Vue ref: "Or pay an invoice instead" button.</summary>
    public void ShowInvoice()
    {
        CurrentScreen = InvestScreen.Invoice;
    }

    /// <summary>Go back to wallet selector from invoice.</summary>
    public void BackToWalletSelector()
    {
        CurrentScreen = InvestScreen.WalletSelector;
    }

    /// <summary>Simulate paying via invoice → success.
    /// Vue ref: handlePayment() → paymentStatus "received" → success.</summary>
    public async void PayViaInvoice()
    {
        IsProcessing = true;
        PaymentStatusText = "Payment received!";
        PaymentReceived = true;
        await Task.Delay(2000);
        CurrentScreen = InvestScreen.Success;
        IsProcessing = false;
    }

    /// <summary>Close modal overlays, return to invest form.</summary>
    public void CloseModal()
    {
        CurrentScreen = InvestScreen.InvestForm;
        SelectedWallet = null;
        foreach (var w in Wallets) w.IsSelected = false;
        IsProcessing = false;
        PaymentStatusText = "Awaiting payment...";
        PaymentReceived = false;
    }
}

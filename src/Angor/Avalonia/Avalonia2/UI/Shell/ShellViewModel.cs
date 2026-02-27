using System.Collections.ObjectModel;
using Avalonia2.UI.Sections.FindProjects;
using Avalonia2.UI.Sections.Funders;
using Avalonia2.UI.Sections.Funds;
using Avalonia2.UI.Sections.Home;
using Avalonia2.UI.Sections.MyProjects;
using Avalonia2.UI.Sections.Portfolio;
using Avalonia2.UI.Sections.Settings;

namespace Avalonia2.UI.Shell;

/// <summary>
/// A shared signature/funding request that lives in the SignatureStore.
/// Created when an investor invests, read by Funders (founder side) and
/// Portfolio/InvestmentDetail (investor side).
/// Vue: signatures ref in App.vue (line 7447).
/// </summary>
public class SharedSignature
{
    public int Id { get; set; }
    public string ProjectId { get; set; } = "";
    public string ProjectTitle { get; set; } = "";
    public string Amount { get; set; } = "0.0000";
    public string Currency { get; set; } = "BTC";
    public string Date { get; set; } = "";
    public string Time { get; set; } = "";
    /// <summary>Status: waiting, approved, rejected</summary>
    public string Status { get; set; } = "waiting";
    public string InvestorName { get; set; } = "";
    public string Npub { get; set; } = "";
    public bool HasMessages { get; set; }

    // Helpers
    public bool IsWaiting => Status == "waiting";
    public bool IsApproved => Status == "approved";
    public bool IsRejected => Status == "rejected";
}

/// <summary>
/// Centralized store for signature/funding requests.
/// Shared between Funders (founder view) and Portfolio (investor view).
/// Vue: signatures ref + approveFunder/rejectFunder in App.vue.
/// 
/// Approval threshold (Vue: 0.01 BTC in handleInvestment):
///   - Amount &lt; 0.01 BTC → auto-approved (status='approved'), investment immediately active
///   - Amount >= 0.01 BTC → status='waiting', requires founder manual approval
/// </summary>
public class SignatureStore
{
    private int _nextId = 100;

    /// <summary>All signatures across all projects.</summary>
    public ObservableCollection<SharedSignature> AllSignatures { get; } = new();

    /// <summary>Event raised when a signature's status changes (approve/reject).</summary>
    public event Action<SharedSignature>? SignatureStatusChanged;

    /// <summary>
    /// Add a new signature for an investment.
    /// Vue: handleInvestment() in App.vue (line 8806).
    /// Applies auto-approval threshold: &lt; 0.01 BTC = auto-approved.
    /// </summary>
    public SharedSignature AddSignature(string projectId, string projectTitle, string amount)
    {
        var amountValue = double.TryParse(amount, System.Globalization.NumberStyles.Float,
            System.Globalization.CultureInfo.InvariantCulture, out var a) ? a : 0;

        // Vue threshold: investments < 0.01 BTC are auto-approved
        var requiresApproval = amountValue >= 0.01;
        var now = DateTime.Now;

        var sig = new SharedSignature
        {
            Id = _nextId++,
            ProjectId = projectId,
            ProjectTitle = projectTitle,
            Amount = amount,
            Currency = "BTC",
            Date = now.ToString("MMM dd, yyyy"),
            Time = now.ToString("HH:mm"),
            Status = requiresApproval ? "waiting" : "approved",
            InvestorName = $"Investor {AllSignatures.Count(s => s.ProjectId == projectId) + 1}",
            Npub = $"npub1{Guid.NewGuid():N}"[..64],
            HasMessages = false
        };

        AllSignatures.Insert(0, sig);
        return sig;
    }

    /// <summary>
    /// Approve a signature. Vue: approveFunder() in App.vue (line 7450).
    /// </summary>
    public void Approve(int id)
    {
        var sig = AllSignatures.FirstOrDefault(s => s.Id == id);
        if (sig == null) return;
        sig.Status = "approved";
        SignatureStatusChanged?.Invoke(sig);
    }

    /// <summary>
    /// Reject a signature. Vue: rejectFunder() in App.vue (line 7461).
    /// </summary>
    public void Reject(int id)
    {
        var sig = AllSignatures.FirstOrDefault(s => s.Id == id);
        if (sig == null) return;
        sig.Status = "rejected";
        SignatureStatusChanged?.Invoke(sig);
    }

    /// <summary>Approve all waiting signatures.</summary>
    public void ApproveAll()
    {
        foreach (var sig in AllSignatures.Where(s => s.Status == "waiting").ToList())
        {
            sig.Status = "approved";
            SignatureStatusChanged?.Invoke(sig);
        }
    }

    /// <summary>Clear all signatures (used by Reset Data).</summary>
    public void Clear() => AllSignatures.Clear();
}

/// <summary>
/// Provides access to shared ViewModels that persist across sidebar navigations.
/// Holds PortfolioViewModel, SignatureStore, and prototype-level settings so data survives navigation.
/// </summary>
public static class SharedViewModels
{
    // IMPORTANT: Declaration order matters — static field initializers run top-to-bottom.
    // Prototype must come before Portfolio because PortfolioViewModel's constructor
    // reads SharedViewModels.Prototype.ShowPopulatedApp.
    // Signatures must come before Portfolio because PortfolioViewModel's constructor
    // subscribes to SharedViewModels.Signatures.SignatureStatusChanged.
    public static SignatureStore Signatures { get; } = new();
    public static PrototypeSettings Prototype { get; } = new();
    public static PortfolioViewModel Portfolio { get; } = new();
}

/// <summary>
/// Global prototype-level settings (e.g. show populated vs empty states).
/// Sections observe ShowPopulatedApp to decide whether to load sample data.
/// </summary>
public partial class PrototypeSettings : ReactiveObject
{
    /// <summary>
    /// When true, sections show hardcoded sample data (populated state).
    /// When false, sections show empty states.
    /// Default = true so the app starts populated for demos.
    /// </summary>
    [Reactive] private bool showPopulatedApp = true;
}

/// <summary>
/// Base type for sidebar navigation entries (items and group headers).
/// </summary>
public abstract record NavEntry;

/// <summary>
/// A selectable sidebar navigation item with an icon resource key and display label.
/// Icon refers to a StreamGeometry key in Icons.axaml (e.g. "NavIconHome").
/// IconIsFilled indicates the icon uses Fill instead of Stroke rendering (e.g. the wallet icon).
/// </summary>
public record NavItem(string Label, string Icon, bool IconIsFilled = false) : NavEntry;

/// <summary>
/// A non-selectable group header with a divider line and uppercase label.
/// </summary>
public record NavGroupHeader(string Title) : NavEntry;

public partial class ShellViewModel : ReactiveObject
{
    [Reactive] private NavItem? selectedNavItem;
    [Reactive] private bool isSettingsOpen;
    [Reactive] private string? sectionTitleOverride;

    /// <summary>
    /// When true, the next MyProjectsView instance will auto-open the create wizard.
    /// Consumed (reset to false) by MyProjectsView on init.
    /// </summary>
    [Reactive] private bool pendingLaunchWizard;

    /// <summary>
    /// Shell-level modal overlay state. Any section can push a modal view here
    /// to have it rendered above the entire app (sidebar + content).
    /// </summary>
    [Reactive] private bool isModalOpen;
    [Reactive] private object? modalContent;

    public ShellViewModel()
    {
        NavEntries = new ObservableCollection<NavEntry>
        {
            // Ungrouped
            new NavItem("Home", "NavIconHome"),
            new NavItem("Funds", "NavIconWallet", IconIsFilled: true),
            // INVESTOR group
            new NavGroupHeader("INVESTOR"),
            new NavItem("Find Projects", "NavIconSearch"),
            new NavItem("Funded", "NavIconTrendUp"),
            // FOUNDER group
            new NavGroupHeader("FOUNDER"),
            new NavItem("My Projects", "NavIconDocument"),
            new NavItem("Funders", "NavIconUsers"),
        };

        selectedNavItem = (NavItem)NavEntries[0];

        // When a sidebar item is selected, leave settings mode
        this.WhenAnyValue(x => x.SelectedNavItem)
            .Where(item => item != null)
            .Subscribe(_ =>
            {
                IsSettingsOpen = false;
                SectionTitleOverride = null;
                this.RaisePropertyChanged(nameof(CurrentSectionContent));
                this.RaisePropertyChanged(nameof(SelectedSectionName));
            });

        this.WhenAnyValue(x => x.IsSettingsOpen)
            .Where(open => open)
            .Subscribe(_ =>
            {
                this.RaisePropertyChanged(nameof(CurrentSectionContent));
                this.RaisePropertyChanged(nameof(SelectedSectionName));
            });

        this.WhenAnyValue(x => x.SectionTitleOverride)
            .Subscribe(_ => this.RaisePropertyChanged(nameof(SelectedSectionName)));
    }

    public ObservableCollection<NavEntry> NavEntries { get; }

    public string SelectedSectionName => IsSettingsOpen ? "Settings" : (SectionTitleOverride ?? SelectedNavItem?.Label ?? "");

    public void NavigateToSettings()
    {
        SelectedNavItem = null;
        IsSettingsOpen = true;
    }

    /// <summary>
    /// Navigate to "My Projects" and auto-open the create project wizard.
    /// Called from the Home view's "Launch a Project" button.
    /// </summary>
    public void NavigateToMyProjectsAndLaunch()
    {
        PendingLaunchWizard = true;
        var myProjectsItem = NavEntries.OfType<NavItem>().FirstOrDefault(n => n.Label == "My Projects");
        if (myProjectsItem != null)
        {
            SelectedNavItem = myProjectsItem;
        }
    }

    /// <summary>
    /// Navigate to "Find Projects" section.
    /// Called from the Home view's "Find Projects" button.
    /// </summary>
    public void NavigateToFindProjects()
    {
        var findProjectsItem = NavEntries.OfType<NavItem>().FirstOrDefault(n => n.Label == "Find Projects");
        if (findProjectsItem != null)
        {
            SelectedNavItem = findProjectsItem;
        }
    }

    /// <summary>
    /// Navigate to "Funded" (Portfolio) section.
    /// Called after a successful investment to show the user their funded projects.
    /// </summary>
    public void NavigateToFunded()
    {
        var fundedItem = NavEntries.OfType<NavItem>().FirstOrDefault(n => n.Label == "Funded");
        if (fundedItem != null)
        {
            SelectedNavItem = fundedItem;
        }
    }

    /// <summary>
    /// Show a modal overlay above the entire app window.
    /// The content control will be displayed centered over a backdrop scrim.
    /// </summary>
    public void ShowModal(object content)
    {
        ModalContent = content;
        IsModalOpen = true;
    }

    /// <summary>
    /// Close the shell-level modal overlay.
    /// </summary>
    public void HideModal()
    {
        IsModalOpen = false;
        ModalContent = null;
    }

    public bool IsDarkThemeEnabled
    {
        get => Application.Current?.ActualThemeVariant == Avalonia.Styling.ThemeVariant.Dark;
        set
        {
            if (Application.Current != null)
            {
                Application.Current.RequestedThemeVariant = value
                    ? Avalonia.Styling.ThemeVariant.Dark
                    : Avalonia.Styling.ThemeVariant.Light;
            }
            this.RaisePropertyChanged();
        }
    }

    public object? CurrentSectionContent
    {
        get
        {
            if (IsSettingsOpen) return new SettingsView();

            return SelectedNavItem?.Label switch
            {
                "Home" => new HomeView(),
                "Funds" => new FundsView(),
                "Find Projects" => new FindProjectsView(),
                "Funded" => new PortfolioView(),
                "My Projects" => new MyProjectsView(),
                "Funders" => new FundersView(),
                _ => null,
            };
        }
    }
}

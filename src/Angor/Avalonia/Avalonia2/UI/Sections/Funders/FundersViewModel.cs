using System.Collections.ObjectModel;

namespace Avalonia2.UI.Sections.Funders;

/// <summary>
/// A signature/funding request from an investor.
/// Vue reference: App.vue desktop Funders page (lines 3152-3335)
/// Each card shows: project title, amount, date/time, status badge,
/// and action buttons (Approve/Reject/Chat/Expand).
/// </summary>
public class SignatureRequestViewModel
{
    public int Id { get; set; }
    public string ProjectTitle { get; set; } = "";
    public string Amount { get; set; } = "0.0000";
    public string Currency { get; set; } = "BTC";
    public string Date { get; set; } = "";
    public string Time { get; set; } = "";
    /// <summary>Status: waiting, approved, rejected</summary>
    public string Status { get; set; } = "waiting";
    /// <summary>Investor npub key (shown in expanded details)</summary>
    public string Npub { get; set; } = "npub1aunjpz36t2vwtqxyph2jc30c4feng4gv5yhhw6yckgzxa0rn52tq7tsnm7";
    /// <summary>Whether the chat has messages</summary>
    public bool HasMessages { get; set; }

    // Status visibility helpers for XAML
    public bool IsWaiting => Status == "waiting";
    public bool IsApproved => Status == "approved";
    public bool IsRejected => Status == "rejected";
}

/// <summary>
/// Funders ViewModel — founder's view of incoming signature/funding requests.
/// Vue reference: App.vue desktop Funders page (lines 3152-3335).
/// Shows "Funders Overview" heading, "Approve All" button,
/// filter tabs (Awaiting Approval / Approved / Rejected),
/// and signature request cards with Approve/Reject/Chat/Expand actions.
/// </summary>
public partial class FundersViewModel : ReactiveObject
{
    [Reactive] private bool hasFunders = true;

    /// <summary>
    /// Current filter tab: waiting, approved, rejected.
    /// Vue: funderFilter reactive variable.
    /// </summary>
    [Reactive] private string currentFilter = "waiting";

    /// <summary>
    /// Tracks which signature cards are expanded (showing npub).
    /// </summary>
    public ObservableCollection<int> ExpandedSignatureIds { get; } = new();

    // ── Signature counts (Vue: signatureCounts) ──
    public int WaitingCount => AllSignatures.Count(s => s.Status == "waiting");
    public int ApprovedCount => AllSignatures.Count(s => s.Status == "approved");
    public int RejectedCount => AllSignatures.Count(s => s.Status == "rejected");
    public bool HasRejected => RejectedCount > 0;

    // ── All signatures ──
    public ObservableCollection<SignatureRequestViewModel> AllSignatures { get; } = new()
    {
        new SignatureRequestViewModel
        {
            Id = 1,
            ProjectTitle = "Hope With \u20bfitcoin",
            Amount = "0.5000",
            Currency = "BTC",
            Date = "Feb 25, 2026",
            Time = "14:30",
            Status = "waiting",
            Npub = "npub1q8s7k4x9z2m3n4p5r6t7u8v9w0x1y2z3a4b5c6d7e8f",
            HasMessages = true
        },
        new SignatureRequestViewModel
        {
            Id = 2,
            ProjectTitle = "Hope With \u20bfitcoin",
            Amount = "0.7500",
            Currency = "BTC",
            Date = "Feb 20, 2026",
            Time = "09:15",
            Status = "waiting",
            Npub = "npub1m2d9f3a5b6c7d8e9f0g1h2i3j4k5l6m7n8o9p0q1r",
            HasMessages = false
        },
        new SignatureRequestViewModel
        {
            Id = 3,
            ProjectTitle = "Bitcoin Education Hub",
            Amount = "1.2500",
            Currency = "BTC",
            Date = "Feb 18, 2026",
            Time = "11:45",
            Status = "approved",
            Npub = "npub1x7r2c5b4d6e8f0a1b3c5d7e9f1g3h5i7j9k1l3m5",
            HasMessages = true
        },
        new SignatureRequestViewModel
        {
            Id = 4,
            ProjectTitle = "Hope With \u20bfitcoin",
            Amount = "0.2500",
            Currency = "BTC",
            Date = "Feb 15, 2026",
            Time = "16:20",
            Status = "rejected",
            Npub = "npub1a2b3c4d5e6f7g8h9i0j1k2l3m4n5o6p7q8r9s0t1u",
            HasMessages = false
        }
    };

    /// <summary>
    /// Filtered signatures based on current filter tab.
    /// </summary>
    [Reactive] private ObservableCollection<SignatureRequestViewModel> filteredSignatures = new();

    public FundersViewModel()
    {
        // Initialize filtered list
        UpdateFilteredSignatures();

        // React to filter changes
        this.WhenAnyValue(x => x.CurrentFilter)
            .Subscribe(_ => UpdateFilteredSignatures());
    }

    private void UpdateFilteredSignatures()
    {
        var filtered = AllSignatures.Where(s => s.Status == CurrentFilter).ToList();
        FilteredSignatures = new ObservableCollection<SignatureRequestViewModel>(filtered);

        // Update HasFunders based on whether there are any signatures at all
        HasFunders = AllSignatures.Count > 0;
    }

    public void ApproveSignature(int id)
    {
        var sig = AllSignatures.FirstOrDefault(s => s.Id == id);
        if (sig != null)
        {
            sig.Status = "approved";
            UpdateFilteredSignatures();
            this.RaisePropertyChanged(nameof(WaitingCount));
            this.RaisePropertyChanged(nameof(ApprovedCount));
        }
    }

    public void RejectSignature(int id)
    {
        var sig = AllSignatures.FirstOrDefault(s => s.Id == id);
        if (sig != null)
        {
            sig.Status = "rejected";
            UpdateFilteredSignatures();
            this.RaisePropertyChanged(nameof(WaitingCount));
            this.RaisePropertyChanged(nameof(RejectedCount));
            this.RaisePropertyChanged(nameof(HasRejected));
        }
    }

    public void ApproveAll()
    {
        foreach (var sig in AllSignatures.Where(s => s.Status == "waiting").ToList())
        {
            sig.Status = "approved";
        }
        UpdateFilteredSignatures();
        this.RaisePropertyChanged(nameof(WaitingCount));
        this.RaisePropertyChanged(nameof(ApprovedCount));
    }

    public void ToggleExpanded(int id)
    {
        if (ExpandedSignatureIds.Contains(id))
            ExpandedSignatureIds.Remove(id);
        else
            ExpandedSignatureIds.Add(id);
    }

    public bool IsExpanded(int id) => ExpandedSignatureIds.Contains(id);

    public void SetFilter(string filter)
    {
        CurrentFilter = filter;
    }
}

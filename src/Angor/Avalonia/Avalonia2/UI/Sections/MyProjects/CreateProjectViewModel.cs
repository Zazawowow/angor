using System.Collections.ObjectModel;
using Avalonia2.UI.Sections.MyProjects.Deploy;
using ReactiveUI;

namespace Avalonia2.UI.Sections.MyProjects;

/// <summary>
/// A stage in the project's release/payout schedule.
/// Generated from the payout pattern selection.
/// </summary>
public class ProjectStageViewModel
{
    public int StageNumber { get; set; }
    public string Percentage { get; set; } = "0%";
    public string ReleaseDate { get; set; } = "";
    public string AmountBtc { get; set; } = "0.00000000";

    /// <summary>Label for this stage row — "Stage", "Monthly Payout", "Weekly Payout", "Payment", etc.</summary>
    public string StageLabel { get; set; } = "Stage";
}

/// <summary>
/// ViewModel for the Create/Launch Project wizard.
/// Visual-layer only — all fields are sample/stub data with no SDK dependencies.
///
/// Vue wizard structure (6 steps for all 3 project types):
///   1. Project Type — choose invest / fund / subscription
///   2. Project Profile — name, about, website
///   3. Project Images — banner URL, profile/logo URL
///   4. Funding Configuration / Goal / Subscription Price
///   5. Stages / Payouts
///   6. Review & Deploy
///
/// Vue layout: two-column — vertical stepper on left, content on right.
/// Welcome screen shown on step 1 before type selection (Xe.value=true).
/// </summary>
public partial class CreateProjectViewModel : ReactiveObject
{
    // ── Wizard navigation state ──
    [Reactive] private int currentStep = 1;
    [Reactive] private int maxStepReached = 1;
    [Reactive] private bool showWelcome = true; // Vue: Xe.value — welcome card before type selection

    // ── Step 1: Project Type ──
    [Reactive] private string projectType = ""; // "investment", "fund", "subscription"

    // ── Step 2: Project Profile ──
    [Reactive] private string projectName = "";
    [Reactive] private string projectAbout = "";
    [Reactive] private string projectWebsite = "";

    // ── Step 3: Project Images ──
    [Reactive] private string bannerUrl = "";
    [Reactive] private string profileUrl = "";

    // ── Step 4: Funding Config (varies by type) ──
    [Reactive] private string targetAmount = "";
    [Reactive] private string startDate = "";
    [Reactive] private string endDate = "";
    [Reactive] private int penaltyDays = 90;
    [Reactive] private string approvalThreshold = "0.001";
    [Reactive] private string subscriptionPrice = ""; // subscription type only

    // CalendarDatePicker dates (Investment type)
    [Reactive] private DateTime? investStartDate;
    [Reactive] private DateTime? investEndDate;

    // ── Step 5: Stages/Payouts ──
    // Investment type: duration-based
    [Reactive] private string durationValue = "0"; // numeric input
    [Reactive] private string durationUnit = "Months"; // Months, Weeks, Days
    [Reactive] private int? durationPreset; // 3, 6, 12, 18, 24
    [Reactive] private string releaseFrequency = ""; // Weekly, Monthly, Bi-Monthly, Quarterly

    // Fund/Subscription type: payout-based
    [Reactive] private string payoutFrequency = ""; // "Monthly" or "Weekly"
    [Reactive] private int? selectedInstallments; // 3, 6, 12
    [Reactive] private int? monthlyPayoutDate; // 1-29
    [Reactive] private string weeklyPayoutDay = ""; // Mon, Tue, ... Sun

    // Shared
    [Reactive] private bool isAdvancedEditor;

    // Kept for backward compat (old pattern approach — can be removed later)
    [Reactive] private string selectedPayoutPattern = ""; // pattern1..4
    [Reactive] private string payoutDay = "1";

    // ── Step 6: Deploy ──
    [Reactive] private bool isDeploying;
    [Reactive] private bool isDeployed;

    /// <summary>
    /// Callback to notify parent when project is deployed.
    /// Called with the wizard VM so the parent can extract project data,
    /// add it to the list, and close the wizard.
    /// In the Vue prototype, goToMyProjects() does all of this in one shot:
    /// creates project, adds to list, closes wizard, navigates to my-projects.
    /// </summary>
    public Action? OnProjectDeployed { get; set; }

    /// <summary>The deploy flow overlay ViewModel.</summary>
    public DeployFlowViewModel DeployFlow { get; } = new();

    public CreateProjectViewModel()
    {
        // Default start date to today
        StartDate = DateTime.UtcNow.ToString("yyyy-MM-dd");
        InvestStartDate = DateTime.Now;

        // Generate sample stages when a payout pattern is selected (legacy)
        this.WhenAnyValue(x => x.SelectedPayoutPattern)
            .Where(p => !string.IsNullOrEmpty(p))
            .Subscribe(_ => GenerateStages());

        // Sync CalendarDatePicker dates to string properties for review step
        this.WhenAnyValue(x => x.InvestStartDate)
            .Subscribe(d => StartDate = d?.ToString("yyyy-MM-dd") ?? "");
        this.WhenAnyValue(x => x.InvestEndDate)
            .Subscribe(d => EndDate = d?.ToString("yyyy-MM-dd") ?? "");

        // When duration preset changes, sync to duration value
        this.WhenAnyValue(x => x.DurationPreset)
            .Where(d => d.HasValue)
            .Subscribe(d =>
            {
                DurationValue = d!.Value.ToString();
                DurationUnit = "Months";
            });

        // Track whether monthly or weekly payout date section is visible
        this.WhenAnyValue(x => x.PayoutFrequency)
            .Subscribe(_ =>
            {
                this.RaisePropertyChanged(nameof(IsPayoutMonthly));
                this.RaisePropertyChanged(nameof(IsPayoutWeekly));
            });
    }

    public string DeployButtonText => IsDeploying ? "Deploying..." : "Deploy Project";

    // ── Step names — 6 steps shown in the vertical stepper ──
    public string[] StepNames => ProjectType switch
    {
        "fund" => ["Project Type", "Project Profile", "Project Images", "Goal", "Payouts", "Review & Deploy"],
        "subscription" => ["Project Type", "Project Profile", "Project Images", "Subscription Price", "Payouts", "Review & Deploy"],
        _ => ["Project Type", "Project Profile", "Project Images", "Funding Configuration", "Stages", "Review & Deploy"]
    };

    public int TotalSteps => 6;

    // ── Step visibility helpers (for XAML binding) ──
    public bool IsStep1 => CurrentStep == 1 && !ShowWelcome;
    public bool IsWelcome => CurrentStep == 1 && ShowWelcome;
    public bool IsStep2 => CurrentStep == 2;
    public bool IsStep3 => CurrentStep == 3;
    public bool IsStep4 => CurrentStep == 4;
    public bool IsStep5 => CurrentStep == 5;
    public bool IsStep6 => CurrentStep == 6;

    // ── Navigation bar visibility — show when past welcome screen ──
    public bool ShowNavFooter => !ShowWelcome;

    // ── Project type visibility ──
    public bool IsInvestment => ProjectType == "investment";
    public bool IsFund => ProjectType == "fund";
    public bool IsSubscription => ProjectType == "subscription";
    public bool IsTypeSelected => !string.IsNullOrEmpty(ProjectType);

    // ── Cancel/Previous label for nav footer ──
    public string BackButtonText => CurrentStep == 1 ? "Cancel" : "Previous";

    // ── Type-specific terminology (Vue JS bundle exact mappings) ──
    public string ActionVerb => ProjectType switch
    {
        "fund" => "Fund",
        "subscription" => "Subscribe",
        _ => "Invest"
    };

    public string AmountNoun => ProjectType switch
    {
        "fund" => "Funding",
        "subscription" => "Subscription",
        _ => "Investment"
    };

    public string StageLabel => ProjectType switch
    {
        "fund" or "subscription" => "Payment",
        _ => "Stage"
    };

    public string ScheduleTitle => ProjectType switch
    {
        "fund" or "subscription" => "Payment Schedule",
        _ => "Release Schedule"
    };

    public string InvestorNoun => ProjectType switch
    {
        "fund" => "Funders",
        "subscription" => "Subscribers",
        _ => "Investors"
    };

    public string TargetLabel => ProjectType switch
    {
        "fund" => "Goal Amount",
        "subscription" => "Subscription Price",
        _ => "Target Amount"
    };

    public string Step4Title => ProjectType switch
    {
        "fund" => "Goal",
        "subscription" => "Subscription Price",
        _ => "Funding Configuration"
    };

    public string Step5Title => ProjectType switch
    {
        "fund" or "subscription" => "Payouts",
        _ => "Stages"
    };

    // ── Navigation commands ──
    public bool CanGoNext => CurrentStep < TotalSteps && IsCurrentStepValid;
    public bool CanGoBack => CurrentStep > 1;
    public bool IsLastStep => CurrentStep == TotalSteps;

    public bool IsCurrentStepValid => CurrentStep switch
    {
        1 => !string.IsNullOrEmpty(ProjectType),
        2 => !string.IsNullOrWhiteSpace(ProjectName) && !string.IsNullOrWhiteSpace(ProjectAbout),
        3 => true, // images are optional
        4 => ProjectType switch
        {
            "subscription" => !string.IsNullOrWhiteSpace(SubscriptionPrice),
            _ => !string.IsNullOrWhiteSpace(TargetAmount)
        },
        5 => Stages.Count > 0,
        6 => true,
        _ => false
    };

    /// <summary>Dismiss welcome screen, show type selection.</summary>
    public void DismissWelcome()
    {
        ShowWelcome = false;
        RaiseAllStepProperties();
    }

    public void GoNext()
    {
        if (!CanGoNext) return;
        CurrentStep++;
        if (CurrentStep > MaxStepReached)
            MaxStepReached = CurrentStep;
        RaiseAllStepProperties();
    }

    public void GoBack()
    {
        if (!CanGoBack) return;
        CurrentStep--;
        RaiseAllStepProperties();
    }

    public void GoToStep(int step)
    {
        if (step < 1 || step > MaxStepReached) return;
        CurrentStep = step;
        RaiseAllStepProperties();
    }

    public void SelectProjectType(string type)
    {
        ProjectType = type;
        this.RaisePropertyChanged(nameof(IsInvestment));
        this.RaisePropertyChanged(nameof(IsFund));
        this.RaisePropertyChanged(nameof(IsSubscription));
        this.RaisePropertyChanged(nameof(IsTypeSelected));
        this.RaisePropertyChanged(nameof(StepNames));
        this.RaisePropertyChanged(nameof(Step4Title));
        this.RaisePropertyChanged(nameof(Step5Title));
        this.RaisePropertyChanged(nameof(TargetLabel));
        this.RaisePropertyChanged(nameof(ActionVerb));
        this.RaisePropertyChanged(nameof(AmountNoun));
        this.RaisePropertyChanged(nameof(StageLabel));
        this.RaisePropertyChanged(nameof(ScheduleTitle));
        this.RaisePropertyChanged(nameof(InvestorNoun));
        this.RaisePropertyChanged(nameof(CanGoNext));
    }

    public void SelectPayoutPattern(string pattern)
    {
        SelectedPayoutPattern = pattern;
        this.RaisePropertyChanged(nameof(CanGoNext));
    }

    /// <summary>
    /// Start deployment — launches the deploy flow overlay.
    /// Vue ref: openDeployModal() → wallet picker or QR modal → success → goToMyProjects().
    /// goToMyProjects() creates the project, adds to list, closes wizard, navigates to my-projects.
    /// </summary>
    public void Deploy()
    {
        DeployFlow.OnDeployCompleted = () =>
        {
            IsDeployed = true;
            this.RaisePropertyChanged(nameof(DeployButtonText));
            // Matches Vue goToMyProjects(): add project to list + close wizard + navigate to list
            OnProjectDeployed?.Invoke();
        };
        DeployFlow.Show(ProjectName ?? "My Project");
    }

    // ── Step 5: Payout frequency visibility (Fund/Subscription) ──
    public bool IsPayoutMonthly => PayoutFrequency == "Monthly";
    public bool IsPayoutWeekly => PayoutFrequency == "Weekly";

    // ── Step 5: Fund/Subscription — can we generate payouts? ──
    public bool CanGeneratePayouts =>
        !string.IsNullOrEmpty(PayoutFrequency) &&
        SelectedInstallments.HasValue &&
        ((IsPayoutMonthly && MonthlyPayoutDate.HasValue) ||
         (IsPayoutWeekly && !string.IsNullOrEmpty(WeeklyPayoutDay)));

    // ── Step 5: Investment — can we generate stages? ──
    public bool CanGenerateStages =>
        int.TryParse(DurationValue, out var dv) && dv > 0 &&
        !string.IsNullOrEmpty(ReleaseFrequency);

    /// <summary>
    /// Generate stages for Investment type based on duration + frequency.
    /// </summary>
    public void GenerateInvestmentStages()
    {
        if (!int.TryParse(DurationValue, out var durationNum) || durationNum <= 0) return;
        if (string.IsNullOrEmpty(ReleaseFrequency)) return;

        Stages.Clear();

        // Convert duration to days
        var durationDays = DurationUnit switch
        {
            "Weeks" => durationNum * 7,
            "Days" => durationNum,
            _ => durationNum * 30 // Months
        };

        // Convert frequency to days
        var frequencyDays = ReleaseFrequency switch
        {
            "Weekly" => 7,
            "Bi-Monthly" => 60,
            "Quarterly" => 120,
            _ => 30 // Monthly
        };

        var stageCount = Math.Max(1, durationDays / frequencyDays);
        var baseDate = DateTime.TryParse(StartDate, out var sd) ? sd : DateTime.UtcNow;
        var targetBtc = double.TryParse(TargetAmount, out var t) ? t : 1.0;
        var percentPerStage = 100.0 / stageCount;

        for (int i = 0; i < stageCount; i++)
        {
            var releaseDate = baseDate.AddDays((i + 1) * frequencyDays);
            var pct = i < stageCount - 1
                ? Math.Round(percentPerStage)
                : 100 - Math.Round(percentPerStage) * (stageCount - 1);

            var freqLabel = ReleaseFrequency switch
            {
                "Weekly" => "Weekly Release",
                "Bi-Monthly" => "Bi-Monthly Release",
                "Quarterly" => "Quarterly Release",
                _ => "Monthly Release"
            };

            Stages.Add(new ProjectStageViewModel
            {
                StageNumber = i + 1,
                Percentage = $"{pct}%",
                ReleaseDate = releaseDate.ToString("dd MMM yyyy"),
                AmountBtc = (targetBtc * pct / 100).ToString("F8"),
                StageLabel = freqLabel
            });
        }

        this.RaisePropertyChanged(nameof(HasStages));
        this.RaisePropertyChanged(nameof(CanGoNext));
        this.RaisePropertyChanged(nameof(ScheduleSummary));
    }

    /// <summary>
    /// Generate payouts for Fund/Subscription type based on frequency + installments + day.
    /// </summary>
    public void GeneratePayoutSchedule()
    {
        if (!SelectedInstallments.HasValue) return;
        if (string.IsNullOrEmpty(PayoutFrequency)) return;

        Stages.Clear();

        var count = SelectedInstallments.Value;
        var baseDate = DateTime.TryParse(StartDate, out var sd) ? sd : DateTime.UtcNow;
        var targetBtc = double.TryParse(TargetAmount, out var t) ? t : 1.0;

        // For subscription, use SubscriptionPrice as sats
        if (ProjectType == "subscription" && long.TryParse(SubscriptionPrice, out var sats))
            targetBtc = sats / 100_000_000.0;

        var percentPerStage = 100.0 / count;

        for (int i = 0; i < count; i++)
        {
            DateTime releaseDate;
            if (PayoutFrequency == "Weekly")
            {
                releaseDate = baseDate.AddDays((i + 1) * 7);
            }
            else // Monthly
            {
                releaseDate = baseDate.AddMonths(i + 1);
                if (MonthlyPayoutDate.HasValue)
                {
                    var day = Math.Min(MonthlyPayoutDate.Value,
                        DateTime.DaysInMonth(releaseDate.Year, releaseDate.Month));
                    releaseDate = new DateTime(releaseDate.Year, releaseDate.Month, day);
                }
            }

            var pct = i < count - 1
                ? Math.Round(percentPerStage)
                : 100 - Math.Round(percentPerStage) * (count - 1);

            Stages.Add(new ProjectStageViewModel
            {
                StageNumber = i + 1,
                Percentage = $"{pct}%",
                ReleaseDate = releaseDate.ToString("dd MMM yyyy"),
                AmountBtc = (targetBtc * pct / 100).ToString("F8"),
                StageLabel = PayoutFrequency == "Weekly" ? "Weekly Payout" : "Monthly Payout"
            });
        }

        this.RaisePropertyChanged(nameof(HasStages));
        this.RaisePropertyChanged(nameof(CanGoNext));
        this.RaisePropertyChanged(nameof(ScheduleSummary));
    }

    /// <summary>Clear all generated stages/payouts.</summary>
    public void ClearStages()
    {
        Stages.Clear();
        this.RaisePropertyChanged(nameof(HasStages));
        this.RaisePropertyChanged(nameof(CanGoNext));
    }

    /// <summary>Toggle between simple and advanced editor (Investment).</summary>
    public void ToggleAdvancedEditor()
    {
        IsAdvancedEditor = !IsAdvancedEditor;
    }

    // ── Stages (generated from payout pattern) ──
    public ObservableCollection<ProjectStageViewModel> Stages { get; } = new();

    public bool HasStages => Stages.Count > 0;

    // ── Generate button labels ──
    public string GenerateButtonLabel => ProjectType switch
    {
        "fund" => "Generate Payout Schedule",
        "subscription" => "Generate Payout Schedule",
        _ => "Generate Fund Release Schedule"
    };

    public string Step5Subtitle => ProjectType switch
    {
        "fund" or "subscription" => "Select your payout pattern and schedule",
        _ => "Tell us how long the project will take and when you need payments"
    };

    /// <summary>
    /// Summary text shown in the Step 6 stages header (right side).
    /// E.g. "Monthly payouts (3 installments)" or "Quarterly releases (4 stages)".
    /// </summary>
    public string ScheduleSummary
    {
        get
        {
            if (Stages.Count == 0) return "";
            if (IsInvestment)
            {
                var freq = ReleaseFrequency;
                if (string.IsNullOrEmpty(freq)) freq = "Monthly";
                return $"{freq} releases ({Stages.Count} stages)";
            }
            else
            {
                var freq = PayoutFrequency;
                if (string.IsNullOrEmpty(freq)) freq = "Monthly";
                return $"{freq} payouts ({Stages.Count} installments)";
            }
        }
    }

    private void GenerateStages()
    {
        Stages.Clear();

        var (count, frequencyMonths) = SelectedPayoutPattern switch
        {
            "pattern1" => (3, 1),   // 3 monthly
            "pattern2" => (6, 1),   // 6 monthly
            "pattern3" => ProjectType == "subscription" ? (12, 1) : (6, 0), // 12 monthly or 6 weekly
            "pattern4" => (9, 0),   // 9 weekly
            _ => (3, 1)
        };

        var baseDate = DateTime.TryParse(StartDate, out var sd) ? sd : DateTime.UtcNow;
        var percentPerStage = 100.0 / count;
        var targetBtc = double.TryParse(TargetAmount, out var t) ? t : 1.0;

        for (int i = 0; i < count; i++)
        {
            var releaseDate = frequencyMonths > 0
                ? baseDate.AddMonths((i + 1) * frequencyMonths)
                : baseDate.AddDays((i + 1) * 7); // weekly

            var pct = i < count - 1
                ? Math.Round(percentPerStage)
                : 100 - Math.Round(percentPerStage) * (count - 1);

            Stages.Add(new ProjectStageViewModel
            {
                StageNumber = i + 1,
                Percentage = $"{pct}%",
                ReleaseDate = releaseDate.ToString("dd MMM yyyy"),
                AmountBtc = (targetBtc * pct / 100).ToString("F8"),
                StageLabel = frequencyMonths > 0 ? "Monthly Payout" : "Weekly Payout"
            });
        }

        this.RaisePropertyChanged(nameof(CanGoNext));
        this.RaisePropertyChanged(nameof(HasStages));
        this.RaisePropertyChanged(nameof(ScheduleSummary));
    }

    private void RaiseAllStepProperties()
    {
        this.RaisePropertyChanged(nameof(IsWelcome));
        this.RaisePropertyChanged(nameof(IsStep1));
        this.RaisePropertyChanged(nameof(IsStep2));
        this.RaisePropertyChanged(nameof(IsStep3));
        this.RaisePropertyChanged(nameof(IsStep4));
        this.RaisePropertyChanged(nameof(IsStep5));
        this.RaisePropertyChanged(nameof(IsStep6));
        this.RaisePropertyChanged(nameof(ShowNavFooter));
        this.RaisePropertyChanged(nameof(BackButtonText));
        this.RaisePropertyChanged(nameof(CanGoNext));
        this.RaisePropertyChanged(nameof(CanGoBack));
        this.RaisePropertyChanged(nameof(IsLastStep));
    }
}

using System;
using System.Collections.ObjectModel;
using System.Linq;
using ReactiveUI;

namespace Avalonia2.UI.Sections.MyProjects;

/// <summary>
/// A deployed project shown in the My Projects list after wizard completion.
/// </summary>
public class MyProjectItemViewModel
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string ProjectType { get; set; } = "investment";
    public string TargetAmount { get; set; } = "0.00000000";
    public string Status { get; set; } = "Open";
    public int InvestorCount { get; set; }
    public string Raised { get; set; } = "0.00000000";
    public double Progress { get; set; }
    public string? BannerUrl { get; set; }
    public string? LogoUrl { get; set; }
    public string StartDate { get; set; } = "";

    /// <summary>Banner as Uri for ProjectCard binding.</summary>
    public Uri? BannerUri => !string.IsNullOrEmpty(BannerUrl) ? new Uri(BannerUrl) : null;

    /// <summary>Logo as Uri for ProjectCard binding.</summary>
    public Uri? LogoUri => !string.IsNullOrEmpty(LogoUrl) ? new Uri(LogoUrl) : null;

    public string TypePillText => ProjectType switch
    {
        "fund" => "Fund",
        "subscription" => "Subscription",
        _ => "Investment"
    };

    // Vue uses "Invest" for the pill matching; ensure we return the expected value
    public string TypePillValue => ProjectType switch
    {
        "fund" => "Fund",
        "subscription" => "Subscription",
        _ => "Invest"
    };

    public string InvestorLabel => ProjectType switch
    {
        "fund" => "Funders",
        "subscription" => "Subscribers",
        _ => "Investors"
    };

    public string TargetLabel => ProjectType switch
    {
        "fund" => "Goal:",
        "subscription" => "Subscribers:",
        _ => "Target:"
    };
}

/// <summary>
/// My Projects ViewModel — manages the empty state, create wizard toggle,
/// and list of deployed projects. Visual layer only.
/// </summary>
public partial class MyProjectsViewModel : ReactiveObject
{
    [Reactive] private bool showCreateWizard;

    public MyProjectsViewModel()
    {
    }

    /// <summary>
    /// True when the user has deployed at least one project.
    /// </summary>
    public bool HasProjects => Projects.Count > 0;

    /// <summary>
    /// Sum of all raised amounts, formatted for display.
    /// </summary>
    public string TotalRaised
    {
        get
        {
            var total = Projects.Sum(p =>
            {
                if (double.TryParse(p.Raised, System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture, out var v))
                    return v;
                return 0;
            });
            return total.ToString("F5");
        }
    }

    /// <summary>
    /// List of deployed projects (populated after wizard completion).
    /// </summary>
    public ObservableCollection<MyProjectItemViewModel> Projects { get; } = new();

    /// <summary>
    /// Open the create project wizard.
    /// </summary>
    public void LaunchCreateWizard()
    {
        ShowCreateWizard = true;
    }

    /// <summary>
    /// Called after a project is successfully deployed.
    /// Adds the project to the list and exits the wizard.
    /// </summary>
    public void OnProjectDeployed(CreateProjectViewModel wizardVm)
    {
        Projects.Add(new MyProjectItemViewModel
        {
            Name = wizardVm.ProjectName,
            Description = wizardVm.ProjectAbout,
            ProjectType = wizardVm.ProjectType,
            TargetAmount = !string.IsNullOrEmpty(wizardVm.TargetAmount) ? wizardVm.TargetAmount : "0",
            Status = "Open",
            StartDate = wizardVm.StartDate,
            BannerUrl = wizardVm.BannerUrl,
            LogoUrl = wizardVm.ProfileUrl,
        });
        this.RaisePropertyChanged(nameof(HasProjects));
        this.RaisePropertyChanged(nameof(TotalRaised));
    }

    /// <summary>
    /// Close the wizard and return to the project list / empty state.
    /// </summary>
    public void CloseCreateWizard()
    {
        ShowCreateWizard = false;
    }

    /// <summary>
    /// Populate with sample projects for visual testing of the populated state.
    /// Called from code-behind when needed for development preview.
    /// </summary>
    public void LoadSampleProjects()
    {
        Projects.Clear();
        Projects.Add(new MyProjectItemViewModel
        {
            Name = "Hope With Bitcoin",
            Description = "Supporting communities through Bitcoin adoption and financial literacy programs across developing nations.",
            ProjectType = "fund",
            TargetAmount = "2.00000",
            Status = "Open",
            InvestorCount = 12,
            Raised = "0.50000",
            Progress = 25,
            StartDate = "2025-03-01",
            BannerUrl = "https://angor.tx1138.com/projects/hope-with-bitcoin-banner.webp",
            LogoUrl = "https://angor.tx1138.com/projects/hope-with-bitcoin-logo.webp",
        });
        Projects.Add(new MyProjectItemViewModel
        {
            Name = "Zap AI",
            Description = "AI-powered tools for the Bitcoin ecosystem with advanced analytics and lightning-fast predictions.",
            ProjectType = "investment",
            TargetAmount = "0.75000",
            Status = "Open",
            InvestorCount = 5,
            Raised = "0.45678",
            Progress = 61,
            StartDate = "2025-01-15",
            BannerUrl = "https://angor.tx1138.com/projects/zap-ai-banner.png",
            LogoUrl = "https://angor.tx1138.com/projects/zap-ai-logo.png",
        });
        Projects.Add(new MyProjectItemViewModel
        {
            Name = "Bitcoin Education Hub",
            Description = "A comprehensive online learning platform dedicated to Bitcoin technology and development.",
            ProjectType = "subscription",
            TargetAmount = "1.50000",
            Status = "Open",
            InvestorCount = 30,
            Raised = "1.20000",
            Progress = 80,
            StartDate = "2024-11-10",
        });
        this.RaisePropertyChanged(nameof(HasProjects));
        this.RaisePropertyChanged(nameof(TotalRaised));
    }
}

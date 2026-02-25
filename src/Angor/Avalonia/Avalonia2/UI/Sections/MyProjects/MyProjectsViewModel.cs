using System.Collections.ObjectModel;
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

    public string TypePillText => ProjectType switch
    {
        "fund" => "Fund",
        "subscription" => "Subscription",
        _ => "Investment"
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
    }

    /// <summary>
    /// Close the wizard and return to the project list / empty state.
    /// </summary>
    public void CloseCreateWizard()
    {
        ShowCreateWizard = false;
    }
}

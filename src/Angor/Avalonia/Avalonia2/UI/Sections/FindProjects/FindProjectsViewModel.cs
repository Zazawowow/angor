using System.Collections.ObjectModel;

namespace Avalonia2.UI.Sections.FindProjects;

/// <summary>
/// Sample project data for the visual layer. The backend team will replace
/// this with real project data from Nostr via the SDK.
/// All data below is extracted from the Vue reference at angor.tx1138.com/app.html.
/// </summary>
public class ProjectItemViewModel
{
    public string ProjectName { get; set; } = "";
    public string ShortDescription { get; set; } = "";
    public int InvestorCount { get; set; }
    /// <summary>"Investors" (invest), "Funders" (fund), or "Subscribers" (subscription)</summary>
    public string InvestorLabel { get; set; } = "Investors";
    public string Raised { get; set; } = "0.00000";
    public string Target { get; set; } = "0.00000";
    /// <summary>"Target:" (invest), "Goal:" (fund), or "Total Subscribers:" (subscription)</summary>
    public string TargetLabel { get; set; } = "Target:";
    public double Progress { get; set; }
    public string ProjectType { get; set; } = "Invest";
    public string Status { get; set; } = "Open";
    public string? BannerUrl { get; set; }
    public string? AvatarUrl { get; set; }
}

/// <summary>
/// FindProjects ViewModel — visual layer only.
/// Shows a responsive grid of sample project cards (no search bar per Vue reference).
/// The backend team will wire up real Nostr-based project discovery via SDK.
/// Data: 24 mainnet projects from the Vue reference JS bundle.
/// Order: IDs 20-24 first (recent), then IDs 1-18 (established).
/// </summary>
public partial class FindProjectsViewModel : ReactiveObject
{
    private const string BaseUrl = "https://angor.tx1138.com";

    public ObservableCollection<ProjectItemViewModel> Projects { get; } = new()
    {
        // ── Recent Projects (ids 20-24, prepended via unshift in Vue) ──
        new()
        {
            ProjectName = "Hope With \u20bfitcoin",
            ShortDescription = "A humanitarian initiative in Benin feeding vulnerable people, supporting orphanages, a...",
            InvestorCount = 1,
            InvestorLabel = "Funders",
            Raised = "0.50000",
            Target = "0.20000",
            TargetLabel = "Goal:",
            Progress = 100,
            ProjectType = "Fund",
            Status = "Open",
            BannerUrl = BaseUrl + "/projects/hope-with-bitcoin-banner.webp",
            AvatarUrl = BaseUrl + "/projects/hope-with-bitcoin-logo.webp"
        },
        new()
        {
            ProjectName = "Alchemist MinersTech (SuriaBit)",
            ShortDescription = "SuriaBit is Alchemist Miners' sustainable Bitcoin fund deploying miners at Malaysian...",
            InvestorCount = 0,
            InvestorLabel = "Investors",
            Raised = "0.00000",
            Target = "1.60000",
            TargetLabel = "Target:",
            Progress = 0,
            ProjectType = "Invest",
            Status = "Open",
            BannerUrl = BaseUrl + "/projects/alchemist-banner.jpg",
            AvatarUrl = BaseUrl + "/projects/alchemist-logo.jpg"
        },
        new()
        {
            ProjectName = "Penlock",
            ShortDescription = "A printable paper calculator that enables users to split a seed phrase into a secure 2-of-3...",
            InvestorCount = 1,
            InvestorLabel = "Funders",
            Raised = "0.05900",
            Target = "0.04000",
            TargetLabel = "Goal:",
            Progress = 100,
            ProjectType = "Fund",
            Status = "Funded",
            BannerUrl = BaseUrl + "/projects/penlock-banner.jpg",
            AvatarUrl = BaseUrl + "/projects/penlock-logo.jpg"
        },
        new()
        {
            ProjectName = "Bitasha",
            ShortDescription = "Bitcoin mining is a probabilistic search of the nonce space with a hope of winning the...",
            InvestorCount = 1,
            InvestorLabel = "Funders",
            Raised = "0.01001",
            Target = "0.01000",
            TargetLabel = "Goal:",
            Progress = 100,
            ProjectType = "Fund",
            Status = "Funded",
            BannerUrl = BaseUrl + "/projects/bitasha-banner.jpeg",
            AvatarUrl = BaseUrl + "/projects/bitasha-logo.png"
        },
        new()
        {
            ProjectName = "CryoBrick",
            ShortDescription = "Secure. Affordable. Inconspicuous. Bitcoin Custody Reimagined. CryoBrick is a self-...",
            InvestorCount = 1,
            InvestorLabel = "Investors",
            Raised = "0.01001",
            Target = "0.01000",
            TargetLabel = "Target:",
            Progress = 100,
            ProjectType = "Invest",
            Status = "Funded",
            BannerUrl = BaseUrl + "/projects/cryobrick-banner.png",
            AvatarUrl = BaseUrl + "/projects/cryobrick-logo.png"
        },
        // ── Established Projects (ids 1-18) ──
        new()
        {
            ProjectName = "Angor UX",
            ShortDescription = "Creation of Angor's new UX and UI for the desktop app and related screens",
            InvestorCount = 2,
            InvestorLabel = "Investors",
            Raised = "0.21234",
            Target = "1.00000",
            TargetLabel = "Target:",
            Progress = 21,
            ProjectType = "Invest",
            Status = "Open",
            BannerUrl = BaseUrl + "/projects/angor-ux-banner.gif",
            AvatarUrl = BaseUrl + "/projects/angor-ux-logo.svg"
        },
        new()
        {
            ProjectName = "Zap AI",
            ShortDescription = "AI-powered analytics and insights for Bitcoin Lightning Network node operators.",
            InvestorCount = 5,
            InvestorLabel = "Investors",
            Raised = "0.45678",
            Target = "0.75000",
            TargetLabel = "Target:",
            Progress = 61,
            ProjectType = "Invest",
            Status = "Funding Closed",
            BannerUrl = BaseUrl + "/projects/zap-ai-banner.png",
            AvatarUrl = BaseUrl + "/projects/zap-ai-logo.png"
        },
        new()
        {
            ProjectName = "Nostria - Your Social Network",
            ShortDescription = "A comprehensive Nostr client with advanced relay management and content discovery features.",
            InvestorCount = 8,
            InvestorLabel = "Subscribers",
            Raised = "0.67000",
            Target = "1.00000",
            TargetLabel = "Total Subscribers:",
            Progress = 67,
            ProjectType = "Subscription",
            Status = "Open",
            BannerUrl = BaseUrl + "/projects/nostria-banner.jpg",
            AvatarUrl = BaseUrl + "/projects/nostria-logo.png"
        },
        new()
        {
            ProjectName = "Generasi Merdeka (Sovereign Generation)",
            ShortDescription = "Empowering Indonesian youth through Bitcoin education and circular economy initiatives.",
            InvestorCount = 3,
            InvestorLabel = "Subscribers",
            Raised = "0.12345",
            Target = "0.50000",
            TargetLabel = "Total Subscribers:",
            Progress = 25,
            ProjectType = "Subscription",
            Status = "Funding Closed",
            BannerUrl = BaseUrl + "/projects/generasi-merdeka-banner.jpg",
            AvatarUrl = BaseUrl + "/projects/generasi-merdeka-logo.jpg"
        },
        new()
        {
            ProjectName = "Bitcoin-Basketball Integration Project",
            ShortDescription = "Bringing Bitcoin payments and rewards to amateur basketball leagues and tournaments.",
            InvestorCount = 4,
            InvestorLabel = "Subscribers",
            Raised = "0.08900",
            Target = "0.35000",
            TargetLabel = "Total Subscribers:",
            Progress = 25,
            ProjectType = "Subscription",
            Status = "Open",
            BannerUrl = BaseUrl + "/projects/bitcoin-basketball-logo.jpg",
            AvatarUrl = BaseUrl + "/projects/bitcoin-basketball-logo.jpg"
        },
        new()
        {
            ProjectName = "Code Orange Dev School",
            ShortDescription = "Bitcoin development bootcamp teaching open-source contribution and protocol development.",
            InvestorCount = 11,
            InvestorLabel = "Subscribers",
            Raised = "1.20000",
            Target = "1.20000",
            TargetLabel = "Total Subscribers:",
            Progress = 100,
            ProjectType = "Subscription",
            Status = "Funded",
            BannerUrl = BaseUrl + "/projects/code-orange-banner.jpg",
            AvatarUrl = BaseUrl + "/projects/code-orange-logo.jpg"
        },
        new()
        {
            ProjectName = "Cruzada21 - VEINTIUNO.LAT",
            ShortDescription = "Community-driven Bitcoin adoption campaign across Latin American countries.",
            InvestorCount = 7,
            InvestorLabel = "Funders",
            Raised = "0.34560",
            Target = "0.80000",
            TargetLabel = "Goal:",
            Progress = 43,
            ProjectType = "Fund",
            Status = "Open",
            BannerUrl = BaseUrl + "/projects/cruzada21-banner.jpg",
            AvatarUrl = BaseUrl + "/projects/cruzada21-logo.jpg"
        },
        new()
        {
            ProjectName = "SeedSigner + BIP-353",
            ShortDescription = "Integrating BIP-353 human-readable payment addresses into the SeedSigner hardware wallet.",
            InvestorCount = 2,
            InvestorLabel = "Funders",
            Raised = "0.05600",
            Target = "0.15000",
            TargetLabel = "Goal:",
            Progress = 37,
            ProjectType = "Fund",
            Status = "Funding Closed",
            BannerUrl = BaseUrl + "/projects/seedsigner-banner.png",
            AvatarUrl = BaseUrl + "/projects/seedsigner-logo.png"
        },
        new()
        {
            ProjectName = "ZapTracker",
            ShortDescription = "Real-time Lightning zap tracking and analytics dashboard for content creators.",
            InvestorCount = 6,
            InvestorLabel = "Funders",
            Raised = "0.23450",
            Target = "0.50000",
            TargetLabel = "Goal:",
            Progress = 47,
            ProjectType = "Fund",
            Status = "Open",
            BannerUrl = BaseUrl + "/projects/zaptracker-banner.png",
            AvatarUrl = BaseUrl + "/projects/zaptracker-logo.webp"
        },
        new()
        {
            ProjectName = "Sweepstack",
            ShortDescription = "Automated UTXO consolidation and privacy-preserving transaction batching tool.",
            InvestorCount = 4,
            InvestorLabel = "Funders",
            Raised = "0.12340",
            Target = "0.25000",
            TargetLabel = "Goal:",
            Progress = 49,
            ProjectType = "Fund",
            Status = "Funding Closed",
            BannerUrl = BaseUrl + "/projects/sweepstack-banner.png",
            AvatarUrl = BaseUrl + "/projects/sweepstack-logo.png"
        },
        new()
        {
            ProjectName = "BlindBit",
            ShortDescription = "Silent payments implementation for enhanced Bitcoin transaction privacy.",
            InvestorCount = 9,
            InvestorLabel = "Funders",
            Raised = "0.45600",
            Target = "0.60000",
            TargetLabel = "Goal:",
            Progress = 76,
            ProjectType = "Fund",
            Status = "Open",
            BannerUrl = BaseUrl + "/projects/blindbit-logo.png",
            AvatarUrl = BaseUrl + "/projects/blindbit-logo.png"
        },
        new()
        {
            ProjectName = "git-futz",
            ShortDescription = "Git-based collaboration tools with Nostr integration for open-source Bitcoin projects.",
            InvestorCount = 3,
            InvestorLabel = "Funders",
            Raised = "0.07800",
            Target = "0.20000",
            TargetLabel = "Goal:",
            Progress = 39,
            ProjectType = "Fund",
            Status = "Open",
            BannerUrl = BaseUrl + "/projects/git-futz-banner.png",
            AvatarUrl = BaseUrl + "/projects/git-futz-logo.png"
        },
        new()
        {
            ProjectName = "Receipt.Cash",
            ShortDescription = "Point-of-sale receipt system with Lightning payment verification and NFC support.",
            InvestorCount = 5,
            InvestorLabel = "Investors",
            Raised = "0.18900",
            Target = "0.40000",
            TargetLabel = "Target:",
            Progress = 47,
            ProjectType = "Invest",
            Status = "Funding Closed",
            BannerUrl = BaseUrl + "/projects/receipt-cash-banner.webp",
            AvatarUrl = BaseUrl + "/projects/receipt-cash-logo.webp"
        },
        new()
        {
            ProjectName = "Finding Home Episode 3",
            ShortDescription = "Documentary series following Bitcoin adoption stories in communities around the world.",
            InvestorCount = 12,
            InvestorLabel = "Investors",
            Raised = "0.67800",
            Target = "0.90000",
            TargetLabel = "Target:",
            Progress = 75,
            ProjectType = "Invest",
            Status = "Open",
            BannerUrl = BaseUrl + "/projects/finding-home-banner.png",
            AvatarUrl = BaseUrl + "/projects/finding-home-logo.png"
        },
        new()
        {
            ProjectName = "YakiHonne",
            ShortDescription = "Nostr-based long-form content publishing platform with Bitcoin monetization.",
            InvestorCount = 15,
            InvestorLabel = "Investors",
            Raised = "1.50000",
            Target = "1.50000",
            TargetLabel = "Target:",
            Progress = 100,
            ProjectType = "Invest",
            Status = "Funded",
            BannerUrl = BaseUrl + "/projects/yakihonne-banner.png",
            AvatarUrl = BaseUrl + "/projects/yakihonne-logo.png"
        },
        new()
        {
            ProjectName = "Network Effect",
            ShortDescription = "Research and data visualization platform for Bitcoin network analysis and metrics.",
            InvestorCount = 8,
            InvestorLabel = "Subscribers",
            Raised = "0.89000",
            Target = "1.80000",
            TargetLabel = "Total Subscribers:",
            Progress = 49,
            ProjectType = "Subscription",
            Status = "Open",
            BannerUrl = BaseUrl + "/projects/network-effect-banner.png",
            AvatarUrl = BaseUrl + "/projects/network-effect-logo.jpg"
        },
        new()
        {
            ProjectName = "Nostria (Pre-Seed)",
            ShortDescription = "Early-stage funding round for the Nostria Nostr client development team.",
            InvestorCount = 10,
            InvestorLabel = "Subscribers",
            Raised = "0.54300",
            Target = "0.85000",
            TargetLabel = "Total Subscribers:",
            Progress = 64,
            ProjectType = "Subscription",
            Status = "Funding Closed",
            BannerUrl = BaseUrl + "/projects/nostria-banner.jpg",
            AvatarUrl = BaseUrl + "/projects/nostria-logo.png"
        },
        new()
        {
            ProjectName = "Bloom Fest 25",
            ShortDescription = "Annual Bitcoin community festival celebrating decentralized technology and culture.",
            InvestorCount = 6,
            InvestorLabel = "Investors",
            Raised = "0.31200",
            Target = "0.65000",
            TargetLabel = "Target:",
            Progress = 48,
            ProjectType = "Invest",
            Status = "Open",
            BannerUrl = BaseUrl + "/projects/bloom-fest-banner.jpg",
            AvatarUrl = BaseUrl + "/projects/bloom-fest-logo.jpg"
        },
    };
}

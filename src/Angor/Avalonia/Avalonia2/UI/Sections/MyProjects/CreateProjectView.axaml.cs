using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using Avalonia.VisualTree;
using ReactiveUI;

namespace Avalonia2.UI.Sections.MyProjects;

public partial class CreateProjectView : UserControl
{
    // Stepper elements — resolved by name in AXAML
    private Border[] _stepCircles = [];
    private TextBlock[] _stepNums = [];
    private TextBlock[] _stepLabels = [];
    private Border[] _stepLines = [];
    private Button[] _stepButtons = [];

    // Type card elements — each card has: border, iconBg, iconPath, title, radio
    private Border[] _typeCards = [];
    private Border[] _iconBgs = [];
    private Path[] _iconPaths = [];
    private TextBlock[] _titleTexts = [];
    private Border[] _radioCircles = [];
    private string? _selectedType;

    // ListBox preset controls — Step 4
    private ListBox? _investAmountPresets;
    private ListBox? _fundAmountPresets;
    private ListBox? _subPricePresets;
    private ListBox? _durationPresets;

    // ListBox controls — Step 5 (Investment)
    private ListBox? _investDurationPresets;
    private ListBox? _investFrequencyPresets;

    // ListBox controls — Step 5 (Fund/Subscription)
    private ListBox? _payoutFrequencyList;
    private ListBox? _installmentOptionsList;
    private ListBox? _monthlyDateGrid;
    private ListBox? _weeklyDayList;

    // Stepper colors
    private static readonly Color CompletedGreen = Color.Parse("#2D5A3D");
    private static readonly Color CurrentGreen = Color.Parse("#4B7C5A");
    private static readonly Color InactiveGray = Color.Parse("#888888");
    private static readonly Color LineGreen = Color.Parse("#2D5A3D");

    // Type card accent colors (match reference: Accent, BitcoinAccent, gray)
    private static readonly Color InvestColor = Color.Parse("#4B7C5A");
    private static readonly Color FundColor = Color.Parse("#F7931A");
    private static readonly Color SubscriptionColor = Color.Parse("#9E9E9E");

    // Original icon colors for the unselected state (stroke or fill)
    // Invest: stroke #4B7C5A, Fund: fill #F7931A, Subscription: stroke #9E9E9E
    private static readonly IBrush InvestIconStroke = new SolidColorBrush(InvestColor);
    private static readonly IBrush FundIconFill = new SolidColorBrush(FundColor);
    private static readonly IBrush SubscriptionIconStroke = new SolidColorBrush(SubscriptionColor);

    // Track whether each icon uses Stroke (true) or Fill (false)
    private static readonly bool[] IconUsesStroke = [true, false, true];

    public CreateProjectView()
    {
        InitializeComponent();
        DataContext = new CreateProjectViewModel();

        AddHandler(Button.ClickEvent, OnButtonClick, RoutingStrategies.Bubble);

        // Update stepper visuals whenever the current step changes
        if (DataContext is CreateProjectViewModel vm)
        {
            vm.WhenAnyValue(x => x.CurrentStep)
                .Subscribe(_ => UpdateStepper());

            // Update stepper labels when project type changes (step names change)
            vm.WhenAnyValue(x => x.ProjectType)
                .Subscribe(_ => UpdateStepperLabels());
        }
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        ResolveNamedElements();
        UpdateStepper();
    }

    private void ResolveNamedElements()
    {
        _stepCircles =
        [
            this.FindControl<Border>("StepCircle1")!,
            this.FindControl<Border>("StepCircle2")!,
            this.FindControl<Border>("StepCircle3")!,
            this.FindControl<Border>("StepCircle4")!,
            this.FindControl<Border>("StepCircle5")!,
            this.FindControl<Border>("StepCircle6")!,
        ];
        _stepNums =
        [
            this.FindControl<TextBlock>("StepNum1")!,
            this.FindControl<TextBlock>("StepNum2")!,
            this.FindControl<TextBlock>("StepNum3")!,
            this.FindControl<TextBlock>("StepNum4")!,
            this.FindControl<TextBlock>("StepNum5")!,
            this.FindControl<TextBlock>("StepNum6")!,
        ];
        _stepLabels =
        [
            this.FindControl<TextBlock>("StepLabel1")!,
            this.FindControl<TextBlock>("StepLabel2")!,
            this.FindControl<TextBlock>("StepLabel3")!,
            this.FindControl<TextBlock>("StepLabel4")!,
            this.FindControl<TextBlock>("StepLabel5")!,
            this.FindControl<TextBlock>("StepLabel6")!,
        ];
        _stepLines =
        [
            this.FindControl<Border>("StepLine1")!,
            this.FindControl<Border>("StepLine2")!,
            this.FindControl<Border>("StepLine3")!,
            this.FindControl<Border>("StepLine4")!,
            this.FindControl<Border>("StepLine5")!,
        ];
        _stepButtons =
        [
            this.FindControl<Button>("StepBtn1")!,
            this.FindControl<Button>("StepBtn2")!,
            this.FindControl<Button>("StepBtn3")!,
            this.FindControl<Button>("StepBtn4")!,
            this.FindControl<Button>("StepBtn5")!,
            this.FindControl<Button>("StepBtn6")!,
        ];

        // Type card elements — now Border (not Button) to prevent hover chrome
        _typeCards =
        [
            this.FindControl<Border>("TypeInvestCard")!,
            this.FindControl<Border>("TypeFundCard")!,
            this.FindControl<Border>("TypeSubscriptionCard")!,
        ];
        _iconBgs =
        [
            this.FindControl<Border>("IconBgInvest")!,
            this.FindControl<Border>("IconBgFund")!,
            this.FindControl<Border>("IconBgSubscription")!,
        ];
        _iconPaths =
        [
            this.FindControl<Path>("IconPathInvest")!,
            this.FindControl<Path>("IconPathFund")!,
            this.FindControl<Path>("IconPathSubscription")!,
        ];
        _titleTexts =
        [
            this.FindControl<TextBlock>("TitleInvest")!,
            this.FindControl<TextBlock>("TitleFund")!,
            this.FindControl<TextBlock>("TitleSubscription")!,
        ];
        _radioCircles =
        [
            this.FindControl<Border>("RadioInvest")!,
            this.FindControl<Border>("RadioFund")!,
            this.FindControl<Border>("RadioSubscription")!,
        ];

        // Wire up PointerPressed on type cards (Border, not Button)
        var typeNames = new[] { "investment", "fund", "subscription" };
        for (int i = 0; i < _typeCards.Length; i++)
        {
            var typeName = typeNames[i];
            _typeCards[i].PointerPressed += (_, _) =>
            {
                Vm?.SelectProjectType(typeName);
                HighlightTypeCard(typeName);
            };
        }

        // Resolve ListBox preset controls — Step 4
        _investAmountPresets = this.FindControl<ListBox>("InvestAmountPresets");
        _fundAmountPresets = this.FindControl<ListBox>("FundAmountPresets");
        _subPricePresets = this.FindControl<ListBox>("SubPricePresets");
        _durationPresets = this.FindControl<ListBox>("DurationPresets");

        // Resolve ListBox controls — Step 5 (Investment)
        _investDurationPresets = this.FindControl<ListBox>("InvestDurationPresets");
        _investFrequencyPresets = this.FindControl<ListBox>("InvestFrequencyPresets");

        // Resolve ListBox controls — Step 5 (Fund/Subscription)
        _payoutFrequencyList = this.FindControl<ListBox>("PayoutFrequencyList");
        _installmentOptionsList = this.FindControl<ListBox>("InstallmentOptionsList");
        _monthlyDateGrid = this.FindControl<ListBox>("MonthlyDateGrid");
        _weeklyDayList = this.FindControl<ListBox>("WeeklyDayList");

        // Wire up Step 4 ListBox selection changed handlers
        if (_investAmountPresets != null)
            _investAmountPresets.SelectionChanged += (_, _) => OnAmountPresetSelected(_investAmountPresets);
        if (_fundAmountPresets != null)
            _fundAmountPresets.SelectionChanged += (_, _) => OnAmountPresetSelected(_fundAmountPresets);
        if (_subPricePresets != null)
            _subPricePresets.SelectionChanged += (_, _) => OnSubPricePresetSelected(_subPricePresets);
        if (_durationPresets != null)
            _durationPresets.SelectionChanged += (_, _) => OnDurationPresetSelected(_durationPresets);

        // Wire up Step 5 ListBox selection changed handlers (Investment)
        if (_investDurationPresets != null)
            _investDurationPresets.SelectionChanged += (_, _) => OnInvestDurationPresetSelected();
        if (_investFrequencyPresets != null)
            _investFrequencyPresets.SelectionChanged += (_, _) => OnInvestFrequencySelected();

        // Wire up Step 5 ListBox selection changed handlers (Fund/Subscription)
        if (_payoutFrequencyList != null)
            _payoutFrequencyList.SelectionChanged += (_, _) => OnPayoutFrequencySelected();
        if (_installmentOptionsList != null)
            _installmentOptionsList.SelectionChanged += (_, _) => OnInstallmentOptionsSelected();
        if (_monthlyDateGrid != null)
            _monthlyDateGrid.SelectionChanged += (_, _) => OnMonthlyDateSelected();
        if (_weeklyDayList != null)
            _weeklyDayList.SelectionChanged += (_, _) => OnWeeklyDaySelected();
    }

    private CreateProjectViewModel? Vm => DataContext as CreateProjectViewModel;

    private void OnButtonClick(object? sender, RoutedEventArgs e)
    {
        if (e.Source is not Button btn) return;

        switch (btn.Name)
        {
            case "StartButton":
                Vm?.DismissWelcome();
                UpdateStepper();
                break;
            case "NextStepButton":
                Vm?.GoNext();
                break;
            case "PrevStepButton":
                if (Vm?.CurrentStep == 1)
                    NavigateBackToMyProjects();
                else
                    Vm?.GoBack();
                break;
            case "DeployButton":
                Vm?.Deploy();
                break;
            // Image upload buttons
            case "UploadBannerButton": _ = PickImageAsync(true); break;
            case "UploadAvatarButton": _ = PickImageAsync(false); break;
            // Step 5 buttons
            case "GenerateStagesButton": Vm?.GenerateInvestmentStages(); break;
            case "GeneratePayoutsButton": Vm?.GeneratePayoutSchedule(); break;
            case "DeleteStagesButton": Vm?.ClearStages(); break;
            case "ToggleEditorButton": Vm?.ToggleAdvancedEditor(); break;
            // Stepper step buttons
            case "StepBtn1": Vm?.GoToStep(1); break;
            case "StepBtn2": Vm?.GoToStep(2); break;
            case "StepBtn3": Vm?.GoToStep(3); break;
            case "StepBtn4": Vm?.GoToStep(4); break;
            case "StepBtn5": Vm?.GoToStep(5); break;
            case "StepBtn6": Vm?.GoToStep(6); break;
        }
    }

    #region Stepper

    /// <summary>
    /// Update the vertical stepper circles, lines, and labels based on current step.
    /// </summary>
    private void UpdateStepper()
    {
        if (Vm == null || _stepCircles.Length == 0) return;

        Avalonia.Threading.Dispatcher.UIThread.Post(() =>
        {
            for (int i = 0; i < 6; i++)
            {
                var stepNum = i + 1;
                var circle = _stepCircles[i];
                var num = _stepNums[i];

                if (stepNum < Vm.CurrentStep)
                {
                    // Completed — green filled circle with white checkmark
                    circle.Background = new SolidColorBrush(CompletedGreen);
                    circle.BorderBrush = new SolidColorBrush(CompletedGreen);
                    num.Text = "\u2713";
                    num.Foreground = Brushes.White;
                    _stepLabels[i].Foreground = new SolidColorBrush(CompletedGreen);
                    _stepLabels[i].FontWeight = FontWeight.SemiBold;
                }
                else if (stepNum == Vm.CurrentStep)
                {
                    // Current — green border, transparent fill, green number
                    circle.Background = Brushes.Transparent;
                    circle.BorderBrush = new SolidColorBrush(CurrentGreen);
                    num.Text = stepNum.ToString();
                    num.Foreground = new SolidColorBrush(CurrentGreen);
                    _stepLabels[i].Foreground = this.FindResource("TextStrong") as IBrush
                                                ?? new SolidColorBrush(CurrentGreen);
                    _stepLabels[i].FontWeight = FontWeight.Bold;
                }
                else
                {
                    // Future — gray border, transparent fill, gray number
                    circle.Background = Brushes.Transparent;
                    circle.BorderBrush = new SolidColorBrush(InactiveGray);
                    num.Text = stepNum.ToString();
                    num.Foreground = new SolidColorBrush(InactiveGray);
                    _stepLabels[i].Foreground = this.FindResource("TextMuted") as IBrush
                                                ?? new SolidColorBrush(InactiveGray);
                    _stepLabels[i].FontWeight = FontWeight.Normal;
                }
            }

            // Connecting lines — green when the step above is completed
            var strokeBrush = this.FindResource("Stroke") as IBrush
                              ?? new SolidColorBrush(InactiveGray);
            for (int i = 0; i < 5; i++)
            {
                var lineStepNum = i + 1;
                _stepLines[i].Background = lineStepNum < Vm.CurrentStep
                    ? new SolidColorBrush(LineGreen)
                    : strokeBrush;
            }
        }, Avalonia.Threading.DispatcherPriority.Loaded);
    }

    /// <summary>
    /// Update stepper labels when project type changes (step 4/5 names vary).
    /// </summary>
    private void UpdateStepperLabels()
    {
        if (Vm == null || _stepLabels.Length == 0) return;

        Avalonia.Threading.Dispatcher.UIThread.Post(() =>
        {
            var names = Vm.StepNames;
            for (int i = 0; i < Math.Min(names.Length, _stepLabels.Length); i++)
            {
                _stepLabels[i].Text = names[i];
            }
        }, Avalonia.Threading.DispatcherPriority.Loaded);
    }

    #endregion

    #region Type Cards

    /// <summary>
    /// Set the selected type card and apply styles to all cards.
    /// Reference behaviour:
    ///   Selected: accent-colored 2px border, transparent bg, title turns accent color,
    ///             icon square gets solid accent bg with white icon,
    ///             radio becomes solid accent circle with white checkmark.
    ///   Unselected: SurfaceLow 2px border, Surface bg, TextStrong title,
    ///               icon square has SurfaceLow bg with original colored icon,
    ///               radio has SurfaceLow bg/border (empty).
    ///   No hover effect on either state.
    /// </summary>
    private void HighlightTypeCard(string type)
    {
        _selectedType = type;
        ApplyTypeCardStyles();
    }

    /// <summary>
    /// Apply visual states to all three type cards based on current selection.
    /// </summary>
    private void ApplyTypeCardStyles()
    {
        if (_typeCards.Length == 0) return;

        var types = new[] { "investment", "fund", "subscription" };
        var colors = new[] { InvestColor, FundColor, SubscriptionColor };

        var surfaceLowBrush = this.FindResource("SurfaceLow") as IBrush
                              ?? new SolidColorBrush(Color.Parse("#E5E7EB"));
        var borderBrush = this.FindResource("Border") as IBrush
                          ?? new SolidColorBrush(Color.Parse("#E5E7EB"));
        var textStrongBrush = this.FindResource("TextStrong") as IBrush
                              ?? new SolidColorBrush(Color.Parse("#0A0A0A"));

        for (int i = 0; i < 3; i++)
        {
            var isSelected = types[i] == _selectedType;
            var accentColor = colors[i];
            var accentBrush = new SolidColorBrush(accentColor);

            var card = _typeCards[i];
            var iconBg = _iconBgs[i];
            var iconPath = _iconPaths[i];
            var title = _titleTexts[i];
            var radio = _radioCircles[i];

            if (isSelected)
            {
                // === SELECTED STATE ===
                card.BorderBrush = accentBrush;
                card.BorderThickness = new Thickness(2);
                card.Background = Brushes.Transparent;

                title.Foreground = accentBrush;

                iconBg.Background = accentBrush;
                iconBg.BorderBrush = accentBrush;
                if (IconUsesStroke[i])
                {
                    iconPath.Stroke = Brushes.White;
                    iconPath.Fill = null;
                }
                else
                {
                    iconPath.Fill = Brushes.White;
                    iconPath.Stroke = null;
                }

                radio.Background = accentBrush;
                radio.BorderBrush = accentBrush;
                radio.Child = new Viewbox
                {
                    Width = 12, Height = 12,
                    Child = new Path
                    {
                        Data = StreamGeometry.Parse("M4 12l5 5L20 7"),
                        Stroke = Brushes.White,
                        StrokeThickness = 2.5,
                        StrokeLineCap = PenLineCap.Round,
                        StrokeJoin = PenLineJoin.Round,
                        Width = 24, Height = 24,
                        Stretch = Stretch.Uniform
                    }
                };
            }
            else
            {
                // === UNSELECTED STATE ===
                card.BorderBrush = borderBrush;
                card.BorderThickness = new Thickness(1);
                card.Background = Brushes.Transparent;

                title.Foreground = textStrongBrush;

                iconBg.Background = surfaceLowBrush;
                iconBg.BorderBrush = Brushes.Transparent;
                if (IconUsesStroke[i])
                {
                    iconPath.Stroke = i == 0 ? InvestIconStroke : SubscriptionIconStroke;
                    iconPath.Fill = null;
                }
                else
                {
                    iconPath.Fill = FundIconFill;
                    iconPath.Stroke = null;
                }

                radio.Background = surfaceLowBrush;
                radio.BorderBrush = surfaceLowBrush;
                radio.Child = null;
            }
        }
    }

    #endregion

    #region ListBox Preset Handlers

    /// <summary>
    /// Handle amount preset selection (Investment target amount or Fund goal).
    /// Reads the Tag from the selected ListBoxItem and sets TargetAmount on the VM.
    /// </summary>
    private void OnAmountPresetSelected(ListBox lb)
    {
        if (lb.SelectedItem is not ListBoxItem item) return;
        if (item.Tag is string tag && Vm != null)
        {
            Vm.TargetAmount = tag;
        }
    }

    /// <summary>
    /// Handle subscription price preset selection.
    /// Reads the Tag from the selected ListBoxItem and sets SubscriptionPrice on the VM.
    /// </summary>
    private void OnSubPricePresetSelected(ListBox lb)
    {
        if (lb.SelectedItem is not ListBoxItem item) return;
        if (item.Tag is string tag && Vm != null)
        {
            Vm.SubscriptionPrice = tag;
        }
    }

    /// <summary>
    /// Handle duration preset selection (Investment end date).
    /// Reads the Tag (months as string) and sets the end date relative to now.
    /// </summary>
    private void OnDurationPresetSelected(ListBox lb)
    {
        if (lb.SelectedItem is not ListBoxItem item) return;
        if (item.Tag is string tag && int.TryParse(tag, out var months) && Vm != null)
        {
            Vm.InvestEndDate = DateTime.Now.AddMonths(months);
        }
    }

    /// <summary>
    /// Handle Investment duration preset selection on Step 5.
    /// Sets DurationPreset and syncs DurationValue/DurationUnit on the VM.
    /// </summary>
    private void OnInvestDurationPresetSelected()
    {
        if (_investDurationPresets?.SelectedItem is not ListBoxItem item) return;
        if (item.Tag is string tag && int.TryParse(tag, out var months) && Vm != null)
        {
            Vm.DurationPreset = months;
        }
    }

    /// <summary>
    /// Handle Investment release frequency selection on Step 5.
    /// Sets ReleaseFrequency on the VM.
    /// </summary>
    private void OnInvestFrequencySelected()
    {
        if (_investFrequencyPresets?.SelectedItem is not ListBoxItem item) return;
        if (item.Tag is string tag && Vm != null)
        {
            Vm.ReleaseFrequency = tag;
        }
    }

    /// <summary>
    /// Handle Fund/Sub payout frequency selection on Step 5.
    /// Sets PayoutFrequency ("Monthly" or "Weekly") on the VM.
    /// </summary>
    private void OnPayoutFrequencySelected()
    {
        if (_payoutFrequencyList?.SelectedItem is not ListBoxItem item) return;
        if (item.Tag is string tag && Vm != null)
        {
            Vm.PayoutFrequency = tag;
        }
    }

    /// <summary>
    /// Handle Fund/Sub installment count selection on Step 5.
    /// Sets SelectedInstallments (3, 6, or 9) on the VM.
    /// </summary>
    private void OnInstallmentOptionsSelected()
    {
        if (_installmentOptionsList?.SelectedItem is not ListBoxItem item) return;
        if (item.Tag is string tag && int.TryParse(tag, out var count) && Vm != null)
        {
            Vm.SelectedInstallments = count;
        }
    }

    /// <summary>
    /// Handle Fund/Sub monthly payout date selection on Step 5.
    /// Sets MonthlyPayoutDate (1-29) on the VM.
    /// </summary>
    private void OnMonthlyDateSelected()
    {
        if (_monthlyDateGrid?.SelectedItem is not ListBoxItem item) return;
        if (item.Tag is string tag && int.TryParse(tag, out var day) && Vm != null)
        {
            Vm.MonthlyPayoutDate = day;
        }
    }

    /// <summary>
    /// Handle Fund/Sub weekly payout day selection on Step 5.
    /// Sets WeeklyPayoutDay ("Mon".."Sun") on the VM.
    /// </summary>
    private void OnWeeklyDaySelected()
    {
        if (_weeklyDayList?.SelectedItem is not ListBoxItem item) return;
        if (item.Tag is string tag && Vm != null)
        {
            Vm.WeeklyPayoutDay = tag;
        }
    }

    #endregion

    #region Image Picker

    /// <summary>
    /// Open a file picker dialog to select an image for banner or avatar.
    /// </summary>
    private async Task PickImageAsync(bool isBanner)
    {
        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel == null) return;

        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = isBanner ? "Select Banner Image" : "Select Profile Image",
            AllowMultiple = false,
            FileTypeFilter = new[]
            {
                new FilePickerFileType("Images") { Patterns = new[] { "*.png", "*.jpg", "*.jpeg", "*.webp", "*.bmp" } }
            }
        });

        if (files.Count == 0) return;

        var file = files[0];
        try
        {
            await using var stream = await file.OpenReadAsync();
            var bitmap = new Bitmap(stream);

            if (isBanner)
            {
                var bannerImage = this.FindControl<Image>("BannerPreviewImage");
                if (bannerImage != null)
                {
                    bannerImage.Source = bitmap;
                    bannerImage.IsVisible = true;
                }
            }
            else
            {
                var avatarImage = this.FindControl<Image>("AvatarPreviewImage");
                var avatarIcon = this.FindControl<Viewbox>("AvatarUploadIcon");
                if (avatarImage != null)
                {
                    avatarImage.Source = bitmap;
                    avatarImage.IsVisible = true;
                }
                if (avatarIcon != null)
                {
                    avatarIcon.IsVisible = false;
                }
            }
        }
        catch
        {
            // File read error — silently ignore for prototype
        }
    }

    #endregion

    private void NavigateBackToMyProjects()
    {
        var myProjectsView = this.FindAncestorOfType<MyProjectsView>();
        if (myProjectsView?.DataContext is MyProjectsViewModel myVm)
        {
            myVm.ShowCreateWizard = false;
        }
    }
}

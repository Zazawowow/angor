namespace AngorApp.UI.Sections.Funders;

public class FundersSectionViewModelSample : IFundersSectionViewModel
{
    public string Message { get; set; } = "Funders section";

    public IEnumerable<IFundersGroup> Groups { get; } =
    [
        new FunderGroupSample()
        {
            Name = "Awaiting Approval",
            Funders =
            [
                new FunderItemSample() { Amount = new AmountUI(10000), Status = FunderStatus.Pending },
                new FunderItemSample() { Amount = new AmountUI(10000), Status = FunderStatus.Pending },
            ]
        },
        new FunderGroupSample()
        {
            Name = "Approved",
            Funders =
            [
                new FunderItemSample() { Amount = new AmountUI(10000), Status = FunderStatus.Approved },
                new FunderItemSample() { Amount = new AmountUI(10000), Status = FunderStatus.Approved },
            ]
        },
        new FunderGroupSample()
        {
            Name = "Rejected",
            Funders =
            [
                new FunderItemSample() { Amount = new AmountUI(10000), Status = FunderStatus.Rejected },
            ]
        },
    ];

    public IEnhancedCommand Load { get; }
    public IObservable<bool> IsEmpty { get; set; } = Observable.Return(false);
}
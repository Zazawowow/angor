namespace AngorApp.UI.Sections.Funders
{
    public class FunderGroupSample : IFundersGroup
    {
        public string Name { get; set; } = "Default";

        public IReadOnlyCollection<IFunderItem> Funders { get; set; } =
        [
            new FunderItemSample() { Amount = new AmountUI(10000)},
            new FunderItemSample(),
            new FunderItemSample(),
        ];
    }
}
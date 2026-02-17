namespace AngorApp.UI.Sections.Funders
{
    public interface IFundersGroup
    {
        public string Name { get; }
        public IReadOnlyCollection<IFunderItem> Funders { get; }
    }
}
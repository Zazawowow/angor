using Zafiro.CSharpFunctionalExtensions;

namespace AngorApp.UI.Sections.Funders;

public interface IFundersSectionViewModel
{
    public IEnumerable<IFundersGroup> Groups { get; }
    public IEnhancedCommand Load { get; }
    public IObservable<bool> IsEmpty { get; }
}
using Avalonia;
using Avalonia.Controls;

namespace Avalonia2.UI.Shared.Controls;

/// <summary>
/// A responsive grid panel that auto-calculates column count based on available width.
/// Emulates CSS grid: grid-template-columns: repeat(auto-fill, minmax(MinItemWidth, 1fr)).
/// Each item stretches equally to fill the row.
/// </summary>
public class ResponsiveGrid : Panel
{
    public static readonly StyledProperty<double> MinItemWidthProperty =
        AvaloniaProperty.Register<ResponsiveGrid, double>(nameof(MinItemWidth), 300);

    public static readonly StyledProperty<double> ColumnSpacingProperty =
        AvaloniaProperty.Register<ResponsiveGrid, double>(nameof(ColumnSpacing), 20);

    public static readonly StyledProperty<double> RowSpacingProperty =
        AvaloniaProperty.Register<ResponsiveGrid, double>(nameof(RowSpacing), 20);

    /// <summary>
    /// Minimum width of each item. The panel will fit as many columns as possible
    /// where each column is at least this wide.
    /// </summary>
    public double MinItemWidth
    {
        get => GetValue(MinItemWidthProperty);
        set => SetValue(MinItemWidthProperty, value);
    }

    public double ColumnSpacing
    {
        get => GetValue(ColumnSpacingProperty);
        set => SetValue(ColumnSpacingProperty, value);
    }

    public double RowSpacing
    {
        get => GetValue(RowSpacingProperty);
        set => SetValue(RowSpacingProperty, value);
    }

    private int GetColumnCount(double availableWidth)
    {
        if (availableWidth <= 0 || MinItemWidth <= 0)
            return 1;

        // How many columns of MinItemWidth fit? (accounting for gaps between them)
        var cols = (int)((availableWidth + ColumnSpacing) / (MinItemWidth + ColumnSpacing));
        return Math.Max(1, cols);
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        var columnCount = GetColumnCount(availableSize.Width);
        var itemWidth = (availableSize.Width - (columnCount - 1) * ColumnSpacing) / columnCount;
        itemWidth = Math.Max(itemWidth, 0);

        var visibleChildren = Children.Where(c => c.IsVisible).ToList();
        var rowCount = visibleChildren.Count > 0
            ? (int)Math.Ceiling((double)visibleChildren.Count / columnCount)
            : 0;

        double maxRowHeight = 0;
        double totalHeight = 0;

        for (var i = 0; i < visibleChildren.Count; i++)
        {
            visibleChildren[i].Measure(new Size(itemWidth, double.PositiveInfinity));
            maxRowHeight = Math.Max(maxRowHeight, visibleChildren[i].DesiredSize.Height);

            // End of row or end of items
            if ((i + 1) % columnCount == 0 || i == visibleChildren.Count - 1)
            {
                totalHeight += maxRowHeight;
                if (i < visibleChildren.Count - 1)
                    totalHeight += RowSpacing;
                maxRowHeight = 0;
            }
        }

        return new Size(availableSize.Width, totalHeight);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        var columnCount = GetColumnCount(finalSize.Width);
        var itemWidth = (finalSize.Width - (columnCount - 1) * ColumnSpacing) / columnCount;
        itemWidth = Math.Max(itemWidth, 0);

        var visibleChildren = Children.Where(c => c.IsVisible).ToList();
        double y = 0;
        double maxRowHeight = 0;

        for (var i = 0; i < visibleChildren.Count; i++)
        {
            var col = i % columnCount;
            var x = col * (itemWidth + ColumnSpacing);

            var child = visibleChildren[i];
            child.Arrange(new Rect(x, y, itemWidth, child.DesiredSize.Height));
            maxRowHeight = Math.Max(maxRowHeight, child.DesiredSize.Height);

            // End of row
            if ((i + 1) % columnCount == 0)
            {
                y += maxRowHeight + RowSpacing;
                maxRowHeight = 0;
            }
        }

        // Final row height
        if (visibleChildren.Count % columnCount != 0)
            y += maxRowHeight;

        return new Size(finalSize.Width, y);
    }
}

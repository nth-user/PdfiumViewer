using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PdfiumViewer.Core
{
    /// <summary>
    /// Represents a marker on a PDF page.
    /// </summary>
    public interface IPdfMarker
    {
        /// <summary>
        /// The page where the marker is drawn on.
        /// </summary>
        int Page { get; }

        /// <summary>
        /// Draw the marker.
        /// </summary>
        /// <param name="frame">The PdfFrame to draw the marker onto.</param>
        IEnumerable<UIElement> Draw(PdfFrame frame);
    }
}

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PdfiumViewer.Core
{
    public class PdfMarker : IPdfMarker
    {
        public int Page { get; }
        public Point[] Bounds { get; }
        public Brush Fill { get; }
        public Brush Stroke { get; }
        public double StrokeThickness { get; }

        public event PdfMarkerClickedEventHandler Clicked;

        public PdfMarker(int page, Point[] bounds, Brush fill) : this(page, bounds, fill, Brushes.Transparent, 0)
        {
        }

        public PdfMarker(int page, Point[] bounds, Brush fill, Brush stroke, double strokeThickness)
        {
            Page = page;
            Bounds = bounds;
            Fill = fill;
            Stroke = stroke;
            StrokeThickness = strokeThickness;
        }

        public IEnumerable<UIElement> Draw(PdfFrame frame)
        {
            if (frame == null)
                throw new ArgumentNullException(nameof(frame));

            var rect = frame.BoundsToFrame(Bounds);
            var rectangle = new Rectangle
            {
                Width = rect.Width,
                Height = rect.Height,
                Fill = Fill,
                Stroke = Stroke,
                StrokeThickness = StrokeThickness,
            };
            Canvas.SetTop(rectangle, rect.Top);
            Canvas.SetLeft(rectangle, rect.Left);
            if(Clicked != null)
            {
                rectangle.Cursor = Cursors.Arrow;
                rectangle.MouseDown += Rectangle_MouseDown;
                rectangle.MouseEnter += Rectangle_MouseEnter;
                rectangle.MouseLeave += Rectangle_MouseLeave;
            }
            return new[] { rectangle };
        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Clicked?.Invoke(this, e);
        }

        private static void Rectangle_MouseLeave(object sender, MouseEventArgs e)
        {
            var rectangle = (Rectangle)sender;
            rectangle.StrokeThickness -= 2;
        }

        private static void Rectangle_MouseEnter(object sender, MouseEventArgs e)
        {
            var rectangle = (Rectangle)sender;
            rectangle.StrokeThickness += 2;
        }
        public delegate void PdfMarkerClickedEventHandler(object sender, MouseEventArgs e);
    }
}

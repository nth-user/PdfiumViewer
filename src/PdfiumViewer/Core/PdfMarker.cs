﻿using System;
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
        public Rectangle Rectangle { get; }
        public object Tag { get; set; }

        public PdfMarker(int page, Point[] bounds, Brush fill, string tooltip = null) : this(page, bounds, fill, Brushes.Transparent, 0, tooltip)
        {
        }

        public PdfMarker(int page, Point[] bounds, Brush fill, Brush stroke, double strokeThickness, string tooltip = null)
        {
            Page = page;
            Bounds = bounds;
            Rectangle = new Rectangle
            {
                Fill = fill,
                Stroke = stroke,
                StrokeThickness = strokeThickness,
                ToolTip = tooltip,
            };
        }

        public IEnumerable<FrameworkElement> Draw(PdfFrame frame)
        {
            if (frame == null)
                throw new ArgumentNullException(nameof(frame));

            var rect = frame.BoundsToFrame(Bounds);
            Rectangle.Width = rect.Width;
            Rectangle.Height = rect.Height;
            Canvas.SetLeft(Rectangle, rect.Left);
            Canvas.SetTop(Rectangle, rect.Top);
            return new[] { Rectangle };
        }
        public double GetDistance(Point pt)
        {
            var center = new Point(Canvas.GetLeft(Rectangle) + Rectangle.Width / 2, Canvas.GetTop(Rectangle) + Rectangle.Height / 2);
            return Point.Subtract(center, pt).Length;
        }
    }
}

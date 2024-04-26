using PdfiumViewer.Enums;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PdfiumViewer.Core
{
    /// <summary>
    /// Interaktionslogik für PdfPage.xaml
    /// </summary>
    public partial class PdfFrame : UserControl
    {
        public PdfDocument Document { get; }
        public int PageIndex { get; private set; }
        public bool IsRendered { get; private set; }

        public IReadOnlyList<PdfPageLink> Links { get; private set; }
        private PdfFrame()
        {
            InitializeComponent();
        }

        public PdfFrame(PdfDocument document) : this()
        {
            Document = document;
        }
        public bool SetPage(int i)
        {
            if(PageIndex != i)
            {
                PageIndex = i;
                Links = Document.GetPageLinks(i);
                return true;
            }
            return false;
        }

        public BitmapImage Render(int dpi, PdfRotation rotation, PdfRenderFlags flags)
        {
            var bitmap = Document.Render(PageIndex, (int)Width, (int)Height, dpi, dpi, rotation, flags);
            BitmapImage bitmapImage;
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;
                bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad; // not a mistake - see below
                bitmapImage.EndInit();
                bitmap.Dispose();
            }
            // Why BitmapCacheOption.OnLoad?
            // It seems counter intuitive, but this flag has two effects:
            // It enables caching if caching is possible, and it causes the load to happen at EndInit().
            // In our case caching is impossible, so all it does it cause the load to happen immediately.

            Dispatcher.Invoke(() =>
            {
                image.Source = bitmapImage;
            });
            IsRendered = true;
            return bitmapImage;
        }

        public void Clear()
        {
            image.Source = null;
            canvas.Children.Clear();
            IsRendered = false;
        }
    }
}

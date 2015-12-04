using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ECalc.Docs
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class DocumentationViewer : UserControl
    {
        private MarkdownSharp.Markdown _markdown;
        private string _template;

        public DocumentationViewer()
        {
            InitializeComponent();
            _markdown = new MarkdownSharp.Markdown();
            Uri uri = new Uri("/ECalc.Docs;component/Documentation/template.html", UriKind.Relative);
            using (StreamReader sr = new StreamReader(Application.GetResourceStream(uri).Stream))
            {
                _template = sr.ReadToEnd();
            }
        }

        private void TvTOC_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                TreeViewItem selected = (TreeViewItem)TvTOC.SelectedItem;
                var file = selected.ToolTip.ToString();
                Uri uri = new Uri("/ECalc.Docs;component/Documentation/" + file, UriKind.Relative);
                using (StreamReader sr = new StreamReader(Application.GetResourceStream(uri).Stream))
                {
                    StringBuilder PageContent = new StringBuilder();
                    PageContent.Append(_template);
                    PageContent.Append(_markdown.Transform(sr.ReadToEnd()));
                    PageContent.Append("</div></body></html>");
                    DocPanel.Text = PageContent.ToString();
                }
            }
            catch (Exception) { }
        }
    }
}

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
            var uri = new Uri("/ECalc.Docs;component/Documentation/template.html", UriKind.Relative);
            using (StreamReader sr = new StreamReader(Application.GetResourceStream(uri).Stream))
            {
                _template = sr.ReadToEnd();
            }
        }

        private void LoadMarkDown(string file)
        {
            var uri = new Uri("/ECalc.Docs;component/Documentation/" + file, UriKind.Relative);
            using (StreamReader sr = new StreamReader(Application.GetResourceStream(uri).Stream))
            {
                var PageContent = new StringBuilder();
                PageContent.Append(_template);
                PageContent.Append(_markdown.Transform(sr.ReadToEnd()));
                PageContent.Append("</div></body></html>");
                DocPanel.Text = PageContent.ToString();
            }
        }

        private void TvTOC_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var selected = (TreeViewItem)TvTOC.SelectedItem;
            if (selected.ToolTip == null) return;
            var file = selected.ToolTip.ToString();
            if (string.IsNullOrEmpty(file)) return;
            LoadMarkDown(file);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadMarkDown("welcome.md");
        }
    }
}

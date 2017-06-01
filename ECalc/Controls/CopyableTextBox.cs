using AppLib.Common.MessageHandler;
using ECalc.Classes;
using System.Windows;
using System.Windows.Controls;

namespace ECalc.Controls
{
    public class CopyableTextBox : TextBox
    {
        private Button _CopyButton;

        static CopyableTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CopyableTextBox), new FrameworkPropertyMetadata(typeof(CopyableTextBox)));
        }

        public override void OnApplyTemplate()
        {
            _CopyButton = GetTemplateChild("PART_CopyButton") as Button;
            if (_CopyButton != null)
                _CopyButton.Click += _CopyButton_Click;

            base.OnApplyTemplate();
        }

        private void _CopyButton_Click(object sender, RoutedEventArgs e)
        {
            MessageSender.Instance.SendMessage(new CopyPasteData(Text));
        }
    }
}

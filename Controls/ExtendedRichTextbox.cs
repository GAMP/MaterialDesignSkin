using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace MaterialDesignSkin.Controls
{
    public class ExtendedRichTextbox : RichTextBox
    {
        public ExtendedRichTextbox()
        {
        }

        public static readonly DependencyProperty DocumentProperty =
            DependencyProperty.Register("Document", typeof(FlowDocument),
            typeof(ExtendedRichTextbox), new FrameworkPropertyMetadata
            (null, new PropertyChangedCallback(OnDocumentChanged)));

        public new FlowDocument Document
        {
            get
            {
                return (FlowDocument)this.GetValue(DocumentProperty);
            }

            set
            {
                this.SetValue(DocumentProperty, value);
            }
        }

        public static void OnDocumentChanged(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            if (obj is RichTextBox rtb)
            {
                rtb.Document =args.NewValue as FlowDocument;
            }
        }
    }
}

using DevTools.Models;

namespace DevTools.Views
{
    public partial class PopupOverlayWindow : OverlayBaseWindow
    {
        public PopupOverlayWindow(string text)
        {
            InitializeComponent();
            Message.Text = text;
        }
    }
}

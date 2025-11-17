using System.Windows;

namespace WpfAppPrac5
{
    public partial class InputBox : Window
    {
        public string Value => Input.Text;

        public InputBox(string message, string defaultValue = "")
        {
            InitializeComponent();
            Prompt.Text = message;
            Input.Text = defaultValue;
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}

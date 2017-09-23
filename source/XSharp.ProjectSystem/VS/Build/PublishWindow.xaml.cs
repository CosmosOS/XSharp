using System;
using System.Collections.Generic;
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
using Microsoft.VisualStudio.PlatformUI;

namespace XSharp.ProjectSystem.VS.Build
{
    /// <summary>
    /// Interaction logic for PublishWindow.xaml
    /// </summary>
    public partial class PublishWindow : DialogWindow
    {
        public PublishWindow()
        {
            InitializeComponent();
        }

        public PublishWindow(string aHelpTopic) : base(aHelpTopic)
        {
            InitializeComponent();
        }
    }
}

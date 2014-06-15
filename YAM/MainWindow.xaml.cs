using System.Collections.Generic;
using System.Windows;

namespace YAM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataContext dc;

        public MainWindow()
        {
            InitializeComponent();

            dc = (YAM.DataContext)this.DataContext;

            dc.ChildPlayer = ucPlayer.GetUserControl();

            ucPlayer.TriggerUpdateTitleEntry += TriggerUpdateTitleEntry;
        }

        private void TriggerUpdateTitleEntry(YAM_Player.Playlist valueChange)
        {
            dc.UpdateTitleEntry(valueChange);
        }
    }
}

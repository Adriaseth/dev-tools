using DevTools.Enums;
using DevTools.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace DevTools.Views
{
    public partial class TimerView : UserControl
    {
        public TimerView(TimerViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }

        private void btnStartStopTimer_Click(object sender, RoutedEventArgs e)
        {
            ((TimerViewModel)DataContext).StartStopTimer();
        }

        private void btnStandAdd_Click(object sender, RoutedEventArgs e)
        {
            ((TimerViewModel)DataContext).AddOrSubtractFromTimer(TimerAction.Add, TimerModel.Stand, 1);
        }

        private void btnStandSubtract_Click(object sender, RoutedEventArgs e)
        {
            ((TimerViewModel)DataContext).AddOrSubtractFromTimer(TimerAction.Subtract, TimerModel.Stand, 1);
        }

        private void btnSitAdd_Click(object sender, RoutedEventArgs e)
        {
            ((TimerViewModel)DataContext).AddOrSubtractFromTimer(TimerAction.Add, TimerModel.Sit, 1);
        }

        private void btnSitSubtract_Click(object sender, RoutedEventArgs e)
        {
            ((TimerViewModel)DataContext).AddOrSubtractFromTimer(TimerAction.Subtract, TimerModel.Sit, 1);
        }
    }
}

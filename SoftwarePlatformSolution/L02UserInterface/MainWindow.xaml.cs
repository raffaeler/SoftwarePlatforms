using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace L02UserInterface;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        cbMax.Items.Add(10000);
        cbMax.Items.Add(50000);
        cbMax.Items.Add(100000);
        cbMax.SelectedValue = 100000;
    }

    private int Tid() => Thread.CurrentThread.ManagedThreadId;

    private void btPrimesSync_Click(object sender, RoutedEventArgs e)
    {
        var max = (int)cbMax.SelectedItem;

        icContainer.Items.Clear();
        icContainer.Items.Add($"Main thread id: {Tid()}");
        icContainer.Items.Add($"Sync computing the max prime in {max}");

        var lastPrime = ComputeLastPrimeNumber(max);
        icContainer.Items.Add(lastPrime);
    }

    private async void btPrimesAsync_Click(object sender, RoutedEventArgs e)
    {
        var max = (int)cbMax.SelectedItem;
    
        icContainer.Items.Clear();
        icContainer.Items.Add($"Main thread id: {Tid()}");
        icContainer.Items.Add($"Async computing the max prime in {max}");

        var lastPrime = await Task.Run(() =>
        {
            return ComputeLastPrimeNumber(max);
        });

        icContainer.Items.Add(lastPrime);
    }

    private long ComputeLastPrimeNumber(int max)
    {
        var tid = Tid();    // the thread id we are executing on

        Action update = () => icContainer.Items.Add($"Compute thread id: {tid}");

        if (Dispatcher.Thread.ManagedThreadId != tid)
            Dispatcher.Invoke(update);
        else
            update();
        
        var primes = new Primes(max);
        return primes.Last();
    }

}

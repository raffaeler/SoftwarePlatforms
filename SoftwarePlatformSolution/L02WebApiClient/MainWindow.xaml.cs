using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace L02WebApiClient;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private DispatcherTimer _timer = new DispatcherTimer();
    private bool _isRising = true;

    public MainWindow()
    {
        InitializeComponent();

        cbApi.Items.Add(new UserAction("Get Todos", GetTodos));
        cbApi.Items.Add(new UserAction("Get Todo", GetTodo, spTitle));
        cbApi.Items.Add(new UserAction("Post Todo", PostTodo, spTitle, spText));
        cbApi.SelectionChanged += CbApi_SelectionChanged;
        cbApi.SelectedIndex = 0;

        _timer.Interval = TimeSpan.FromMilliseconds(10);
        _timer.Tick += Timer_Tick;
        _timer.Start();

        this.Title = $"Process Id: {Process.GetCurrentProcess().Id}";
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        if(_isRising)
            progress.Value++;
        else
            progress.Value--;

        if (progress.Value == 1000 || progress.Value == 0) _isRising = !_isRising;
    }

    private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Return)
        {
            MakeCall();
        }
    }

    private void CbApi_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var userAction = cbApi.SelectedValue as UserAction;
        if (userAction == null) return;


        spTitle.Visibility = Visibility.Collapsed;
        spText.Visibility = Visibility.Collapsed;
        foreach (var ctl in userAction.Controls)
        {
            ctl.Visibility = Visibility.Visible;
        }
    }

    private void btMakeCall_Click(object sender, RoutedEventArgs e)
    {
        MakeCall();
    }

    private async void MakeCall()
    {
        icContainer.Items.Clear();

        switch (cbApi.SelectedIndex)
        {
            case 0:
                await GetTodos();
                break;

            case 1:
                await GetTodo(tbTitle.Text);
                break;

            case 2:
                await PostTodo(tbTitle.Text, tbText.Text);
                break;
        }
    }

    private async Task GetTodos()
    {
        icContainer.Items.Clear();
        try
        {
            WebClient client = new();
            var todos = await client.MakeGetAPI("/Todos");
            message.Text = "MakeGetAPI completed";
            foreach (var todo in todos)
            {
                icContainer.Items.Add(todo);
            }
        }
        catch (Exception)
        {
            icContainer.Items.Add($"Error while posting TODO");
        }
    }

    private async Task GetTodo(string title)
    {
        icContainer.Items.Clear();
        try
        {
            WebClient client = new();
            var todos = await client.MakeGetAPI($"/Todos/{title}");
            foreach (var todo in todos)
            {
                icContainer.Items.Add(todo);
            }
        }
        catch (Exception)
        {
            icContainer.Items.Add($"Error while getting TODO");
        }
    }

    private async Task PostTodo(string title, string text)
    {
        icContainer.Items.Clear();
        try
        {
            WebClient client = new();
            var todos = await client.MakePostAPI($"/Todos", title, text);
            icContainer.Items.Add($"Todo posted Ok");
        }
        catch (Exception)
        {
            icContainer.Items.Add($"Error while posting TODO");
        }
    }

}

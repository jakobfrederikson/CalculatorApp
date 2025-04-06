using System;
using System.Diagnostics;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace CalculatorApp.Views;

public partial class MainWindow : Window
{
    private double _numberOne;
    private double _numberTwo;
    private bool _clearedForNumberTwo;
    private string _operation = string.Empty;
    private Button? _operationButton;
    private bool _onNextNumber;
    
    public MainWindow()
    {
        InitializeComponent();
        this.KeyDown += OnKeyDown;
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.D0:
            case Key.NumPad0:
                ButtonNumberZero.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                break;
            case Key.D1:
            case Key.NumPad1:
                ButtonNumberOne.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                break;
            case Key.D2:
            case Key.NumPad2:
                ButtonNumberTwo.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                break;
            case Key.D3:
            case Key.NumPad3:
                ButtonNumberThree.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                break;
            case Key.D4:
            case Key.NumPad4:
                ButtonNumberFour.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                break;
            case Key.D5:
            case Key.NumPad5:
                ButtonNumberFive.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                break;
            case Key.D6:
            case Key.NumPad6:
                ButtonNumberSix.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                break;
            case Key.D7:
            case Key.NumPad7:
                ButtonNumberSeven.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                break;
            case Key.D8:
            case Key.NumPad8:
                ButtonNumberEight.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                break;
            case Key.D9:
            case Key.NumPad9:
                ButtonNumberNine.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                break;
        }
    }

    private void IntegerButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button button) return;
        
        var buttonText = button.Content?.ToString();

        if (_onNextNumber && !_clearedForNumberTwo)
        {
            Output.Text = buttonText;
            _clearedForNumberTwo = true;
            _numberTwo = Convert.ToDouble(Output.Text);

            return;
        }
        
        if (Output.Text == "0") Output.Text = buttonText;
        else if (Output.Text?.Length < 9) Output.Text += buttonText;
        else if (Output.Text?.Length == 9 &&
                 Output.Text?.LastOrDefault() == '.')
        {
            Output.Text += buttonText;
        }

        if (!_onNextNumber)
        {
            _numberOne = Convert.ToDouble(Output.Text);
        }
        else
        {
            _numberTwo = Convert.ToDouble(Output.Text);
        }
    }

    private void OperationButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button button) return;

        if (_operationButton != button)
        {
            if (_operationButton != null)
            {
                _operationButton.Background = new SolidColorBrush(Colors.Orange);    
            }
            _operationButton = button;
            _operationButton.Background = new SolidColorBrush(Colors.Chocolate);
            _operation = _operationButton.Content?.ToString();
            
            _onNextNumber = true;
        }
        else
        {
            // If the same operation button is clicked and no number entered,
            // then assume user wants to re-edit numberOne.
            if (_onNextNumber && _clearedForNumberTwo) return;
            _operationButton.Background = new SolidColorBrush(Colors.Orange);
            _operationButton = null;
            _operation = string.Empty;
            _onNextNumber = false;
        }
    }

    private void ACButton_OnClick(object? sender, RoutedEventArgs e)
    {
        Output.Text = "0";
        _numberOne = 0;
        _numberTwo = 0;
        _operation = string.Empty;
        _operationButton = null;
        _onNextNumber = false;
        _clearedForNumberTwo = false;
    }

    private void PlusMinusButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (!_onNextNumber)
        {
            _numberOne *= -1;
            Output.Text = _numberOne.ToString();
        }
        else
        {
            _numberTwo *= -1;
            Output.Text = _numberTwo.ToString();
        }
    }

    private void EqualsButton_OnClick(object? sender, RoutedEventArgs e)
    {
        switch (_operation.ToLower())
        {
            case "+": _numberOne += _numberTwo; break;
            case "-": _numberOne -= _numberTwo; break;
            case "x": _numberOne *= _numberTwo; break;
            case "/": _numberOne /= _numberTwo; break;
            case "%": _numberOne %= _numberTwo; break;
        }
        _numberOne = Math.Round(_numberOne, 3);
        Output.Text = _numberOne.ToString();
        _onNextNumber = false;
        _clearedForNumberTwo = false;

        if (_operationButton == null) return;
        _operationButton.Background = new SolidColorBrush(Colors.Orange);
        _operationButton = null;
    }

    private void DecimalButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (_onNextNumber && !_clearedForNumberTwo)
        {
            Output.Text = "0.";
            _clearedForNumberTwo = true;
        }
        
        if (Output.Text != null && Output.Text.Contains('.')) return;
        
        if (Output.Text?.Length < 9)
        {
            Output.Text += ".";
        }
    }
}
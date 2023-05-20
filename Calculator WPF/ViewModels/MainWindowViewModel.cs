using Calculator_WPF.Commands;
using Calculator_WPF.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static System.Math;

namespace Calculator_WPF.ViewModels
{
    class MainWindowViewModel : ViewModel
    {
        double first = 0, second = 0, result = 0;
        string op = "";

        #region Properties

        #region Input text
        public string _input = "0";

        public string Input
        {
            get => _input;
            set => Set(ref _input, value);
        }
        #endregion

        #region History text
        public string _history = "";

        public string History
        {
            get => _history;
            set => Set(ref _history, value);
        }
        #endregion

        #endregion

        #region Commands

        #region CloseCommand
        public ICommand CloseCommand { get; }

        private bool OnCloseCommandExecute(object p) => true;
        private void OnCloseCommandExecuted(object p)
        {
            System.Windows.Application.Current.Shutdown();
        }
        #endregion

        #region MinimizeCommand
        public ICommand MinimizeCommand { get; }

        private bool OnMinimizeCommandExecute(object p) => true;
        private void OnMinimizeCommandExecuted(object p)
        {
            Application.Current.Windows[0].WindowState = WindowState.Minimized;
        }
        #endregion

        #region NumberClickCommand
        public ICommand NumberClickCommand { get; }

        private bool OnNumberClickCommandExecute(object p) => true;
        private void OnNumberClickCommandExecuted(object p)
        {
            string inp = (string)p;

            if (Input == "0" || Input == "-0" || Input == "Error")
                Input = "";

            if (History.Contains('='))
            {
                History = "";
                op = "";
            }

            if (Input.Contains('.') && Input.Substring(Input.IndexOf('.') + 1).Length >= 3)
                return;

            if (Input?.Length < 9 && (double.Parse(Input + inp, NumberStyles.Any, CultureInfo.InvariantCulture) <= 500000
                && double.Parse(Input + inp, NumberStyles.Any, CultureInfo.InvariantCulture) >= -300000))
            {
                Input += inp;
            }
        }
        #endregion

        #region DotClickCommand
        public ICommand DotClickCommand { get; }

        private bool OnDotClickCommandExecute(object p) => true;
        private void OnDotClickCommandExecuted(object p)
        {
            if (Input.IndexOf('.') > 0 || Input == "Error")
                return;

            if (Input == "")
                Input += "0.";

            double res;
            if (double.TryParse(Input, NumberStyles.Any, CultureInfo.InvariantCulture, out res) && res < 500000 && res > -300000)
                Input += ".";

            if (History.Contains('='))
            {
                History = "";
                op = "";
            }
        }
        #endregion

        #region ChangeSignCommand
        public ICommand ChangeSignCommand { get; }

        private bool OnChangeSignCommandExecute(object p) => true;
        private void OnChangeSignCommandExecuted(object p)
        {
            if (Input == "Error")
                return;

            double res;
            if (double.TryParse(Input, NumberStyles.Any, CultureInfo.InvariantCulture, out res))
            {
                res = Round(-res, 3);
                Input = res.ToString().Length < 9 ? res.ToString().Replace(',', '.') : res.ToString("0.000E+00").Replace(',', '.');
            }

            if (History.Contains('='))
            {
                History = "";
                op = "";
            }
        }
        #endregion

        #region BackSpaceCommand
        public ICommand BackSpaceCommand { get; }

        private bool OnBackSpaceCommandExecute(object p) => true;
        private void OnBackSpaceCommandExecuted(object p)
        {
            if (Input.Length == 1 || (Input.Length == 2 && Input[0] == '-') || Input == "Error")
                Input = "";

            if (Input.Length > 0)
                Input = Input.Substring(0, Input.Length - 1);
        }
        #endregion

        #region ClearEntryCommand
        public ICommand ClearEntryCommand { get; }

        private bool OnClearEntryCommandExecute(object p) => true;
        private void OnClearEntryCommandExecuted(object p)
        {
            Input = "0";
            if (History.Contains("="))
                History = "";
        }
        #endregion

        #region ClearCommand
        public ICommand ClearCommand { get; }

        private bool OnClearCommandExecute(object p) => true;
        private void OnClearCommandExecuted(object p)
        {
            Input = "0";
            History = "";
            op = "";
            first = second = result = 0;
        }
        #endregion

        #region PiClickCommand
        public ICommand PiClickCommand { get; }

        private bool OnPiClickCommandExecute(object p) => true;
        private void OnPiClickCommandExecuted(object p)
        {
            if (Input.Length == 0 || Input == "0" || Input == "-0")
                Input = "3.141";
        }
        #endregion

        #region ExpClickCommand
        public ICommand ExpClickCommand { get; }

        private bool OnExpClickCommandExecute(object p) => true;
        private void OnExpClickCommandExecuted(object p)
        {
            if (Input.Length == 0 || Input == "0" || Input == "-0")
                Input = "2.718";
        }
        #endregion

        #region PercentClickCommand
        public ICommand PercentClickCommand { get; }

        private bool OnPercentClickCommandExecute(object p) => true;
        private void OnPercentClickCommandExecuted(object p)
        {
            if (double.TryParse(Input, NumberStyles.Any, CultureInfo.InvariantCulture, out double res))
            {
                History = $"{Input}% =";
                Input = Round((res / 100), 3).ToString().Replace(',', '.');
            }
        }
        #endregion

        #region TrigonometricClickCommand
        public ICommand TrigonometricClickCommand { get; }

        private bool OnTrigonometricClickCommandExecute(object p) => true;
        private void OnTrigonometricClickCommandExecuted(object p)
        {

            if (double.TryParse(Input, NumberStyles.Any, CultureInfo.InvariantCulture, out double res))
                if ((string)p == "cos")
                {
                    History = $"cos({Input}) =";
                    Input = Round(Cos(res), 3).ToString().Replace(',', '.');
                }
                else
                {
                    History = $"sin({Input}) =";
                    Input = Round(Sin(res), 3).ToString().Replace(',', '.');
                }
        }
        #endregion

        #region SquareRootCommand
        public ICommand SquareRootCommand { get; }

        private bool OnSquareRootCommandExecute(object p) => true;
        private void OnSquareRootCommandExecuted(object p)
        {
            if (double.TryParse(Input, NumberStyles.Any, CultureInfo.InvariantCulture, out double res))
            {
                History = $"√{Input} =";
                if (res < 0)
                {
                    Input = "Error";
                }
                else
                    Input = Round(Pow(res, 1 / 2f), 3).ToString().Replace(',', '.');
            }
        }
        #endregion

        #region SquareCommand
        public ICommand SquareCommand { get; }

        private bool OnSquareCommandExecute(object p) => true;
        private void OnSquareCommandExecuted(object p)
        {
            if (double.TryParse(Input, NumberStyles.Any, CultureInfo.InvariantCulture, out double res))
            {
                History = $"{Input}² =";
                Input = Round(Pow(res, 2), 3).ToString().Replace(',', '.');
            }
        }
        #endregion

        #region CubeCommand
        public ICommand CubeCommand { get; }
        private bool OnCubeCommandExecute(object p) => true;
        private void OnCubeCommandExecuted(object p)
        {
            if (double.TryParse(Input, NumberStyles.Any, CultureInfo.InvariantCulture, out double res))
            {
                History = $"{Input}³ =";
                Input = Round(Pow(res, 3), 3).ToString().Replace(',', '.');
            }
        }
        #endregion

        #region Log10Command
        public ICommand Log10Command { get; }
        private bool OnLog10CommandExecute(object p) => true;
        private void OnLog10CommandExecuted(object p)
        {
            if (double.TryParse(Input, NumberStyles.Any, CultureInfo.InvariantCulture, out double res))
            {
                History = $"lg({Input}) =";
                if (res <= 0)
                {
                    Input = "Error";
                }
                else
                    Input = Round(Log10(res), 3).ToString().Replace(',', '.');
            }
        }
        #endregion

        #region OneDivXCommand
        public ICommand OneDivXCommand { get; }
        private bool OnOneDivXCommandExecute(object p) => true;
        private void OnOneDivXCommandExecuted(object p)
        {
            if (double.TryParse(Input, NumberStyles.Any, CultureInfo.InvariantCulture, out double res))
            {
                History = $"1 ÷ {Input} =";
                if (res == 0)
                {
                    Input = "Error";
                }
                else
                    Input = Round(1 / res, 3).ToString().Replace(',', '.');
            }
        }
        #endregion

        #region EqualsClickCommand
        public ICommand EqualsClickCommand { get; }

        private bool OnEqualsClickCommandExecute(object p) => true;
        private void OnEqualsClickCommandExecuted(object p)
        {
            string temp = "";
            string tempRes = "";
            double res;
            if (!double.TryParse(Input, NumberStyles.Any, CultureInfo.InvariantCulture, out res) || History == "" || History.Contains('='))
                return;

            switch (op)
            {
                case "-":
                    temp = $"{first} - {res} =".Replace(',', '.');
                    result = Round(first - res, 3);
                    tempRes = $"{result}".Replace(',', '.');
                    break;
                case "+":
                    temp = $"{first} + {res} =".Replace(',', '.');
                    result = Round(first + res, 3);
                    tempRes = $"{result}".Replace(',', '.');
                    break;
                case "*":
                    temp = $"{first} × {res} =".Replace(',', '.');
                    result = Round(first * res, 3);
                    tempRes = $"{result}".Replace(',', '.');
                    break;
                case "/":
                    temp = $"{first} ÷ {res} =".Replace(',', '.');

                    if (op == "/" && res == 0)
                        tempRes = "Error";
                    else
                    {
                        result = Round(first / res, 3);
                        tempRes = $"{result}".Replace(',', '.');
                    }
                    break;
            }
            Input = tempRes.Length <= 9 ? tempRes : result.ToString("0.000E+00").Replace(',', '.');
            History = temp.Length < 17 ? temp : "";
        }
        #endregion

        #region OperationClickCommand
        public ICommand OperationClickCommand { get; }

        private bool OnOperationClickCommandExecute(object p) => true;
        private void OnOperationClickCommandExecuted(object p)
        {
            double res;
            if (!double.TryParse(Input, NumberStyles.Any, CultureInfo.InvariantCulture, out res))
                return;

            if (res == 0 && op == "/")
            {
                Input = "Error";
                History = "";
                first = second = result = 0;
                return;
            }

            if (History == "" || History.Contains('='))
                first = res;
            else
            {
                switch (op)
                {
                    case "-":
                        first = Round(first - res, 3);
                        break;
                    case "+":
                        first = Round(first + res, 3);
                        break;
                    case "*":
                        first = Round(first * res, 3);
                        break;
                    case "/":
                        first = Round(first / res, 3);
                        break;
                }
            }

            string temp = "";
            op = (string)p;
            switch (op)
            {
                case "-":
                    temp = $"{first} -".Replace(',', '.');
                    break;
                case "+":
                    temp = $"{first} +".Replace(',', '.');
                    break;
                case "*":
                    temp = $"{first} ×".Replace(',', '.');
                    break;
                case "/":
                    temp = $"{first} ÷".Replace(',', '.');
                    break;
                default:
                    break;
            }
            Input = "0";
            History = temp.Length < 17 ? temp : first.ToString("0.000E+00") + temp.Substring(temp.Length - 2);
        }
        #endregion

        #endregion

        public MainWindowViewModel()
        {
            CloseCommand = new LambdaCommand(OnCloseCommandExecuted, OnCloseCommandExecute);

            MinimizeCommand = new LambdaCommand(OnMinimizeCommandExecuted, OnMinimizeCommandExecute);

            NumberClickCommand = new LambdaCommand(OnNumberClickCommandExecuted, OnNumberClickCommandExecute);

            BackSpaceCommand = new LambdaCommand(OnBackSpaceCommandExecuted, OnBackSpaceCommandExecute);

            DotClickCommand = new LambdaCommand(OnDotClickCommandExecuted, OnDotClickCommandExecute);

            ChangeSignCommand = new LambdaCommand(OnChangeSignCommandExecuted, OnChangeSignCommandExecute);

            ClearEntryCommand = new LambdaCommand(OnClearEntryCommandExecuted, OnClearEntryCommandExecute);

            ClearCommand = new LambdaCommand(OnClearCommandExecuted, OnClearCommandExecute);

            OperationClickCommand = new LambdaCommand(OnOperationClickCommandExecuted, OnOperationClickCommandExecute);

            EqualsClickCommand = new LambdaCommand(OnEqualsClickCommandExecuted, OnEqualsClickCommandExecute);

            PiClickCommand = new LambdaCommand(OnPiClickCommandExecuted, OnPiClickCommandExecute);

            ExpClickCommand = new LambdaCommand(OnExpClickCommandExecuted, OnExpClickCommandExecute);

            PercentClickCommand = new LambdaCommand(OnPercentClickCommandExecuted, OnPercentClickCommandExecute);

            TrigonometricClickCommand = new LambdaCommand(OnTrigonometricClickCommandExecuted, OnTrigonometricClickCommandExecute);

            SquareRootCommand = new LambdaCommand(OnSquareRootCommandExecuted, OnSquareRootCommandExecute);

            SquareCommand = new LambdaCommand(OnSquareCommandExecuted, OnSquareCommandExecute);

            CubeCommand = new LambdaCommand(OnCubeCommandExecuted, OnCubeCommandExecute);

            Log10Command = new LambdaCommand(OnLog10CommandExecuted, OnLog10CommandExecute);

            OneDivXCommand = new LambdaCommand(OnOneDivXCommandExecuted, OnOneDivXCommandExecute);
        }
    }
}

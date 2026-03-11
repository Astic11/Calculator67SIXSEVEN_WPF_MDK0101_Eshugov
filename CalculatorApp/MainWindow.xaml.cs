using System;
using System.Windows;
using System.Windows.Input;

namespace CalculatorApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Метод для получения чисел из текстовых полей
        private bool TryGetNumbers(out double first, out double second)
        {
            first = 0;
            second = 0;

            // Пытаемся преобразовать текст в числа
            if (!double.TryParse(FirstNumberTextBox.Text, out first))
            {
                ShowError("Введите корректное первое число!");
                return false;
            }

            if (!double.TryParse(SecondNumberTextBox.Text, out second))
            {
                ShowError("Введите корректное второе число!");
                return false;
            }

            return true;
        }

        // Метод для получения только первого числа
        private bool TryGetFirstNumber(out double first)
        {
            if (!double.TryParse(FirstNumberTextBox.Text, out first))
            {
                ShowError("Введите корректное первое число!");
                return false;
            }
            return true;
        }

        // Показ сообщения об ошибке
        private void ShowError(string message)
        {
            MessageBox.Show(message, "Ошибка ввода",
                MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        // Добавление записи в историю
        private void AddToHistory(string operation, string expression, string result)
        {
            string historyItem = $"{DateTime.Now:T} - {expression} = {result}";
            HistoryListBox.Items.Insert(0, historyItem);
        }

        // Обработчик кнопки сложения
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (TryGetNumbers(out double first, out double second))
            {
                double result = first + second;
                ResultTextBox.Text = result.ToString();
                AddToHistory("Сложение", $"{first} + {second}", result.ToString());
            }
        }

        // Обработчик кнопки вычитания
        private void SubtractButton_Click(object sender, RoutedEventArgs e)
        {
            if (TryGetNumbers(out double first, out double second))
            {
                double result = first - second;
                ResultTextBox.Text = result.ToString();
                AddToHistory("Вычитание", $"{first} - {second}", result.ToString());
            }
        }

        // Обработчик кнопки умножения
        private void MultiplyButton_Click(object sender, RoutedEventArgs e)
        {
            if (TryGetNumbers(out double first, out double second))
            {
                double result = first * second;
                ResultTextBox.Text = result.ToString();
                AddToHistory("Умножение", $"{first} × {second}", result.ToString());
            }
        }

        // Обработчик кнопки деления
        private void DivideButton_Click(object sender, RoutedEventArgs e)
        {
            if (TryGetNumbers(out double first, out double second))
            {
                // Проверка деления на ноль
                if (second == 0)
                {
                    MessageBox.Show("Деление на ноль невозможно!",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                double result = first / second;
                ResultTextBox.Text = result.ToString();
                AddToHistory("Деление", $"{first} ÷ {second}", result.ToString());
            }
        }

        // Обработчик кнопки возведения в степень
        private void PowerButton_Click(object sender, RoutedEventArgs e)
        {
            if (TryGetNumbers(out double first, out double second))
            {
                double result = Math.Pow(first, second);
                ResultTextBox.Text = result.ToString();
                AddToHistory("Степень", $"{first} ^ {second}", result.ToString());
            }
        }

        // Обработчик кнопки квадратного корня
        private void SqrtButton_Click(object sender, RoutedEventArgs e)
        {
            if (TryGetFirstNumber(out double first))
            {
                if (first < 0)
                {
                    MessageBox.Show("Нельзя извлечь корень из отрицательного числа!",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                double result = Math.Sqrt(first);
                ResultTextBox.Text = result.ToString();
                AddToHistory("Корень", $"√{first}", result.ToString());
            }
        }

        // Обработчик кнопки остатка от деления
        private void ModButton_Click(object sender, RoutedEventArgs e)
        {
            if (TryGetNumbers(out double first, out double second))
            {
                if (second == 0)
                {
                    MessageBox.Show("Деление на ноль невозможно!",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                double result = first % second;
                ResultTextBox.Text = result.ToString();
                AddToHistory("Остаток", $"{first} % {second}", result.ToString());
            }
        }

        // Обработчик кнопки очистки
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            FirstNumberTextBox.Text = string.Empty;
            SecondNumberTextBox.Text = string.Empty;
            ResultTextBox.Text = string.Empty;
            FirstNumberTextBox.Focus();
        }

        // Обработка нажатия клавиш в текстовых полях
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            // Разрешаем только цифры, точку и управляющие клавиши
            if (!(e.Key >= Key.D0 && e.Key <= Key.D9) &&
                !(e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) &&
                e.Key != Key.Back && e.Key != Key.Delete &&
                e.Key != Key.Left && e.Key != Key.Right &&
                e.Key != Key.Tab && e.Key != Key.Enter &&
                e.Key != Key.OemPeriod && e.Key != Key.Decimal)
            {
                e.Handled = true;
            }

            // Если нажат Enter, фокус переходит на второе поле
            if (e.Key == Key.Enter)
            {
                if (sender == FirstNumberTextBox)
                {
                    SecondNumberTextBox.Focus();
                }
                else if (sender == SecondNumberTextBox)
                {
                    AddButton_Click(sender, e);
                }
            }
        }

        // Обработка нажатия клавиш в окне
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            // Сочетания клавиш для операций
            if (Keyboard.Modifiers == ModifierKeys.None)
            {
                switch (e.Key)
                {
                    case Key.OemPlus:
                    case Key.Add:
                        AddButton_Click(sender, e);
                        e.Handled = true;
                        break;
                    case Key.OemMinus:
                    case Key.Subtract:
                        SubtractButton_Click(sender, e);
                        e.Handled = true;
                        break;
                    case Key.Multiply:
                        MultiplyButton_Click(sender, e);
                        e.Handled = true;
                        break;
                    case Key.Divide:
                    case Key.OemQuestion:
                        DivideButton_Click(sender, e);
                        e.Handled = true;
                        break;
                    case Key.P:
                        PowerButton_Click(sender, e);
                        e.Handled = true;
                        break;
                    case Key.R:
                        SqrtButton_Click(sender, e);
                        e.Handled = true;
                        break;
                    case Key.Enter:
                        if (!string.IsNullOrEmpty(FirstNumberTextBox.Text) &&
                            !string.IsNullOrEmpty(SecondNumberTextBox.Text))
                        {
                            AddButton_Click(sender, e);
                        }
                        break;
                    case Key.Escape:
                        ClearButton_Click(sender, e);
                        break;
                }
            }
        }
    }
}
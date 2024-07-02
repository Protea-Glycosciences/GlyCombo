using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace glycombo 
{
    public static class NumericInputBehavior
    {
        public static readonly DependencyProperty IsNumericInputProperty =
            DependencyProperty.RegisterAttached(
                "IsNumericInput",
                typeof(bool),
                typeof(NumericInputBehavior),
                new PropertyMetadata(false, OnIsNumericInputChanged));

        public static bool GetIsNumericInput(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsNumericInputProperty);
        }

        public static void SetIsNumericInput(DependencyObject obj, bool value)
        {
            obj.SetValue(IsNumericInputProperty, value);
        }

        private static void OnIsNumericInputChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox textBox)
            {
                if ((bool)e.NewValue)
                {
                    textBox.PreviewTextInput += OnPreviewTextInput;
                    textBox.PreviewKeyDown += OnPreviewKeyDown;
                    DataObject.AddPastingHandler(textBox, OnPaste);
                }
                else
                {
                    textBox.PreviewTextInput -= OnPreviewTextInput;
                    textBox.PreviewKeyDown -= OnPreviewKeyDown;
                    DataObject.RemovePastingHandler(textBox, OnPaste);
                }
            }
        }

        private static void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private static void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
            else if (e.Key == Key.Enter)
            {
                // Allow Enter key for new line
                e.Handled = false;
            }
        }

        private static void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                var text = (string)e.DataObject.GetData(typeof(string));
                if (!IsTextAllowed(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private static bool IsTextAllowed(string text)
        {
            var regex = new Regex(@"^[0-9]*\.?[0-9]*$");
            return regex.IsMatch(text) || text.Contains("\n") || text.Contains("\r");
        }
    }
}
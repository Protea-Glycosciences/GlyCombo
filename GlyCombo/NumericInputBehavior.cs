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
                    textBox.PreviewTextInput += TextBox_PreviewTextInput;
                    DataObject.AddPastingHandler(textBox, OnPaste);
                }
                else
                {
                    textBox.PreviewTextInput -= TextBox_PreviewTextInput;
                    DataObject.RemovePastingHandler(textBox, OnPaste);
                }
            }
        }

        private static void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            string newText = textBox.Text.Insert(textBox.SelectionStart, e.Text);

            e.Handled = !Regex.IsMatch(newText, @"^[0-9]*\.?[0-9]*$") || (e.Text == "." && textBox.Text.Contains("."));
        }

        private static void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string text = (string)e.DataObject.GetData(typeof(string));
                TextBox textBox = sender as TextBox;
                string newText = textBox.Text.Insert(textBox.SelectionStart, text);

                if (!Regex.IsMatch(newText, @"^[0-9]*\.?[0-9]*$"))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }
    }
}
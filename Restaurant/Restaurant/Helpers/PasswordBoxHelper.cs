using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace Restaurant.Helpers
{
    public static class PasswordBoxHelper
    {
        private static bool _isUpdating = false;
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.RegisterAttached("Password", typeof(string), typeof(PasswordBoxHelper), new FrameworkPropertyMetadata(string.Empty, OnPasswordPropertyChanged));

        public static string GetPassword(DependencyObject d)
        {
            return (string)d.GetValue(PasswordProperty);
        }

        public static void SetPassword(DependencyObject d, string value)
        {
            d.SetValue(PasswordProperty, value);
        }

        private static void OnPasswordPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (_isUpdating) return;

            PasswordBox passwordBox = d as PasswordBox;
            passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;

            if (e.NewValue != null)
            {
                _isUpdating = true;
                passwordBox.Password = e.NewValue.ToString();
                _isUpdating = false;
            }

            passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
        }

        private static void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            System.Diagnostics.Debug.WriteLine($"Password changed to: {passwordBox.Password}");
            SetPassword(passwordBox, passwordBox.Password);
        }
    }
}

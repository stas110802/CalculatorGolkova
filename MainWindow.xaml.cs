using System;
using System.Collections.Generic;
using System.Data;
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

namespace Calculator
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static bool isCleared = true;
        public MainWindow()
        {
            InitializeComponent();
            text.IsReadOnly = true;
            text.Text = "0";
            this.Opacity = 1;
            
            foreach (UIElement uIElement in GroupsOfButtons.Children)
            {
                if(uIElement is Button)
                {
                    ((Button)uIElement).Click += ButtonClick;
                }
            }
        }      
        
        private void ButtonClick(Object sender, RoutedEventArgs routeEvent)
        {
            var textButton = ((Button)routeEvent.OriginalSource).Content.ToString();
            
            // стереть весь текст
            if(textButton == "C")
            {
                text.ClearTextBox(ref isCleared, true);
            }
            // стереть 1 символ
            else if (textButton == "x")
            {
                if(text.Text.Length > 0 && text.Text != "0")
                {
                    text.Text = text.Text.Substring(0, text.Text.Length-1);
                    if (text.Text.Length == 0) 
                    {
                        text.ClearTextBox(ref isCleared, true);
                    }                   
                }
                else
                {
                    text.Text = "0";
                }
            }
            // посчитать
            else if (textButton == "=")
            {
                try
                {
                    text.Text = new DataTable().Compute(text.Text, null).ToString();
                }
                catch (Exception error)
                {
                    MessageBox.Show($"{error.Message}", "Ошибка при компиляции",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                    text.ClearTextBox(ref isCleared, true);
                }               
            }
            // добавить  1 символ
            else
            {
                if (isCleared)
                {
                    text.Clear();
                    isCleared = false;                   
                    text.Text += textButton;
                }
                else
                {
                    text.Check(textButton);                 
                }
            }
        }        
    }

    

    public static class Utilites
    {
        static string[] operation = { "+", "-", "/", "*",};
        public static void ClearTextBox(this TextBox textBox, ref bool isCleared, bool flag)
        {
            textBox.Clear();
            textBox.Text = "0";
            isCleared = flag;
        }
        public static void Check(this TextBox text, string button)
        {
            if (button == ".")
            {
                text.CheckForDot();
            }
            else
            {
                text.CheckForOperation(button);
            }
        }

        public static void CheckForOperation(this TextBox textBox, string button)
        {
            foreach (var item in operation)
            {
                if (item == button && button == textBox.Text.Last().ToString())
                {
                    return;
                }
            }
            textBox.Text += button;
        }

        public static void CheckForDot(this TextBox textBox, string button = ".")
        {
            for (int i = textBox.Text.Length-1; i >= 0; i--)
            {
                if (textBox.Text[i].ToString() == ".")
                {
                    return;
                }
                else 
                {
                    foreach (var item in operation)
                    {
                        if(textBox.Text[i].ToString() == item)
                        {
                            textBox.Text += button;
                            return;
                        }
                    }
                }
            }
            textBox.Text += button;
        }
    }
}

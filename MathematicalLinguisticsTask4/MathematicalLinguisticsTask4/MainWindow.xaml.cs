﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace MathematicalLinguisticsTask4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ReversePolishNotationConverter Converter = new ReversePolishNotationConverter()
        {
            Digits = new List<char>() { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' },
            Operators = new List<Operator>()
            {
                new Operator()
                {
                    Symbol = '+',
                    IsLeftAssociative = true,
                    IsRightAssociative = true,
                    PriorityLevel = 0
                },
                new Operator()
                {
                    Symbol = '-',
                    IsLeftAssociative = true,
                    IsRightAssociative = false,
                    PriorityLevel = 0
                },
                new Operator()
                {
                    Symbol = '*',
                    IsLeftAssociative = true,
                    IsRightAssociative = true,
                    PriorityLevel = 1
                },
                new Operator()
                {
                    Symbol = '/',
                    IsLeftAssociative = true,
                    IsRightAssociative = false,
                    PriorityLevel = 1
                },
                new Operator()
                {
                    Symbol = '^',
                    IsLeftAssociative = false,
                    IsRightAssociative = true,
                    PriorityLevel = 2
                }
            }
        };

        public MainWindow()
        {
            InitializeComponent();
        }

        private void buttonConvert_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                textBoxPostfixNotation.Text = Converter.Convert(textBoxInfixNotation.Text);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(
                    $"Incorrect syntax in the infix notation expression:{Environment.NewLine}{ex.Message}{Environment.NewLine}Please correct it.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }

        }

        private void textBoxInfixNotation_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            textBox.Text = textBox.Text.Replace(" ", "");
        }
    }
}

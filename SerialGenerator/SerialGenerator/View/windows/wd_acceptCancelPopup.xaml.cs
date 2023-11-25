﻿using SerialGenerator;
using SerialGenerator.Classes;
using SerialGenerator.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace POS.View.windows
{
    /// <summary>
    /// Interaction logic for wd_acceptCancelPopup.xaml
    /// </summary>
    public partial class wd_acceptCancelPopup : Window
    {
         
        //public bool isOk { get; set; }
        public bool isOk;
        public wd_acceptCancelPopup()
        {
            try
            {
                InitializeComponent();
                this.DataContext = this;
            }
            catch(Exception ex)
            { HelpClass.ExceptionMessage(ex, this); }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
               
                if (sender != null)
                    HelpClass.StartAwait(grid_main);

                #region translate
                if (MainWindow.lang.Equals("en"))
            {
                MainWindow.resourcemanager = new ResourceManager("SerialGenerator.en_file", Assembly.GetExecutingAssembly());
                    grid_main.FlowDirection = FlowDirection.LeftToRight;
            }
            else
            {
                MainWindow.resourcemanager = new ResourceManager("SerialGenerator.ar_file", Assembly.GetExecutingAssembly());
                    grid_main.FlowDirection = FlowDirection.RightToLeft;
            }
            translate();
                #endregion
                if (sender != null)
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                if (sender != null)
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }

        }

        private void translate()
        {
            btn_ok.Content = MainWindow.resourcemanager.GetString("trOK");
            btn_cancel.Content = MainWindow.resourcemanager.GetString("trCancel");
        }

        private void Btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                isOk = false;
                this.Close();
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Btn_ok_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                isOk = true;
                this.Close();
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        #region contentText
        public static readonly DependencyProperty contentTextDependencyProperty = DependencyProperty.Register("contentText",
            typeof(string),
            typeof(wd_acceptCancelPopup),
            new PropertyMetadata("DEFAULT"));
        public string contentText
        {
            set
            { SetValue(contentTextDependencyProperty, value); }
            get
            { return (string)GetValue(contentTextDependencyProperty); }
        }
        #endregion

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch (Exception)
            {

            }
        }


        private   void HandleKeyPress(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                Btn_ok_Click(null, null);
            }
        }

    }
}

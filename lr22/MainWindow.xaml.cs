using lr22.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Shapes;

namespace lr22
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Ini Ini;
        public static MainWindow Window;
        public MainWindow()
        {
            Ini = new Ini("config.ini");
            SetLanguageDictionary();
            InitializeComponent();
            Window = this;
            Window.Content = new Notepad();
        }
        private void SetLanguageDictionary()
        {
            if (Ini.read("Language", "Settings") == null)
            {
                switch (Thread.CurrentThread.CurrentCulture.ToString())
                {
                    case "uk-UA":
                    case "ru-UA":
                    case "ru-RU":
                        MyProject.Language.Resources.Culture = new CultureInfo("uk-UA");
                        break;
                    case "en-GB":
                        MyProject.Language.Resources.Culture = new CultureInfo("en-GB");
                        break;
                    default
                        : //default english because there can be so many different system language, we rather fallback on english in this case.
                        MyProject.Language.Resources.Culture = new CultureInfo("en-GB");
                        break;
                }
                Ini.write("Language", Thread.CurrentThread.CurrentCulture.ToString(), "Settings");
            }
            else
            {
                switch (Ini.read("Language", "Settings"))
                {
                    case "uk-UA":
                    case "ru-UA":
                    case "ru-RU":
                        MyProject.Language.Resources.Culture = new CultureInfo("uk-UA");
                        break;
                    case "en-GB":
                        MyProject.Language.Resources.Culture = new CultureInfo("en-GB");
                        break;
                    default
                        : //default english because there can be so many different system language, we rather fallback on english in this case.
                        MyProject.Language.Resources.Culture = new CultureInfo("en-GB");
                        break;
                }
            }
        }
    }
}

using lr22.Core;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
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
    /// Логика взаимодействия для Notepad.xaml
    /// </summary>
    public partial class Notepad
    {

        public Notepad()
        {
            InitializeComponent();
            SetSettingsLanguage();
            cmbFontFamily.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            cmbFontSize.ItemsSource = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
        }

        private void SetSettingsLanguage()
        {
            switch (MyProject.Language.Resources.Culture.ToString())
            {
                case "uk-UA":
                case "ru-UA":
                case "ru-RU":
                    settings_lang_uk.IsSelected = true;
                    break;
                case "en-GB":
                    settings_lang_en.IsSelected = true;
                    break;
                default:
                    settings_lang_en.IsSelected = true;
                    break;
            }
        }

        private void changeLanguane_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (changeLanguane.SelectedValue.ToString().Split(" ")[1].Trim() == "EN")
            {
                if (MainWindow.Ini.read("Language", "Settings") != "en-GB")
                {
                    MyProject.Language.Resources.Culture = new System.Globalization.CultureInfo("en-GB");
                    MainWindow.Ini.write("Language", "en-GB", "Settings");

                    MainWindow.Window.Content = new Notepad();
                    MainWindow.Window.Title = MyProject.Language.Resources.notepad;
                } 
            }
            else if (changeLanguane.SelectedValue.ToString().Split(" ")[1].Trim() == "UK")
            {
                if (MainWindow.Ini.read("Language", "Settings") != "uk-UA")
                {
                    MyProject.Language.Resources.Culture = new System.Globalization.CultureInfo("uk-UA");
                    MainWindow.Ini.write("Language", "uk-UA", "Settings");

                    MainWindow.Window.Content = new Notepad();
                    MainWindow.Window.Title = MyProject.Language.Resources.notepad;
                }
            }
        }

        private void rtbEditor_SelectionChanged(object sender, RoutedEventArgs e)
        {
            object temp = rtbEditor.Selection.GetPropertyValue(Inline.FontWeightProperty);
            btnBold.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontWeights.Bold));
            temp = rtbEditor.Selection.GetPropertyValue(Inline.FontStyleProperty);
            btnItalic.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontStyles.Italic));
            temp = rtbEditor.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            btnUnderline.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(TextDecorations.Underline));

            temp = rtbEditor.Selection.GetPropertyValue(Inline.FontFamilyProperty);
            cmbFontFamily.SelectedItem = temp;
            temp = rtbEditor.Selection.GetPropertyValue(Inline.FontSizeProperty);
            cmbFontSize.Text = temp.ToString();
        }

        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Rich Text Format (*.rtf)|*.rtf|All files (*.*)|*.*";
            if (dlg.ShowDialog() == true)
            {
                FileStream fileStream = new FileStream(dlg.FileName, FileMode.Open);
                TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
                range.Load(fileStream, DataFormats.Rtf);
            }
        }

        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Rich Text Format (*.rtf)|*.rtf|All files (*.*)|*.*";
            if (dlg.ShowDialog() == true)
            {
                FileStream fileStream = new FileStream(dlg.FileName, FileMode.Create);
                TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
                range.Save(fileStream, DataFormats.Rtf);
            }
        }

        private void cmbFontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbFontFamily.SelectedItem != null)
                rtbEditor.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, cmbFontFamily.SelectedItem);
        }

        private void cmbFontSize_TextChanged(object sender, TextChangedEventArgs e)
        {
            rtbEditor.Selection.ApplyPropertyValue(Inline.FontSizeProperty, cmbFontSize.Text);
        }

        private void AddPictureToRichTextBox()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg";
            if (openFileDialog.ShowDialog() == true)
            {
                string imagePath = openFileDialog.FileName;

                BitmapImage bitmap = new BitmapImage(new Uri(imagePath));
                Image image = new Image();
                image.Source = bitmap;

                InlineUIContainer container = new InlineUIContainer(image);
                container.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(Image_Clicked);

                Paragraph paragraph = new Paragraph(container);
                rtbEditor.Document.Blocks.Add(paragraph);
            }
        }

        private void Image_Clicked(object sender, MouseButtonEventArgs e)
        {
            Image clickedImage = sender as Image;

            InlineUIContainer container = new InlineUIContainer(clickedImage, rtbEditor.Selection.Start);

            Paragraph paragraph = new Paragraph(container);

            rtbEditor.Document.Blocks.InsertAfter(rtbEditor.CaretPosition.Paragraph, paragraph);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddPictureToRichTextBox();
        }

        private void left_Click(object sender, RoutedEventArgs e)
        {
            TextAlignment alignment = TextAlignment.Left;
            Paragraph paragraph = rtbEditor.Document.Blocks.FirstBlock as Paragraph;
            if (paragraph != null)
            {
                paragraph.TextAlignment = alignment;
            }
        }

        private void right_Click(object sender, RoutedEventArgs e)
        {
            TextAlignment alignment = TextAlignment.Right;
            Paragraph paragraph = rtbEditor.Document.Blocks.FirstBlock as Paragraph;
            if (paragraph != null)
            {
                paragraph.TextAlignment = alignment;
            }
        }

        private void center_Click(object sender, RoutedEventArgs e)
        {
            TextAlignment alignment = TextAlignment.Center;
            Paragraph paragraph = rtbEditor.Document.Blocks.FirstBlock as Paragraph;
            if (paragraph != null)
            {
                paragraph.TextAlignment = alignment;
            }
        }

    }
}
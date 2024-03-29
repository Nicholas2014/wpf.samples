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

namespace TextEditor
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private DocumentManager _documentManager { get; set; }
        private PrintManager _printManager { get; set; }
        public MainWindow()
        {
            InitializeComponent();

            _documentManager = new DocumentManager(body);
            if (_documentManager.OpenDocument())
            {
                status.Text = "Document Loaded.";
            }

            _printManager = new PrintManager(body);
        }

        private void TextEditorToolbar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (toolbar.IsSynchronizing)
            {
                return;
            }

            var source = e.OriginalSource as ComboBox;
            if (source == null)
            {
                return;
            }

            switch (source.Name)
            {
                case "fonts":
                    _documentManager.ApplyToSelection(TextBlock.FontFamilyProperty, source.SelectedItem);
                    break;
                case "fontSize":
                    _documentManager.ApplyToSelection(TextBlock.FontSizeProperty, source.SelectedItem);
                    break;
                default:
                    break;
            }

            body.Focus();
        }

        private void body_SelectionChanged(object sender, RoutedEventArgs e)
        {
            toolbar.SynchronizeWith(body.Selection);
        }

        private void NewDocument(object sender, ExecutedRoutedEventArgs e)
        {
            _documentManager.NewDocument();
            status.Text = "New Document";
        }
        private void OpenDocument(object sender, ExecutedRoutedEventArgs e)
        {
            _documentManager.OpenDocument();
            status.Text = "Document Loaded";
        }
        private void SaveDocument(object sender, ExecutedRoutedEventArgs e)
        {
            _documentManager.SaveDocument();
            status.Text = "Document Saved";
        }
        private void SaveAsDocument(object sender, ExecutedRoutedEventArgs e)
        {
            _documentManager.SaveDocumentAs();
            status.Text = "Document Saved";
        }
        private void PrintDocument(object sender, ExecutedRoutedEventArgs e)
        {
            var printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                printDialog.PrintDocument(((IDocumentPaginatorSource)body.Document).DocumentPaginator, "Text Editor Printing");
            }
        }
        private void PrintPreview(object sender, ExecutedRoutedEventArgs e)
        {
            _printManager.PrintPreview();
        }
        private void ApplicationClose(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        private void SaveDocument_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _documentManager.CanSaveDocument();
        }
    }
}

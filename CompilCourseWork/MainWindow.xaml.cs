using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using CompilCourseWork.model;
using Microsoft.Win32;

namespace CompilCourseWork
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void DisplayResults(List<Token> tokens)
        {
            InputSecond.Document.Blocks.Clear();
            
            FlowDocument flowDocument = new FlowDocument();
            
            Table table = new Table();
            table.CellSpacing = 5;
            table.Background = Brushes.White;
            
            table.Columns.Add(new TableColumn { Width = new GridLength(100) }); 
            table.Columns.Add(new TableColumn { Width = new GridLength(200) }); 
            table.Columns.Add(new TableColumn { Width = new GridLength(200) }); 
            table.Columns.Add(new TableColumn { Width = new GridLength(200) }); 
            
            TableRowGroup rowGroup = new TableRowGroup();
            TableRow headerRow = new TableRow { Background = Brushes.LightGray };
            headerRow.Cells.Add(new TableCell(new Paragraph(new Run("Код")) { FontWeight = FontWeights.Bold }));
            headerRow.Cells.Add(new TableCell(new Paragraph(new Run("Тип")) { FontWeight = FontWeights.Bold }));
            headerRow.Cells.Add(new TableCell(new Paragraph(new Run("Лексема")) { FontWeight = FontWeights.Bold }));
            headerRow.Cells.Add(new TableCell(new Paragraph(new Run("Позиция")) { FontWeight = FontWeights.Bold }));
            rowGroup.Rows.Add(headerRow);
            
            foreach (var token in tokens)
            {
                TableRow row = new TableRow();
                row.Cells.Add(new TableCell(new Paragraph(new Run(token.Code.ToString()))));
                row.Cells.Add(new TableCell(new Paragraph(new Run(token.Type))));
                row.Cells.Add(new TableCell(new Paragraph(new Run(token.Lexeme))));
                row.Cells.Add(new TableCell(new Paragraph(new Run(token.Position))));
                rowGroup.Rows.Add(row);
            }
            table.RowGroups.Add(rowGroup);

            
            flowDocument.Blocks.Add(table);
            
            InputSecond.Document = flowDocument;
        }
        private void Launch_Click(object sender, RoutedEventArgs e)
        {
            AnalyzeText();
        }
        private void AnalyzeText()
        {
            TextRange textRange = new TextRange(InputFirst.Document.ContentStart, InputFirst.Document.ContentEnd);
            string input = textRange.Text;

            Scanner scanner = new Scanner(input);
            List<Token> tokens = scanner.Analyze();

            Parser parser = new Parser(tokens);
            bool isSyntaxValid = parser.Parse();

            DisplayResults(tokens);

            if (isSyntaxValid)
            {
                InputSecond.Document.Blocks.Add(new Paragraph(new Run("Синтаксический анализ завершен успешно!")) { Foreground = Brushes.Green });
            }
            else
            {
                InputSecond.Document.Blocks.Add(new Paragraph(new Run("Синтаксическая ошибка!")) { Foreground = Brushes.Red });
            }
        }
        
        
        
        private void MenuItem_New_Click(object sender, RoutedEventArgs e)
        {
            InputFirst.Document.Blocks.Clear();
        }

        private void MenuItem_Open_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                using (StreamReader reader = new StreamReader(openFileDialog.FileName))
                {
                    InputFirst.Document.Blocks.Clear();
                    InputFirst.Document.Blocks.Add(new Paragraph(new Run(reader.ReadToEnd())));
                }
            }
        }

        private void MenuItem_Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_currentFilePath))
            {
                MenuItem_SaveAs_Click(sender, e);
            }
            else
            {
                SaveToFile(_currentFilePath);
            }
        }

        private void MenuItem_SaveAs_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                _currentFilePath = saveFileDialog.FileName;
                SaveToFile(_currentFilePath);
            }
        }

        private void SaveToFile(string filePath)
        {
            var textRange = new TextRange(InputFirst.Document.ContentStart, InputFirst.Document.ContentEnd);
            File.WriteAllText(filePath, textRange.Text);
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(InputFirst.Document.ContentStart.ToString()))
            {
                var result = MessageBox.Show("Вы хотите сохранить изменения?", "Подтверждение", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    SaveToFile(_currentFilePath);
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        
        private void MenuItem_Undo_Click(object sender, RoutedEventArgs e)
        {
            if (InputFirst.CanUndo)
            {
                InputFirst.Undo();
            }
        }

        private void MenuItem_Redo_Click(object sender, RoutedEventArgs e)
        {
            if (InputFirst.CanRedo)
            {
                InputFirst.Redo();
            }
        }

        private void MenuItem_Cut_Click(object sender, RoutedEventArgs e)
        {
            InputFirst.Cut();
        }

        private void MenuItem_Copy_Click(object sender, RoutedEventArgs e)
        {
            InputFirst.Copy();
        }

        private void MenuItem_Paste_Click(object sender, RoutedEventArgs e)
        {
            InputFirst.Paste();
        }

        private void MenuItem_Delete_Click(object sender, RoutedEventArgs e)
        {
            InputFirst.Selection.Text = string.Empty;
        }

        private void MenuItem_SelectAll_Click(object sender, RoutedEventArgs e)
        {
            InputFirst.SelectAll();
        }
        private void MenuItem_Help_Click(object sender, RoutedEventArgs e)
        {
            OpenPdf("C:\\Users\\batal\\Desktop\\4 семак дерьмецо)\\Моделирование\\Лаба3\\CompilCourseWork\\CompilCourseWork\\resources\\help.pdf");
        }

        private void MenuItem_About_Click(object sender, RoutedEventArgs e)
        {
            OpenPdf("C:\\Users\\batal\\Desktop\\4 семак дерьмецо)\\Моделирование\\Лаба3\\CompilCourseWork\\CompilCourseWork\\resources\\about.pdf");
        }

        private void OpenPdf(string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
            if (File.Exists(filePath))
            {
                Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
            }
            else
            {
                MessageBox.Show("Файл не найден: " + filePath, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private string _currentFilePath = string.Empty;

        private void FontSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FontSizeComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                if (double.TryParse(selectedItem.Content.ToString(), out double newSize))
                {
                    InputFirst.FontSize = newSize;
                    InputSecond.FontSize = newSize;
                    LineNumbers.FontSize = newSize;
                }
            }
        }

        private void UpdateLineNumbers()
        {
            var textRange = new TextRange(InputFirst.Document.ContentStart, InputFirst.Document.ContentEnd);
            var text = textRange.Text;
            var lineCount = text.Split('\n').Length;

            LineNumbers.Text = string.Join("\n", Enumerable.Range(1, lineCount));
        }

        private void InputFirst_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            LineNumberScroller.ScrollToVerticalOffset(e.VerticalOffset);
        }
        
        private void InputFirst_TextChanged(object sender, TextChangedEventArgs e)
        {
            InputFirst.TextChanged -= InputFirst_TextChanged;

            TextRange textRange = new TextRange(InputFirst.Document.ContentStart, InputFirst.Document.ContentEnd);
            textRange.ClearAllProperties();

            HighlightKeywords();

            UpdateLineNumbers();

            InputFirst.TextChanged += InputFirst_TextChanged;
        }
        private void HighlightKeywords()
        {
            string[] keywords = {  "fun", "val", "var", "class", "object", "interface", "if", "else", "when", "for", "while", "return",
                "true", "false", "null", "const"  };
            foreach (string keyword in keywords)
            {
                HighlightText(keyword, Brushes.Blue, FontWeights.Bold);
            }
        }
        private void HighlightText(string text, Brush foreground, FontWeight fontWeight)
        {
            TextPointer position = InputFirst.Document.ContentStart;

            while (position != null)
            {
                if (position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                {
                    string textRun = position.GetTextInRun(LogicalDirection.Forward);
                    int index = textRun.IndexOf(text, StringComparison.OrdinalIgnoreCase);

                    if (index >= 0)
                    {
                        TextPointer start = position.GetPositionAtOffset(index);
                        TextPointer end = start.GetPositionAtOffset(text.Length);
                        TextRange range = new TextRange(start, end);

                        range.ApplyPropertyValue(TextElement.ForegroundProperty, foreground);
                        range.ApplyPropertyValue(TextElement.FontWeightProperty, fontWeight);
                    }
                }

                position = position.GetNextContextPosition(LogicalDirection.Forward);
            }
        }
    }
}

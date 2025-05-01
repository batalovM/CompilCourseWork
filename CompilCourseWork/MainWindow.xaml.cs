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
        private TextManager textManager = new TextManager();
        public MainWindow()
        {
            InitializeComponent();
            // Parser parser = new Parser();
            // string input = "const val pi: Double= 3.14;";
            //
            // List<string> result = parser.Parse(input);
            //
            // foreach (var message in result)
            // {
            //     Console.WriteLine(message);
            // }
        }
        private string GetTextFromRichTextBox(RichTextBox richTextBox)
        {
            TextRange textRange = new TextRange(
                richTextBox.Document.ContentStart,
                richTextBox.Document.ContentEnd);
            return textRange.Text.Trim();
        }
        // private void Launch_Click(object sender, RoutedEventArgs e)
        // {
        //     InputSecond.Document = new FlowDocument();
        //     string inputText = GetTextFromRichTextBox(InputFirst);
        //     Parser parser = new Parser();
        //     List<string> parseResults = parser.Parse(inputText);
        //     DisplayResults(parseResults);
        // }
        // private void DisplayResults(List<string> parseResults)
        // {
        //     FlowDocument flowDoc = new FlowDocument();
        //     Paragraph paragraph = new Paragraph();
        //     if (parseResults.Count == 0)
        //     {
        //         paragraph.Inlines.Add(new Run("Разбор завершен успешно!"));
        //     }
        //     else
        //     {
        //         paragraph.Inlines.Add(new Run($"Найдено {parseResults.Count} ошибок:"));
        //         paragraph.Inlines.Add(new LineBreak());
        //         foreach (var message in parseResults)
        //         {
        //             paragraph.Inlines.Add(new Run(message));
        //             paragraph.Inlines.Add(new LineBreak());
        //         }
        //     }
        //     flowDoc.Blocks.Add(paragraph);
        //     InputSecond.Document = flowDoc;
        // }
        private void Launch_Click(object sender, RoutedEventArgs e)
{
    InputSecond.Document = new FlowDocument();
    string inputText = GetTextFromRichTextBox(InputFirst);
    
    try
    {
        var lexer = new Lexer(inputText);
        var parser = new Parser(lexer);
        var result = parser.Parse();
        
        DisplayResults(parser.Tetrads, null);
    }
    catch (Exception ex)
    {
        DisplayResults(null, new List<string> { ex.Message });
    }
}

private void DisplayResults(List<Tetrad> tetrads, List<string> errors)
{
    FlowDocument flowDoc = new FlowDocument();
    Paragraph paragraph = new Paragraph();

    if (errors != null && errors.Count > 0)
    {
        // Вывод ошибок
        paragraph.Inlines.Add(new Run($"Найдено {errors.Count} ошибок:") { Foreground = Brushes.Red });
        paragraph.Inlines.Add(new LineBreak());
        
        foreach (var error in errors)
        {
            paragraph.Inlines.Add(new Run(error) { Foreground = Brushes.Red });
            paragraph.Inlines.Add(new LineBreak());
        }
    }
    else if (tetrads != null && tetrads.Count > 0)
    {
        // Вывод успешного разбора и тетрад
        paragraph.Inlines.Add(new Run("Разбор завершен успешно!") { Foreground = Brushes.Green });
        paragraph.Inlines.Add(new LineBreak());
        paragraph.Inlines.Add(new LineBreak());
        
        paragraph.Inlines.Add(new Run("Сгенерированные тетрады:") { FontWeight = FontWeights.Bold });
        paragraph.Inlines.Add(new LineBreak());
        
        for (int i = 0; i < tetrads.Count; i++)
        {
            paragraph.Inlines.Add(new Run($"{i + 1}: {tetrads[i]}"));
            paragraph.Inlines.Add(new LineBreak());
        }
        
        paragraph.Inlines.Add(new LineBreak());
        paragraph.Inlines.Add(new Run($"Результат: {tetrads.Last().Result}") { FontWeight = FontWeights.Bold });
    }
    else
    {
        paragraph.Inlines.Add(new Run("Нет данных для отображения."));
    }

    flowDoc.Blocks.Add(paragraph);
    InputSecond.Document = flowDoc;
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
            OpenPdf("C:\\Users\\batal\\RiderProjects\\CompilCourseWork\\CompilCourseWork\\resources\\help.pdf");
        }

        private void MenuItem_About_Click(object sender, RoutedEventArgs e)
        {
            OpenPdf("C:\\Users\\batal\\RiderProjects\\CompilCourseWork\\CompilCourseWork\\resources\\about.pdf");
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
        private void TextTaskStatement_Click(object sender, RoutedEventArgs e)
        {
            textManager.OpenTaskStatement();
        }
        private void TextGrammar_Click(object sender, RoutedEventArgs e)
        {
            textManager.OpenGrammar();
        }

        private void TextGrammarClassification_Click(object sender, RoutedEventArgs e)
        {
            textManager.OpenGrammarClassification();
        }

        private void TextAnalysisMethod_Click(object sender, RoutedEventArgs e)
        {
            textManager.OpenAnalysisMethod();
        }

        private void TextErrorDiagnostics_Click(object sender, RoutedEventArgs e)
        {
            textManager.OpenErrorDiagnostics();
        }

        private void TextTestExample_Click(object sender, RoutedEventArgs e)
        {
            textManager.OpenTestExample();
        }

        private void TextBibliography_Click(object sender, RoutedEventArgs e)
        {
            textManager.OpenBibliography();
        }

        private void TextSourceCode_Click(object sender, RoutedEventArgs e)
        {
            textManager.OpenSourceCode();
        }
    }
}

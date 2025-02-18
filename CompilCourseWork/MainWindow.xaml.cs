using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using Microsoft.Win32;

namespace CompilCourseWork
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
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
        
    }
}
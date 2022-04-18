using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
using TemplateEngine.Docx;

namespace DemoDiplom
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string inputTemplateName = string.Empty;
        public MainWindow()
        {
            InitializeComponent();
            Directory.CreateDirectory("OutputDocuments");
            Directory.CreateDirectory("Templates");
            if(Directory.Exists("Templates/template_cursach.docx")) inputTemplateName = Directory.GetCurrentDirectory() + "/Templates/template_cursach.docx";
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            //Get_StudentsGroups();
        }

        private void BtnCreateDocument_Click(object sender, RoutedEventArgs e)
        {
            if(File.Exists("OutputDocuments/OutputDocument.docx"))
            {
                if(MessageBox.Show("Файл с отчётом уже существует, перезаписать?", "Внимание!", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    File.Delete("OutputDocuments/OutputDocument.docx");
                    if (inputTemplateName != string.Empty)
                    {
                        File.Copy(inputTemplateName, "OutputDocument.docx");
                        var valuesToFill = new Content(
                            new FieldContent("Student_FIO", TbFIO.Text),
                            new FieldContent("Student_Group", TbGroups.Text)
                            );
                        using (var outputDocument = new TemplateProcessor("OutputDocument.docx"))
                        {
                            outputDocument.FillContent(valuesToFill);
                            outputDocument.SaveChanges();
                        }
                        File.Move("OutputDocument.docx", "OutputDocuments/OutputDocument.docx");
                    }
                    else
                    {
                        MessageBox.Show("Выберите шаблон для отчёта!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Warning);
                        BtnLoadTemplate_Click(sender, e);
                    }
                }
                
            }
            else
            {
                if (inputTemplateName != string.Empty)
                {
                    File.Copy(inputTemplateName, "OutputDocument.docx");
                    var valuesToFill = new Content(
                        new FieldContent("Student_FIO", TbFIO.Text),
                        new FieldContent("Student_Group", TbGroups.Text)
                        );
                    using (var outputDocument = new TemplateProcessor("OutputDocument.docx"))
                    {
                        outputDocument.FillContent(valuesToFill);
                        outputDocument.SaveChanges();
                    }
                    File.Move("OutputDocument.docx", "OutputDocuments/OutputDocument.docx");
                }
                else
                {
                    MessageBox.Show("Выберите шаблон для отчёта!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    BtnLoadTemplate_Click(sender, e);
                }
            }
        }

        private void BtnLoadTemplate_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "template"; 
            dlg.DefaultExt = ".docx"; 
            dlg.Filter = "Word documents (.docx)|*.docx"; 

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                inputTemplateName = dlg.FileName;
            }
            if(File.Exists("Templates/" + dlg.SafeFileName))
            {
                File.Delete("Templates/" + dlg.SafeFileName);
            }
            File.Copy(inputTemplateName, "Templates/" + dlg.SafeFileName);
        }
    }
}

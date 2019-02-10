using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Xml.Serialization;

namespace WpfSampleApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ObservableCollection<Customer> custdata = new ObservableCollection<Customer>();
            DG1.DataContext = custdata;
        }

        private void itemLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Xml files (*.xml)|*.xml|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                ObservableCollection<Customer> custdata = GetData(openFileDialog.FileName);
                DG1.DataContext = custdata;
            }
        }

        private ObservableCollection<Customer> GetData(string filename)
        {
            var data = new ObservableCollection<Customer>();
            try
            {
                using (StreamReader reader = new StreamReader(filename))
                {
                    reader.ReadToEnd();

                    // it is a hack for returning stream to the beginning
                    // (without it the error "there is an error in xml document (0, 0)" is generated)
                    reader.DiscardBufferedData();
                    reader.BaseStream.Seek(0, SeekOrigin.Begin);

                    XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Customer>));
                    data = (ObservableCollection<Customer>)serializer.Deserialize(reader);
                }
            }
            // TODO: catching generic exception is not a good idea
            // it should intercept IO exceptions, serialization exceptions separately and react differently
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                // TODO: ideally it must report to the error log, not just the messagebox
            }

            return data;
        }

        private void itemSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Xml files (*.xml)|*.xml|All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == true)
            {
                SerializeData(saveFileDialog.FileName, (ObservableCollection<Customer>) DG1.DataContext);
            }
        }

        private void SerializeData(string filename, ObservableCollection<Customer> data)
        {
            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(ObservableCollection<Customer>));
                TextWriter writer = new StreamWriter(filename);
                ser.Serialize(writer, data);
                writer.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                // TODO: ideally it must report to the error log, not just the messagebox
            }
        }

        private void itemExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}

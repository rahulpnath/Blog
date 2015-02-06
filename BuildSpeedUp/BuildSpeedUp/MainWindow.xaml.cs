using System.Windows;

namespace BuildSpeedUp
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Xml;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private string solutionPath;

        private void LoadProjects(object sender, RoutedEventArgs e)
        {
             this.solutionPath = solutionPathTextBox.Text;

            if (string.IsNullOrWhiteSpace(solutionPath) || !Directory.Exists(solutionPath))
            {
                MessageBox.Show("Please enter a vaild solution path");
                return;
            }

            var allProjectFiles = Directory.GetFiles(solutionPath, "*.csproj", SearchOption.AllDirectories);
            this.allProjects.ItemsSource = allProjectFiles;
        }

        private void UpdateReferences(object sender, RoutedEventArgs e)
        {
            var commonrelativePath = commonPathRelative.Text;

            if (string.IsNullOrWhiteSpace(commonrelativePath))
            {
                MessageBox.Show("Enter a relative path to set the dlls output");
                return;
            }
            var solutionUri = new Uri(commonrelativePath);

            foreach (var selectedItem in allProjects.SelectedItems)
            {
                var selectedProject = selectedItem as string;
                var selectedUri = new Uri(selectedProject);
                var relative = selectedUri.MakeRelativeUri(solutionUri).ToString();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(selectedProject);

                // For changing the output path

                XmlNamespaceManager mgr = new XmlNamespaceManager(xmlDoc.NameTable);
                mgr.AddNamespace("x", "http://schemas.microsoft.com/developer/msbuild/2003");
                foreach (XmlNode item in xmlDoc.SelectNodes("//x:OutputPath", mgr))
                {
                    item.InnerText = relative;
                }

                // For making dll reference copy local false

                foreach (XmlNode item in xmlDoc.SelectNodes("//x:Reference", mgr))
                {
                    bool isPrivateNodeExists = false;
                    foreach (XmlNode childNode in item.ChildNodes)
                    {
                        if (childNode.Name == "Private")
                        {
                            isPrivateNodeExists = true;
                            childNode.InnerText = bool.FalseString;
                        }
                    }
                    if (!isPrivateNodeExists)
                    {
                        var privateElement = xmlDoc.CreateElement("Private", xmlDoc.DocumentElement.NamespaceURI);
                        privateElement.InnerText = bool.FalseString;
                        item.AppendChild(privateElement);
                    }
                }

                // For making project reference copy local false

                //foreach (XmlNode item in xmlDoc.SelectNodes("//x:ProjectReference", mgr))
                //{
                //    bool isPrivateNodeExists = false;
                //    foreach (XmlNode childNode in item.ChildNodes)
                //    {
                //        if (childNode.Name == "Private")
                //        {
                //            isPrivateNodeExists = true;
                //            childNode.InnerText = bool.FalseString;
                //        }
                //    }
                //    if (!isPrivateNodeExists)
                //    {
                //        var privateElement = xmlDoc.CreateElement("Private", xmlDoc.DocumentElement.NamespaceURI);
                //        privateElement.InnerText = bool.FalseString;
                //        item.AppendChild(privateElement);
                //    }
                //}

                xmlDoc.Save(selectedProject);
            }
        }
    }
}

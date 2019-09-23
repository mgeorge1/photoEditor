using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
namespace photoEditor1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        bool loadingJPEGs = false;

        private void LoadDirectories(String dir)
        {
            treeView1.Nodes.Clear();
        //https://docs.microsoft.com/en-us/dotnet/api/system.io.directoryinfo.getdirectories?view=netframework-4.8#System_IO_DirectoryInfo_GetDirectories
        //https://www.c-sharpcorner.com/article/display-sub-directories-and-files-in-treeview/
        //https://docs.microsoft.com/en-us/dotnet/api/system.io.directoryinfo.getfiles?view=netframework-4.8

            DirectoryInfo directoryInfo = new DirectoryInfo(dir);
            TreeNode topNode = treeView1.Nodes.Add("TOP", directoryInfo.Name);
            topNode.Tag = directoryInfo.FullName;
            topNode.Checked = true;

            

            recursiveDirectoryLoader(topNode, directoryInfo);
        }

        private void recursiveDirectoryLoader(TreeNode treeNode, DirectoryInfo directory)
        {

            DirectoryInfo directoryInfo = new DirectoryInfo(directory.FullName);
            DirectoryInfo[] subDirectories = directoryInfo.GetDirectories();
            if (subDirectories.Count() > 0)
            {
                foreach (DirectoryInfo subDirectory in subDirectories)
                {
                    if (subDirectory.GetDirectories().Count() > 0)
                    {
                        TreeNode treeNode1 = treeNode.Nodes.Add(subDirectory.Name);
                        treeNode1.Tag = subDirectory.FullName;
                        recursiveDirectoryLoader(treeNode1, subDirectory);
                    }
                    else
                    {
                        treeNode.Nodes.Add(subDirectory.Name).Tag = (String)subDirectory.FullName;
                    }
                }
            }
        }

        async private void LoadJPEGsFromDirectoryAsync(String dir)
        {
            treeView1.Enabled = false;
            listView1.Clear();
            await Task.Run(() =>
            {
                Invoke((Action)delegate ()
                {
                    progressBar1.Visible = true;
                    progressBar1.Style = ProgressBarStyle.Marquee;
                });
                //https://docs.microsoft.com/en-us/dotnet/api/system.io.searchoption?view=netframework-4.8
                DirectoryInfo directory = new DirectoryInfo(dir);
                FileInfo[] files = directory.GetFiles("*.JPG", SearchOption.TopDirectoryOnly);

                ImageList imageListSmall = new ImageList();
                ImageList imageListLarge = new ImageList();
                imageListLarge.ImageSize = new Size(32, 32);
                foreach (FileInfo file in files)
                {
                    try
                    {
                        byte[] bytes = System.IO.File.ReadAllBytes(file.FullName);
                        MemoryStream ms = new MemoryStream(bytes);
                        Image img = Image.FromStream(ms); // Use this instead of Image.FromFile()
                        Console.WriteLine("Filename: " + file.Name);
                        Console.WriteLine("Last mod: " + file.LastWriteTime.ToString());
                        Console.WriteLine("File size: " + file.Length);
                        Invoke((Action)delegate ()
                        {
                            imageListSmall.Images.Add(new Bitmap(img));
                            imageListLarge.Images.Add(new Bitmap(img));
                        });
                    }
                    catch
                    {
                        Console.WriteLine("This is not an image file");
                    }



                    //pictureBox1.Image = Image.FromFile((String)file.FullName);
                    ListViewItem item = new ListViewItem(file.Name);   // Text and image index
                    item.SubItems.Add(file.CreationTime.ToString());   // Column 2
                    item.SubItems.Add(file.Length.ToString());         // Column 3
                    item.Tag = file.FullName;

                    Invoke((Action)delegate ()
                    {
                        listView1.Items.Add(item);
                    

                    listView1.LargeImageList = imageListLarge;
                    listView1.SmallImageList = imageListSmall;
                    });

                }
            });
            // Create columns (Width of -2 indicates auto-size)
            listView1.Columns.Add("Name", 200, HorizontalAlignment.Left);
            listView1.Columns.Add("Date", 200, HorizontalAlignment.Left);
            listView1.Columns.Add("Size", 100, HorizontalAlignment.Right);

            Invoke((Action)delegate ()
            {
                progressBar1.Visible = false;
                progressBar1.Style = ProgressBarStyle.Blocks;

            });
            treeView1.Enabled = true;
        }
        private void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
                label1.Text = (String)treeView1.SelectedNode.Tag;
                LoadJPEGsFromDirectoryAsync(label1.Text);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadDirectories("C:\\Users\\Maggie\\Pictures");
            listView1.Clear();
            LoadJPEGsFromDirectoryAsync("C:\\Users\\Maggie\\Pictures");
        }

        private void TreeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
        }

        private void ListView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            label1.Text = (String)listView1.SelectedItems[0].Tag;
        }

        private void LocateOnDiskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //https://stackoverflow.com/questions/9646114/open-file-location
            if(listView1.SelectedItems[0]!=null)
            Process.Start("explorer.exe", "/select, " + (String)listView1.SelectedItems[0].Tag);
        }

        private void SelectRootFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            String newPath = folderBrowserDialog1.SelectedPath;
            label1.Text = newPath;
            LoadDirectories(newPath);
            LoadJPEGsFromDirectoryAsync(newPath);
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm aboutForm = new AboutForm();
            aboutForm.ShowDialog(this);
        }

        private void SmallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.View = View.SmallIcon;

        }

        private void LargeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.View = View.LargeIcon;
        }

        private void DetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.View = View.Details;
        }

        private void DetailsToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            detailsToolStripMenuItem.Checked = !detailsToolStripMenuItem.Checked;
        }

        private void SmallToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            smallToolStripMenuItem.Checked = !smallToolStripMenuItem.Checked;
        }

        private void LargeToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            largeToolStripMenuItem.Checked = !largeToolStripMenuItem.Checked;
        }
    }
}
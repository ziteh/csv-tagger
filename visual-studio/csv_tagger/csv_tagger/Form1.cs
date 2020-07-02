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
using System.Globalization;
using CsvHelper;

namespace csv_tagger
{
    public partial class Form1 : Form
    {
        public tagsCol[] tagsDatabase = new tagsCol[100];
        public string MessageText;
        public int maxTagLayer = 0;
        public Form1()
        {

            InitializeComponent();

            // Init tagsDatabase
            for (int i = 0; i < 100; i++)
            {
                tagsDatabase[i] = new tagsCol();
                tagsDatabase[i].tagsLayer = new string[10];
            }

            // Read tags_database.csv
            using (var reader = new StreamReader(@"../../../../../csv/tags_database.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<Tags>();

                foreach (var tags in records)
                {
                    tagsDatabase[(tags.serial_number - 1)].serialNumber = tags.serial_number;

                    tagsDatabase[(tags.serial_number - 1)].tagsLayer[0] = tags.tags_layer0;
                    tagsDatabase[(tags.serial_number - 1)].tagsLayer[1] = tags.tags_layer1;
                    tagsDatabase[(tags.serial_number - 1)].tagsLayer[2] = tags.tags_layer2;
                    tagsDatabase[(tags.serial_number - 1)].tagsLayer[3] = tags.tags_layer3;
                    tagsDatabase[(tags.serial_number - 1)].tagsLayer[4] = tags.tags_layer4;
                    tagsDatabase[(tags.serial_number - 1)].tagsLayer[5] = tags.tags_layer5;
                    tagsDatabase[(tags.serial_number - 1)].tagsLayer[6] = tags.tags_layer6;
                    tagsDatabase[(tags.serial_number - 1)].tagsLayer[7] = tags.tags_layer7;
                    tagsDatabase[(tags.serial_number - 1)].tagsLayer[8] = tags.tags_layer8;
                    tagsDatabase[(tags.serial_number - 1)].tagsLayer[9] = tags.tags_layer9;
                }
            }

            sortTags();
//            MessageBox.Show("OK");
//            creatTagsTreeView();
            for (int i = 0; i <50; ++i)
            {
                MessageText += tagsDatabase[i].layer.ToString() + "\n";
            }
            MessageBox.Show(MessageText+"maxlayer:"+ maxTagLayer.ToString());
        }

        public void sortTags()
        {
            for (int i = 0; i < 100; ++i)
            {
                for (int layer = 0; layer != -1;)
                {
                    // empty-tags
                    if(tagsDatabase[i].tagsLayer[layer] == null)
                    {
                        tagsDatabase[i].type = tagType.emptyTag;
                        tagsDatabase[i].layer = -1;

                        // Break loop
                        layer = -1;
                    }
                    // sub-tags
                    else if (tagsDatabase[i].tagsLayer[layer] == "@")
                    {
                        // Next layer
                        ++layer;

                        // Uadate max-tag-layer
                        if (layer > maxTagLayer)
                        {
                            maxTagLayer = layer;
                        }
                    }
                    // folder-tags
                    else if (tagsDatabase[i].tagsLayer[layer].IndexOf("#") == 0)
                    {
                        tagsDatabase[i].type = tagType.folderTag;
                        tagsDatabase[i].layer = layer;

                        // Break loop
                        layer = -1;
                    }
                    // normal-tags
                    else
                    {
                        tagsDatabase[i].type = tagType.normalTag;
                        tagsDatabase[i].layer = layer;
                        
                        // Break loop
                        layer = -1;
                    }
                }
            }
        }

        public void creatTagsTreeView()
        {
            for (int i = 0; i<100; ++i)
            {
                int j = 0;

                // sub-tags
                if (tagsDatabase[i].tagsLayer[j] == "@")
                {

                }
                // folder-tags
                else if (tagsDatabase[i].tagsLayer[j] == "#")
                {

                }
                // normal-tags
                else
                {
                    System.Windows.Forms.TreeNode treeNode = new System.Windows.Forms.TreeNode(tagsDatabase[i].tagsLayer[j]);
                }
            }


            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Node0-0-0");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Node0-0", new System.Windows.Forms.TreeNode[] {
            treeNode1});
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Node0-1");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Node0-2");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Node0-3");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Node0", new System.Windows.Forms.TreeNode[] {
            treeNode2,
            treeNode3,
            treeNode4,
            treeNode5});
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Node1");
            treeNode1.Name = "Node0-0-0";
            treeNode1.Text = "Node0-0-0";
            treeNode2.Name = "Node0-0";
            treeNode2.Text = "Node0-0";
            treeNode3.Name = "Node0-1";
            treeNode3.Text = "Node0-1";
            treeNode4.Name = "Node0-2";
            treeNode4.Text = "Node0-2";
            treeNode5.Name = "Node0-3";
            treeNode5.Text = "Node0-3";
            treeNode6.Name = "Node0";
            treeNode6.Text = "Node0";
            treeNode7.Name = "Node1";
            treeNode7.Text = "Node1";
            this.treeViewTags.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode6,
            treeNode7});

        }
    }

    public class tagsCol
    {
        public int serialNumber;
        public string[] tagsLayer;
        public tagType type;
        public int layer;
    }
    public enum tagType
    {
        emptyTag,
        normalTag,
        folderTag
    }
}

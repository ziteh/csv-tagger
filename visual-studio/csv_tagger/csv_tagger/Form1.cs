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
            creatTagsTreeView();

            /*for (int i = 0; i <50; ++i)
            {
                MessageText += tagsDatabase[i].layer.ToString() + "\n";
            }
            MessageBox.Show(MessageText+"maxlayer:"+ maxTagLayer.ToString());*/
        }

        // Count layer of tag and get type of tag. 
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
            /*for (int i =0;i<50;++i)
            {
                for (int j = maxTagLayer; j >= 0; --j)
                {
                    if (tagsDatabase[i].layer == j)
                    {
                        System.Windows.Forms.TreeNode treeNode = new System.Windows.Forms.TreeNode(tagsDatabase[i].tagsLayer[(tagsDatabase[i].layer)]);
                    }
                }
            }*/
            string rootTag;
            rootTag = "rootTag1";
            treeViewTags.Nodes.Add(rootTag);

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

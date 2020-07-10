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
using System.Security.AccessControl;

namespace csv_tagger
{
    public partial class Form1 : Form
    {
        // Symbol.
        private const string symbolSubTag = "@";
        private const string symbolFolderTag = "#";

        // Path of CSV file.
        private const string pathTagsDatabaseCSV = @"../../../../../csv/tags_database.csv";

        private int maxTagRow = 50;
        private const int maxNumberOfLayer = 10;

        private tagsCol[] tagsDatabase = new tagsCol[100];

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
            using (var reader = new StreamReader(pathTagsDatabaseCSV))
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

            //                        MessageBox.Show("OK");

            arrangeTags();
            creatTreeViewTags();
            treeViewTags.ExpandAll();

//            MessageBox.Show(maxTagRow.ToString());
//            MessageBox.Show(tagsDatabase[maxTagRow].tagsLayer[0]);
        }

        private void creatTreeViewTags()
        {
            int row = 0;
            TreeNode node;

            while (isEmptyTag(row) == false)
            {
                // If it's root-tag.
                if (tagsDatabase[row].layer == 0)
                {
                    int lestRowOfThisRootTag = row;
                    int maxLayerOfThisRootTag = 0;

                    // Add root tag.
                    node = treeViewTags.Nodes.Add(tagsDatabase[row].tagsLayer[0]);

                    // Get the lest row of this root-tag and the max layer of this root-tag.
                    while (tagsDatabase[lestRowOfThisRootTag + 1].layer != 0)
                    {
                        ++lestRowOfThisRootTag;

                        if (tagsDatabase[lestRowOfThisRootTag].layer > maxLayerOfThisRootTag)
                        {
                            maxLayerOfThisRootTag = tagsDatabase[lestRowOfThisRootTag].layer;
                        }
                    }

                    for (int layer = 1; layer < maxLayerOfThisRootTag + 1; ++layer)
                    {
                        // Have sub-tag.
                        for (int j = row + 1; j < lestRowOfThisRootTag + 1; ++j)
                        {
                            // If next tag isn't my sub-tag.
                            if ((tagsDatabase[j].layer == layer) &&
                                (tagsDatabase[j + 1].layer != layer + 1))
                            {
                                node.Nodes.Add(tagsDatabase[j].tagsLayer[tagsDatabase[j].layer]);
                            }
                        }

                        // Didn't have sub-tag.
                        for (int j = row + 1; j < lestRowOfThisRootTag + 1; ++j)
                        {
                            // If next tag is my sub-tag.
                            if ((tagsDatabase[j].layer == layer) &&
                                (tagsDatabase[j + 1].layer == layer + 1))
                            {
                                node = node.Nodes.Add(tagsDatabase[j].tagsLayer[tagsDatabase[j].layer]);
                            }
                        }
                    }
                }
                // Next row of tagsDatabase.
                ++row;

                // Update max row.
                maxTagRow = row-1;
            }
        }

        private void arrangeTags()
        {
            for (int row = 0; row < 50; ++row)
            {
                tagsDatabase[row].layer = getLayerOfTag(row);
                tagsDatabase[row].type = getTypeOfTag(row);
            }
        }

        /// <summary>
        /// Get the layer of a row of tagsDatabase.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private int getLayerOfTag(int row)
        {
            int layer = 0;

            if (isEmptyTag(row))
            {
                // empty-tags
                layer = -1;
            }
            else
            {
                while (tagsDatabase[row].tagsLayer[layer] == symbolSubTag)
                {
                    // Next layer.
                    ++layer;
                }
            }

            return (int)layer;
        }

        /// <summary>
        /// Get the type of a row of tagsDatabase.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private tagType getTypeOfTag(int row)
        {
            tagType type;

            // empty-tags
            if (isEmptyTag(row))
            {
                type = tagType.emptyTag;
            }
            // folder-tags
            else if (tagsDatabase[row].tagsLayer[getLayerOfTag(row)].IndexOf(symbolFolderTag) == 0)
            {
                type = tagType.folderTag;
            }
            // normal-tags
            else
            {
                type = tagType.normalTag;
            }

            return (tagType)type;
        }

        /// <summary>
        /// Return TRUE if the tag is empty-tag.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private bool isEmptyTag(int row)
        {
            if(string.IsNullOrEmpty(tagsDatabase[row].tagsLayer[0]) == true)
                return true;
            else
                return false;
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

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

        private const int maxNumberOfLayer = 10;

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

            //            sortTags();
            //            MessageBox.Show("OK");
            arrangeTags();
            creatTreeViewTags();

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
                    if (tagsDatabase[i].tagsLayer[layer] == null)
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

        public void creatTreeViewTags()
        {
            int row = 0;
            int layer = 0;
            TreeNode node;

            while (isEmptyTag(row) == false)
            {
                // Update layer.
                layer = getLayerOfTag(row);

                if (layer == 0)
                {
                    // Add root tag.
                    node = treeViewTags.Nodes.Add(tagsDatabase[row].tagsLayer[0]);
                }
                else
                {
                    if (layer == 1)
                    {

                    }
                }
            }

            node = treeViewTags.Nodes.Add("Master node");
            node.Nodes.Add("Child node");
            node.Nodes.Add("Child node 2");
            node = node.Nodes.Add("CN 3");
            node.Nodes.Add("CCN1");

            node = treeViewTags.Nodes.Add("Master node 2");
            node.Nodes.Add("mychild");
            node.Nodes.Add("mychild");

            treeViewTags.ExpandAll();
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
                while (tagsDatabase[row].tagsLayer[layer] == "@")
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
            else if (tagsDatabase[row].tagsLayer[getLayerOfTag(row)].IndexOf("#") == 0)
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
            if (tagsDatabase[row].tagsLayer[0] == null)
                return true;
            else
                return false;
        }

        private tagRelationType getNextTagRelation (int myRow)
        {
            int myTagLayer = getLayerOfTag(myRow);
            int nextTagLayer = getLayerOfTag(myRow + 1);

            if (nextTagLayer == myTagLayer + 1)
                return tagRelationType.mySubTag;
            else if (nextTagLayer == myTagLayer - 1)
                return tagRelationType.myMasterTag;
            else if (nextTagLayer == myTagLayer)
                return tagRelationType.myPeerTag;
            else
                return tagRelationType.noRelation;
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

    public enum tagRelationType
    {
        mySubTag,
        myPeerTag,
        myMasterTag,
        noRelation
    }
}

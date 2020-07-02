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
        public Form1()
        {
            tagsCol[] tagsDatabase = new tagsCol[100];

            InitializeComponent();

            // Init tagsDatabase
            for (int i = 0; i < 100; i++)
            {
                tagsDatabase[i] = new tagsCol();
                tagsDatabase[i].tagsLayer = new string[10];
            }

            // Read tags_database.csv
            using (var reader = new StreamReader(@"C:\Users\wk415\Google Drive (wk415937@gmail.com)\01-Projects\PFDB_system\tags_database.csv"))
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
        }
    }
    public class tagsCol
    {
        public int serialNumber;
        public string[] tagsLayer;
    }
}

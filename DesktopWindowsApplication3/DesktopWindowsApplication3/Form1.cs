using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;

namespace DesktopWindowsApplication3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        IMapDocument mapDoc;

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            mapDoc = new MapDocumentClass();
            mapDoc.Open(textBox4.Text);
            IMap map = mapDoc.get_Map(0);
            axMapControl1.Map = map;
            IActiveView actv = map as IActiveView;
            axMapControl1.ActiveView.Extent = actv.Extent;
            axMapControl1.Refresh();
            for (int i = 0; i < mapDoc.MapCount; i++) 
            {
                listBox1.Items.Add(mapDoc.get_Map(i).Name);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            IMap mymap = mapDoc.get_Map(listBox1.SelectedIndex);
            axMapControl1.Map = mymap;
            IActiveView actv = mymap as IActiveView;
            axMapControl1.ActiveView.Extent = actv.Extent;
            axMapControl1.Refresh();
            listBox2.Items.Clear();
            IEnumLayer mylayers = mymap.get_Layers();
            ILayer mylayer = mylayers.Next();
            while(mylayer!=null)
            {
                listBox2.Items.Add(mylayer.Name);
                mylayer.Visible = true;
                mylayer = mylayers.Next();

            }
        }

        private void axMapControl1_OnMouseDown(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnMouseDownEvent e)
        {
            label1.Text = e.mapX + " , " + e.mapY;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
            IMap focusmap = axMapControl1.ActiveView.FocusMap;
            IWorkspaceFactory workspacefact = new AccessWorkspaceFactoryClass();
            IWorkspace workspace;
            workspace = workspacefact.OpenFromFile(textBox3.Text, 0);
            IFeatureWorkspace featworkspace = workspace as IFeatureWorkspace;
            IFeatureClass featclass;
            featclass = featworkspace.OpenFeatureClass(textBox2.Text);
            IFeatureLayer featlayer = new FeatureLayerClass();

            //label4.Text = textBox3.Text;
            //label2.Text = x;



            featlayer.FeatureClass = featclass;

            featlayer.Name = textBox2.Text;

            focusmap.AddLayer(featlayer);

            //focusmap.MoveLayer(featlayer, 1);
            Refreshlayers();
        }
        private void Refreshlayers()
        {
            ILayer alayer;
            IEnumLayer allLayers;
            listBox2.Items.Clear();
            allLayers = axMapControl1.ActiveView.FocusMap.get_Layers(null, true);
            alayer = allLayers.Next();
            while (alayer != null)
            {
                listBox2.Items.Add(alayer.Name);
                alayer = allLayers.Next();
            }
            axMapControl1.Refresh();
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

            int i = listBox2.SelectedIndex;
            try
            {

                IMap mymap = mapDoc.get_Map(listBox1.SelectedIndex);
                axMapControl1.Map = mymap;
                IActiveView act = mymap as IActiveView;
                axMapControl1.ActiveView.Extent = act.Extent;
                axMapControl1.Refresh();
                IEnumLayer mLayers = mymap.get_Layers();
                ILayer Layer = mLayers.Next();
                while (Layer != null)
                {
                    if (Layer.Name.ToString() == listBox2.Items[i].ToString())
                    {
                        mymap.DeleteLayer(Layer);
                        break;
                    }
                    Layer.Visible = true;
                    Layer = mLayers.Next();
                }
                listBox2.Items.RemoveAt(i);
                // listBox2.Items.Clear();
            }
            catch
            {
                MessageBox.Show("please select a layer to delete please");

            }

           
        }

        private void button7_Click(object sender, EventArgs e)
        {
            IFeatureLayerDefinition featlydef;
            featlydef = (IFeatureLayerDefinition)(axMapControl1.ActiveView.FocusMap.get_Layer(listBox2.SelectedIndex));
            featlydef.DefinitionExpression = textBox1.Text;
            axMapControl1.ActiveView.Refresh();


        }

        private void button8_Click(object sender, EventArgs e)
        {
            IPageLayout pagely = mapDoc.PageLayout;
            IActiveView activeview = (IActiveView)pagely;
            IMap map = activeview.FocusMap;

            activeview = (IActiveView)mapDoc.PageLayout;
           // activeview.Activate(GetDesktopWindow());

            map.MapScale = 0.0000000002;
                activeview.Refresh();
                mapDoc.Save(true, true);
        }
    }
}
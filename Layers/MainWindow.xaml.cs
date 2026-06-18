//
// Copyright (c) 2012, MindFusion LLC - Bulgaria.
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace MindFusion.Mapping.Wpf.Samples.CS.Layers
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
		}

		private void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			mapFiles = GetFiles("Maps", true);
			
			for (int i = 0; i < mapFiles.Count; i++)
			{
				FileInfo file = mapFiles[i];
				if (file.Name == "ne_50m_admin_0_countries.shp")
				{
					MapLayer layer0 = new MapLayer();
					layer0.Visible = true;
					mapView.Layers.Add(layer0);

					LoadMap(i, 0);
				}
				else if (file.Name == "ne_50m_urban_areas.shp")
				{
					MapLayer layer1 = new MapLayer();
					layer1.Visible = false;
					layer1.LineColor = Colors.Yellow;
					layer1.FillColors[0] = Colors.Yellow;
					mapView.Layers.Add(layer1);

					LoadMap(i, 1);
				}
				else if (file.Name == "50m_rivers_lake_centerlines.shp")
				{
					MapLayer layer2 = new MapLayer();
					layer2.Visible = false;
					layer2.LineColor = Colors.Blue;
					mapView.Layers.Add(layer2);

					LoadMap(i, 2);
				}
			}

		
			mapView.Focus();
		}

		private void LoadMap(int index, int layer)
		{
			MapLayer mapLayer = mapView.Layers[layer] as MapLayer;
			
			Map map = new Map();			
			map.LoadFromFile(mapFiles[index].FullName);
			mapLayer.Map = map;
		}

		private List<FileInfo> GetFiles(string folderName, bool master)
		{
			List<FileInfo> allFiles = new List<FileInfo>();
			string testDirName = master ? @"..\" + folderName + @"\" : folderName;


			for (int i = 0; i < 10; i++)
			{
				if (Directory.Exists(testDirName))
					break;
				testDirName = @"..\" + testDirName;
			}

			if (Directory.Exists(testDirName))
			{
				var dir = new DirectoryInfo(testDirName);

				FileInfo[] files = dir.GetFiles("*.shp");
				foreach (FileInfo f in files)
					allFiles.Add(f);

				DirectoryInfo[] directories = dir.GetDirectories();
				foreach (DirectoryInfo info in directories)
				{
					List<FileInfo> subfiles = GetFiles(info.FullName, false);
					if (subfiles.Count > 0)
					{
						allFiles.Add(new FileInfo(info.Name));
						foreach (FileInfo file in subfiles)
							allFiles.Add(file);
					}
				}
			}

			return allFiles;
		}
	

		private void ChBxShowLayer1_Checked(object sender, RoutedEventArgs e)
		{
			if (mapView != null && mapView.Layers.Count > 0)
				mapView.Layers[0].Visible = true;
		}

		private void ChBxShowLayer2_Checked(object sender, RoutedEventArgs e)
		{
			if (mapView != null && mapView.Layers.Count > 0)
				mapView.Layers[1].Visible = true;
		}

		private void ChBxShowLayer3_Checked(object sender, RoutedEventArgs e)
		{
			if (mapView != null && mapView.Layers.Count > 0)
				mapView.Layers[2].Visible = true;
		}

		private void ChBxShowLayer1_Unchecked(object sender, RoutedEventArgs e)
		{
			if (mapView != null && mapView.Layers.Count > 0)
				mapView.Layers[0].Visible = false;
		}

		private void ChBxShowLayer2_Unchecked(object sender, RoutedEventArgs e)
		{
			if (mapView != null && mapView.Layers.Count > 0)
				mapView.Layers[1].Visible = false;
		}

		private void ChBxShowLayer3_Unchecked(object sender, RoutedEventArgs e)
		{
			if (mapView != null && mapView.Layers.Count > 0)
				mapView.Layers[2].Visible = false;
		}

		private List<FileInfo> mapFiles;	
	}
}

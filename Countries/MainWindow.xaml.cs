//
// Copyright (c) 2012, MindFusion LLC - Bulgaria.
//

using System;
using System.IO;
using System.Windows;
using System.Windows.Media;


namespace MindFusion.Mapping.Wpf.Samples.CS.Countries
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			// find the folder containing admin. division maps
			var countryFolder = FindFolder("countries_level_1");
			if (countryFolder != null)
			{
				// get all map files
				var dir = new DirectoryInfo(countryFolder);
				mapFiles = dir.GetFiles("*.shp");
				foreach (var map in mapFiles)
				{
					// add map name to listbox
					var name = System.IO.Path.GetFileNameWithoutExtension(map.Name);
					lbCountries.Items.Add(name);
				}

				// select first country
				lbCountries.SelectedIndex = 0;
			}

			mapView.Focus();
		}	

		private void lbCountries_SelectedIndexChanged(object sender, EventArgs e)
		{
			int index = lbCountries.SelectedIndex;
			if (mapFiles != null && index >= 0 && index < mapFiles.Length)
			{
				lblCountryName.Text = lbCountries.SelectedItem.ToString();
				LoadMap(index);
			}
		}

		private string FindFolder(string folderName)
		{
			string testDir = @"..\Maps\";
			for (int i = 0; i < 10; i++)
			{
				if (Directory.Exists(testDir))
					break;
				testDir = @"..\" + testDir;
			}

			var testFolder = testDir + folderName;
			if (Directory.Exists(testFolder))
				return testFolder;

			return "";
		}

		private void LoadMap(int index)
		{
			MapLayer layer = mapView.Layers[0] as MapLayer;
			layer.LineColor = Colors.Black;

			Map map = new Map();
			map.LoadFromFile(mapFiles[index].FullName, true, "NAME_1");
			map.AssignColors();
			layer.Map = map;
		}

		FileInfo[] mapFiles;		
	}
}
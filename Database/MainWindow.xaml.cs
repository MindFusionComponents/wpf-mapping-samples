//
// Copyright (c) 2012, MindFusion LLC - Bulgaria.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Diagnostics;


namespace MindFusion.Mapping.Wpf.Samples.CS.Database
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

		void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			var countryFolder = FindFolder("50m_cultural");
			if (countryFolder != null)
			{
				var dir = new DirectoryInfo(countryFolder);
				FileInfo[] mapFiles = dir.GetFiles("*.shp");
				foreach (var file in mapFiles)
				{
					if (file.Name == "ne_50m_admin_0_countries.shp")
						LoadMap(file.FullName);
				}
			}
			mapView.Focus();
		}

		private void mapView_MapElementClick(object sender, MindFusion.Mapping.Wpf.MapEventArgs e)
		{
			if (e.Button != MouseButton.Left)
				return;

			DateTime newClick = DateTime.Now;
			var shape = e.MapElement as MindFusion.Mapping.Shape;

			// if the shape is double-clicked then load database,
			// otherwise open a new tab and laod wiki
			if (newClick - firstClick > TimeSpan.FromSeconds(0.5))
			{
				stPnlProperties.Children.Clear();

				var layer = mapView.Layers[0] as MapLayer;
				var cols = layer.Map.Database.Columns;

				for (int i = 0; i < cols.Count; i++)
				{
					var column = cols[i];

					StackPanel panel = new StackPanel() { Orientation = Orientation.Horizontal, Background = brushes[i % 2] };
					panel.Children.Add(new TextBlock() { Text = column.Name, Width = 100, Foreground = new SolidColorBrush(Color.FromArgb(255, 100, 100, 100)) });
					panel.Children.Add(new TextBlock() { Text = shape.DatabaseRow[column] });
					stPnlProperties.Children.Add(panel);
				}
			}
			else
			{
				string s = "http://en.wikipedia.org/wiki/" + shape.Label.Text;
				Process.Start("IExplore.exe", s);
			}

			firstClick = newClick;
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

		private void LoadMap(string path)
		{
			MapLayer layer = mapView.Layers[0] as MapLayer;
			Map map = new Map();
			map.LoadFromFile(path, true, "NAME");
			map.AssignColors();
			layer.Map = map;
		}

		private DateTime firstClick = DateTime.Now;

		private Brush[] brushes = new Brush[]{
			new SolidColorBrush(Color.FromArgb(255, 242, 242, 242)),
			new SolidColorBrush(Color.FromArgb(255, 240, 255, 255))
		};
	}
}

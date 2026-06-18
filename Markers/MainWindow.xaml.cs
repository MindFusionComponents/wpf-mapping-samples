//
// Copyright (c) 2012, MindFusion LLC - Bulgaria.
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace MindFusion.Mapping.Wpf.Samples.CS.Markers
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
				if (mapFiles[i].Name == "ne_50m_admin_0_countries.shp")
				{
					mapIndex = i;
					break;
				}
			}

			MapLayer layer = new MapLayer();
			mapView.Layers.Add(layer);

			layer.EnableHighlight = false;
			layer.FillColors[0] = Color.FromArgb(255, 238, 232, 170);
			layer.LineColor = Colors.LightGray;

			LoadMap(mapIndex, 0);

			var decorationLayer = new DecorationLayer(layer.BoundingBox);
			mapView.Layers.Add(decorationLayer);

			// define some landmark data
			var landmarks = new[]
			{
				new
				{
					Image = "bigben100.png",
					Latitude = Map.Latitude(51, 32, 'N'),
					Longitude = Map.Longitude(0, -5, 'W'),
					Label = "Big Ben\nLondon",
					ImageX = 50, ImageY = 90
				},

				new
				{
					Image = "christ_redeemer.png",
					Latitude = Map.Latitude(22, 57, 'S'),
					Longitude = Map.Longitude(43, 12, 'W'),
					Label = "Christ the Redeemer\nRio de Janeiro",
					ImageX = 50, ImageY = 100
				},

				new
				{
					Image = "colosseum100.png",
					Latitude = Map.Latitude(41, 54, 'N'),
					Longitude = Map.Longitude(12, 27, 'E'),
					Label = "Coliseum\nRome",
					ImageX = 50, ImageY = 70
				},

				new
				{
					Image = "egypt100.png",
					Latitude = Map.Latitude(29, 58, 'N'),
					Longitude = Map.Longitude(31, 9, 'E'),
					Label = "Great Pyramid\nGiza",
					ImageX = 50, ImageY = 60
				},

				new
				{
					Image = "eiffel100.png",
					Latitude = Map.Latitude(48, 48, 'N'),
					Longitude = Map.Longitude(2, 20, 'E'),
					Label = "Eiffel Tower\nParis",
					ImageX = 50, ImageY = 80
				},

				new
				{
					Image = "liberty100.png",
					Latitude = Map.Latitude(40, 40.8, 'N'),
					Longitude = Map.Longitude(74, 0.2, 'W'),
					Label = "Statue of Liberty\nNew York",
					ImageX = 70, ImageY = 90
				},

				new
				{
					Image = "mayan_pyramid.png",
					Latitude = Map.Latitude(19, 41, 'N'),
					Longitude = Map.Longitude(98, 50, 'W'),
					Label = "Mayan Pyramids\nTeotihuacan",
					ImageX = 50, ImageY = 90
				},

				new
				{
					Image = "sydney_opera.png",
					Latitude = Map.Latitude(34, 0, 'S'),
					Longitude = Map.Longitude(151, 0, 'E'),
					Label = "Sydney Opera House\nSydney",
					ImageX = 70, ImageY = 90
				},

				new
				{
					Image = "tajmahal100.png",
					Latitude = Map.Latitude(27, 11, 'N'),
					Longitude = Map.Longitude(78, 1, 'E'),
					Label = "Taj Mahal\nAgra",
					ImageX = 50, ImageY = 70
				}
			};

			// create decoration images
			foreach (var landmark in landmarks)
			{
				var bmp = new BitmapImage(
					new Uri("/Markers;component/Images/" + landmark.Image, UriKind.Relative));
				var img = new DecorationImage(
					bmp,
					landmark.ImageX, landmark.ImageY,
					landmark.Latitude, landmark.Longitude);
				img.Label = new Label(landmark.Label);
				decorationLayer.Decorations.Add(img);
			}
			
			var office = new DecorationBubble(
				6, Colors.Blue, Colors.White,
				42 + 40.0 / 60, 23 + 20.0 / 60) { Label = new Label("MindFusion\nSofia") };
			decorationLayer.Decorations.Add(office);

			mapView.ZoomFactor *= 6;
			mapView.ScrollTo(office.Latitude, office.Longitude);
			mapView.Focus();
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

		private void LoadMap(int index, int layer)
		{
			MapLayer mapLayer = mapView.Layers[layer] as MapLayer;

			Map map = new Map();
			map.LoadFromFile(mapFiles[index].FullName);
			mapLayer.Map = map;
		}

		private List<FileInfo> mapFiles;
		private int mapIndex;
	}
}

//
// Copyright (c) 2012, MindFusion LLC - Bulgaria.
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace MindFusion.Mapping.Wpf.Samples.CS.Palette
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
				if (mapFiles[i].Name == "United States of America.shp")
				{
					mapIndex = i;
					break;
				}
			}

			MapLayer layer = new MapLayer();
			mapView.Layers.Add(layer);

			LoadMap(mapIndex, 0);

			CustomItem item = new CustomItem() { Text = "Agency", ImagePath = new Uri("pack://application:,,,/Palette;component/Images/Agency.png", UriKind.RelativeOrAbsolute) };
			CustomItem item2 = new CustomItem() { Text = "Factory", ImagePath = new Uri("pack://application:,,,/Palette;component/Images/Factory.png", UriKind.RelativeOrAbsolute) };
			CustomItem item3 = new CustomItem() { Text = "Office", ImagePath = new Uri("pack://application:,,,/Palette;component/Images/Office.png", UriKind.RelativeOrAbsolute) };
			CustomItem item4 = new CustomItem() { Text = "Warehouse", ImagePath = new Uri("pack://application:,,,/Palette;component/Images/Warehouse.png", UriKind.RelativeOrAbsolute) };

			AttachEvents(item);
			AttachEvents(item2);
			AttachEvents(item3);
			AttachEvents(item4);

			lbImages.Items.Add(item);
			lbImages.Items.Add(item2);
			lbImages.Items.Add(item3);
			lbImages.Items.Add(item4);
		}

		private void AttachEvents(CustomItem item)
		{
			item.MouseLeftButtonDown += new MouseButtonEventHandler(item_MouseLeftButtonDown);
		}

		private void item_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			// Select the clcked item and start dragging
			CustomItem draggedItem = sender as CustomItem;
			if (draggedItem == null)
				return;

			lbImages.SelectedItem = sender;
			DragDrop.DoDragDrop(draggedItem, draggedItem, DragDropEffects.Copy);
		}

		private void mapView_DragOver(object sender, DragEventArgs e)
		{
			var clientPoint = e.GetPosition(mapView);
			var geoLocation = mapView.ClientToMap(clientPoint);
			var state = mapView.BaseMap.HitTest(geoLocation);

			e.Effects = state != null ? DragDropEffects.Copy : DragDropEffects.None;
		}

		private void mapView_Drop(object sender, DragEventArgs e)
		{
			var clientPoint = e.GetPosition(mapView);
			var geoLocation = mapView.ClientToMap(clientPoint);
			var state = mapView.BaseMap.HitTest(geoLocation);

			e.Effects = state != null ? DragDropEffects.Copy : DragDropEffects.None;

			if (state != null)
			{
				DecorationLayer decorationLayer = mapView.Layers[1] as DecorationLayer;

				CustomItem item = e.Data.GetData(typeof(CustomItem)) as CustomItem;

				var decoration = new DecorationImage(
					new BitmapImage(item.ImagePath), geoLocation.Y, geoLocation.X);
				decoration.Label = new Label(item.Text +
					" in " + state.DatabaseRow["NAME_1"]);
				decorationLayer.Decorations.Add(decoration);
			}
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
					if (info.Name != "countries_level_0")
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
			}

			return allFiles;
		}

		private void LoadMap(int index, int layer)
		{
			MapLayer mapLayer = mapView.Layers[layer] as MapLayer;

			Map map = new Map();
			map.LoadFromFile(mapFiles[index].FullName, true, mapLayer.LabelField);
			mapLayer.Map = map;
			RefreshUI(mapLayer);
			mapView.ZoomFactor = 0;
		}

		private void RefreshUI(MapLayer layer)
		{
			var bounds = layer.Map.BoundingBox;
			bounds.XMin = -125;
			bounds.YMin = 23;
			bounds.YMax = 48;
			layer.Map.BoundingBox = bounds;

			DecorationLayer dLayer = new DecorationLayer(layer.Map.BoundingBox);
			mapView.Layers.Add(dLayer);
		}

		private List<FileInfo> mapFiles;
		private int mapIndex;
		private List<object> hitResultsList = new List<object>();
	}


	public class CustomItem : Control
	{
		public CustomItem()
		{
			DefaultStyleKey = typeof(CustomItem);
		}

		/// <summary>
		/// Gets or sets the path to the image associated this item.
		/// </summary>
		public Uri ImagePath
		{
			get { return (Uri)GetValue(ImagePathProperty); }
			set { SetValue(ImagePathProperty, value); }
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
		}

		/// <summary>
		/// Identifies the ImagePath dependency property.
		/// </summary>
		public static readonly DependencyProperty ImagePathProperty = DependencyProperty.Register(
			"ImagePath",
			typeof(Uri),
			typeof(CustomItem),
			new PropertyMetadata(new Uri("/Palette;component/Images/Agency.png", UriKind.Relative)));

		/// <summary>
		/// Gets or sets the label of this item.
		/// </summary>
		public string Text
		{
			get { return (string)GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}

		/// <summary>
		/// Identifies the Text dependency property.
		/// </summary>
		public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
			"Text",
			typeof(string),
			typeof(CustomItem),
			new PropertyMetadata("Item"));
	}
}

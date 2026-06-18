//
// Copyright (c) 2012, MindFusion LLC - Bulgaria.
//

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace MindFusion.Mapping.Wpf.Samples.CS.Explorer
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			this.Loaded += new RoutedEventHandler(UserControlLoaded);
		}

		private void UserControlLoaded(object sender, RoutedEventArgs e)
		{
			mapFiles = GetFiles("Maps", true);

			for (int i = 0; i < mapFiles.Count; i++)
			{
				FileInfo file = mapFiles[i];
				ListBoxItem item = null;
				if (file.Name.EndsWith(".shp"))
				{
					item = new FileListBoxItem();
					(item as FileListBoxItem).Text = System.IO.Path.GetFileNameWithoutExtension(file.Name);
				}
				else
				{
					item = new DirListBoxItem();
					(item as DirListBoxItem).Text = file.Name;
				}

				lbCountries.Items.Add(item);
			}

			lbCountries.SelectedIndex = 0;
			mapView.Focus();
		}

		private void lbCountries_SelectedIndexChanged(object sender, EventArgs e)
		{
			int index = lbCountries.SelectedIndex;
			if (mapFiles != null && index >= 0 && index < mapFiles.Count)
			{
				if (mapFiles[index].Name.EndsWith(".shp"))
					LoadMap(index);
			}
		}

		private void LoadMap(int index)
		{
			MapLayer layer = mapView.Layers[0] as MapLayer;

			Map map = new Map();
			map.LoadFromFile(mapFiles[index].FullName, true, "NAME_1");
			map.AssignColors();
			layer.Map = map;

			RefreshUI(layer);
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

		private void mapView_MapElementClick(object sender, MindFusion.Mapping.Wpf.MapEventArgs e)
		{
			// show respective list box row when a map shape is clicked
			var index = (mapView.Layers[0] as MapLayer).Map.Shapes.IndexOf((MindFusion.Mapping.Shape)e.MapElement);
			infoGrid.SelectedIndex = index;
		}


		private void RefreshUI(MapLayer layer)
		{
			if (layer == null)
				return;

			infoGrid.ItemsSource = null;
			infoGrid.View = new GridView();

			GridView gridView = infoGrid.View as GridView;
			gridView.Columns.Clear();

			foreach (var column in layer.Map.Database.Columns)
			{
				gridView.Columns.Add(new GridViewColumn()
				{
					Header = column.Name,
					DisplayMemberBinding = new Binding()
					{
						Converter = new RowConverter(),
						ConverterParameter = column.Name,
						Mode = BindingMode.OneTime,
					},
				});
			}
			infoGrid.ItemsSource = layer.Map.Database.Rows;
		}

		private void infoGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			// highlight map shape when a list box row is clicked
			if (infoGrid.SelectedItem != null)
			{
				var currentRow = infoGrid.SelectedIndex;
				var layer = mapView.Layers[0] as MapLayer;

				// this might happen while loading new maps
				if (currentRow >= layer.Map.Shapes.Count)
					return;

				// center on this shape if the map is zoomed in
				var shape = layer.Map.Shapes[currentRow];
				if (mapView.ZoomFactor > 1.5)
					mapView.ScrollTo(shape.GetCenter());

				// highlight the shape
				layer.Highlight(shape);
			}
		}

		class RowConverter : IValueConverter
		{
			public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
			{
				Row row = value as Row;
				string index = parameter as string;
				return row[index];
			}

			public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
			{
				throw new NotImplementedException();
			}
		}

		private List<FileInfo> mapFiles;
	}


	public class FileListBoxItem : ListBoxItem
	{
		public FileListBoxItem()
		{
			DefaultStyleKey = typeof(FileListBoxItem);
		}

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
			typeof(FileListBoxItem),
			new PropertyMetadata(""));

		public string MapLocation { set; get; }
		public string DatabaseLocation { set; get; }
	}


	public class DirListBoxItem : ListBoxItem
	{
		public DirListBoxItem()
		{
			DefaultStyleKey = typeof(DirListBoxItem);
		}

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
			typeof(DirListBoxItem),
			new PropertyMetadata(""));
	}
}

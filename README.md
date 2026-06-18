# WPF Mapping & GIS Samples (`wpf-mapping-samples`)

A comprehensive collection of C# WPF (Windows Presentation Foundation) sample applications demonstrating the geographic visualization, shapefile parsing, and interactive GIS capabilities of the **MindFusion.Mapping for WPF** control.

**Offline GIS & Shapefile Mapping:** MindFusion.Mapping enables WPF applications to load, render, style, and interact with professional **ESRI shapefiles** (`.shp`) and their associated attribute databases (`.dbf`) entirely offline without requiring external mapping web services.

All sample projects in this repository are pre-configured to reference the official **[MindFusion.Pack.Wpf](https://www.nuget.org/packages/MindFusion.Pack.Wpf) NuGet package** directly rather than referencing local files. This enables automatic package restore and seamless out-of-the-box building.

---

## 🚀 Key Features

*   **ESRI Shapefile Support:** Load and render professional vector map files, including polygons, polylines, and multi-point geographical layers.
*   **Integrated DBF Database:** Seamlessly read and query the relational attribute data (`.dbf`) linked to geographic shapes, enabling rich tooltips, labels, and property displays.
*   **Multi-Layer Architecture:** Support for an unlimited number of overlapping map layers, allowing you to combine administrative boundaries, physical geography, roads, and markers.
*   **GPS Markers & Decorations:** Place custom icons, text labels, and dynamic bubbles at specific GPS coordinates (latitude and longitude) using specialized decoration layers.
*   **Interactive Navigation:** Built-in support for smooth panning (drag-to-pan), mouse-wheel zooming, shape selection, and programmatic fit-to-view bounds.

---

## 📂 Samples Demonstrated

This repository includes **6 specialized GIS sample projects** showcasing various mapping features:

*   🌍 **Countries** — Shows the administrative divisions of world countries. Users can select and swap different maps from a panel, utilize mouse-wheel zooming, and pan across the geography by dragging.
*   🗄️ **Database** — Demonstrates the powerful link between map shapes and their underlying DBF database. Left-clicking a country displays its metadata in a property grid, while double-clicking programmatically queries and renders the country's Wikipedia article.
*   🧭 **Explorer** — Showcases bidirectional synchronization between spatial shapes and database views. Clicking a row in the database table highlights and centers the map shape, and clicking a shape highlights its database row.
*   🥞 **Layers** — Demonstrates how to compile several independent map layers (such as landmass, state lines, and rivers) into a single map viewport, showing how to dynamically toggle individual layer visibilities.
*   📍 **Markers** — Illustrates how to add custom markers, images, and bubbles (`DecorationImage`, `DecorationBubble`) to specific latitude and longitude coordinates using a decoration layer.
*   🎨 **Palette** — Implements an interactive image palette, demonstrating drag-and-drop mechanics to let users drag custom icons from a list and drop them onto the map canvas to place decorations at precise locations.

---

## ⚙️ Getting Started

### Prerequisites
*   **IDE:** Visual Studio 2022, 2026 or newer.
*   **Framework:** .NET Framework 8 SDK/Runtime.
*   **Package Manager:** NuGet (integrated natively into Visual Studio).

### How to Build & Run
1.  **Clone the Repository:**
    ```bash
    git clone https://github.com/MindFusionComponents/wpf-mapping-samples.git
    cd wpf-mapping-samples
    ```
2.  **Open a Sample:**
    *   Navigate to any sample folder (e.g., `Database`, `Markers`, or `Explorer`).
    *   Double-click the `.sln` or `.csproj` file to open it in Visual Studio.
3.  **Restore NuGet Packages:**
    *   When you build or debug the project, Visual Studio will automatically restore the missing `MindFusion.Mapping.Wpf` package and its dependencies.
4.  **Run:**
    *   Press `F5` or click **Start** in Visual Studio to compile and run the sample!

---

## 📄 License and Product Info

*   These samples are designed to work with **MindFusion.Mapping for WPF**.
*   For product documentation, licensing, or evaluation licenses, visit the [MindFusion Official Website](https://mindfusion.eu) and [WPF Mapping Product Page](https://mindfusion.dev/wpf-map.html).

# Dynmap Image Export [![Build artifact](https://img.shields.io/github/workflow/status/Virenbar/DynmapImageExport/Build%20Artifact?label=Build&logo=github)](https://github.com/Virenbar/DynmapImageExport/actions/workflows/build-artifact.yml)

Command line tool that downloads tiles from Dynmap HTTP server and merges them into one image.

![terminal](/assets/images/terminal.gif)

```text
.\DynmapImageExport <command> <url> [arguments] [options]

Commands:
    list, ls <url>                                        Show available worlds and maps
    i, info <url> <world> <map>                           Show info about map
    m, merge <url> <world> <map> <center> <range> <zoom>  Merge dynmap to image [center: [0,64,0], range: [2], zoom: 0]

Arguments:
    <url>     Dynmap URL
    <world>   World name
    <map>     Map name
    <center>  Center of image [x,y,z] [default: [0,64,0]]
    <range>   Range of image in tiles [all]|[vert,horz]|[top,right,bottom,left] [default: [2]]
    <zoom>    Zoom [default: 0]

Options:
    -?, -h, --help  Show help and usage information
    -v, --verbose   Show trace log
    -o, --output <output>  Output path
```

## Installation

* Install [.NET 6.x](https://dotnet.microsoft.com/download)
* Download [latest release](https://github.com/Virenbar/DynmapImageExport/releases)

## Usage

1. First see available worlds and maps

    ```text
    > .\DynmapImageExport list <url>

    Example:
    > .\DynmapImageExport ls https://map.minecrafting.ru/
    List for: https://map.minecrafting.ru/
    Available worlds and maps
    ├── world - Верхний мир
    │   ├── flat    Плоский
    │   └── se_view 3D юго-восток
    ...
    ```

2. Then available zoom levels

    ```text
    > .\DynmapImageExport info <url> <world> <map>

    Example:
    > .\DynmapImageExport i https://map.minecrafting.ru/ world flat
    Info for: https://map.minecrafting.ru/ - world - flat
    World: world - Верхний мир
    Map: flat - Плоский
    Perspective: iso_S_90_lowres PPB: 4
    Zoom levels
    ├── 0 - 4:1
    ├── 1 - 2:1
    ├── 2 - 1:1
    ├── 3 - 1:2
    ├── 4 - 1:4
    ├── 5 - 1:8
    └── 6 - 1:16
    ```

3. Make a image

    ```text
    > .\DynmapImageExport merge <url> <world> <map> <center> <range> <zoom>

    Example:
    > .\DynmapImageExport m https://map.minecrafting.ru/ world flat [0,100,0] [6,6,5,5] 2
    ```

Used arguments:

* `https://map.minecrafting.ru/` - Dynmap HTTP server
* `world` - World name
* `flat` - Map name
* `[0,100,0]` - Minecraft coordinates of central tile of image.
* `[6,6,5,5]` - Number of tiles in each direction from central tile. 6 to top, 6 to right, 5 to bottom, 5 to left.  
    This will produce 12x12 tiles image(1536x1536 in pixels)
* `2` - Zoom level. 1:1 scale in this example.

### Range notes

Range is number of tiles from center tile (i.e. padding)  

* `[2]` - 5x5 tiles image - 2 in each direction  
* `[2,3]` - 5x7 tiles image - 2 to top and bottom, 3 to right and left  
* `[2,3,2,2]` - 5x6 tiles image - 2 to top, 3 to right, 2 to bottom, 2 to left

## Example

### Flat map

```console
.\DynmapImageExport m https://map.minecrafting.ru/ world flat [0,100,0] [5,6,5,5] 2
```

![flat](/assets/images/Minecrafting.ru-flat.png)

### Isometric map

```console
.\DynmapImageExport m https://map.minecrafting.ru/ world se_view [0,100,0] [5,11,5,10]
```

![se_view](/assets/images/Minecrafting.ru-se_view.png)

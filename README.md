# DynmapImageExport [![Build artifact](https://img.shields.io/github/workflow/status/Virenbar/DynmapImageExport/Build%20Artifact?label=Build&logo=github)](https://github.com/Virenbar/DynmapImageExport/actions/workflows/build-artifact.yml)

Tool to download and merge Dynmap tiles into one image

## Usage

```text
DynmapImageExport [command] [options]

Commands:
list, ls  Show available worlds and maps
i, info   Show info about map
m, merge  Merge dynmap to image
```

1. Use list command to see available maps
2. Use info command to see valid zoom levels
3. Use merge command to export map to image

* Center is minecraft coordinates
* Range is number of tiles from center tile (i.e. padding)  
[2] - 5x5 tiles image  
[2,3] - 5x7 tiles image  
[2,3,2,2] - 5x6 tiles image
* Zoom depends on config of dynmap

Use `[command] -h` to see help for command

### List command

Show available worlds and maps

```text
DynmapImageExport list <url>

<url>  Dynmap URL
```

### Info command

Show info about map

```text
DynmapImageExport info <url> <world> <map>

<url>    Dynmap URL
<world>  World name
<map>    Map name
```

### Merge command

Merge dynmap map to image

```text
DynmapImageExport merge <url> <world> <map> [<center> [<range> [<zoom>]]] [options]

<url>     Dynmap URL
<world>   World name
<map>     Map name
<center>  Center of image [x,y,z] [default: [0,64,0]]
<range>   Range of image in tiles [all]|[vert,horz]|[top,right,bottom,left] [default: [2]]
<zoom>    Zoom [default: 0]

Options:
-o, --output <output>  Output path
```

## Example

### Flat map

`m https://map.minecrafting.ru/ world flat [0,100,0] [5,6,5,5] 2`  

![alt](/assets/images/Minecrafting.ru-flat.png)

### Isometric map

`m https://map.minecrafting.ru/ world se_view [0,100,0] [5,11,5,10]`  

![alt](/assets/images/Minecrafting.ru-se_view.png)

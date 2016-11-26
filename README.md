# TilemapTools

## WARNING

This project is unstable and currently far from being "implemented". I would not recommend using it. Especially the `TilemapTools.Xenko` project.

## Overview 

A collection of .NET Standard C# libraries for using tile maps in games.

The primary goal of this project is to provide tile map support for the [Xenko](http://xenko.com) game engine.


## Assemblies

#### Versions/Branches

| Branch | Version |  Status  | .NET Standard | Xenko |
| ------ |:-------:| :-------:| :------------:| :---: |
| master | 0.1.0   | unstable | 1.4           | 1.9.0 |


### TilemapTools

#### Overview

Includes classes for reading [Tiled Map Editor](http://www.mapeditor.org/) TMX and TSX files. And some base classes for building tilemap libraries.


#### Tiled

**Example**
```C#
using System.IO;
using TilemapTools.Tiled;
using TilemapTools.Tiled.Serialization;
//...
var options = new TiledSerializerOptions(File.OpenRead);
var serializer = new TiledSerializer(options);
var map = serializer.LoadTileMap("Dungeon.tmx");
//...
```


#### Grid

The `TilemapTools.Grid` class is a sparse grid storing it's cell contents in blocks.

**Example**
```C#
using TilemapTools;
using SiliconStudio.Core.Mathematics;
//...
var grid = new Grid<Tile, Vector2>();
grid.CellSize = new Vector2(1,1); //cell size in world units

// Allows negative cells
grid[-10,22] = new Tile();

//...
```

### TilemapTools.Xenko

#### Overview

Includes a TileMapComponent, TileSet/Tile classes and associated processors and renderers. Support for importing tiled maps and tile sets is also planned (seperate assembly).



using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using TilemapTools.Mathematics;

namespace TilemapTools.Tiled.Serialization
{
    public class TiledSerializer
    {
        private readonly TiledSerializerOptions options;

        public TiledSerializer(TiledSerializerOptions options = null)
        {
            options = options ?? TiledSerializerOptions.Default;

            this.options = options.Clone();
        }

        public TileMap LoadTileMap(string source)
        {   
            return LoadTileMap(options.OpenStream(source), source);
        }

        public TileMap LoadTileMap(Stream stream, string source = "")
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            var document = XDocument.Load(stream);

            return LoadTileMap(document, source);
        }

        public TileMap LoadTileMap(XDocument document, string source = "")
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));

            return ReadTileMap(document, source);
        }

        public TileSet LoadTileSet(string source)
        {
            return LoadTileSet(options.OpenStream(source), source);
        }

        public TileSet LoadTileSet(Stream stream, string source = "")
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            var document = XDocument.Load(stream);

            return LoadTileSet(document, source);
        }

        public TileSet LoadTileSet(XDocument document, string source = "")
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));

            return ReadTileSet(document.Root, Path.GetDirectoryName(source));
        }

        protected virtual TileMap ReadTileMap(XDocument document, string source = "")
        {
            var map = new TileMap();
            map.Source = source;

            var mapElement = document.Element("map");

            map.Version = (string)mapElement.Attribute("version");
            map.Width = (int)mapElement.Attribute("width");
            map.Height = (int)mapElement.Attribute("height");
            map.TileWidth = (int)mapElement.Attribute("tilewidth");
            map.TileHeight = (int)mapElement.Attribute("tileheight");
            map.HexSideLength = (int?)mapElement.Attribute("hexsidelength");

            var orientValue = (string)mapElement.Attribute("orientation");
            map.Orientation = ParseEnum<Orientation>(orientValue);

            var staggerAxisValue = (string)mapElement.Attribute("staggeraxis");
            map.StaggerAxis = ParseEnum<StaggerAxis>(staggerAxisValue);

            var staggerIndexValue = (string)mapElement.Attribute("staggerindex");
            map.StaggerIndex = ParseEnum<StaggerIndex>(staggerIndexValue);

            var renderValue = (string)mapElement.Attribute("renderorder");
            map.RenderOrder = ParseEnum<RenderOrder>(renderValue);

            map.NextObjectID = (int?)mapElement.Attribute("nextobjectid");

            var backgroundColorValue = (string)mapElement.Attribute("backgroundcolor");
            map.BackgroundColor = Color.FromString(backgroundColorValue);

            map.Properties = ReadProperties(mapElement);
            map.TileSets = ReadTileSets(mapElement, map.SourcePath);

            var width = map.Width;
            var height = map.Height;

            map.Layers = ReadLayers(mapElement, width, height, map.SourcePath);

            return map;
        }

        protected virtual TiledElementList<TileSet> ReadTileSets(XElement mapElement, string path)
        {
            TiledElementList <TileSet> tileSets = null;

            var tilesetElements = mapElement.Elements("tileset");

            if(tilesetElements != null)
            {
                tileSets = new TiledElementList<TileSet>();

                foreach (var tilesetElement in tilesetElements)
                {
                    tileSets.Add(ReadTileSet(tilesetElement, path));
                }
            }            
            
            return options.GetCollectionOrDefault(tileSets);
        }

        protected virtual TileSet ReadTileSet(XElement tilesetElement, string path)
        {
            TileSet tileSet = null;

            var firstGid = (int?)tilesetElement.Attribute("firstgid");
            var source = (string)tilesetElement.Attribute("source");

            if (source != null)
            {
                if (options.PreLoadTileSet)
                {
                    source = path != null ? options.Combine(path, source) : source;
                    tileSet = LoadTileSet(source);
                }

                tileSet.FirstGlobalId = firstGid.GetValueOrDefault(0);
                tileSet.Source = source;

                return tileSet;
            }

            tileSet = new TileSet();

            tileSet.FirstGlobalId = firstGid.GetValueOrDefault(0);
            tileSet.Name = (string)tilesetElement.Attribute("name");
            tileSet.TileWidth = (int)tilesetElement.Attribute("tilewidth");
            tileSet.TileHeight = (int)tilesetElement.Attribute("tileheight");
            tileSet.Spacing = (int?)tilesetElement.Attribute("spacing") ?? 0;
            tileSet.Margin = (int?)tilesetElement.Attribute("margin") ?? 0;
            tileSet.Columns = (int?)tilesetElement.Attribute("columns");
            tileSet.TileCount = (int?)tilesetElement.Attribute("tilecount");
            tileSet.TileOffset = ReadTileOffset(tilesetElement.Element("tileoffset"));
            tileSet.Image = ReadImage(tilesetElement.Element("image"), path);
            tileSet.Terrains = ReadTileTerrains(tilesetElement);
            tileSet.Tiles = ReadTileSetTiles(tilesetElement, path, tileSet);
            tileSet.Properties = ReadProperties(tilesetElement);

            return tileSet;
        }

        protected virtual TiledElementList<Terrain> ReadTileTerrains(XElement tilesetElement)
        {
            TiledElementList<Terrain> terrains = null;
            var terrainTypeElements = tilesetElement.Element("terraintypes")?.Elements("terrain");
            if (terrainTypeElements != null)
            {
                terrains = new TiledElementList<Terrain>();

                foreach (var e in terrainTypeElements)
                {
                    terrains.Add(ReadTerrain(e));
                }

            }

            return options.GetCollectionOrDefault(terrains);
        }

        protected virtual Collection<Tile> ReadTileSetTiles(XElement tilesetElement, string path, TileSet tileSet)
        {
            var tiles = new Collection<Tile>();

            foreach (var tileElement in tilesetElement.Elements("tile"))
            {
                tiles.Add(ReadTileSetTile(tileElement, tileSet.Terrains, path));
            }

            return options.GetCollectionOrDefault(tiles);
        }

        protected virtual Tile ReadTileSetTile(XElement tileElement, TiledElementList<Terrain> terrains, string path)
        {
            var tile = new Tile();

            tile.Id = (int)tileElement.Attribute("id");
            tile.TerrainEdges = ReadTerrainEdges(tileElement, terrains);

            tile.Probability = (double?)tileElement.Attribute("probability") ?? 1.0;

            var imageElement = tileElement.Element("image");

            if (imageElement != null)
                tile.Image = ReadImage(imageElement, path);

            var objectGroups = new TiledElementList<ObjectGroup>();
            var objectGroupElements = tileElement.Elements("objectgroup");

            foreach (var objectGroupElement in objectGroupElements)
                objectGroups.Add(ReadObjectGroup(objectGroupElement));

            tile.ObjectGroups = options.GetCollectionOrDefault(objectGroups);

            tile.AnimationFrames = ReadAnimationFrames(tileElement);
            tile.Properties = ReadProperties(tileElement);

            return tile;
        }

        protected virtual Collection<AnimationFrame> ReadAnimationFrames(XElement tileElement)
        {
            var animationFrameElements = tileElement.Element("animation")?.Elements("frame");
            var animationFrames = new Collection<AnimationFrame>();

            if (animationFrameElements != null)
            {
                foreach (var animationFrameElement in animationFrameElements)
                    animationFrames.Add(ReadAnimationFrame(animationFrameElement));
            }

            return options.GetCollectionOrDefault(animationFrames);
        }

        protected virtual Collection<Terrain> ReadTerrainEdges(XElement tileElement, TiledElementList<Terrain> terrains)
        {
            var terrainEdges = new Collection<Terrain>();

            var strTerrain = (string)tileElement.Attribute("terrain") ?? ",,,";

            foreach (var v in strTerrain.Split(','))
            {
                int index;
                Terrain edge;

                var success = int.TryParse(v, out index);
                if (success)
                    edge = terrains[index];
                else
                    edge = null;

                terrainEdges.Add(edge);

            }

            return options.GetCollectionOrDefault(terrainEdges);
        }

        protected virtual Terrain ReadTerrain(XElement terrainElement)
        {
            var terrain = new Terrain();

            terrain.Name = (string)terrainElement.Attribute("name");
            terrain.Tile = (int)terrainElement.Attribute("tile");
            terrain.Properties = ReadProperties(terrainElement);

            return terrain;
        }

        protected virtual TiledElementList<ILayer> ReadLayers(XElement mapElement, int width, int height, string path)
        {
            var layers = new TiledElementList<ILayer>();

            foreach (var element in mapElement.Elements())
            {
                if (element.Name.LocalName.Equals("layer", StringComparison.OrdinalIgnoreCase))
                {
                    Layer layer = ReadLayer(element, width, height);
                    layers.Add(layer);
                }
                else if (element.Name.LocalName.Equals("objectgroup", StringComparison.OrdinalIgnoreCase))
                {
                    ObjectGroup layer = ReadObjectGroup(element);
                    layers.Add(layer);
                }
                else if (element.Name.LocalName.Equals("imagelayer", StringComparison.OrdinalIgnoreCase))
                {
                    ImageLayer layer = ReadImageLayer(element, width, height, path);
                    layers.Add(layer);
                }
            }

            return options.GetCollectionOrDefault(layers);
        }

        protected virtual ImageLayer ReadImageLayer(XElement layerElement, int width, int height, string path)
        {
            var layer = ReadLayerBase<ImageLayer>(layerElement, width, height);

            layer.Image = ReadImage(layerElement.Element("image"), path);

            return layer;
        }

        protected virtual ObjectGroup ReadObjectGroup(XElement layerElement)
        {
            var layer = ReadLayerBase<ObjectGroup>(layerElement, 0, 0);

            var drawOrderValue = (string)layerElement.Attribute("draworder");
            layer.DrawOrder = ParseEnum<ObjectGroupDrawOrder>(drawOrderValue);


            var objects = new TiledElementList<TiledObject>();
            var objectElements = layerElement.Elements("object");

            foreach (var objectElement in objectElements)
            {
                objects.Add(ReadObject(objectElement));
            }

            layer.Objects = options.GetCollectionOrDefault(objects);

            return layer;
        }

        protected virtual TiledObject ReadObject(XElement objectElement)
        {
            var tiledObject = new TiledObject
            {
                Id = (int)objectElement.Attribute("id"),
                Name = (string)objectElement.Attribute("name") ?? String.Empty,
                X = (double)objectElement.Attribute("x"),
                Y = (double)objectElement.Attribute("y"),
                Width = (double?)objectElement.Attribute("width") ?? 0.0,
                Height = (double?)objectElement.Attribute("height") ?? 0.0,
                Type = (string)objectElement.Attribute("type") ?? String.Empty,
                Visible = (bool?)objectElement.Attribute("visible") ?? true,
                Rotation = (double?)objectElement.Attribute("rotation") ?? 0.0,
            };

            var gidAttribute = objectElement.Attribute("gid");
            var ellipseElement = objectElement.Element("ellipse");
            var polygonElement = objectElement.Element("polygon");
            var polylineElement = objectElement.Element("polyline");

            if (gidAttribute != null)
            {
                tiledObject.Tile = new LayerTile((uint)gidAttribute);
                tiledObject.ObjectType = TiledObjectType.Tile;
            }
            else if (ellipseElement != null)
            {
                tiledObject.ObjectType = TiledObjectType.Ellipse;
            }
            else if (polygonElement != null)
            {
                tiledObject.Points = ParsePoints(polygonElement);
                tiledObject.ObjectType = TiledObjectType.Polygon;
            }
            else if (polylineElement != null)
            {
                tiledObject.Points = ParsePoints(polylineElement);
                tiledObject.ObjectType = TiledObjectType.Polyline;
            }
            else tiledObject.ObjectType = TiledObjectType.Basic;

            return tiledObject;
        }

        protected virtual Layer ReadLayer(XElement layerElement, int width, int height)
        {
            var layer = ReadLayerBase<Layer>(layerElement, width, height);

            var data = ReadData(layerElement.Element("data"));

            layer.Tiles = new Collection<LayerTile>(data.ReadTiles(width, height).ToList());

            return layer;
        }

        private TLayer ReadLayerBase<TLayer>(XElement layerElement, int width, int height) where TLayer : LayerBase, new()
        {
            //width and height not currently used

            var layer = new TLayer
            {
                Name = (string)layerElement.Attribute("name"),
                Opacity = (double?)layerElement.Attribute("opacity") ?? 1.0,
                Visible = (bool?)layerElement.Attribute("visible") ?? true,
                OffsetX = (double?)layerElement.Attribute("offsetx") ?? 0.0,
                OffsetY = (double?)layerElement.Attribute("offsety") ?? 0.0,
            };

            layer.Properties = ReadProperties(layerElement);

            return layer;
        }

        protected virtual PropertyDictionary ReadProperties(XElement element)
        {
            PropertyDictionary properties = new PropertyDictionary();

            var propertiesElement = element.Element("properties");

            if (propertiesElement != null)
            {
                var props = from prop in propertiesElement.Elements("property")
                            let typeValue = (string)prop.Attribute("type")
                            select new Property
                            {
                                Name = (string)prop.Attribute("name"),
                                Type = ParseEnum<PropertyType>(typeValue),
                                Value = (string)prop.Attribute("value"),
                            };

                foreach (var prop in props)
                {
                    properties.Add(prop.Name, prop);
                }
            }

            return options.GetCollectionOrDefault(properties);
        }

        protected virtual Image ReadImage(XElement imageElement, string path)
        {
            var image = new Image();

            var sourceAttribute = imageElement.Attribute("source");

            if (sourceAttribute != null)
                // Append directory if present
                image.Source = options.Combine(path, (string)sourceAttribute);
            else
            {
                var formatAttribute = imageElement.Attribute("format");
                if (formatAttribute != null)
                    image.Format = (string)formatAttribute;

                var dataElement = imageElement.Element("data");
                if (dataElement != null)
                {
                    image.Data = ReadData(dataElement);
                }
            }

            var transAttribute = imageElement.Attribute("trans");
            if (transAttribute != null)
                image.TransparentColor = Color.FromString((string)transAttribute);

            image.Width = (int?)imageElement.Attribute("width");
            image.Height = (int?)imageElement.Attribute("height");

            return image;
        }

        protected virtual Data ReadData(XElement dataElement)
        {
            var data = new Data();

            var encodingAttribute = dataElement.Attribute("encoding");
            data.Encoding = ParseEnum<DataEncoding>((string)encodingAttribute);

            var compressionAttribute = dataElement.Attribute("compression");
            data.Compression = ParseEnum<DataCompression>((string)compressionAttribute);

            data.Content = dataElement.Value;

            return data;
        }

        private static TileOffset ReadTileOffset(XElement tileOffsetElement)
        {
            if (tileOffsetElement == null)
            {
                return new TileOffset(0, 0);
            }
            else
            {
                return new TileOffset((int)tileOffsetElement.Attribute("x"), (int)tileOffsetElement.Attribute("y"));
            }
        }

        private static AnimationFrame ReadAnimationFrame(XElement animationFrameElement)
        {
            if (animationFrameElement == null)
            {
                return new AnimationFrame(0, 0);
            }
            else
            {
                return new AnimationFrame((int)animationFrameElement.Attribute("tileid"), (int)animationFrameElement.Attribute("duration"));
            }
        }

        private static Collection<Point> ParsePoints(XElement pointsElement)
        {
            var pointString = (string)pointsElement.Attribute("points");

            var points = from pointPair in pointString.Split(' ')
                         let point = pointPair.Split(',')
                         select new Point
                         {
                             X = double.Parse(point[0], NumberStyles.Float, CultureInfo.InvariantCulture),
                             Y = double.Parse(point[1], NumberStyles.Float, CultureInfo.InvariantCulture),
                         };

            return new Collection<Point>(points.ToList());
        }

        internal static TEnum ParseEnum<TEnum>(string value) where TEnum : struct
        {
            if (string.IsNullOrEmpty(value))
                return default(TEnum);

            //HACK: Should find better way of handling enum values
            value = value.Replace("-", "");

            return (TEnum)Enum.Parse(typeof(TEnum), value, true);
        }
    }
}
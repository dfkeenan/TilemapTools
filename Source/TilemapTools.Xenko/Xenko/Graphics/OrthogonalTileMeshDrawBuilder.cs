using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Graphics;

namespace TilemapTools.Xenko.Graphics
{
    public abstract class OrthogonalTileMeshDrawBuilder<TVertex> : TileMeshDrawBuilder<TVertex>, IDisposable
        where TVertex : struct, IVertex
    {
        
        public OrthogonalTileMeshDrawBuilder(VertexDeclaration layout):base(layout, 6)
        {

        }

        protected override void BuildTile(List<TVertex> vertices, int textureWidth, int textureHeight, Rectangle source, RectangleF dest)
        {
            var left = dest.X;
            var top = dest.Y;
            var right = dest.X + dest.Width;
            var bottom = dest.Y + dest.Height * -1;

            vertices.Add(CreateVertex(source.TopLeft, new Vector2(left, top), new Vector2(textureWidth, textureHeight)));
            vertices.Add(CreateVertex(source.BottomLeft, new Vector2(left, bottom), new Vector2(textureWidth, textureHeight)));
            vertices.Add(CreateVertex(source.TopRight, new Vector2(right, top), new Vector2(textureWidth, textureHeight)));
            vertices.Add(CreateVertex(source.BottomRight, new Vector2(right, bottom), new Vector2(textureWidth, textureHeight)));
        }

        protected override void BuildIndicies(short[] indices, int tileCount)
        {
            for (int i = 0; i < tileCount; i++)
            {
                indices[i * IndiciesPerTile + 0] = (short)(i * 4);
                indices[i * IndiciesPerTile + 1] = (short)(i * 4 + 2);
                indices[i * IndiciesPerTile + 2] = (short)(i * 4 + 1);
                indices[i * IndiciesPerTile + 3] = (short)(i * 4 + 3);
                indices[i * IndiciesPerTile + 4] = (short)(i * 4 + 1);
                indices[i * IndiciesPerTile + 5] = (short)(i * 4 + 2);
            }
        }
    }

    public class OrthogonalTileMeshDrawBuilder : OrthogonalTileMeshDrawBuilder<VertexPositionTexture>
    {
        public OrthogonalTileMeshDrawBuilder() : base(VertexPositionTexture.Layout)
        {

        }

        protected override VertexPositionTexture CreateVertex(Vector2 source, Vector2 destination, Vector2 size)
        {
            return new VertexPositionTexture(new Vector3(destination, 0f), (source / size));
        }
    }
}

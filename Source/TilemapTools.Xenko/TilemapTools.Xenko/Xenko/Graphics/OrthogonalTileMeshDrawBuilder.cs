using System;
using System.Collections.Generic;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Graphics;

namespace TilemapTools.Xenko.Graphics
{
    public class OrthogonalTileMeshDrawBuilder : TileMeshDrawBuilder
    {
        
        public OrthogonalTileMeshDrawBuilder():base(6, 4)
        {

        }

        protected override void BuildTile(VertexPositionTexture[] vertices, ref int vertexIndex, int textureWidth, int textureHeight, Rectangle source, RectangleF dest)
        {
            var left = dest.X;
            var top = dest.Y;
            var right = dest.X + dest.Width;
            var bottom = dest.Y + dest.Height * -1;

            vertices[vertexIndex++] = CreateVertex(source.TopLeft, new Vector2(left, top), new Vector2(textureWidth, textureHeight));
            vertices[vertexIndex++] = CreateVertex(source.BottomLeft, new Vector2(left, bottom), new Vector2(textureWidth, textureHeight));
            vertices[vertexIndex++] = CreateVertex(source.TopRight, new Vector2(right, top), new Vector2(textureWidth, textureHeight));
            vertices[vertexIndex++] = CreateVertex(source.BottomRight, new Vector2(right, bottom), new Vector2(textureWidth, textureHeight));
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
}

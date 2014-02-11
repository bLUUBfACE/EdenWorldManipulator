using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eden_World_Manipulator
{
    public class Block
    {
        public readonly BlockType BlockType;
        public readonly Painting Painting;
        public readonly int X, Y, Z;

        public Block(BlockType type, Painting painting, int x, int y, int z)
        {
            this.BlockType = type;
            this.Painting = painting;
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public Block(byte? type, byte? painting, int x, int y, int z)
            : this((BlockType)type, (Painting)painting, x, y, z) { }

        public Block(BlockType type, Painting painting)
            : this(type, painting, -1, -1, -1) { }
    }
}

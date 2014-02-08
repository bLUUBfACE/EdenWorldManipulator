using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eden_World_Manipulator
{
    public class Block
    {
        public readonly byte BlockType, Painting;
        public readonly int X, Y, Z;

        public Block(BlockType type, Painting painting, int x, int y, int z)
            : this((byte)type, (byte)painting, x, y, z) { }

        public Block(byte? type, byte? painting, int x, int y, int z)
            : this((byte)type, (byte)painting, x, y, z) { }

        public Block(BlockType type, Painting painting)
            : this((byte)type, (byte)painting, -1, -1, -1) { }

        private Block(byte type, byte painting, int x, int y, int z)
        {
            this.BlockType = type;
            this.Painting = painting;
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
    }
}

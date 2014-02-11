using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eden_World_Manipulator
{
    public static partial class Manipulator
    {
        static int maxX, maxY, maxZ;

        static World currentWorld;

        public static void Manipulate(this World world, Func<Block, Block> manipulatorDelegate)
        {
            byte?[,,,] newMap = new byte?[world.Map.GetLength(0), world.Map.GetLength(1), world.Map.GetLength(2), 2];

            currentWorld = world;
            maxX = world.Map.GetLength(0) - 1; maxY = world.Map.GetLength(0) - 1; maxZ = world.Map.GetLength(0) - 1;

            Block block;
            for (int x = 0; x < world.Map.GetLength(0); x++)
            {
                for (int y = 0; y < world.Map.GetLength(1); y++)
                {
                    for (int z = 0; z < world.Map.GetLength(2); z++)
                    {
                        if (world.Map[x, y, z, 0] != null)
                        {
                            block = getBlockAtPosition(x, y, z);
                            block = manipulatorDelegate(block);
                            newMap[x, y, z, 0] = (byte)block.BlockType;
                            newMap[x, y, z, 1] = (byte)block.Painting;
                        }
                    }
                }
            }
            world.Map = newMap;
        }

        static List<Block> getNeighbourBlocks(Block block, bool xAxis = true, bool yAxis = true, bool zAxis = true)
        {
            List<Block> neighbourBlocks = new List<Block>();
            if (xAxis)
            {
                if (block.X > 0 && getBlockAtPosition(block.X - 1, block.Y, block.Z) != null) neighbourBlocks.Add(getBlockAtPosition(block.X - 1, block.Y, block.Z));
                if (block.X < maxX && getBlockAtPosition(block.X + 1, block.Y, block.Z) != null) neighbourBlocks.Add(getBlockAtPosition(block.X + 1, block.Y, block.Z));
            }
            if (yAxis)
            {
                if (block.Y > 0 && getBlockAtPosition(block.X, block.Y - 1, block.Z) != null) neighbourBlocks.Add(getBlockAtPosition(block.X, block.Y - 1, block.Z));
                if (block.Y < maxY && getBlockAtPosition(block.X, block.Y + 1, block.Z) != null) neighbourBlocks.Add(getBlockAtPosition(block.X, block.Y + 1, block.Z));
            }
            if (zAxis)
            {
                if (block.Z > 0 && getBlockAtPosition(block.X, block.Y, block.Z - 1) != null) neighbourBlocks.Add(getBlockAtPosition(block.X, block.Y, block.Z - 1));
                if (block.Z < maxZ && getBlockAtPosition(block.X, block.Y, block.Z + 1) != null) neighbourBlocks.Add(getBlockAtPosition(block.X, block.Y, block.Z + 1));
            }
            return neighbourBlocks;
        }        

        static Block getBlockAtPosition(int x, int y, int z)
        {
            if (currentWorld.Map[x, y, z, 0] == null)
                return null;
            return new Block((BlockType)currentWorld.Map[x, y, z, 0], (Painting)currentWorld.Map[x, y, z, 1], x, y, z);
        }
    }
}

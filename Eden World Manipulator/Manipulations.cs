using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eden_World_Manipulator
{
    public static partial class Manipulator
    {
        static Random random = new Random();

        public static Tuple<float, float, float> CenterPosition = new Tuple<float, float, float>(100.5f, 100.5f, 32.5f);
        public static float SphereRadius = 30;

        public static Block BottomlessManipulation(Block block)
        {
            if (((((BlockType)block.BlockType == BlockType.Dirt && block.Z <= 31 && block.Z > 15) || ((BlockType)block.BlockType == BlockType.Stone && block.Z <= 15)) && getNeighbourBlocks(block).All(b => (BlockType)b.BlockType != BlockType.Air && (BlockType)b.BlockType != BlockType.Water && (b.BlockType < 24 || b.BlockType > 39)))
                || ((BlockType)block.BlockType == BlockType.Grass && block.Z == 32 && getNeighbourBlocks(block, true, true, false).All(b => (BlockType)b.BlockType == BlockType.Grass) && (BlockType)getBlockAtPosition(block.X, block.Y, block.Z - 1).BlockType == BlockType.Dirt)
                || block.BlockType == 1)
                return new Block(BlockType.Air, Painting.Unpainted);
            return block;
        }
        public static Block NaturalManipulation(Block block)
        {
            BlockType newBlockType = (BlockType)block.BlockType;
            Painting newPainting = Painting.Unpainted;

            switch (newBlockType)
            {
                case BlockType.Brick:
                case BlockType.Slate:
                case BlockType.Shingles:
                    newBlockType = BlockType.DarkStone;
                    if (newPainting == Painting.Unpainted)
                    {
                        if (newBlockType == BlockType.Brick) newPainting = Painting.MediumDarkOrange;
                        if (newBlockType == BlockType.Slate) newPainting = Painting.Gray;
                        if (newBlockType == BlockType.Shingles) newPainting = Painting.DarkGray;
                    }
                    break;
                case BlockType.TNT:
                case BlockType.Ladder:
                case BlockType.Fence:
                case BlockType.Wallpaper:
                case BlockType.Bouncy:
                case BlockType.Cloud:
                case BlockType.Glass:
                    newBlockType = BlockType.Wood;
                    if (newPainting == Painting.Unpainted)
                    {
                        if (newBlockType == BlockType.Wallpaper || newBlockType == BlockType.Cloud || newBlockType == BlockType.Glass) newPainting = Painting.LightGray_White;
                        if (newBlockType == BlockType.Bouncy) newPainting = Painting.DarkGray;
                        if (newBlockType == BlockType.TNT) newPainting = Painting.DarkOrange;
                    }
                    break;
                case BlockType.Water:
                case BlockType.Water3_4:
                case BlockType.Water2_4:
                case BlockType.Water1_4:
                    if (newPainting == Painting.Unpainted) newPainting = Painting.DarkBlue;
                    break;
                case BlockType.Ivy:
                case BlockType.Grass:
                    newBlockType = BlockType.Leaves;
                    if (newBlockType == BlockType.Ivy && newPainting == Painting.Unpainted)
                        newPainting = random.Next(2) == 0 ? Painting.LightGray_White : Painting.MediumLightGreen;
                    break;
                case BlockType.ShinglesRampSouth:
                case BlockType.ShinglesRampWest:
                case BlockType.ShinglesRampNorth:
                case BlockType.ShinglesRampEast:
                case BlockType.ShinglesWedge_SouthEast:
                case BlockType.ShinglesWedge_SouthWest:
                case BlockType.ShinglesWedge_NorthWest:
                case BlockType.ShinglesWedge_NorthEast:
                    newBlockType -= 8;
                    if (newPainting == Painting.Unpainted) newPainting = Painting.MediumDarkGray;
                    break;
                case BlockType.NeonSquare:
                    newBlockType = BlockType.Ice;
                    if (newPainting == Painting.Unpainted) newPainting = Painting.LightGray_White;
                    break;
            }

            return new Block(newBlockType, newPainting);
        }

        public static Block SphereCreation(Block block)
        {
            float distanceX = Math.Abs(CenterPosition.Item1 - ((float)block.X + 0.5f)), distanceY = Math.Abs(CenterPosition.Item2 - ((float)block.Y + 0.5f)), distanceZ = Math.Abs(CenterPosition.Item3 - ((float)block.Z + 0.5f));
            float distance = (float)Math.Sqrt(distanceX * distanceX + distanceY * distanceY + distanceZ * distanceZ);

            if (distance <= 30 && distance >= 30 - 1)
                return new Block(BlockType.Glass, Painting.MediumLightCyan);
            if (distance <= 20 && distance >= 20 - 1)
                return new Block(BlockType.Glass, Painting.MediumLightGreen);
            if (distance <= 10 && distance >= 10 - 1)
                return new Block(BlockType.Glass, Painting.MediumLightOrange);
            if (distance < 1)
                return new Block(BlockType.Brick, 0);

            return new Block((BlockType)block.BlockType != BlockType.Bedrock ? BlockType.Air : BlockType.Bedrock, Painting.Unpainted);
        }
        public static Block CylinderCreation(Block block)
        {
            float distanceX = Math.Abs(CenterPosition.Item1 - ((float)block.X + 0.5f)), distanceY = Math.Abs(CenterPosition.Item2 - ((float)block.Y + 0.5f));
            float distance = (float)Math.Sqrt(distanceX * distanceX + distanceY * distanceY);

            if ((distance <= 90 && distance >= 90 - 1) || block.Z == 1 || block.Z == 63)
                return new Block(BlockType.Glass, Painting.MediumLightCyan);
            if (distance < 1)
                return new Block(BlockType.Brick, 0);

            return new Block((BlockType)block.BlockType != BlockType.Bedrock ? BlockType.Air : BlockType.Bedrock, Painting.Unpainted);
        }
        public static Block ChessCreation(Block block)
        {
            if (block.BlockType == 0) return block;
            if ((block.X % 6 < 3) ^ (block.Y % 6 < 3))
                return new Block(BlockType.Cloud, Painting.LightGray_White);
            return new Block(BlockType.Cloud, Painting.VeryDarkGray_Black);
        }
        public static Block OceanCreation(Block block)
        {
            if (block.Z == 1) return new Block(BlockType.Sand, Painting.Unpainted);
            else if (block.Z < 32) return new Block(BlockType.Water, Painting.Unpainted);
            else return new Block(BlockType.Air, Painting.Unpainted);
        }
    }
}

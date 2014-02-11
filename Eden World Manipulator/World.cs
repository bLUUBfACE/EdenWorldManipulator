using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace Eden_World_Manipulator
{
    public class World
    {
        public byte?[, , ,] Map;
        public string Name;
        public Dictionary<int, Point> Chunks;
        public Rectangle WorldArea = default(Rectangle);

        private byte[] otherBytes; //Header, 12000 creature bytes, chunk pointers

        public static World LoadWorld(string path)
        {
            byte[] bytes;
            using(FileStream stream = new FileStream(path, FileMode.Open))
            {
                bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);                
            }

            int chunkPointerStartIndex = bytes[35] * 256 * 256 * 256 + bytes[34] * 256 * 256 + bytes[33] * 256 + bytes[32];

            byte[] nameArray = bytes.TakeWhile((b, i) => (i < 40 || b != 0)).ToArray();
            string worldName = Encoding.ASCII.GetString(nameArray, 40, nameArray.Length - 40);
            Rectangle worldArea = Rectangle.Empty; 
            Dictionary<int, Point> chunks = new Dictionary<int, Point>();

            int currentChunkPointerIndex = chunkPointerStartIndex;
            do
            {
                chunks.Add(
                    bytes[currentChunkPointerIndex + 11] * 256 * 256 * 256 + bytes[currentChunkPointerIndex + 10] * 256 * 256 + bytes[currentChunkPointerIndex + 9] * 256 + bytes[currentChunkPointerIndex + 8],//Adress
                    new Point(bytes[currentChunkPointerIndex + 1] * 256 + bytes[currentChunkPointerIndex], bytes[currentChunkPointerIndex + 5] * 256 + bytes[currentChunkPointerIndex + 4])); //Position
            } while ((currentChunkPointerIndex += 16) < bytes.Length);            

            worldArea.X = chunks.Values.Min(p => p.X);
            worldArea.Y = chunks.Values.Min(p => p.Y);
            worldArea.Width = chunks.Values.Max(p => p.X) - worldArea.X + 1;
            worldArea.Height = chunks.Values.Max(p => p.Y) - worldArea.Y + 1;

            byte?[, , ,] map = new byte?[worldArea.Width * 16, worldArea.Height * 16, 64, 2];

            foreach (int adress in chunks.Keys)
            {
                int baseX = (chunks[adress].X - worldArea.X) * 16, baseY = (chunks[adress].Y - worldArea.Y) * 16;
                for (int baseHeight = 0; baseHeight < 4; baseHeight++)
                {
                    for (int x = 0; x < 16; x++)
                    {
                        for (int y = 0; y < 16; y++)
                        {
                            for (int z = 0; z < 16; z++)
                            {
                                map[baseX + x, baseY + y, baseHeight * 16 + z, 0] = bytes[adress + baseHeight * 8192 + x * 256 + y * 16 + z]; //Block
                                map[baseX + x, baseY + y, baseHeight * 16 + z, 1] = bytes[adress + baseHeight * 8192 + x * 256 + y * 16 + z + 4096]; //Color
                            }
                        }
                    }
                }
            }

            int otherBytesStartIndex = 192 + chunks.Count * 8 * 4096; 

            byte[] remainingBytes = new byte[192 + bytes.Length - otherBytesStartIndex]; //Header, creature data, chunk pointers

            for (int i = 0; i < 192; i++) { remainingBytes[i] = bytes[i]; }
            for (int i = 0; i < bytes.Length - otherBytesStartIndex; i++) { remainingBytes[192 + i] = bytes[otherBytesStartIndex + i]; }

            return new World(map, chunks, worldName, remainingBytes, worldArea);
        }

        public void SaveWorld(string path)
        {
            using (FileStream stream = new FileStream(path, FileMode.CreateNew))
            {
                //Write header
                for (int i = 0; i < 192; i++)
                {
                    stream.WriteByte(otherBytes[i]);
                }

                //Write block data
                foreach (int adress in this.Chunks.Keys.OrderBy(k => k))
                {
                    int baseX = (this.Chunks[adress].X - this.WorldArea.X) * 16, baseY = (this.Chunks[adress].Y - this.WorldArea.Y) * 16;
                    for (int baseHeight = 0; baseHeight < 4; baseHeight++)
                    {
                        for (int mode = 0; mode < 2; mode++) //0 = block types, 1 = colors
                        {
                            for (int x = 0; x < 16; x++)
                            {
                                for (int y = 0; y < 16; y++)
                                {
                                    for (int z = 0; z < 16; z++)
                                    {
                                        if(mode == 0) //block type
                                            stream.WriteByte((byte)this.Map[baseX + x, baseY + y, baseHeight * 16 + z, 0]);
                                        else //color
                                            stream.WriteByte((byte)this.Map[baseX + x, baseY + y, baseHeight * 16 + z, 1]);
                                    }
                                }
                            }
                        }
                    }
                }

                //Write creature bytes + chunk pointers
                for (int i = 192; i < otherBytes.Length; i++)
                {
                    stream.WriteByte(otherBytes[i]);
                }
            }
        }

        private World(byte?[, , ,] map, Dictionary<int, Point> chunks, string name, byte[] otherBytes, Rectangle worldArea) //otherBytes: 0 - 191 (header), 12000 creature bytes, chunk pointer bytes
        {
            this.Map = map;
            this.Chunks = chunks;
            this.Name = name;
            this.otherBytes = otherBytes;
            this.WorldArea = worldArea;
        }

        public void Draw(Graphics graphics, int chunkDrawingSize)
        {
            foreach (int address in Chunks.Keys)
            {
                graphics.DrawRectangle(Pens.Black, new Rectangle((Chunks[address].X - WorldArea.X) * chunkDrawingSize, (Chunks[address].Y - WorldArea.Y) * chunkDrawingSize, chunkDrawingSize, chunkDrawingSize));
            }
        }
    }
}

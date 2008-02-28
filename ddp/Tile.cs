using System;
using System.Collections.Generic;
using System.Text;

namespace ddp
{
    class Tile
    {
        private int _x;

        public int X
        {
            get { return _x; }
            set { _x = value; }
        }

        private int _y;

        public int Y
        {
            get { return _y; }
            set { _y = value; }
        }

        private int _imageIndex;

        public int ImageIndex
        {
            get { return _imageIndex; }
            set { _imageIndex = value; }
        }

        private int _offsetX;

        public int OffsetX
        {
            get { return _offsetX; }
            set { _offsetX = value; }
        }

        private int _offsetY;

        public int OffsetY
        {
            get { return _offsetY; }
            set { _offsetY = value; }
        }

        public Tile(int x, int y, int imageIndex, int offsetX, int offsetY)
        {
            _x = x;
            _y = y;
            _imageIndex = imageIndex;
            _offsetX = offsetX;
            _offsetY = OffsetY;
        }

        public Tile(int x, int y)
        {
            _x = x;
            _y = y;

            _imageIndex = -1;  //´ú±íÎ´·ÖÅä
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace ddp
{
    class Tile
    {
        private int _blockWidth = 50;
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
            set { 
                _imageIndex = value; 
            }
        }

        private Graphics _graphics;

        public Graphics Graph
        {
            get { return _graphics; }
            set { _graphics = value; }
        }


        public void Draw()
        {
            string fileName = Application.StartupPath + "/Images/" + ImageIndex + ".png";

            Bitmap bmp = new Bitmap(fileName);
            Graph.DrawImage(bmp, OffsetX, OffsetY,_blockWidth,_blockWidth);

        }

        public void Hide()
        {
            Graph.FillRectangle(new SolidBrush(Color.Black), new Rectangle(OffsetX, OffsetY, _blockWidth, _blockWidth));
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

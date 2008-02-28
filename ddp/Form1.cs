using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ddp
{
    public partial class Form1 : Form
    {
        private int VertCount = 8;
        private int HorzCount = 8;
        private Tile[,] MapTiles = null;

        private int _blockWidth = 50;
        private Tile _selTile = null;
        private bool _GameStart = false;
        public Form1()
        {
            InitializeComponent();
        }


        private int _offsetX=50;

        public int OffsetX
        {
            get { return _offsetX; }
            set { _offsetX = value; }
        }

        private void InitGameData()
        {
            MapTiles = new Tile[HorzCount, VertCount];
            Random rnd = new Random();
            for(int i=0;i<HorzCount;i++)
                for (int j = 0; j < VertCount; j++)
                {
                    MapTiles[i, j] = new Tile(i,j);
                    MapTiles[i, j].ImageIndex = rnd.Next(8) + 1;
                    MapTiles[i, j].OffsetX = _offsetX+j * _blockWidth;
                    MapTiles[i, j].OffsetY = i * _blockWidth;
                }

            for (int i = 0; i < HorzCount; i++)
                for (int j = 0; j < VertCount; j++)
                {                    
                    if(CheckCanHide(MapTiles[i,j]))
                    {
                        while (CheckCanHide(MapTiles[i, j]))
                        {
                            MapTiles[i, j].ImageIndex = rnd.Next(8) + 1;
                        }
                    }
                }

            
            ShowData();
        }

        private void PaintPanel()
        {
            Graphics g = Graphics.FromHwnd(panel1.Handle);

            for (int i = 0; i < HorzCount; i++)
                for (int j = 0; j < VertCount; j++)
                {
                    int ImageIndex = MapTiles[i, j].ImageIndex;
                    string fileName = Application.StartupPath + "/Images/" + ImageIndex + ".png";

                    Bitmap bmp = new Bitmap(fileName);

                    g.DrawImage(bmp, MapTiles[i, j].OffsetX, MapTiles[i, j].OffsetY,_blockWidth,_blockWidth);
                }

        }

        /// <summary>
        /// 查看是否有解
        /// 1、有解
        /// 2、无解，需要重新Shuffle
        /// </summary>
        /// <returns></returns>

        private int CalSolution()
        {
            int solu = 2;
            for (int i = 0; i < HorzCount; i++)
                for (int j = 0; j < VertCount; j++)
                {
                    if (CheckCanHide(MapTiles[i, j]))
                    {
                        solu = 1;
                        return solu;
                    }
                }

            return solu;
        }

        /// <summary>
        /// 查找可以水平消除的方块组合
        /// </summary>
        /// <returns></returns>
        private Tile[] GetCanHideHorzTiles(Tile t)
        {
            int x = t.X;
            int y = t.Y;

            int temp_x = 0;
            int temp_y = 0;


            //查找水平方向
            List<Tile> tiles = new List<Tile>();

            //左侧
            temp_x = x;
            temp_y = y - 1;
            if (!isPointOutOfBorder(temp_x, temp_y))
            {
                Tile temp_tile=MapTiles[temp_x,temp_y];
                while (temp_tile.ImageIndex == t.ImageIndex)
                {
                    tiles.Add(temp_tile);
                    temp_y--;
                    if (isPointOutOfBorder(temp_x, temp_y))
                        break;
                    temp_tile = MapTiles[temp_x, temp_y];
                }
            }

            tiles.Add(t);

            //右侧
            temp_y = y + 1;
            if (!isPointOutOfBorder(temp_x, temp_y))
            {
                Tile temp_tile = MapTiles[temp_x, temp_y];
                while (temp_tile.ImageIndex == t.ImageIndex)
                {
                    tiles.Add(temp_tile);
                    temp_y++;
                    if (isPointOutOfBorder(temp_x, temp_y))
                        break;
                    temp_tile = MapTiles[temp_x, temp_y];
                }
            }

            return tiles.ToArray();

        }


        /// <summary>
        /// 查找可以竖直方向消除的方块
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private Tile[] GetCanHideVertTiles(Tile t)
        {
            int x = t.X;
            int y = t.Y;

            int temp_x = 0;
            int temp_y = 0;


            //查找水平方向
            List<Tile> tiles = new List<Tile>();

            //上方
            temp_x = x-1;
            temp_y = y;
            if (!isPointOutOfBorder(temp_x, temp_y))
            {
                Tile temp_tile = MapTiles[temp_x, temp_y];
                while (temp_tile.ImageIndex == t.ImageIndex)
                {
                    tiles.Add(temp_tile);
                    temp_y--;
                    if (isPointOutOfBorder(temp_x, temp_y))
                        break;
                    temp_tile = MapTiles[temp_x, temp_y];
                }
            }

            tiles.Add(t);

            //下方
            temp_x = x + 1;
            if (!isPointOutOfBorder(temp_x, temp_y))
            {
                Tile temp_tile = MapTiles[temp_x, temp_y];
                while (temp_tile.ImageIndex == t.ImageIndex)
                {
                    tiles.Add(temp_tile);
                    temp_x++;
                    if (isPointOutOfBorder(temp_x, temp_y))
                        break;
                    temp_tile = MapTiles[temp_x, temp_y];
                }
            }

            return tiles.ToArray();

        }

        /// <summary>
        /// 查询是否可以进行位置交换
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        private bool CanExchange(Tile t1, Tile t2)
        {
            Tile temp_tile = new Tile(t2.X, t2.Y);
            temp_tile.ImageIndex = t1.ImageIndex;

            if (CheckCanHide(temp_tile))
                return true;

            temp_tile = new Tile(t1.X, t1.Y);
            temp_tile.ImageIndex = t2.ImageIndex;

            if (CheckCanHide(temp_tile))
                return true;

            return false;

        }
        private bool isPointOutOfBorder(int x, int y)
        {
            return x == -1 || y == -1 || x >= HorzCount || y >= VertCount;
        }

        private bool CheckCanHide(Tile t)
        {
            int x = t.X;
            int y = t.Y;

            

            int temp_x = 0;
            int temp_y = 0;

            //上方检测
            temp_x = x - 1;
            temp_y = y;
            int SamCol = 1;
            if(!isPointOutOfBorder(temp_x,temp_y))
            {
                Tile temp_tile=MapTiles[temp_x,temp_y];

                while (temp_tile.ImageIndex == t.ImageIndex && temp_tile.ImageIndex!=-1)
                {
                    SamCol++;

                    temp_x--;
                    if(isPointOutOfBorder(temp_x,temp_y))
                        break;
                    temp_tile = MapTiles[temp_x, temp_y];
                }

                if (SamCol >= 3)
                    return true;
            }
           

            //下方检测
            temp_x = x + 1;
            temp_y = y;
            SamCol = 1;
            if (!isPointOutOfBorder(temp_x, temp_y))
            {
                Tile temp_tile = MapTiles[temp_x, temp_y];

                while (temp_tile.ImageIndex == t.ImageIndex && temp_tile.ImageIndex != -1)
                {
                    SamCol++;

                    temp_x++;
                    if(isPointOutOfBorder(temp_x,temp_y))
                        break;
                    temp_tile = MapTiles[temp_x, temp_y];
                }

                if (SamCol >= 3)
                    return true;
            }

            //左侧检测
            temp_x = x; 
            temp_y = y-1;
            SamCol = 1;
            if (!isPointOutOfBorder(temp_x, temp_y))
            {
                Tile temp_tile = MapTiles[temp_x, temp_y];

                while (temp_tile.ImageIndex == t.ImageIndex && temp_tile.ImageIndex != -1)
                {
                    SamCol++;

                    temp_y--;

                    if(isPointOutOfBorder(temp_x,temp_y))
                        break;
                    temp_tile = MapTiles[temp_x, temp_y];
                }

                if (SamCol >= 3)
                    return true;
            }

            //右侧检测
            temp_x = x;
            temp_y = y + 1;
            SamCol = 1;
            if (!isPointOutOfBorder(temp_x, temp_y))
            {
                Tile temp_tile = MapTiles[temp_x, temp_y];

                while (temp_tile.ImageIndex == t.ImageIndex && temp_tile.ImageIndex != -1)
                {
                    SamCol++;

                    temp_y++;
                    if(isPointOutOfBorder(temp_x,temp_y))
                        break;
                    temp_tile = MapTiles[temp_x, temp_y];
                }

                if (SamCol >= 3)
                    return true;
            }

            return false;

        }
        private void ShowData()
        {
            //throw new Exception("The method or operation is not implemented.");

            if (listView1.Columns.Count == 0)
            {
                for (int i = 0; i < HorzCount; i++)
                {
                    ColumnHeader header = new ColumnHeader();
                    header.Text = i.ToString();
                    header.Width = 30;
                    listView1.Columns.Add(header);
                }
            }

            listView1.Items.Clear();
            for (int i = 0; i < HorzCount; i++)
            {
                ListViewItem item = new ListViewItem();
                item.Text = MapTiles[i, 0].ImageIndex.ToString();
                for (int j = 1; j < VertCount; j++)
                {
                    item.SubItems.Add(MapTiles[i, j].ImageIndex.ToString());
                }

                listView1.Items.Add(item);
            }

            PaintPanel();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _GameStart = true;
            InitGameData();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string pos1 = textBox1.Text;
            string[] div=pos1.Split(new char[]{','});
            int x1 = int.Parse(div[0]);
            int y1 = int.Parse(div[1]);

            pos1 = textBox2.Text;
            div = pos1.Split(new char[] { ',' });
            int x2 = int.Parse(div[0]);
            int y2 = int.Parse(div[1]);


            if (CanExchange(MapTiles[x1, y1], MapTiles[x2, y2]))
            {
                MessageBox.Show("Can Hide!");
                Exchange(ref MapTiles[x1, y1], ref MapTiles[x2, y2]);

                Tile[] horzTiles = GetCanHideHorzTiles(MapTiles[x1, y1]);

                if (horzTiles.Length >= 3)
                {
                    foreach (Tile t in horzTiles)
                    {
                        DownTitle(t, 1);
                    }
                }

                Tile[] vertTiles = GetCanHideVertTiles(MapTiles[x1, y1]);
                int ilen = vertTiles.Length;

                if (ilen >= 3)
                {
                    DownTitle(MapTiles[x1, y1], ilen);
                }

                ScanCanAutoHide();
                ShowData();
            }
            else
            {
                MessageBox.Show("Invalid Exchange!");
            }
        }

        private void ScanCanAutoHide()
        {
            for (int y = 0; y < HorzCount; y++)
            {
                for (int k = 0; k < VertCount; k++)
                {
                    if (CheckCanHide(MapTiles[k, y]))
                    {
                        Tile[] horzTiles = GetCanHideHorzTiles(MapTiles[k, y]);

                        if (horzTiles.Length >= 3)
                        {
                            foreach (Tile t1 in horzTiles)
                            {
                                DownTitle(t1, 1);
                            }
                        }

                        Tile[] vertTiles = GetCanHideVertTiles(MapTiles[k, y]);
                        int ilen = vertTiles.Length;

                        if (ilen >= 3)
                        {
                            DownTitle(MapTiles[k, y], ilen);
                        }

                        ShowData();
                    }
                }
            }

            //if (CalSolution() == 1)
            //    ScanCanAutoHide();
        }

        private void Exchange(ref Tile t1, ref Tile t2)
        {
           
            int imageIndex=0;

            imageIndex = t1.ImageIndex;
            t1.ImageIndex = t2.ImageIndex;
            t2.ImageIndex = imageIndex;
        }

        private void DownTitle(Tile t,int n)
        {
            int x = t.X;
            int y = t.Y;

            int temp_x = x - 1;
            int need_gen_num = n;
            while (temp_x != -1)
            {
                MapTiles[temp_x + n, y].ImageIndex = MapTiles[temp_x,y].ImageIndex;
                temp_x--;
            }

            Random rnd=new Random((int)System.DateTime.Now.Millisecond);
            for (int i = 0; i < need_gen_num; i++)
            {
                MapTiles[i, y].ImageIndex = rnd.Next(8) + 1;
            }            
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            int y = e.X / _blockWidth;
            int x = e.Y / _blockWidth;

            label1.Text = x.ToString() + "," + y.ToString();

            if (isPointOutOfBorder(x, y))
            {
                _selTile = null;
                return;
            }

            if (_selTile == null)
            {
                _selTile = MapTiles[x, y];
                return;
            }
            else
            {
                Tile tile2 = MapTiles[x, y];
                if (CanExchange(_selTile, tile2))
                {
                    //MessageBox.Show("Can Hide!");
                    Exchange(ref _selTile, ref tile2);

                    Tile[] horzTiles = GetCanHideHorzTiles(tile2);

                    if (horzTiles.Length >= 3)
                    {
                        foreach (Tile t in horzTiles)
                        {
                            DownTitle(t, 1);
                        }
                    }

                    Tile[] vertTiles = GetCanHideVertTiles(tile2);
                    int ilen = vertTiles.Length;

                    if (ilen >= 3)
                    {
                        DownTitle(tile2, ilen);
                    }

                    _selTile = null;
                    ScanCanAutoHide();
                    ShowData();
                }
                else
                {
                    MessageBox.Show("Invalid Exchange!");
                }
            }
            
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            if (_GameStart)
            {
                Graphics g = e.Graphics;
                for (int i = 0; i < HorzCount; i++)
                    for (int j = 0; j < VertCount; j++)
                    {
                        int ImageIndex = MapTiles[i, j].ImageIndex;
                        string fileName = Application.StartupPath + "/Images/" + ImageIndex + ".png";

                        Bitmap bmp = new Bitmap(fileName);

                        g.DrawImage(bmp, MapTiles[i, j].OffsetX, MapTiles[i, j].OffsetY, _blockWidth, _blockWidth);
                    }
            }
        }
    }
}
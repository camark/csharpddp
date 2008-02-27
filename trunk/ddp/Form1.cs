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
        public Form1()
        {
            InitializeComponent();
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
                }

            for (int i = 0; i < HorzCount; i++)
                for (int j = 0; j < VertCount; j++)
                {                    
                    if(CheckCanHide(MapTiles[i,j]))
                    {
                        while (!CheckCanHide(MapTiles[i, j]))
                        {
                            MapTiles[i, j].ImageIndex = rnd.Next(8) + 1;
                        }
                    }
                }

            ShowData();
        }

        /// <summary>
        /// �鿴�Ƿ��н�
        /// 1���н�
        /// 2���޽⣬��Ҫ����Shuffle
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
        /// ���ҿ���ˮƽ�����ķ������
        /// </summary>
        /// <returns></returns>
        private Tile[] GetCanHideHorzTiles(Tile t)
        {
            int x = t.X;
            int y = t.Y;

            int temp_x = 0;
            int temp_y = 0;


            //����ˮƽ����
            List<Tile> tiles = new List<Tile>();

            //���
            temp_x = x;
            temp_y = y - 1;
            if (!isPointOutOfBorder(temp_x, temp_y))
            {
                Tile temp_tile=MapTiles[temp_x,temp_y];
                while (!isPointOutOfBorder(temp_x, temp_y) && temp_tile.ImageIndex == t.ImageIndex)
                {
                    tiles.Add(temp_tile);
                    temp_y--;
                    temp_tile = MapTiles[temp_x, temp_y];
                }
            }

            //�Ҳ�
            temp_y = y + 1;
            if (!isPointOutOfBorder(temp_x, temp_y))
            {
                Tile temp_tile = MapTiles[temp_x, temp_y];
                while (!isPointOutOfBorder(temp_x, temp_y) && temp_tile.ImageIndex == t.ImageIndex)
                {
                    tiles.Add(temp_tile);
                    temp_y++;
                    temp_tile = MapTiles[temp_x, temp_y];
                }
            }

            return tiles.ToArray();

        }


        /// <summary>
        /// ���ҿ�����ֱ���������ķ���
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private Tile[] GetCanHideVertTiles(Tile t)
        {
            int x = t.X;
            int y = t.Y;

            int temp_x = 0;
            int temp_y = 0;


            //����ˮƽ����
            List<Tile> tiles = new List<Tile>();

            //�Ϸ�
            temp_x = x-1;
            temp_y = y;
            if (!isPointOutOfBorder(temp_x, temp_y))
            {
                Tile temp_tile = MapTiles[temp_x, temp_y];
                while (!isPointOutOfBorder(temp_x, temp_y) && temp_tile.ImageIndex == t.ImageIndex)
                {
                    tiles.Add(temp_tile);
                    temp_y--;
                    temp_tile = MapTiles[temp_x, temp_y];
                }
            }

            //�·�
            temp_x = x + 1;
            if (!isPointOutOfBorder(temp_x, temp_y))
            {
                Tile temp_tile = MapTiles[temp_x, temp_y];
                while (!isPointOutOfBorder(temp_x, temp_y) && temp_tile.ImageIndex == t.ImageIndex)
                {
                    tiles.Add(temp_tile);
                    temp_x++;
                    temp_tile = MapTiles[temp_x, temp_y];
                }
            }

            return tiles.ToArray();

        }

        private bool isPointOutOfBorder(int x, int y)
        {
            return x == -1 || y == -1 || x == HorzCount || y == VertCount;
        }

        private bool CheckCanHide(Tile t)
        {
            int x = t.X;
            int y = t.Y;

            

            int temp_x = 0;
            int temp_y = 0;

            //�Ϸ����
            temp_x = x - 1;
            temp_y = y;
            int SamCol = 1;
            if(!isPointOutOfBorder(temp_x,temp_y))
            {
                Tile temp_tile=MapTiles[temp_x,temp_y];

                while (!isPointOutOfBorder(temp_x, temp_y) && temp_tile.ImageIndex == t.ImageIndex && temp_tile.ImageIndex!=-1)
                {
                    SamCol++;

                    temp_x--;
                    temp_tile = MapTiles[temp_x, temp_y];
                }

                if (SamCol >= 3)
                    return true;
            }

            //�·����
            temp_x = x + 1;
            temp_y = y;
            SamCol = 1;
            if (!isPointOutOfBorder(temp_x, temp_y))
            {
                Tile temp_tile = MapTiles[temp_x, temp_y];

                while (!isPointOutOfBorder(temp_x, temp_y) && temp_tile.ImageIndex == t.ImageIndex && temp_tile.ImageIndex != -1)
                {
                    SamCol++;

                    temp_x++;
                    temp_tile = MapTiles[temp_x, temp_y];
                }

                if (SamCol >= 3)
                    return true;
            }

            //�����
            temp_x = x; 
            temp_y = y-1;
            SamCol = 1;
            if (!isPointOutOfBorder(temp_x, temp_y))
            {
                Tile temp_tile = MapTiles[temp_x, temp_y];

                while (!isPointOutOfBorder(temp_x, temp_y) && temp_tile.ImageIndex == t.ImageIndex && temp_tile.ImageIndex != -1)
                {
                    SamCol++;

                    temp_y--;
                    temp_tile = MapTiles[temp_x, temp_y];
                }

                if (SamCol >= 3)
                    return true;
            }

            //�Ҳ���
            temp_x = x;
            temp_y = y + 1;
            SamCol = 1;
            if (!isPointOutOfBorder(temp_x, temp_y))
            {
                Tile temp_tile = MapTiles[temp_x, temp_y];

                while (!isPointOutOfBorder(temp_x, temp_y) && temp_tile.ImageIndex == t.ImageIndex && temp_tile.ImageIndex != -1)
                {
                    SamCol++;

                    temp_y++;
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

                for (int i = 0; i < HorzCount; i++)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = MapTiles[i, 0].ImageIndex.ToString();
                    for (int j = 1; j < VertCount; j++)
                    {
                        item.SubItems.Add(MapTiles[i,j].ImageIndex.ToString());
                    }

                    listView1.Items.Add(item);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            InitGameData();
        }
    }
}
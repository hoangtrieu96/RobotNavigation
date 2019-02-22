using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RobotNavigationGUI
{
    public partial class Form2 : Form
    {
        private Grid[,] _map;
        private int _mapRows;
        private int _mapCols;
        private String _path;
        private int _numberOfNodes;
        private CombineObject _aObject;

        public Form2(String fileName, String searchMethod)
        {
            InitializeComponent();
            _aObject = SetUpAndProcess.InitializeAndProcess(fileName, searchMethod);
            _map = _aObject.Map;
            _path = _aObject.Path;
            _numberOfNodes = _aObject.NumberOfNodes;
            _mapRows = _map.GetLength(0);
            _mapCols = _map.GetLength(1);
            richTextBox1.Text = ("Filename: " + fileName + "\nMethod: " + searchMethod + "\nNumber of nodes: " + _numberOfNodes + "\nPath: " + _path);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics gWindow = panel1.CreateGraphics();

            Brush blackBrush = new SolidBrush(Color.Black);
            Pen blackPen = new Pen(blackBrush, 5);
            Brush grayBrush = new SolidBrush(Color.Gray);
            Brush greenBrush = new SolidBrush(Color.LimeGreen);
            Brush redBrush = new SolidBrush(Color.Red);
            Brush blueBrush = new SolidBrush(Color.LightBlue);
            Brush yellowBrush = new SolidBrush(Color.Yellow);
            Brush whiteBrush = new SolidBrush(Color.White);

            blackPen.StartCap = LineCap.Flat;
            blackPen.EndCap = LineCap.ArrowAnchor;

            int gridWidth = 50;
            panel1.Size = new Size(_mapCols * gridWidth + 15, _mapRows * gridWidth + 15);
            this.Size = new Size(_mapCols * gridWidth + 60, _mapRows * gridWidth + 180);
            richTextBox1.Size = new Size(_mapCols * gridWidth, 100);
            richTextBox1.Location = new Point(panel1.Location.X + 10, _mapRows * gridWidth + 30);

            //Draw final map
            foreach (Grid g in _map)
            {
                gWindow.DrawRectangle(blackPen, g.X * gridWidth + 10, g.Y * gridWidth + 10, gridWidth, gridWidth);
                if (g.Type == GridType.GROUND)
                {
                    gWindow.FillRectangle(whiteBrush, g.X * gridWidth + 12, g.Y * gridWidth + 12, gridWidth - 3, gridWidth - 3);
                }
                if (g.Type == GridType.WALL)
                {
                    gWindow.FillRectangle(grayBrush, g.X * gridWidth + 12, g.Y * gridWidth + 12, gridWidth - 3, gridWidth - 3);
                }
                if ((g.Explored == true || g.Marked == true) && g.Type != GridType.ROBOT)
                {
                    gWindow.FillRectangle(blueBrush, g.X * gridWidth + 12, g.Y * gridWidth + 12, gridWidth - 3, gridWidth - 3);
                }
                if (g.Type == GridType.GOAL)
                {
                    gWindow.FillRectangle(greenBrush, g.X * gridWidth + 12, g.Y * gridWidth + 12, gridWidth - 3, gridWidth - 3);
                }
                if (g.Type == GridType.ROBOT)
                {
                    gWindow.FillRectangle(redBrush, g.X * gridWidth + 12, g.Y * gridWidth + 12, gridWidth - 3, gridWidth - 3);
                }
                if (g.Type == GridType.PATH)
                {
                    gWindow.FillRectangle(yellowBrush, g.X * gridWidth + 12, g.Y * gridWidth + 12, gridWidth - 3, gridWidth - 3);
                    switch (g.PathLeadTo)
                    {
                        case "Up":
                            gWindow.DrawLine(blackPen, g.X * gridWidth + 12 + 25, g.Y * gridWidth + 50, g.X * gridWidth + 12 + 25, g.Y * gridWidth + 22);
                            break;
                        case "Left":
                            gWindow.DrawLine(blackPen, g.X * gridWidth + 50, g.Y * gridWidth + 30, g.X * gridWidth + 22, g.Y * gridWidth + 30);
                            break;
                        case "Down":
                            gWindow.DrawLine(blackPen, g.X * gridWidth + 12 + 25, g.Y * gridWidth + 22, g.X * gridWidth + 12 + 25, g.Y * gridWidth + 50);
                            break;
                        case "Right":
                            gWindow.DrawLine(blackPen, g.X * gridWidth + 22, g.Y * gridWidth + 30, g.X * gridWidth + 50, g.Y * gridWidth + 30);
                            break;
                    }
                }
            }
        }
    }
}

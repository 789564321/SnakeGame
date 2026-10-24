using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System;
using System.ComponentModel;
using System.Data.SqlTypes;
namespace SnakeGame
{
    partial class SnakeGame
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // SnakeGame
            // 
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Name = "SnakeGame";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ResumeLayout(false);
        }

        // 游戏常量
        private const int GridSize = 20;
        private const int MinSpeed = 50;
        private const int SpeedDecrement = 5;
        private const int InitialSpeed = 150;
        private const int ScorePerFood = 10;

        // 游戏对象
        private System.Collections.Generic.List<System.Drawing.Point> snake = new System.Collections.Generic.List<System.Drawing.Point>();
        private System.Drawing.Point food;
        private Direction direction = Direction.Right;
        private Direction nextDirection = Direction.Right;
        private System.Windows.Forms.Panel pnlStart;

        // 游戏状态
        private int score = 0;
        private int gameSpeed = InitialSpeed;
        private bool isGameOver = false;
        private int colorIndex = 0;
        private bool isPaused = false;
        private bool isGameStarted = false;

        // 图形和动画相关
        private System.Windows.Forms.Timer gameTimer = new System.Windows.Forms.Timer();
        private System.Windows.Forms.Timer animationTimer = new System.Windows.Forms.Timer();
        private float foodScale = 1.0f;
        private float foodScaleDir = 0.02f;
        private float foodRotation = 0;
        private System.Windows.Forms.Panel pnlPause;
        private System.Windows.Forms.Label lblScore;
        private System.Windows.Forms.Panel pnlGameOver;
        private System.Windows.Forms.Label lblSpeedInPause;

        // 彩虹色渐变
        private System.Drawing.Color[] rainbowColors = new System.Drawing.Color[]
        {
            System.Drawing.Color.FromArgb(255, 255, 94, 94),
            System.Drawing.Color.FromArgb(255, 255, 125, 94),
            System.Drawing.Color.FromArgb(255, 255, 179, 71),
            System.Drawing.Color.FromArgb(255, 255, 242, 117),
            System.Drawing.Color.FromArgb(255, 147, 255, 150),
            System.Drawing.Color.FromArgb(255, 66, 215, 245),
            System.Drawing.Color.FromArgb(255, 125, 95, 255),
            System.Drawing.Color.FromArgb(255, 199, 116, 232)
        };

        public enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }
    }

    // 扩展方法：绘制圆角矩形
    public static class GraphicsExtensions
    {
        public static void FillRoundedRectangle(this System.Drawing.Graphics g, System.Drawing.Brush brush,
            int x, int y, int width, int height, int radius)
        {
            using (System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath())
            {
                path.AddArc(x, y, radius, radius, 180, 90);
                path.AddArc(x + width - radius, y, radius, radius, 270, 90);
                path.AddArc(x + width - radius, y + height - radius, radius, radius, 0, 90);
                path.AddArc(x, y + height - radius, radius, radius, 90, 90);
                path.CloseAllFigures();
                g.FillPath(brush, path);
            }
        }
    }
}

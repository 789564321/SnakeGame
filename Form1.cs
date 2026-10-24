using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SnakeGame
{
    public partial class SnakeGame : Form
    {

        public SnakeGame()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void CreateStartPanel()
        {
            if (pnlStart != null)
            {
                this.Controls.Remove(pnlStart);
                pnlStart.Dispose();
            }

            pnlStart = new Panel()
            {
                Size = this.ClientSize,
                BackColor = Color.FromArgb(220, 15, 23, 42),
                Visible = true
            };

            // 游戏标题
            var titleLabel = new Label()
            {
                Location = new Point(pnlStart.Width / 2 - 200, pnlStart.Height / 2 - 120),
                Size = new Size(400, 80),
                ForeColor = Color.FromArgb(255, 147, 197, 253),
                Font = new Font("Microsoft YaHei", 32, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Text = "贪吃蛇游戏"
            };

            // 副标题
            var subtitleLabel = new Label()
            {
                Location = new Point(pnlStart.Width / 2 - 150, pnlStart.Height / 2 - 40),
                Size = new Size(300, 30),
                ForeColor = Color.FromArgb(255, 148, 163, 184),
                Font = new Font("Microsoft YaHei", 14, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleCenter,
                Text = "经典重生，绚丽进化"
            };

            // 开始游戏按钮
            var startButton = new Button()
            {
                Location = new Point(pnlStart.Width / 2 - 100, pnlStart.Height / 2 + 20),
                Size = new Size(200, 45),
                BackColor = Color.FromArgb(79, 70, 229),
                ForeColor = Color.White,
                Font = new Font("Microsoft YaHei", 14, FontStyle.Bold),
                Text = "开始游戏",
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            startButton.FlatAppearance.BorderSize = 0;
            startButton.Click += StartGame;

            // 游戏说明
            var instructionLabel = new Label()
            {
                Location = new Point(pnlStart.Width / 2 - 200, pnlStart.Height / 2 + 90),
                Size = new Size(400, 120),
                ForeColor = Color.FromArgb(255, 148, 163, 184),
                Font = new Font("Microsoft YaHei", 11, FontStyle.Regular),
                TextAlign = ContentAlignment.TopCenter,
                Text = "游戏控制:\n" +
                       "↑↓←→ 方向键控制移动\n" +
                       "ESC 暂停/继续游戏\n" +
                       "空格键 重新开始游戏\n" +
                       "在暂停界面可以看到当前游戏速度\n"+
                       "吃到食物可以加速，蛇身会变色！"
            };

            // 版权信息
            var copyrightLabel = new Label()
            {
                Location = new Point(pnlStart.Width / 2 - 150, pnlStart.Height - 50),
                Size = new Size(300, 20),
                ForeColor = Color.FromArgb(255, 100, 116, 139),
                Font = new Font("Microsoft YaHei", 9, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleCenter,
                Text = "贪吃蛇游戏 - 进阶版"
            };

            pnlStart.Controls.Add(titleLabel);
            pnlStart.Controls.Add(subtitleLabel);
            pnlStart.Controls.Add(startButton);
            pnlStart.Controls.Add(instructionLabel);
            pnlStart.Controls.Add(copyrightLabel);
            this.Controls.Add(pnlStart);
            pnlStart.BringToFront();
        }

        private void StartGame(object sender, EventArgs e)
        {
            if (pnlStart != null)
            {
                pnlStart.Visible = false;
            }

            isGameStarted = true;
            // 确保游戏状态正确初始化
            isGameOver = false;
            isPaused = false;

            if (!gameTimer.Enabled)
            {
                gameTimer.Start();
            }
            if (!animationTimer.Enabled)
            {
                animationTimer.Start();
            }

            this.Focus();
        }

        private void InitializeGame()
        {
            // 设置窗口
            this.Text = "进阶版贪吃蛇";
            this.BackColor = Color.FromArgb(15, 23, 42);
            this.ClientSize = new Size(800, 600);
            this.DoubleBuffered = true;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.KeyPreview = true;

            // 创建初始界面
            CreateStartPanel();

            // 初始化蛇
            snake.Clear();
            snake.Add(new Point(10 * GridSize, 10 * GridSize));
            snake.Add(new Point(9 * GridSize, 10 * GridSize));
            snake.Add(new Point(8 * GridSize, 10 * GridSize));

            // 初始化食物
            GenerateFood();
            gameSpeed = InitialSpeed;

            // 初始化游戏计时器
            if (gameTimer != null)
            {
                gameTimer.Stop();
                gameTimer.Dispose();
            }
            gameTimer = new Timer();
            gameTimer.Interval = gameSpeed;
            gameTimer.Tick += GameLoop;

            // 初始化动画计时器
            if (animationTimer != null)
            {
                animationTimer.Stop();
                animationTimer.Dispose();
            }
            animationTimer = new Timer();
            animationTimer.Interval = 30;
            animationTimer.Tick += AnimationLoop;

            // 重置游戏状态
            direction = Direction.Right;
            nextDirection = Direction.Right;
            isGameOver = false;
            isPaused = false;
            isGameStarted = false;
            score = 0;
            colorIndex = 0;
            gameSpeed = InitialSpeed;

            // 创建分数标签
            CreateScoreLabel();

            // 创建暂停面板
            CreatePausePanel();

            // 创建游戏结束面板
            CreateGameOverPanel();

            this.Focus();
        }

        private void CreateScoreLabel()
        {
            if (lblScore != null)
            {
                this.Controls.Remove(lblScore);
                lblScore.Dispose();
            }

            lblScore = new Label()
            {
                Location = new Point(20, 20),
                ForeColor = Color.White,
                Font = new Font("Microsoft YaHei", 12, FontStyle.Bold),
                Text = $"分数: {score}",
                AutoSize = true,
                BackColor = Color.FromArgb(128, 15, 23, 42),
                Padding = new Padding(8, 4, 8, 4),
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(lblScore);
            lblScore.BringToFront();
        }

        private void CreatePausePanel()
        {
            if (pnlPause != null)
            {
                this.Controls.Remove(pnlPause);
                pnlPause.Dispose();
            }

            pnlPause = new Panel()
            {
                Size = this.ClientSize,
                BackColor = Color.FromArgb(128, 15, 23, 42),
                Visible = false
            };

            var pauseLabel = new Label()
            {
                Location = new Point(pnlPause.Width / 2 - 150, pnlPause.Height / 2 - 80),
                Size = new Size(300, 60),
                ForeColor = Color.White,
                Font = new Font("Microsoft YaHei", 24, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Text = "游戏暂停"
            };

            // 游戏速度显示
            lblSpeedInPause = new Label()
            {
                Location = new Point(pnlPause.Width / 2 - 150, pnlPause.Height / 2 - 20),
                Size = new Size(300, 30),
                ForeColor = Color.FromArgb(255, 147, 197, 253),
                Font = new Font("Microsoft YaHei", 14, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Text = $"当前速度: {GetSpeedLevel()}级"
            };

            var resumeButton = new Button()
            {
                Location = new Point(pnlPause.Width / 2 - 100, pnlPause.Height / 2 + 20),
                Size = new Size(200, 40),
                BackColor = Color.FromArgb(79, 70, 229),
                ForeColor = Color.White,
                Font = new Font("Microsoft YaHei", 12, FontStyle.Bold),
                Text = "继续游戏"
            };
            resumeButton.Click += ResumeGame;

            var resumeLabel = new Label()
            {
                Location = new Point(pnlPause.Width / 2 - 100, pnlPause.Height / 2 + 70),
                Size = new Size(200, 30),
                ForeColor = Color.White,
                Font = new Font("Microsoft YaHei", 12),
                TextAlign = ContentAlignment.MiddleCenter,
                Text = "或按ESC继续"
            };

            pnlPause.Controls.Add(pauseLabel);
            pnlPause.Controls.Add(lblSpeedInPause);
            pnlPause.Controls.Add(resumeButton);
            pnlPause.Controls.Add(resumeLabel);
            this.Controls.Add(pnlPause);
            pnlPause.BringToFront();
        }

        /// <summary>
        /// 计算速度等级
        /// </summary>
        /// <returns>速度等级</returns>
        private int GetSpeedLevel()
        {
            // 初始速度为150ms，最小速度为50ms
            // 速度等级 = (初始速度 - 当前速度) / 速度递减步长 + 1
            return (InitialSpeed - gameSpeed) / SpeedDecrement + 1;
        }

        /// <summary>
        /// 更新暂停面板中的速度显示
        /// </summary>
        private void UpdateSpeedInPausePanel()
        {
            if (lblSpeedInPause != null)
            {
                int speedLevel = GetSpeedLevel();
                lblSpeedInPause.Text = $"当前速度: {speedLevel}级";

                // 根据速度等级设置不同的颜色
                if (speedLevel <= 3)
                    lblSpeedInPause.ForeColor = Color.FromArgb(255, 147, 197, 253); // 蓝色
                else if (speedLevel <= 6)
                    lblSpeedInPause.ForeColor = Color.FromArgb(255, 134, 239, 172); // 绿色
                else if (speedLevel <= 9)
                    lblSpeedInPause.ForeColor = Color.FromArgb(255, 253, 224, 71);  // 黄色
                else
                    lblSpeedInPause.ForeColor = Color.FromArgb(255, 248, 113, 113); // 红色
            }
        }

        private void CreateGameOverPanel()
        {
            if (pnlGameOver != null)
            {
                this.Controls.Remove(pnlGameOver);
                pnlGameOver.Dispose();
            }

            pnlGameOver = new Panel()
            {
                Size = this.ClientSize,
                BackColor = Color.FromArgb(128, 15, 23, 42),
                Visible = false
            };

            var gameOverLabel = new Label()
            {
                Location = new Point(pnlGameOver.Width / 2 - 150, pnlGameOver.Height / 2 - 80),
                Size = new Size(300, 60),
                ForeColor = Color.White,
                Font = new Font("Microsoft YaHei", 24, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Text = "游戏结束!"
            };

            var finalScoreLabel = new Label()
            {
                Location = new Point(pnlGameOver.Width / 2 - 100, pnlGameOver.Height / 2),
                Size = new Size(200, 40),
                ForeColor = Color.White,
                Font = new Font("Microsoft YaHei", 16),
                TextAlign = ContentAlignment.MiddleCenter,
                Name = "finalScoreLabel"
            };

            var restartButton = new Button()
            {
                Location = new Point(pnlGameOver.Width / 2 - 80, pnlGameOver.Height / 2 + 60),
                Size = new Size(160, 40),
                BackColor = Color.FromArgb(79, 70, 229),
                ForeColor = Color.White,
                Font = new Font("Microsoft YaHei", 12, FontStyle.Bold),
                Text = "返回标题"
            };
            restartButton.Click += RestartGame;

            pnlGameOver.Controls.Add(gameOverLabel);
            pnlGameOver.Controls.Add(finalScoreLabel);
            pnlGameOver.Controls.Add(restartButton);
            this.Controls.Add(pnlGameOver);
            pnlGameOver.BringToFront();
        }

        private void GenerateFood()
        {
            Random rand = new Random();
            int maxX = (this.ClientSize.Width / GridSize) - 1;
            int maxY = (this.ClientSize.Height / GridSize) - 1;

            do
            {
                food = new Point(
                    rand.Next(1, maxX) * GridSize,
                    rand.Next(1, maxY) * GridSize
                );
            } while (snake.Contains(food));
        }

        private void GameLoop(object sender, EventArgs e)
        {
            if (!isGameStarted || isGameOver || isPaused) return;

            // 更新方向
            direction = nextDirection;

            // 计算新头部位置
            Point head = snake[0];
            switch (direction)
            {
                case Direction.Up:
                    head = new Point(head.X, head.Y - GridSize);
                    break;
                case Direction.Down:
                    head = new Point(head.X, head.Y + GridSize);
                    break;
                case Direction.Left:
                    head = new Point(head.X - GridSize, head.Y);
                    break;
                case Direction.Right:
                    head = new Point(head.X + GridSize, head.Y);
                    break;
            }

            // 检查碰撞
            if (head.X < 0 || head.X >= this.ClientSize.Width ||
                head.Y < 0 || head.Y >= this.ClientSize.Height ||
                snake.Contains(head))
            {
                GameOver();
                return;
            }

            // 添加新头部
            snake.Insert(0, head);

            // 检查是否吃到食物
            if (head == food)
            {
                score += ScorePerFood;
                lblScore.Text = $"分数: {score}";

                // 增加速度
                gameSpeed = Math.Max(MinSpeed, gameSpeed - SpeedDecrement);
                gameTimer.Interval = gameSpeed;

                // 改变颜色
                colorIndex = (colorIndex + 1) % rainbowColors.Length;

                // 生成新食物
                GenerateFood();
            }
            else
            {
                // 移除尾部
                snake.RemoveAt(snake.Count - 1);
            }

            this.Invalidate();
        }

        private void AnimationLoop(object sender, EventArgs e)
        {
            if (!isGameStarted || isGameOver || isPaused) return;

            // 食物动画
            foodScale += foodScaleDir;
            if (foodScale > 1.2f || foodScale < 0.8f)
                foodScaleDir *= -1;

            foodRotation += 2;
            if (foodRotation >= 360)
                foodRotation = 0;

            this.Invalidate();
        }

        private void GameOver()
        {
            isGameOver = true;
            gameTimer.Stop();
            animationTimer.Stop();

            // 更新最终分数显示
            foreach (Control control in pnlGameOver.Controls)
            {
                if (control.Name == "finalScoreLabel")
                {
                    control.Text = $"最终分数: {score}";
                    break;
                }
            }

            pnlGameOver.Visible = true;
        }

        private void RestartGame(object sender, EventArgs e)
        {
            // 隐藏所有面板
            pnlGameOver.Visible = false;
            pnlPause.Visible = false;

            // 停止计时器
            gameTimer.Stop();
            animationTimer.Stop();

            // 重置游戏状态
            isGameStarted = false;
            isGameOver = false;
            isPaused = false;

            // 重新初始化游戏状态
            InitializeGameState();

            // 显示初始界面
            if (pnlStart != null)
            {
                pnlStart.Visible = true;
            }

            // 强制重绘，清除之前的游戏画面
            this.Invalidate();
        }

        private void InitializeGameState()
        {
            // 初始化蛇
            snake.Clear();
            snake.Add(new Point(10 * GridSize, 10 * GridSize));
            snake.Add(new Point(9 * GridSize, 10 * GridSize));
            snake.Add(new Point(8 * GridSize, 10 * GridSize));

            // 初始化食物
            GenerateFood();
            gameSpeed = InitialSpeed;
            if (gameTimer != null)
            {
                gameTimer.Interval = gameSpeed;
            }

            // 重置游戏状态
            direction = Direction.Right;
            nextDirection = Direction.Right;
            score = 0;
            colorIndex = 0;

            // 重置动画状态
            foodScale = 1.0f;
            foodScaleDir = 0.02f;
            foodRotation = 0;

            // 更新分数显示
            if (lblScore != null)
            {
                lblScore.Text = $"分数: {score}";
            }
        }

        private void ResumeGame(object sender, EventArgs e)
        {
            if (isPaused && !isGameOver)
            {
                isPaused = false;
                gameTimer.Start();
                animationTimer.Start();
                pnlPause.Visible = false;
                this.Focus();
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            // 在初始界面时的按键处理
            if (!isGameStarted)
            {
                switch (e.KeyCode)
                {
                    case Keys.Space:
                    case Keys.Enter:
                        // 空格键或回车键开始游戏
                        StartGame(null, EventArgs.Empty);
                        break;
                    case Keys.Escape:
                        DialogResult result = MessageBox.Show("是否结束游戏？", "询问", MessageBoxButtons.OKCancel);
                        if (result == DialogResult.OK)
                        {
                            this.Close();
                        }
                        break;
                }
                return; // 在初始界面时，不处理其他按键
            }

            if (isGameOver)
            {
                if (e.KeyCode == Keys.Space)
                {
                    RestartGame(null, EventArgs.Empty);
                }
                return;
            }

            // 暂停/继续游戏
            if (e.KeyCode == Keys.Escape)
            {
                isPaused = !isPaused;
                if (isPaused)
                {
                    gameTimer.Stop();
                    animationTimer.Stop();
                    // 更新暂停面板中的速度显示
                    UpdateSpeedInPausePanel();
                    pnlPause.Visible = true;
                }
                else
                {
                    gameTimer.Start();
                    animationTimer.Start();
                    pnlPause.Visible = false;
                    this.Focus();
                }
                return;
            }

            if (isPaused) return;

            // 方向控制
            switch (e.KeyCode)
            {
                case Keys.Up:
                    if (direction != Direction.Down)
                        nextDirection = Direction.Up;
                    break;
                case Keys.Down:
                    if (direction != Direction.Up)
                        nextDirection = Direction.Down;
                    break;
                case Keys.Left:
                    if (direction != Direction.Right)
                        nextDirection = Direction.Left;
                    break;
                case Keys.Right:
                    if (direction != Direction.Left)
                        nextDirection = Direction.Right;
                    break;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            DrawGrid(g);
            if (isGameStarted && !isGameOver)
            {
                DrawSnake(g);
                DrawFood(g);
            }
        }

        private void DrawGrid(Graphics g)
        {
            using (Pen pen = new Pen(Color.FromArgb(30, 255, 255, 255), 1))
            {
                for (int y = 0; y < this.ClientSize.Height; y += GridSize)
                {
                    g.DrawLine(pen, 0, y, this.ClientSize.Width, y);
                }

                for (int x = 0; x < this.ClientSize.Width; x += GridSize)
                {
                    g.DrawLine(pen, x, 0, x, this.ClientSize.Height);
                }
            }
        }

        private void DrawSnake(Graphics g)
        {
            for (int i = 0; i < snake.Count; i++)
            {
                int segmentColorIndex = (colorIndex + i / 3) % rainbowColors.Length;
                Color segmentColor = rainbowColors[segmentColorIndex];

                using (LinearGradientBrush brush = new LinearGradientBrush(
                    new Rectangle(snake[i], new Size(GridSize, GridSize)),
                    Color.FromArgb(255, segmentColor),
                    Color.FromArgb(200, segmentColor.R / 2, segmentColor.G / 2, segmentColor.B / 2),
                    LinearGradientMode.ForwardDiagonal))
                {
                    float scale = 1 + (float)Math.Sin(DateTime.Now.Millisecond / 300.0 + i * 0.5) * 0.05f;
                    int size = (int)(GridSize * 0.8 * scale);
                    int offset = (GridSize - size) / 2;

                    g.FillRoundedRectangle(brush,
                        snake[i].X + offset, snake[i].Y + offset,
                        size, size, 4);

                    using (SolidBrush highlightBrush = new SolidBrush(Color.FromArgb(50, 255, 255, 255)))
                    {
                        g.FillRoundedRectangle(highlightBrush,
                            snake[i].X + offset, snake[i].Y + offset,
                            size * 2 / 3, size * 2 / 3, 2);
                    }
                }

                if (i == 0)
                {
                    DrawEyes(g, snake[i]);
                }
            }
        }

        private void DrawEyes(Graphics g, Point head)
        {
            int eyeSize = GridSize / 8;
            int offset = GridSize / 4;

            Point leftEye = new Point();
            Point rightEye = new Point();

            switch (direction)
            {
                case Direction.Right:
                    leftEye = new Point(head.X + GridSize - offset, head.Y + offset);
                    rightEye = new Point(head.X + GridSize - offset, head.Y + GridSize - offset);
                    break;
                case Direction.Left:
                    leftEye = new Point(head.X + offset, head.Y + offset);
                    rightEye = new Point(head.X + offset, head.Y + GridSize - offset);
                    break;
                case Direction.Up:
                    leftEye = new Point(head.X + offset, head.Y + offset);
                    rightEye = new Point(head.X + GridSize - offset, head.Y + offset);
                    break;
                case Direction.Down:
                    leftEye = new Point(head.X + offset, head.Y + GridSize - offset);
                    rightEye = new Point(head.X + GridSize - offset, head.Y + GridSize - offset);
                    break;
            }

            float eyeAnim = (float)Math.Sin(DateTime.Now.Millisecond / 300.0) * 2;

            using (SolidBrush brush = new SolidBrush(Color.White))
            {
                g.FillEllipse(brush, leftEye.X + eyeAnim, leftEye.Y, eyeSize, eyeSize);
                g.FillEllipse(brush, rightEye.X - eyeAnim, rightEye.Y, eyeSize, eyeSize);
            }

            using (SolidBrush brush = new SolidBrush(Color.Black))
            {
                float pupilAnimX = (float)Math.Sin(DateTime.Now.Millisecond / 500.0) * 1;
                float pupilAnimY = (float)Math.Cos(DateTime.Now.Millisecond / 500.0) * 1;

                g.FillEllipse(brush, leftEye.X + eyeSize / 2 + pupilAnimX, leftEye.Y + eyeSize / 2 + pupilAnimY, eyeSize / 2, eyeSize / 2);
                g.FillEllipse(brush, rightEye.X + eyeSize / 2 + pupilAnimX, rightEye.Y + eyeSize / 2 + pupilAnimY, eyeSize / 2, eyeSize / 2);
            }
        }

        private void DrawFood(Graphics g)
        {
            GraphicsState state = g.Save();

            g.TranslateTransform(food.X + GridSize / 2, food.Y + GridSize / 2);
            g.RotateTransform(foodRotation);
            g.ScaleTransform(foodScale, foodScale);

            using (GraphicsPath path = new GraphicsPath())
            {
                int points = 5;
                float outerRadius = GridSize / 2f * 0.9f;
                float innerRadius = outerRadius / 2.5f;

                for (int i = 0; i < points * 2; i++)
                {
                    float radius = (i % 2 == 0) ? outerRadius : innerRadius;
                    float angle = (float)(i * Math.PI / points - Math.PI / 2);

                    float x = (float)Math.Cos(angle) * radius;
                    float y = (float)Math.Sin(angle) * radius;

                    if (i == 0)
                    {
                        path.StartFigure();
                        path.AddLine(new PointF(x, y), new PointF(x, y));
                    }
                    else
                    {
                        path.AddLine(path.GetLastPoint(), new PointF(x, y));
                    }
                }
                path.CloseAllFigures();

                using (PathGradientBrush brush = new PathGradientBrush(path))
                {
                    brush.CenterColor = Color.FromArgb(255, 255, 238, 88);
                    brush.SurroundColors = new Color[] { Color.FromArgb(255, 255, 143, 0) };
                    g.FillPath(brush, path);

                    using (Pen pen = new Pen(Color.FromArgb(200, 255, 255, 255), 2))
                    {
                        g.DrawPath(pen, path);
                    }

                    using (SolidBrush highlight = new SolidBrush(Color.FromArgb(150, 255, 255, 255)))
                    {
                        g.FillEllipse(highlight, -outerRadius / 5, -outerRadius / 5, outerRadius / 2.5f, outerRadius / 2.5f);
                    }
                }
            }

            g.Restore(state);
        }
    }
}
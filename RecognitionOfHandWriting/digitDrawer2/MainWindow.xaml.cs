using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using digitTrainer;
using System;
using dataAnalysis;

namespace digitDrawer2
{
    public class MainWindow : Window
    {
        public Rectangle[,] Grid { get; set; }
        public byte[,] DigitData { get; set; }
        public Canvas MyCanvas { get; set; }
        public bool LeftMouseDown { get; set; }
        public bool RightMouseDown { get; set; }

        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            MyCanvas = this.Get<Canvas>("canvas");
            Grid = new Rectangle[28, 28];
            DigitData = new byte[28, 28];
            for (int i = 0; i < 28; i++)
            {
                for (int j = 0; j < 28; j++)
                {
                    Grid[i, j] = new Rectangle()
                    {
                        Width = 20,
                        Height = 20,
                        StrokeThickness = 0
                    };
                    MyCanvas.Children.Add(Grid[i, j]);
                    Canvas.SetLeft(Grid[i, j], i * 20);
                    Canvas.SetTop(Grid[i, j], j * 20);
                }
            }
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);
            if (e.MouseButton == MouseButton.Left)
            {
                LeftMouseDown = true;
                DrawShapes(e.GetPosition(this), Colors.Black);
            }
            else if (e.MouseButton == MouseButton.Right)
            {
                RightMouseDown = true;
                DrawShapes(e.GetPosition(this), Colors.White);
            }
        }

        protected override void OnPointerReleased(PointerReleasedEventArgs e)
        {
            base.OnPointerReleased(e);
            if (e.MouseButton == MouseButton.Left)
            {
                LeftMouseDown = false;
            }
            else if (e.MouseButton == MouseButton.Right)
            {
                RightMouseDown = false;
            }
        }

        protected override void OnPointerMoved(PointerEventArgs e)
        {
            base.OnPointerMoved(e);
            if (LeftMouseDown)
            {
                DrawShapes(e.GetPosition(this), Colors.Black);
            }
            else if (RightMouseDown)
            {
                DrawShapes(e.GetPosition(this), Colors.White);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            switch (e.Key)
            {
                case Key.Enter:
                    var network = Util.LoadNetworkData("../"+digitTrainer.Program.NetworkDataPath);
                    double[] input = new double[28 * 28];
                    int counter = 0;
                    var croppedImg =CropImage.Crop(DigitData);
                    for (int i = 0; i < 28; i++)
                    {
                        for (int j = 0; j < 28; j++)
                        {
                            input[counter] = croppedImg[j, i]==255?1:0;
                            counter++;
                        }
                    }
                    
                    network.InsertInput(input);
                    network.FeedForward();
                    digitTrainer.DigitData.ShowDigit(input);
                    network.ShowOutput();
                    
                    break;
            }
        }

        public void DrawShapes(Point pos, Color color)
        {
            if (pos.X < 540 && pos.X >= 20 && pos.Y < 540 && pos.Y >= 20)
            {
                int x = (int)pos.X / 20;
                int y = (int)pos.Y / 20;
                
                Grid[x- 1, y- 1].Fill = new SolidColorBrush(color);
                Grid[x, y- 1].Fill = new SolidColorBrush(color);
                Grid[x + 1, y- 1].Fill = new SolidColorBrush(color);
                Grid[x - 1, y].Fill = new SolidColorBrush(color);
                Grid[x, y].Fill = new SolidColorBrush(color);
                Grid[x+ 1, y].Fill = new SolidColorBrush(color);
                Grid[x - 1, y + 1].Fill = new SolidColorBrush(color);
                Grid[x, y + 1].Fill = new SolidColorBrush(color);
                Grid[x+ 1, y + 1].Fill = new SolidColorBrush(color);
                DigitData[x + 1, y - 1] = color.ToUint32() == Colors.Black.ToUint32() ? (byte)255 : (byte)0;
                DigitData[x, y - 1] = color.ToUint32() == Colors.Black.ToUint32() ? (byte)255 : (byte)0;
                DigitData[x - 1, y - 1] = color.ToUint32() == Colors.Black.ToUint32() ? (byte)255 : (byte)0;
                DigitData[x + 1, y] = color.ToUint32() == Colors.Black.ToUint32() ? (byte)255 : (byte)0;
                DigitData[x, y] = color.ToUint32() == Colors.Black.ToUint32() ? (byte)255 : (byte)0;
                DigitData[x - 1, y] = color.ToUint32() == Colors.Black.ToUint32() ? (byte)255 : (byte)0;
                DigitData[x + 1, y + 1] = color.ToUint32() == Colors.Black.ToUint32() ? (byte)255 : (byte)0;
                DigitData[x, y + 1] = color.ToUint32() == Colors.Black.ToUint32() ? (byte)255 : (byte)0;
                DigitData[x - 1, y + 1] = color.ToUint32() == Colors.Black.ToUint32() ? (byte)255 : (byte)0;
            }
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}

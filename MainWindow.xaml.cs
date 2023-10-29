using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ShakerSort
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private const int ArraySize = 59; // Размер сортируемого массива
        private const int Delay = 10; // Задержка анимации (в миллисекундах)

        private int SwapCount = 0;
        private int[] array;
        private Rectangle[] rectangles;
        private Random random;
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void FrameworkElement_OnLoaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)       //очистка канваса, массивов чисел и прямоугольников, текстбоксов, подсчета свапов
        {
            if (array is null || rectangles is null) return;
            canvas.Children.Clear();
            Array.Clear(array, 0, ArraySize);
            Array.Clear(rectangles, 0, ArraySize);
            TbInput.Clear();
            TbOutput.Clear();
            SwapCount = 0;
            lblSwapCount.Content = 0;
        }

        private async void btnStartShakSort_Click(object sender, RoutedEventArgs e)
        {
            btnStart.IsEnabled = false;
            
            await ShakerSort(rectangles);
            
            TbOutput.Text = "Sorted array: {";
            for (var i = 0; i < ArraySize; i++)
            {
                if(i != ArraySize - 1)
                    TbOutput.Text += $"[{array[i]}], ";
                else
                    TbOutput.Text += $"[{array[i]}]";
            }

            TbOutput.Text += "}.";
            
            btnStart.IsEnabled = true;
        }

        private void btnGenerateArr_Click(object sender, RoutedEventArgs e)
        {
            btnGenerate.IsEnabled = false;
            if(!(array is null) && !(rectangles is null))
                btnClear_Click(sender, e);
            InitializeArray();
            InitializeRectangles();
            btnGenerate.IsEnabled = true;
        }
        
        private void InitializeArray()
        {
            array = new int[ArraySize];
            random = new Random();
            TbInput.Text = "Generated array: {";
            for (var i = 0; i < ArraySize; i++)
            {
                array[i] = random.Next(1, 345); // Заполнение массива случайными числами от 1 до 345
                if(i != ArraySize - 1)
                    TbInput.Text += $"[{array[i]}], ";
                else
                    TbInput.Text += $"[{array[i]}]";
            }

            TbInput.Text += "}.";
        }
        
        private void InitializeRectangles()
        {
            rectangles = new Rectangle[ArraySize];

            for (var i = 0; i < ArraySize; i++)
            {
                rectangles[i] = new Rectangle
                {
                    Width = 10,
                    Height = array[i], // Высота прямоугольника соответствует элементу массива
                    Fill = Brushes.Blue
                };
                Canvas.SetTop(rectangles[i], 345 - rectangles[i].Height); // Расположение прямоугольника по оси Y
                Canvas.SetLeft(rectangles[i], i * 12); // Расположение прямоугольника по оси X

                canvas.Children.Add(rectangles[i]); // Добавление прямоугольника на Canvas
            }
        }

        private async Task ShakerSort(Rectangle[] rectangles)
        {
            var left = 0;
            var right = rectangles.Length - 1;
            var swapped = true;

            while (left < right && swapped)
            {
                swapped = false;

                for (var i = left; i < right; i++)
                {
                    rectangles[i].Fill = Brushes.Green;
                    rectangles[i + 1].Fill = Brushes.Green;
                    await Task.Delay(Delay);

                    if (rectangles[i].Height > rectangles[i + 1].Height)
                    {
                        Swap(rectangles, i, i + 1);
                        swapped = true;
                    }

                    rectangles[i].Fill = Brushes.Blue;
                    rectangles[i + 1].Fill = Brushes.Blue;
                }

                right--;

                for (var i = right; i > left; i--)
                {
                    rectangles[i].Fill = Brushes.Red;
                    rectangles[i - 1].Fill = Brushes.Red;
                    await Task.Delay(Delay);

                    if (rectangles[i].Height < rectangles[i - 1].Height)
                    {
                        Swap(rectangles, i, i - 1);
                        swapped = true;
                    }

                    rectangles[i].Fill = Brushes.Blue;
                    rectangles[i - 1].Fill = Brushes.Blue;
                }

                left++;
            }

            foreach (var t in rectangles)
            {
                t.Fill = Brushes.Green;
                await Task.Delay(Delay);
            }
        }
        private void Swap(Rectangle[] rectangles, int i, int j)
        {
            (rectangles[i], rectangles[j]) = (rectangles[j], rectangles[i]);
            (array[i], array[j]) = (array[j], array[i]);
            SwapCount++;
            lblSwapCount.Content = SwapCount;
            var leftTemp = Canvas.GetLeft(rectangles[i]);
            Canvas.SetLeft(rectangles[i], Canvas.GetLeft(rectangles[j]));
            Canvas.SetLeft(rectangles[j], leftTemp);
        }
    }
}
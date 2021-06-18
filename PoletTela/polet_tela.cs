using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Linq;


namespace WPF_Petzold
{
    public class Polet_Tela : Window
    {
        [STAThread]
        public static void Main()
        {
            Application app = new Application();
            app.Run(new Polet_Tela());
        }

        public Polet_Tela()
        {
            Title = "Полёт тела";
            SizeToContent = SizeToContent.WidthAndHeight;

            Grid grid = new Grid();
            grid.Margin = new Thickness(5);
            Content = grid;

            for (int i = 0; i < 6; ++i)
            {
                RowDefinition rowdef = new RowDefinition();
                rowdef.Height = GridLength.Auto;
                grid.RowDefinitions.Add(rowdef);
            }

            for (int i = 0; i < 2; ++i)
            {
                ColumnDefinition coldef = new ColumnDefinition();

                if (i == 1)
                    coldef.Width = new GridLength(100, GridUnitType.Star);
                else
                    coldef.Width = GridLength.Auto;

                grid.ColumnDefinitions.Add(coldef);
            }

            string[] astrLabel = {"Начальная скорость:", "Угол:", "Начальная высота:",
                                  "Масса:", "Коэффициент сопротивления воздуха:"};

            for (int i = 0; i < astrLabel.Length; ++i)
            {
                Label lbl = new Label();
                lbl.Content = astrLabel[i];
                lbl.VerticalContentAlignment = VerticalAlignment.Center;
                grid.Children.Add(lbl);
                Grid.SetRow(lbl, i);
                Grid.SetColumn(lbl, 0);

                TextBox txtbox = new TextBox();
                txtbox.Margin = new Thickness(5);
                grid.Children.Add(txtbox);

                Grid.SetRow(txtbox, i);
                Grid.SetColumn(txtbox, 1);
                Grid.SetColumnSpan(txtbox, 3);
            }
            grid.Children[1].Focus();

            Button btn = new Button();
            btn.Content = "Рассчитать";
            btn.Margin = new Thickness(5);
            btn.Click += delegate { ButtonOnClick(grid); };

            grid.Children.Add(btn);
            Grid.SetRow(btn, 5);
            Grid.SetColumn(btn, 3);
        }

        void ButtonOnClick(Grid grid)
        {
            int[] int_input = new int[5];

            int c = -1;
            for (int i = 1; i < grid.Children.Count; i += 2)
            {
                c++;
                string s = Convert.ToString(grid.Children[i]);
                s = new string(s.Where(el => char.IsDigit(el)).ToArray());
                int value = int.Parse(s);

                int_input[c] = value;
            }

            const double g = 9.8;
            const double delta_time = 0.04;
            double start_height = Convert.ToInt64(int_input[2]);
            double start_speed = Convert.ToInt64(int_input[0]);
            double time = 0;
            double k = Convert.ToInt64(int_input[3]);
            double m = Convert.ToInt64(int_input[4]);

            if (m == 0) m = 0.00000000001;

            double angle = Convert.ToInt64(int_input[1]);
            angle = Math.PI / 180 * angle;

            int N = 200;

            double[] x = new double[N];
            double[] y = new double[N];

            double[] vx = new double[N];
            double[] vy = new double[N];

            vx[0] = start_speed * Math.Cos(angle);
            vy[0] = start_speed * Math.Sin(angle);
            x[0] = 0;
            y[0] = start_height;

            string out_information = $"время: 0 \t\t координата: ({x[0]}; {y[0]})\n";


            for (int i = 1; i < N; ++i)
            {
                time += delta_time;

                vx[i] = vx[i - 1] - (k / m) * vx[i - 1] * delta_time;
                vy[i] = vy[i - 1] - (g + k / m * vy[i - 1]) * delta_time;

                x[i] = x[i - 1] + vx[i - 1] * delta_time;
                y[i] = y[i - 1] + vy[i - 1] * delta_time;

                out_information += $"время: {time:0.00} \t координата: ({x[i]:0.000}; {y[i]:0.000})\n";

                if (y[i] < 0) break;
            }

            MessageBox.Show(out_information, "Отчёт");

        }
    }

}

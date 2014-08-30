using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace ChainReaction
{
    public partial class GamePage : PhoneApplicationPage
    {
        public static int max_players = 2;
        static int rows = 9;
        static int columns = 6;
        Dictionary<string, Brush> color_brush = new Dictionary<string, Brush>();
        DispatcherTimer timer;
        
        //game variables
        int cur_player = 0;
        int game_over = 0;
        List<string> color_list = new List<string>(new[] { "red", "green", "blue", "yellow" });
        Box[,] box_collections = new Box[rows, columns];

        //game variables image
        int _cur_player = 0;
        int _game_over = 0;
        List<string> _color_list = new List<string>(new[] { "red", "green", "blue", "yellow" });
        Box[,] _box_collections = new Box[rows, columns];

        public GamePage()
        {
            InitializeComponent();

            color_brush = new Dictionary<string, Brush>();
            color_brush.Add("red", new SolidColorBrush(Colors.Red));
            color_brush.Add("green", new SolidColorBrush(Colors.Green));
            color_brush.Add("blue", new SolidColorBrush(Colors.Blue));
            color_brush.Add("yellow", new SolidColorBrush(Colors.Yellow));

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Tick += timer_Tick;

            Loaded += GamePage_Loaded;
        }

        void GamePage_Loaded(object sender, RoutedEventArgs e)
        {
            init_Space();
            update_border();
        }

        private void init_Space()
        {
            for (int i = 0; i < rows; i++)
            {
                StackPanel sp = new StackPanel();
                sp.Orientation = System.Windows.Controls.Orientation.Horizontal;
                for (int j = 0; j < columns; j++)
                {
                    box_collections[i, j] = new Box();
                    box_collections[i, j].Tap += box_Tap;

                    sp.Children.Add(box_collections[i, j]);
                    if (i > 0)
                        box_collections[i - 1, j].JoinTo(box_collections[i, j]);
                    if (j > 0)
                        box_collections[i, j - 1].JoinTo(box_collections[i, j]);

                    _box_collections[i, j] = new Box();
                }
                panel.Children.Add(sp);
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            //timer.Stop();

            int red = 0, green = 0, blue = 0, yellow = 0;

            foreach (Box box in box_collections)
            {
                if (box.Count > 0)
                {
                    switch (box.Color)
                    {
                        case "red": ++red; break;
                        case "blue": ++blue; break;
                        case "green": ++green; break;
                        case "yellow": ++yellow; break;
                    }
                }
            }

            if (red == 0)
                color_list.Remove("red");
            if (green == 0)
                color_list.Remove("green");
            if (blue == 0)
                color_list.Remove("blue");
            if (yellow == 0)
                color_list.Remove("yellow");
            
            if (cur_player == Math.Min(max_players, color_list.Count))
                cur_player = 0;

            update_border();

            if (color_list.Count == 1)
            {
                timer.Stop();
                game_over = 1;
                MessageBox.Show(color_list[0] + " wins", "Game Over", MessageBoxButton.OK);
                Box._stop = true;
            }
        }

        private void box_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (game_over == 1)
                return;

            Box._stop = false;
            make_backup();
            //undo_button.IsEnabled = true;

            Box box = sender as Box;

            if (box.Count == 0)
            {
                box.Count = 1;
                box.Color = color_list[cur_player];
                cur_player++;
            }
            else
            {
                if (box.Color == color_list[cur_player])
                {
                    box.Count++;
                    cur_player++;
                }
            }

            if (cur_player == Math.Min(max_players, color_list.Count))
            {
                cur_player = 0;
                timer.Start();
            }

            update_border();
        }

        private void update_border()
        {
            border.BorderBrush = color_brush[color_list[cur_player]];
            foreach (Box b in box_collections)
                b.BorderBrush = color_brush[color_list[cur_player]];
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (game_over == 0)
            {
                var result = MessageBox.Show("Do you want to exit game?", "Leave Game", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
            }
            this.Content = null;
            base.OnBackKeyPress(e);
        }

        private void new_button_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Do you want to play new game?", "New Game", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
                this.Content = new GamePage();
        }

        private void undo_button_Click(object sender, EventArgs e)
        {
            restore_backup();
            //undo_button.IsEnabled = false;
        }

        private void make_backup()
        {
            _cur_player = cur_player;
            _game_over = game_over;

            _color_list = new List<string>();
            foreach (string color in color_list)
                _color_list.Add(color);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    _box_collections[i, j].Color = box_collections[i, j].Color;
                    _box_collections[i, j].Count = box_collections[i, j].Count;
                }
            }
        }
        
        private void restore_backup()
        {
            cur_player = _cur_player;
            game_over = _game_over;

            color_list = new List<string>();
            foreach (string color in _color_list)
                color_list.Add(color);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    box_collections[i, j].Color = _box_collections[i, j].Color;
                    box_collections[i, j].Count = _box_collections[i, j].Count;
                }
            }

            timer.Start();
            update_border();
        }
    }
}
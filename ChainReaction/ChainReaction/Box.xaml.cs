using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace ChainReaction
{
    public partial class Box : UserControl
    {
        public static bool _stop;
        int _angle;
        int _count;
        string _color;
        List<Box> _list;
        DispatcherTimer popper;
        DispatcherTimer timer;
        
        public Box()
        {
            InitializeComponent();
            
            popper = new DispatcherTimer();
            popper.Interval = TimeSpan.FromMilliseconds(200);
            popper.Tick += popper_Tick;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Tick += timer_Tick;
            timer.Start();

            _stop = false;
            _angle = 0;
            _count = 0;
            _color = null;
            _list = new List<Box>();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            RotateTransform rt = new RotateTransform();
            _angle = _angle + _count;
            if (_angle >= 180)
                _angle = -180;
            rt.Angle = _angle;
            grid.RenderTransform = rt;
        }

        void popper_Tick(object sender, EventArgs e)
        {
            popper.Stop();
            if (!_stop)
            {
                foreach (Box b in _list)
                {
                    b.Color = Color;
                    b.Count++;
                    b.update_content();
                }
            }
        }

        public int Count
        {
            get
            {
                return _count;
            }
            set
            {
                _count = value;
                update_content();
                if (_list.Count>0 && _count >= _list.Count)
                {
                    popper.Start();
                    pop_sound();
                    _count -= _list.Count;
                    update_content();
                }
                update_content();
            }
        }

        private void pop_sound()
        {
            Stream stream = TitleContainer.OpenStream("Sounds/Blop.wav");
            SoundEffect effect = SoundEffect.FromStream(stream);
            FrameworkDispatcher.Update();
            effect.Play();
        }

        public string Color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
                if (_color != null)
                    update_content();
            }
        }

        public Brush BorderBrush
        {
            get
            {
                return border.BorderBrush;
            }
            set
            {
                border.BorderBrush = value;
            }
        }

        public void JoinTo(Box box)
        {
            this._list.Add(box);
            box._list.Add(this);
        }

        private void update_content()
        {
            ImageBrush brush = new ImageBrush();
            brush.ImageSource = new BitmapImage(new Uri("Atoms/" + _color + "_" + _count + ".png", UriKind.Relative));
            grid.Background = brush;
        }
    }
}

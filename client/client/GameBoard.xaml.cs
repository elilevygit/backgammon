using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace client
{
    /// <summary>
    /// Interaction logic for GameBoard.xaml
    /// </summary>
    public partial class GameBoard : Window
    {
        bool isrolld = false;
        int _id;
        int[] namw = new int[] { 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 5, 0, 3, 0, 0, 0, 0 };
        int[] nam2 = new int[] { 0, 0, 0, 0, 0, 0, 5, 0, 3, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5 };
        Random dice;
        bool myturn = false;
        int _rolld1;
        int _rolld2;
        private int moved_rolld1;
        private int moved_rolld2;
        private int _rolld1Tmp;
        private int _rolld2Tmp;

        public GameBoard(int id)
        {
            _id = id;
            InitializeComponent();
            dice = new Random();
            reset();
        }

        private void AddEllipse(StackPanel sp, int num, Color c)
        {
            for (int i = 0; i < num; i++)
            {
                sp.Children.Add(new Ellipse() { Fill = new SolidColorBrush(c), Height = 50, Width = 50 });
            }
        }

        private void reset()
        {

            RemoveEllipses(p1);
            RemoveEllipses(p2);
            RemoveEllipses(p3);
            RemoveEllipses(p4);
            RemoveEllipses(p5);
            RemoveEllipses(p6);
            RemoveEllipses(p7);
            RemoveEllipses(p8);
            RemoveEllipses(p9);
            RemoveEllipses(p10);
            RemoveEllipses(p11);
            RemoveEllipses(p12);
            RemoveEllipses(p13);
            RemoveEllipses(p14);
            RemoveEllipses(p15);
            RemoveEllipses(p16);
            RemoveEllipses(p17);
            RemoveEllipses(p18);
            RemoveEllipses(p19);
            RemoveEllipses(p20);
            RemoveEllipses(p21);
            RemoveEllipses(p22);
            RemoveEllipses(p23);
            RemoveEllipses(p24);

            getPolygonsrf(Colors.YellowGreen, namw);
            getPolygonsrf(Colors.Chocolate, nam2);
        }

        private void RemoveEllipses(StackPanel sp)
        {
            sp.Children.Clear();

        }


        private void RemoveEllipse(StackPanel sp)
        {

            sp.Children.RemoveAt(0);
        }
        private void getPolygonsrf(Color c, int[] Positions)
        {
            if (Positions.Length != 25)
            {
                return;
            }

            AddEllipse(p1, Positions[1], c);
            AddEllipse(p2, Positions[2], c);
            AddEllipse(p3, Positions[3], c);
            AddEllipse(p4, Positions[4], c);
            AddEllipse(p5, Positions[5], c);
            AddEllipse(p6, Positions[6], c);
            AddEllipse(p7, Positions[7], c);
            AddEllipse(p8, Positions[8], c);
            AddEllipse(p9, Positions[9], c);
            AddEllipse(p10, Positions[10], c);
            AddEllipse(p11, Positions[11], c);
            AddEllipse(p12, Positions[12], c);
            AddEllipse(p13, Positions[13], c);
            AddEllipse(p14, Positions[14], c);
            AddEllipse(p15, Positions[15], c);
            AddEllipse(p16, Positions[16], c);
            AddEllipse(p17, Positions[17], c);
            AddEllipse(p18, Positions[18], c);
            AddEllipse(p19, Positions[19], c);
            AddEllipse(p20, Positions[20], c);
            AddEllipse(p21, Positions[21], c);
            AddEllipse(p22, Positions[22], c);
            AddEllipse(p23, Positions[23], c);
            AddEllipse(p24, Positions[24], c);

        }





        internal void resume_game(int[] p)
        {
            if (((MainWindow)System.Windows.Application.Current.MainWindow).i_game_host)
            {
                nam2 = p;
                for (int i = 0; i < namw.Length; i++)
                {
                    if (namw[i] > 0 && nam2[i] > 0)
                    {
                        namw[i]--;

                        if (namw[i] > 0 && nam2[i] > 0)
                        {
                            MessageBox.Show("logic problem");
                        }
                    }
                }

            }
            else
            {
                namw = p;
                for (int i = 0; i < namw.Length; i++)
                {
                    if (namw[i] > 0 && nam2[i] > 0)
                    {
                        nam2[i]--;

                        if (namw[i] > 0 && nam2[i] > 0)
                        {
                            MessageBox.Show("logic problem");
                        }
                    }
                }


            }



            reset();


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (((MainWindow)System.Windows.Application.Current.MainWindow).i_game_host)
            {
                move.IsEnabled = true;
                finish.IsEnabled = true;
                roll_dice.IsEnabled = true;
            }
            reset();
        }

        private void move_Click(object sender, RoutedEventArgs e)
        {
            _rolld1Tmp = _rolld1;
            _rolld2Tmp = _rolld2;

            if (rolld1.Text == "null")
            {
                return;
            }

            if ((moved_rolld1 == 1 && moved_rolld2 == 1 && _rolld1 != _rolld2) || (moved_rolld1 == 2 && moved_rolld2 == 2 && _rolld1 == _rolld2))
            {
                MessageBox.Show("נגמר התור לחץ סיים תור");
                return;
            }




            int fromx = 0;
            int toy = 0;

            if (!int.TryParse(from_x.Text, out fromx) || !int.TryParse(to_y.Text, out toy))
            {
                MessageBox.Show("רק מספרים שלמים");
                return;
            }

            if (((MainWindow)System.Windows.Application.Current.MainWindow).i_game_host)
            {

                int moved = fromx - toy;


                if (moved < 0)
                {
                    moved = toy - fromx;
                }

                if (toy < 12 && fromx < 12 && toy < fromx)
                {
                    MessageBox.Show("צעד בכיון הלא נכון");
                    return;
                }
                else if (toy > 12 && fromx > 12 && toy > fromx)
                {
                    MessageBox.Show("צעד בכיון הלא נכון");
                    return;
                }
                else if (toy < 12 && fromx > 12)
                {
                    MessageBox.Show("צעד בכיון הלא נכון");
                    return;
                }

                if (fromx <= 12 && (fromx + _rolld1) > 12 && (fromx + _rolld2) > 12)
                {
                    _rolld1Tmp += 13 - _rolld1*2;  
                    _rolld2Tmp += 13 - _rolld2*2;
                }
                else if (fromx <= 12 && (fromx + _rolld1) > 12 && (fromx + _rolld2) <= 12)
                {
                    _rolld1Tmp += 13 - _rolld1 * 2;
                }
                else if (fromx <= 12 && (fromx + _rolld1) <= 12 && (fromx + _rolld2) > 12)
                {
                    _rolld2Tmp += 13 - _rolld1 * 2; 
                }
               

                if (moved == 0)
                {
                    MessageBox.Show("צעד == 0 ");
                    return;
                }

              







                if (moved_rolld1 == 1 && _rolld1Tmp != _rolld2Tmp && moved == _rolld1Tmp)
                {
                    MessageBox.Show("  צעד אינו מתאים לקוביה הנותרת ");
                    return;

                }

                if (moved_rolld2 == 1 && _rolld1Tmp != _rolld2Tmp && moved == _rolld2Tmp)
                {
                    MessageBox.Show("  צעד אינו מתאים לקוביה הנותרת ");
                    return;

                }


                if (moved != _rolld1Tmp && moved != _rolld2Tmp && !(fromx>12&&fromx<19&&toy==25 && IsAllInHome()))
                {
                    MessageBox.Show(" צעד אינו מתאים לקוביות");
                    return;
                }

                if (namw[fromx] < 1)
                {
                    MessageBox.Show("לא קיים כלי המתאים במקום המצוין");
                    return;
                }

                if (nam2[toy] >= 1 && namw[toy] >= 1)
                {
                    MessageBox.Show(" logic problem cant be in same tail");
                    return;
                }

                if (nam2[toy] == 1)
                {
                    nam2[toy]--;
                }
                else if (nam2[toy] >= 2)
                {
                    MessageBox.Show("צעד לא חוקי כבר יש שם כלי");
                    return;
                }

                namw[fromx]--;
                namw[toy]++;

                ((MainWindow)System.Windows.Application.Current.MainWindow).movePositions(namw, _id);

                if (moved == _rolld1Tmp && moved == _rolld2Tmp)
                {
                    if (moved_rolld1 == 2)
                    {
                        moved_rolld2++;
                    }
                    else
                    {
                        moved_rolld1++;
                    }

                }
                else if (moved == _rolld1Tmp)
                {
                    moved_rolld1++;
                }
                else
                {
                    moved_rolld2++;
                }


            }
            else
            {
                int moved = fromx - toy;
                if (moved < 0)
                {
                    moved = toy - fromx;
                }

                if (toy > 12 && fromx > 12 && toy > fromx)
                {
                    MessageBox.Show("צעד בכיון הלא נכון");
                    return;
                }
                else if (toy < 12 && fromx < 12 && toy < fromx)
                {
                    MessageBox.Show("צעד בכיון הלא נכון");
                    return;
                }
                else if (toy > 12 && fromx < 12)
                {
                    MessageBox.Show("צעד בכיון הלא נכון");
                    return;
                }

                if (fromx > 12 && (fromx - _rolld1) <= 12 && (fromx - _rolld2) <= 12)
                {
                    _rolld1Tmp += 13 - _rolld1 * 2;
                    _rolld2Tmp += 13 - _rolld2 * 2;
                }
                else if (fromx > 12 && (fromx - _rolld1) < 12 && (fromx - _rolld2) >= 12)
                {
                    _rolld1Tmp += 13 - _rolld1 * 2;
                }
                else if (fromx > 12 && (fromx - _rolld1) >= 12 && (fromx - _rolld2) <= 12)
                {
                    _rolld2Tmp += 13 - _rolld2 * 2;
                }

                if (moved == 0)
                {
                    MessageBox.Show("צעד == 0 ");
                    return;
                }

                if (moved_rolld1 == 1 && _rolld1Tmp != _rolld2Tmp && moved == _rolld1Tmp)
                {
                    MessageBox.Show("  צעד אינו מתאים לקוביה הנותרת ");
                    return;
                }

                if (moved_rolld2 == 1 && _rolld1Tmp != _rolld2Tmp && moved == _rolld2Tmp)
                {
                    MessageBox.Show("  צעד אינו מתאים לקוביה הנותרת ");
                    return;

                }

                if (moved != _rolld1Tmp && moved != _rolld2Tmp && !(fromx > 12 && fromx < 19 && toy == 25 && IsAllInHome()))
                {

                    MessageBox.Show(" צעד אינו מתאים לקוביות");
                    return;
                }

                if (nam2[fromx] < 1)
                {
                    MessageBox.Show("לא קיים כלי המתאים במקום המצוין");
                    return;
                }

                if (namw[toy] >= 1 && nam2[toy] >= 1)
                {
                    MessageBox.Show(" logic problem ");
                    return;
                }

                if (namw[toy] == 1)
                {
                    namw[toy]--;
                }
                else if (namw[toy] >= 2)
                {
                    MessageBox.Show("צעד לא חוקי כבר יש שם כלי");
                    return;
                }

                nam2[fromx]--;
                nam2[toy]++;


                ((MainWindow)System.Windows.Application.Current.MainWindow).movePositions(nam2, _id);
                if (moved == _rolld1Tmp && moved == _rolld2Tmp)
                {
                    if (moved_rolld1 == 2)
                    {
                        moved_rolld2++;
                    }
                    else
                    {
                        moved_rolld1++;
                    }

                }
                else if (moved == _rolld1Tmp)
                {
                    moved_rolld1++;
                }
                else
                {
                    moved_rolld2++;
                }

            }

            reset();

        }

        private bool IsAllInHome()
        {
            if (((MainWindow)System.Windows.Application.Current.MainWindow).i_game_host)
            {
                for (int i = 0; i <19 ; i++)
                {
                    if (namw[i] != 0)
                    {
                        return false;
                    }
                }
            }
            else
            {
                for (int i = 0; i < 7; i++)
                {
                    if (nam2[i] != 0)
                    {
                        return false;
                    }
                }

                for (int i = 13; i <= 24; i++)
                {
                    if (nam2[i] != 0)
                    {
                        return false;
                    }
                }

            }

            return true;
        }

        private void finish_Click(object sender, RoutedEventArgs e)
        {
            myturn = false;
            ((MainWindow)System.Windows.Application.Current.MainWindow).finishTurn(_id);
            move.IsEnabled = false;
            finish.IsEnabled = false;
            roll_dice.IsEnabled = false;
            start.IsEnabled = false;
            moved_rolld1 = 0;
            moved_rolld2 = 0;

            _rolld1 = 0;
            _rolld2 = 0;

            rolld1.Text = "null";
            rolld2.Text = "null";

        }
        void App_Exit(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            this.Visibility = Visibility.Hidden;

        }
        private void roll_dice_Click(object sender, RoutedEventArgs e)
        {
            if (!isrolld)
            {
                _rolld1 = dice.Next(1, 6);
                _rolld2 = dice.Next(1, 6);
                rolld1.Text = _rolld1.ToString();
                rolld2.Text = _rolld2.ToString();
                isrolld = true;
            }

        }

        internal void BackToMyTurn()
        {
            myturn = true;
            move.IsEnabled = true;
            finish.IsEnabled = true;
            roll_dice.IsEnabled = true;
            start.IsEnabled = false;
            isrolld = false;
        }
    }
}

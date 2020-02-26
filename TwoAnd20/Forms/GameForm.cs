using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TwoAnd20.Forms
{
    public partial class GameForm : Form
    {
        public GameForm()
        {
            InitializeComponent();
        }

        List<ClickHash> clickList = new List<ClickHash>();

        public ClickHash ClickDataBase(Button button, Image buttonBgImage, Button[] buttonFor)
        {
            var newClick = new ClickHash
            {
                buttonBgImage = buttonBgImage,
                button = button,
                buttonsFor = buttonFor
            };
            clickList.Add(newClick);
            return newClick;
        }
        public Button[] NeighborsButton(Button button)
        {
            int who;
            int.TryParse(string.Join("", button.Name.Where(p => char.IsDigit(p))), out who);
            List<Button[]> buttonsForList = new List<Button[]>();

            #region Массивы
            Button[] buttonsFor1 = { null, null, null, null, button2, button3, button4, button10 };
            Button[] buttonsFor2 = { button1, null, null, null, button3, null, button5, button11 };
            Button[] buttonsFor3 = { button2, button1, null, null, null, null, button6, button24 };

            Button[] buttonsFor4 = { null, null, button1, null, button5, button6, button10, button9 };
            Button[] buttonsFor5 = { button4, null, button2, null, button6, null, button11, button13 };
            Button[] buttonsFor6 = { button5, button4, button3, null, null, null, button24, button12 };

            Button[] buttonsFor16 = { null, null, null, null, button7, button10, button17, button19 };
            Button[] buttonsFor7 = { button16, null, null, null, button10, button11, button31, button20 };
            Button[] buttonsFor10 = { button7, button16, button4, button1, button11, button14, button9, button21 };
            Button[] buttonsFor11 = { button10, button7, button5, button2, button24, button8, button13, button22 };
            Button[] buttonsFor24 = { button11, button10, button6, button3, button8, button18, button12, button23 };
            Button[] buttonsFor8 = { button24, button11, null, null, button18, null, button14, button25 };
            Button[] buttonsFor18 = { button8, button24, null, null, null, null, button15, button26 };

            Button[] buttonsFor17 = { null,null, button16, null, button31, button9, button19, null };
            Button[] buttonsFor31 = { button17, null, button7, null, button9, button13, button20, null };
            Button[] buttonsFor9 = { button31, button17, button10, button4, button13, button12, button21, button27 };
            Button[] buttonsFor13 = { button9, button31, button11, button5, button12, button14, button22, button28 };
            Button[] buttonsFor12 = { button13, button9, button24, button6, button14, button15, button23, button29 };
            Button[] buttonsFor14 = { button12, button13, button8, null, button15, null, button25, null };
            Button[] buttonsFor15 = { button14, button12, button18, null, null, null, button26, null };

            Button[] buttonsFor19 = { null, null, button17, button16, button20, button21, null, null };
            Button[] buttonsFor20 = { button19, null, button31, button7, button21, button22, null, null };
            Button[] buttonsFor21 = { button20, button19, button9, button10, button22, button23, button27, null };
            Button[] buttonsFor22 = { button21, button20, button13, button11, button23, button25, button28, button30 };
            Button[] buttonsFor23 = { button22, button21, button12, button24, button25, button26, button29, null };
            Button[] buttonsFor25 = { button23, button22, button14, button8, button26, null, null, null };
            Button[] buttonsFor26 = { button25, button23, button15, button18, null, null, null, null };

            Button[] buttonsFor27 = { null, null, button21, button9, button28, button29, null, null };
            Button[] buttonsFor28 = { button27, null, button22, button13, button29, null, button30, null };
            Button[] buttonsFor29 = { button28, button27, button23, button12, null, null, null, null };

            Button[] buttonsFor30 = { null, null, button28, button22, null, null, null, null };

            buttonsForList.Add(buttonsFor1);
            buttonsForList.Add(buttonsFor2);
            buttonsForList.Add(buttonsFor3);
            buttonsForList.Add(buttonsFor4);
            buttonsForList.Add(buttonsFor5);
            buttonsForList.Add(buttonsFor6);
            buttonsForList.Add(buttonsFor7);
            buttonsForList.Add(buttonsFor8);
            buttonsForList.Add(buttonsFor9);
            buttonsForList.Add(buttonsFor10);
            buttonsForList.Add(buttonsFor11);
            buttonsForList.Add(buttonsFor12);
            buttonsForList.Add(buttonsFor13);
            buttonsForList.Add(buttonsFor14);
            buttonsForList.Add(buttonsFor15);
            buttonsForList.Add(buttonsFor16);
            buttonsForList.Add(buttonsFor17);
            buttonsForList.Add(buttonsFor18);
            buttonsForList.Add(buttonsFor19);
            buttonsForList.Add(buttonsFor20);
            buttonsForList.Add(buttonsFor21);
            buttonsForList.Add(buttonsFor22);
            buttonsForList.Add(buttonsFor23);
            buttonsForList.Add(buttonsFor24);
            buttonsForList.Add(buttonsFor25);
            buttonsForList.Add(buttonsFor26);
            buttonsForList.Add(buttonsFor27);
            buttonsForList.Add(buttonsFor28);
            buttonsForList.Add(buttonsFor29);
            buttonsForList.Add(buttonsFor30);
            buttonsForList.Add(buttonsFor31);
            #endregion

            // 0 - 1-ый влево, 1 - 2-ой влево
            // 2 - 1-ый вверх, 3 - 2-ой вверх
            // 4 - 1-ый вправо, 5 - 2-ой вправо
            // 6 - 1-ый ввниз, 7 - 2-ой ввниз

            return buttonsForList[Convert.ToInt32(who) - 1];
        }

        public void Click(Button button)
        {
            var neighbors = NeighborsButton(button);
            var checkBackgroundImage = button.BackgroundImage;

            if (clickList.Count != 0)
            {
                var oneClick = clickList.First();
                var firstClick = oneClick.buttonsFor;
                if (oneClick.button == button)
                    return;
                var checkClickAccess = firstClick.FirstOrDefault(p => p == button);
                var index = Array.IndexOf(firstClick, checkClickAccess);

                if(index == 1 || index == 3 || index == 5 || index == 7)
                {
                    Eating(button, index, firstClick, oneClick);
                }

                if (checkClickAccess != null && checkBackgroundImage == null)
                {
                    button.BackgroundImage = oneClick.buttonBgImage;
                    button.BackgroundImageLayout = ImageLayout.Stretch;

                    oneClick.button.BackgroundImage = null;
                    clickList.Remove(oneClick);
                }
            }
            else
            {
                if (button.BackgroundImage != null)
                    ClickDataBase(button, button.BackgroundImage, neighbors);
                else
                    ClickDataBase(button, null, null);
            }
        }

        public void Eating(Button button, int index, Button[] buttons, ClickHash clickList)
        {
            if(clickList.buttonBgImage != TwoAnd20.Properties.Resources.chicken)
            {

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Click(sender as Button);
        }

        private void GameForm_Load(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Click(sender as Button);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Click(sender as Button);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Click(sender as Button);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Click(sender as Button);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Click(sender as Button);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            Click(sender as Button);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Click(sender as Button);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Click(sender as Button);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Click(sender as Button);
        }

        private void button24_Click(object sender, EventArgs e)
        {
            Click(sender as Button);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Click(sender as Button);

        }

        private void button18_Click(object sender, EventArgs e)
        {
            Click(sender as Button);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            Click(sender as Button);
        }

        private void button32_Click(object sender, EventArgs e)
        {
            Click(sender as Button);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Click(sender as Button);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            Click(sender as Button);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            Click(sender as Button);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            Click(sender as Button);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            Click(sender as Button);
        }

        private void button19_Click(object sender, EventArgs e)
        {
            Click(sender as Button);
        }

        private void button20_Click(object sender, EventArgs e)
        {
            Click(sender as Button);
        }

        private void button21_Click(object sender, EventArgs e)
        {
            Click(sender as Button);
        }

        private void button22_Click(object sender, EventArgs e)
        {
            Click(sender as Button);
        }

        private void button23_Click(object sender, EventArgs e)
        {
            Click(sender as Button);
        }

        private void button25_Click(object sender, EventArgs e)
        {
            Click(sender as Button);
        }

        private void button26_Click(object sender, EventArgs e)
        {
            Click(sender as Button);
        }

        private void button27_Click(object sender, EventArgs e)
        {
            Click(sender as Button);
        }

        private void button28_Click(object sender, EventArgs e)
        {
            Click(sender as Button);
        }

        private void button29_Click(object sender, EventArgs e)
        {
            Click(sender as Button);
        }

        private void button30_Click(object sender, EventArgs e)
        {
            Click(sender as Button);
        }
    }
    public class ClickHash
    {
        public Button button { get; set; }
        public Image buttonBgImage { get; set; }
        public Button[] buttonsFor { get; set; }
    }
}

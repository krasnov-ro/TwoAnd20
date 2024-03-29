﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using Newtonsoft.Json;

namespace TwoAnd20.Forms
{
    public partial class GameForm : Form
    {
        List<ClickHash> ClickList = new List<ClickHash>();
        Button LastStepButton = null;
        int ChickenCount = 20;

        public GameForm()
        {
            InitializeComponent();
            label2.Text = ChickenCount.ToString();
        }

        public ClickHash ClickDataBase(Button button, Image buttonBgImage, Button[] buttonFor)
        {
            // Format24bppRgb - курица
            // Format32bppArgb - лиса
            var checkedButtons = new List<int>();

            // вырубаем все кнопки на форме которые не входят в диапазон хода за текущую кнопку
            foreach (var control in this.Controls)
            {
                if ((control is Button) && !buttonFor.Any(p => p == control))
                {
                    (control as Button).Enabled = false;
                }
            }

            #region Обязательства, кушать, для лис

            // Проходимся по всем клеткам, которые в зоне досегаемости текущей клетки
            for (int i = 0; i < buttonFor.Count(); i++)
            {
                if (buttonFor[i] != null)
                {
                    // если клетка курица и облегает нашу клетку, то запишем его id в список
                    if (buttonFor[i]?.BackgroundImage?.PixelFormat == PixelFormat.Format24bppRgb
                        && (i == 0 || i == 2 || i == 4 || i == 6))
                    {
                        checkedButtons.Add(i);
                    }
                    // если клетка не курица, а просто облегает нашу клетку то делаем эту клетку активной
                    else if (i == 0 || i == 2 || i == 4 || i == 6)
                    {
                        buttonFor[i].Enabled = true;
                    }
                    // ну а если ни то ни другое то сделаем клетку не активной
                    else
                    {
                        buttonFor[i].Enabled = false;
                    }
                }
            }

            // если клетку которую мы выбрали лиса и у него есть возможность съесть курицу
            // то заставляем ее это сделать
            if (buttonBgImage.PixelFormat == PixelFormat.Format32bppArgb)
            {
                if (checkedButtons != null)
                {
                    var indexesEnableTrue = new List<Button>(); // список кнопок которые нужно активировать
                                                                // проходимся по клетка-курицам которые облегают нашу клетку с лисой чтобы проверить можно 
                                                                // ли съесть эту курицу
                    foreach (var index in checkedButtons)
                    {
                        if (buttonFor[index] != null)
                        {
                            buttonFor[index].Enabled = false;

                            // если клетка за курицей, которая облегает нашу лису, свободна, то делаем эту 
                            // свободную клетку активной, и записываем его в список клеток которые должны  
                            // быть активны, (понадобится ниже)
                            if (buttonFor[index + 1] != null && buttonFor[index + 1].BackgroundImage == null)
                            {
                                buttonFor[index + 1].Enabled = true;
                                indexesEnableTrue.Add(buttonFor[index + 1]);
                            }
                        }
                    }

                    // если список клеток которые надо активировать больше 0
                    // то мы проходимся по всем клеткам на нашей доске 
                    // и если эти клетки не входят в список наших клеток которые
                    // стоит активировать, то сделаем их неактивными
                    if (indexesEnableTrue.Count() > 0)
                    {
                        foreach (var control in this.Controls)
                        {
                            if ((control is Button) && !indexesEnableTrue.Any(p => p == control))
                            {
                                (control as Button).Enabled = false;
                            }
                        }
                    }

                }
            }
            #endregion 

            var newClick = new ClickHash
            {
                buttonBgImage = buttonBgImage,
                button = button,
                buttonsFor = buttonFor
            };
            ClickList.Add(newClick);
            return newClick;
        }

        /// <summary>
        /// Метод для получения доступных клеток для совершения хода
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public Button[] NeighborsButton(Button button)
        {
            // достаем номер клетки
            int who;
            int.TryParse(string.Join("", button.Name.Where(p => char.IsDigit(p))), out who);

            // список потенциально доступных клеток для кнопки которая нажата на данный момент
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
            Button[] buttonsFor10 = { button7, button16, button4, button1, button11, button24, button9, button21 };
            Button[] buttonsFor11 = { button10, button7, button5, button2, button24, button8, button13, button22 };
            Button[] buttonsFor24 = { button11, button10, button6, button3, button8, button18, button12, button23 };
            Button[] buttonsFor8 = { button24, button11, null, null, button18, null, button14, button25 };
            Button[] buttonsFor18 = { button8, button24, null, null, null, null, button15, button26 };

            Button[] buttonsFor17 = { null, null, button16, null, button31, button9, button19, null };
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

        /// <summary>
        /// Обработка нажатия на клетку
        /// </summary>
        /// <param name="button"></param>
        public void Click(Button button)
        {
            var neighbors = NeighborsButton(button);
            var checkBackgroundImage = button.BackgroundImage;

            if (ClickList.Count != 0)
            {
                var oneClick = ClickList.First(); // Информация о предыдущем нажатии на кнопку 
                var firstClick = oneClick.buttonsFor; // Массив доступных клеток для хода с предыдущей клетки
                var checkClickAccess = firstClick?.FirstOrDefault(p => p == button); // Проверяем есть ли доступ к текущей клетке с предыдущей клетки

                // если пред. нажатая кнопка и текущая кнопка это одно и то же или 
                // массив потенциальных клеток для хода пуст или
                // текущая кнопка не входит в список клеток для хода
                // то очищаем информацию о пред. нажатой кнопке
                if (oneClick.button == button || firstClick == null || checkClickAccess == null)
                {
                    ClickList.Remove(oneClick);
                    return;
                }

                var index = Array.IndexOf(firstClick, checkClickAccess);

                if (index == 1 || index == 3 || index == 5 || index == 7)
                {
                    if (oneClick.buttonBgImage.PixelFormat != PixelFormat.Format24bppRgb &&
                                    button.BackgroundImage == null &&
                                    firstClick[index - 1].BackgroundImage != null)
                    {
                        Eating(button, index, firstClick, oneClick);
                    }
                    else
                    {
                        ClickList.Remove(oneClick);
                        return;
                    }
                }

                if (checkBackgroundImage == null)
                {
                    button.BackgroundImage = oneClick.buttonBgImage;
                    button.BackgroundImageLayout = ImageLayout.Stretch;
                    LastStepButton = button;
                    oneClick.button.BackgroundImage = null;
                    EnableAndDisable(LastStepButton);
                    ClickList.Remove(oneClick);
                    if (button1.BackgroundImage?.PixelFormat == PixelFormat.Format24bppRgb &&
                        button2.BackgroundImage?.PixelFormat == PixelFormat.Format24bppRgb &&
                        button3.BackgroundImage?.PixelFormat == PixelFormat.Format24bppRgb &&
                        button4.BackgroundImage?.PixelFormat == PixelFormat.Format24bppRgb &&
                        button5.BackgroundImage?.PixelFormat == PixelFormat.Format24bppRgb &&
                        button6.BackgroundImage?.PixelFormat == PixelFormat.Format24bppRgb &&
                        button10.BackgroundImage?.PixelFormat == PixelFormat.Format24bppRgb &&
                        button11.BackgroundImage?.PixelFormat == PixelFormat.Format24bppRgb &&
                        button24.BackgroundImage?.PixelFormat == PixelFormat.Format24bppRgb)
                    {
                        MessageBox.Show("Курицы выиграли!", "Chickens Win", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    else if (ChickenCount < 9)
                    {
                        MessageBox.Show("Лисы выиграли!", "Foxes Win", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
            }
            else
            {
                if (button.BackgroundImage != null)
                    ClickDataBase(button, button.BackgroundImage, neighbors);
            }
        }

        /// <summary>
        /// Метод поедания
        /// </summary>
        /// <param name="button"></param>
        /// <param name="index"></param>
        /// <param name="buttons"></param>
        /// <param name="clickList"></param>
        public void Eating(Button button, int index, Button[] buttons, ClickHash clickList)
        {
            buttons[index - 1].BackgroundImage = null;
            label2.Text = (ChickenCount - 1).ToString();
            ChickenCount--;
        }

        /// <summary>
        /// Метод для соблюдения очереди ходов
        /// </summary>
        /// <param name="lastStepButton"></param>
        public void EnableAndDisable(Button lastStepButton)
        {
            var type = lastStepButton.BackgroundImage?.PixelFormat;

            List<Button> buttons = new List<Button>();
            foreach (var button in this.Controls)
            {
                if (button is Button)
                {
                    buttons.Add(button as Button);
                }
            }
            foreach (var button in buttons)
            {
                if (button.BackgroundImage?.PixelFormat == type && type != null)
                {
                    button.Enabled = false;
                }
                else
                {
                    button.Enabled = true;
                }
            }

            /*todo Сделать бота который бы смотрел на тип существа, который совершил этот ход, и в зависимости от того за кого он играет, делал бы ход*/
        }

        /// <summary>
        /// Реализация кнопки рестарта
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void restartButton_Click(object sender, EventArgs e)
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), @"TwoAnd20/Configs");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                List<SettingMenu> _settingMenu = new List<SettingMenu>();
                _settingMenu.Add(new SettingMenu()
                {
                    chikenCount = 20,
                    foxCount = 2,
                    chickenPositions = "17,31,9,13,12,14,15,19,20,21,22,23,25,26,27,28,29,30",
                    foxPositions = "10,24"
                });

                string json = JsonConvert.SerializeObject(_settingMenu);

                //write string to file
                System.IO.File.WriteAllText(Path.Combine(path, @"settings.json"), json);
            }

            string gameSettings = File.ReadAllText(Path.Combine(path, @"settings.json"));

            SettingMenu gameSettingsNormal = Newtonsoft.Json.JsonConvert.DeserializeObject<SettingMenu>(gameSettings.Replace("[", string.Empty).Replace("]",string.Empty));

            ChickenCount = gameSettingsNormal.chikenCount;
            label2.Text = ChickenCount.ToString();

            var foxPositions = GetIntArray(gameSettingsNormal.foxPositions, ',');
            var chickenPositions = GetIntArray(gameSettingsNormal.chickenPositions, ',');

            foreach (var control in Controls)
            {
                var button = (control as Button);
                if (button != null && button.Text == "")
                {
                    var buttonId = 0;
                    int.TryParse(string.Join("", button.Name.Where(p => char.IsDigit(p))), out buttonId);
                    if (foxPositions.FirstOrDefault(p => p == buttonId) != 0)
                    {
                        button.BackgroundImage = Properties.Resources.lis;
                    }
                    else if (chickenPositions.FirstOrDefault(p => p == buttonId) != 0)
                    {
                        button.BackgroundImage = Properties.Resources.chicken;
                    }
                    else
                        button.BackgroundImage = null;
                }
            }
            //gameSettingsNormal.chickenPositions

            //ChickenCount = Convert.ToInt32(ConfigurationManager.AppSettings.Get("chikenCount"));
            //string chickenPositions = ConfigurationManager.AppSettings.Get("chikenButtons");
            //string foxPositions = ConfigurationManager.AppSettings.Get("foxButtons");


            //jsonObj["chikenCount"] = "30";
            //string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
            //File.WriteAllText("settings.json", output);
        }

        /// <summary>
        /// Метод получения массива чисел из строки
        /// </summary>
        /// <param name="str"></param>
        /// <param name="delimetr"></param>
        /// <returns></returns>
        public int[] GetIntArray(string str, char delimetr)
        {
            var delimetrCount = str.Where(p => p == delimetr).Count();
            int[] result = new int[delimetrCount + 1];
            string cutedStr = null;
            var indexOf = str.IndexOf(delimetr);
            for (int i = 0; i <= delimetrCount; i++)
            {
                if (i == 0)
                {
                    result[i] = Convert.ToInt32(str.Substring(i, indexOf));
                }
                else
                {
                    if (cutedStr != null)
                        cutedStr = cutedStr.Substring(indexOf + 1, cutedStr.Length - (indexOf + 1));
                    else
                        cutedStr = str.Substring(indexOf + 1, str.Length - (indexOf + 1));
                    indexOf = cutedStr.IndexOf(delimetr);
                    if (i < delimetrCount)
                        result[i] = Convert.ToInt32(cutedStr.Substring(0, cutedStr.IndexOf(delimetr)));
                    else
                        result[i] = Convert.ToInt32(cutedStr.Substring(0, cutedStr.Length));
                }
            }
            return result;
        }

        #region Методы нажатия на кнопки и загрузки формы
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

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void goMenuButton_Click(object sender, EventArgs e)
        {
            Form menu = new Form();
            menu = new GameMenuForm();
            this.Hide();
            menu.ShowDialog();
        }
        #endregion
    }

    #region Структуры для структурированного хранения некоторых данных
    public struct SettingMenu
    {
        public int chikenCount { get; set; }
        public int foxCount { get; set; }
        public string chickenPositions { get; set; }
        public string foxPositions { get; set; }
    }

    public struct ClickHash
    {
        public Button button { get; set; }
        public Image buttonBgImage { get; set; }
        public Button[] buttonsFor { get; set; }
    }
    #endregion
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TwoAnd20.Forms;

namespace TwoAnd20
{
    public partial class GameMenuForm : Form
    {
        public GameMenuForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GameForm fm = new GameForm();
            ActiveForm.Hide();
            fm.ShowDialog();
        }
        //todo Надо реализовать кнопку настроек
    }
}

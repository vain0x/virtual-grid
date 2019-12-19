using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VirtualGrid.WinFormsDemo
{
    public partial class MainMenuForm : Form
    {
        public MainMenuForm()
        {
            InitializeComponent();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            var form = new Examples.TodoListForm();
            form.FormClosed += (_sender, _ev) => Close();
            form.Show();
            form.Activate();
        }

        private void _tinyCounterExampleButton_Click(object sender, EventArgs e)
        {
            new Examples.TinyCounterForm().ShowDialog();
        }

        private void _todoListExampleButton_Click(object sender, EventArgs e)
        {
            new Examples.TodoListForm().ShowDialog();
        }
    }
}

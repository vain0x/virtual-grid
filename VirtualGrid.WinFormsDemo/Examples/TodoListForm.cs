using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VirtualGrid.Rendering;
using VirtualGrid.WinFormsDemo.Provider.Headers;

namespace VirtualGrid.WinFormsDemo.Examples
{
    public partial class TodoListForm : Form
    {
        private readonly DataGridView _dataGridView = new DataGridView
        {
            Dock = DockStyle.Fill
        };

        private readonly TodoListModel _model = new TodoListModel();

        private readonly TodoListView _view;

        public TodoListForm()
        {
            InitializeComponent();

            _dataGridView.AllowUserToAddRows = false;
            Controls.Add(_dataGridView);

            var gridProvider = new DataGridViewGridProvider(_dataGridView, (elementKey, action) =>
            {
                action();
                BeginInvoke(new Action(_view.Update));
            });

            _view = new TodoListView(_model, gridProvider);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            _view.Initialize();
        }
    }
}

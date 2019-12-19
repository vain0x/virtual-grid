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

namespace VirtualGrid.WinFormsDemo.Examples
{
    public partial class TodoListForm : Form
    {
        private readonly DataGridView _dataGridView = new DataGridView
        {
            Dock = DockStyle.Fill
        };

        private readonly DataGridViewGridProvider _gridProvider;

        private readonly TodoListModel _model = new TodoListModel();

        public TodoListForm()
        {
            InitializeComponent();

            _dataGridView.AllowUserToAddRows = false;
            Controls.Add(_dataGridView);

            _gridProvider = new DataGridViewGridProvider(_dataGridView, Dispatch);
        }

        private void Dispatch(object _elementKey, Action action)
        {
            action();
            BeginInvoke(new Action(Render));
        }

        private void Render()
        {
            var h = _gridProvider.GetBuilder();
            new TodoListView(_model, h).Render();
            _gridProvider.Render(h);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Render();
        }
    }
}

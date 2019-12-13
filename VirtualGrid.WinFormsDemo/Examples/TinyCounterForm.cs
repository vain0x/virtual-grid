using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VirtualGrid.Layouts;
using VirtualGrid.Models;

namespace VirtualGrid.WinFormsDemo.Examples
{
    public partial class TinyCounterForm : Form
    {
        private readonly DataGridView _dataGridView = new DataGridView
        {
            Dock = DockStyle.Fill
        };

        private readonly DataGridViewGridProvider _gridProvider;

        private readonly TinyCounterModel _model = new TinyCounterModel();

        public TinyCounterForm()
        {
            InitializeComponent();

            _dataGridView.AllowUserToAddRows = false;
            Controls.Add(_dataGridView);

            _gridProvider = new DataGridViewGridProvider(_dataGridView, Dispatch);
        }

        private void Dispatch(object elementKey, object action)
        {
            _model.Update(action as string);
            Render(_model);
        }

        private void Render(TinyCounterModel state)
        {
            var h = new VGridBuilder();
            var body = new TinyCounterView(state, h).Render();
            var grid = h.Finish().WithBody(body);
            _gridProvider.Render(grid);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Render(_model);
        }
    }
}

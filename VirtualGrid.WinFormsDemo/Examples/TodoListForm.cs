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
using VirtualGrid.Layouts.BucketGrids;
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

        private readonly BucketGridLayout _gridLayout =
            new BucketGridLayout();

        private readonly DataGridViewGridProvider _gridProvider;

        private readonly TodoListModel _model = new TodoListModel();

        public TodoListForm()
        {
            InitializeComponent();

            _dataGridView.AllowUserToAddRows = false;
            Controls.Add(_dataGridView);

            _gridProvider = new DataGridViewGridProvider(_dataGridView, _gridLayout, Dispatch);
        }

        private void Dispatch(GridElementKey _elementKey, Action action)
        {
            action();
            BeginInvoke(new Action(Render));
        }

        private void Render()
        {
            _gridProvider._renderContext.Clear();
            var h = new BucketGridBuilder<DataGridViewGridProvider>(
                _gridLayout,
                _gridProvider._renderContext,
                new RowHeaderDeltaListener(_gridProvider),
                new ColumnHeaderDeltaListener(_gridProvider)
            );

            new TodoListView(_model, h).Render();
            h.Patch();
            _gridProvider.Render();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Render();
        }
    }
}

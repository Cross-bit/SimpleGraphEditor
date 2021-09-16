﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SimpleGraphEditor.Presenters;
using SimpleGraphEditor.Views;
using System.Runtime.InteropServices;
using SimpleGraphEditor.Vendor;

namespace SimpleGraphEditor.Views
{
    public partial class EdgeProperties : System.Windows.Forms.Form, IEdgePropertiesView
    {

        public EdgeProperties() {
            InitializeComponent();
            InitializeColorPicker();
        }

        void InitializeColorPicker() {
            ColorPicker.AllowFullOpen = true;
            ColorPicker.AnyColor = true;
            ColorPicker.SolidColorOnly = false;
        }

        public GraphPresenter graphPresenter { get; set; }
        public EdgePropertiesPresenter propPresenter { get; set; }
        public Color NewEdgeColor { get; set; } = Color.Black;
        public bool NewEdgeIsDirected { get; set; } = Settings.IsEdgeDirectedDefault;
        public int NewEdgeWidth { get; set; } = Settings.DefaultEdgeWidth;

        private Color _cellsBorderColor = Color.FromArgb(50, 45, 45, 45);
        private int _cellsBorderWidth = 2; // todo asi do settings

        private void IsDirectedCheckBox_MouseClick(object sender, MouseEventArgs e) {
            NewEdgeIsDirected = IsDirectedCheckBox.Checked;
            UpdateData();
        }
        private void EdgeBackColorBtn_MouseClick(object sender, MouseEventArgs e) {
            if (ColorPicker.ShowDialog(this) == DialogResult.OK) {
                EdgeBackColorBtn.BackColor = ColorPicker.Color;
                NewEdgeColor = ColorPicker.Color;
                UpdateData();
            }
        }
        private void WidthUpDown_ClientEntery(object sender, EventArgs e) {
            NewEdgeWidth = (int) WidthUpDown.Value;
            UpdateData();
        }
        private void WidthUpDown_KeyEntry(object sender, KeyEventArgs e) {
            NewEdgeWidth = (int)WidthUpDown.Value;
            UpdateData();
        }
        private void UpdateData() {
            propPresenter.UpdatePropertiesModel();
            propPresenter.UpdateCurrentTemplate();
        }

        #region other 
        // cells border
        private void EdgeProperties_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            var rectangle = e.CellBounds;
            using (var pen = new Pen(_cellsBorderColor, _cellsBorderWidth))
            {
                pen.Alignment = System.Drawing.Drawing2D.PenAlignment.Center;
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;

                if (e.Row == (((TableLayoutPanel)sender).RowCount - 1))
                {
                    rectangle.Height -= 1;
                }

                if (e.Column == (((TableLayoutPanel)sender).ColumnCount - 1))
                {
                    rectangle.Width -= 1;
                }

                e.Graphics.DrawRectangle(pen, rectangle);
            }
        }
        #endregion
    }
}
﻿using System.Collections.Generic;

namespace System.Windows.Forms
{
    public class MultipleInterpretationAttributeGrid : AttributeGrid
    {
        private readonly ComboBox chooser;

        public MultipleInterpretationAttributeGrid() : base()
        {
            chooser = new ComboBox()
            {
                Dock = DockStyle.Fill,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            chooser.SelectedIndexChanged += chooser_SelectedIndexChanged;

            Button save = new Button()
            {
                Dock = DockStyle.Right,
                Text = "Save"
            };
            save.Click += save_Click;

            Panel p = new Panel()
            {
                Dock = DockStyle.Top,
                Size = chooser.PreferredSize
            };
            p.Controls.Add(chooser);
            p.Controls.Add(save);
            Controls.Add(p);

            foreach (Control c in new Control[] {chooser, save, p})
            {
                c.Margin = new Padding(0);
            }
        }

        private void save_Click(object sender, EventArgs e)
        {
            AttributeInterpretation item = (AttributeInterpretation) chooser.SelectedItem;
            if (item != null)
            {
                item.Save();
            }
        }

        private void chooser_SelectedIndexChanged(object sender, EventArgs e)
        {
            AttributeInterpretation item = (AttributeInterpretation) chooser.SelectedItem;
            if (item != null)
            {
                AttributeArray = item.Array;
                TargetChanged();
            }
        }

        public int Add(AttributeInterpretation arr)
        {
            int i = chooser.Items.Add(arr);
            if (AttributeArray == null)
            {
                chooser.SelectedIndex = i;
            }

            return i;
        }

        public void AddRange(IEnumerable<AttributeInterpretation> arrs)
        {
            foreach (AttributeInterpretation arr in arrs)
            {
                Add(arr);
            }
        }

        public void Remove(AttributeInterpretation arr)
        {
            chooser.Items.Remove(arr);
            if (AttributeArray == null)
            {
                AttributeArray = null;
            }
        }

        public void Clear()
        {
            chooser.Items.Clear();
            AttributeArray = null;
        }
    }
}
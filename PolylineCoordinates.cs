using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Simulation_Options
{
	public partial class PolylineCoordinates : Form
	{
		public bool ValueChanged;

		public PolylineCoordinates()
		{
			InitializeComponent();
		}

		private void PolylineCoordinates_Load(object sender, EventArgs e)
		{
			ValueChanged = false;
		}

		public bool GetValueChanged()
		{
			return ValueChanged;
		}

		public void SetData(List<string> data)
		{
			foreach (var line in data)
			{
				string[] cells= line.Split(new[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);
				dataPolylineCoords.Rows.Add(cells[0], cells[1]);
			}
			ValueChanged = false;
		}

		public List<string> UpdatePoly()
		{
			var newData = new List<string>();
			for (int i = 0; i < dataPolylineCoords.Rows.Count; i++)
			{
				newData.Add(dataPolylineCoords.Rows[i].Cells[0].Value + " " + dataPolylineCoords.Rows[i].Cells[1].Value);
			}
			return newData;
		}

		private void btnApply_Click(object sender, EventArgs e)
		{
			if (dataPolylineCoords.Rows.Count == 2)
				MessageBox.Show(Universal.Idioma("A line can't have only one point.", "Una línea no puede tener un solo punto."), 
                    "RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Error);
			else
				Close();
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void dataPolylineCoords_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			ValueChanged = true;
		}
	}
}

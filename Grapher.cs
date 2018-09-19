/****************************************************************************************************
 * GRAPHER
 * What: Window to plot data and save plotdata to file.
 * How: When creating the Object must pass a ArrayList with one ArrayList per every dataset to plot.
 *      Set Texts with setTitle and setAxisName#
 *      Set datasets names with setDataNames.
 *      Save data to file (including extra data that wants to be added)
 *      
 ****************************************************************************************************/
using System.IO;
using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;


namespace Simulation_Options
{

    public partial class Grapher : Form
    {
        //Variables to plot the data
        float _h;
				float _w;
				float _offseth;
				float _offsetw;
				float _mCantsepX;
				float _mCantsepY;
				float _mMaxValueX = 0;
				float _mMaxValueY = 0;
				float _mMinValueX = 9999999;
				float _mMinValueY = 99999999;

	    //Text shown in the grapher
        string _mSTitulo;
				string _mSAxisX;
				string _mSAxisY;
				string _mStatusX;
				string _mStatusY;

	    //To control the movement of the legend
        bool _mBLeyendaMove = false;
        bool _mBLeyendaOver = false;

        //Number of datasets
				readonly int _mINumDatos;

        //Extra Data (For the text file)
	    readonly ArrayList _mExtraData;
	    readonly ArrayList _mExtraDataName;

        //Data colour
	    readonly ArrayList _mColours;

      //Data name
      ArrayList _mSNames;

			//X axis texts
	    private ArrayList _mXAxisTexts;

			//Y axis texts
			private ArrayList _mYAxisTexts;

        //Data
	    readonly ArrayList _mData;

        //To control which data to show
	    readonly bool[] _mBShown;

       bool _mBUpdateAxisY = false;

        public Grapher(ArrayList data)
        {
            _mData = data;
            _mINumDatos = _mData.Count;
            _mBShown = new bool[_mINumDatos];
            _mColours = new ArrayList();
            _mExtraData = new ArrayList();
            _mExtraDataName = new ArrayList();

            //Initialize data
            int step = 100 / _mINumDatos;
            for (int i = 0; i < _mINumDatos; i++)
            {
                //Create colours to use
                Color c = System.Drawing.ColorTranslator.FromWin32(i * step * 1000000 + (100 - i * step) * 10000 + (22) * 100);
                _mColours.Add(c);

                //The first data to show is "All" so all are set to true
                _mBShown[i] = true;

            }

            //Get the max/min to draw the axis correctly
            int size = _mINumDatos;
            for (int i = 0; i < size; i++)
            {
                SetMinMax((ArrayList)_mData[i]);
            }

            InitializeComponent();
        }

        //Sets the name of each dataset
        public void SetDataNames(ArrayList names)
        {
            _mSNames = names;

            int size = _mSNames.Count;

            //Add data values to the combo box
						if(size > 1) cmboDataShown.Items.Add("All");
            
            for (int i = 0; i < size; ++i)
            {
                cmboDataShown.Items.Add((string)_mSNames[i]);
            }

            //Set the showing data to 0
            cmboDataShown.SelectedIndex = 0;
        }

				//Sets the texts for x axis for of each dataset
				public void SetDataXAxis(ArrayList textsForXAxis)
				{
					_mXAxisTexts = textsForXAxis;
				}

				//Sets the texts for y axis for of each dataset
				public void SetDataYAxis(ArrayList textsForYAxis)
				{
					_mYAxisTexts = textsForYAxis;
				}
        //Set the title window and plot of the data to graph
        public void SetWindowAndPlotTitles(string title)
        {
          _mSTitulo = title;
					Text = title;
        }

				//Set the window title
				public void SetWindowTitle(string title)
				{
					Text = title;
				}

				//Set the plot title
				public void SetPlotTitle(string title)
				{
					_mSTitulo = title;
				}

        //Set the name of the X-Axis
        public void SetAxisNameX(string axisX)
        {
            _mSAxisX = axisX;
        }

        //Set the name of the Y-Axis
        public void SetAxisNameY(string axisY)
        {
            _mSAxisY = axisY;
        }

				//Set the status X text:
				public void SetStatusX(string  statusX)
				{
					_mStatusX = statusX;
				}

				//Set the status Y text:
				public void SetStatusY(string statusY)
				{
					_mStatusY = statusY;
				}

        //Set if Y-Axis name should change when dataset is changed
        public void SetYAxisChangeOnDataChange(bool updateAxisY)
        {
            _mBUpdateAxisY = updateAxisY;
        }

        private void Grapher_Load(object sender, EventArgs e)
        {
            //Save window size 
            _h = ClientSize.Height;
            _w = ClientSize.Width;

            //Save distance between window and graph, must leave space to fit text
            _offseth = _h * 0.2f;
            _offsetw = _w * 0.09f;

            //Set the number of separators in the axis
            _mCantsepX = 10;
            _mCantsepY = 10;
        }

        //Calculate the X and Y max/min data
        private void SetMinMax(ArrayList pts)
        {
            int size = pts.Count;
            for (int i = 0; i < size; i++)
            {
                //Max
                if (((PointF)pts[i]).X > _mMaxValueX) _mMaxValueX = ((PointF)pts[i]).X;
                if (((PointF)pts[i]).Y > _mMaxValueY) _mMaxValueY = ((PointF)pts[i]).Y;

                //Min
                if (((PointF)pts[i]).X < _mMinValueX) _mMinValueX = ((PointF)pts[i]).X;
                if (((PointF)pts[i]).Y < _mMinValueY) _mMinValueY = ((PointF)pts[i]).Y;
            }
        }

        //Draw the graph
        private void Graph(ArrayList pts, Graphics g, Color c)
        {
	        int size = pts.Count;
            float axisX = _w - _offsetw * 2;
            float axisY = _h - _offseth * 2;
            float x = 0, y = 0;

            //Place all the values in the array
            for (int i = 0; i < size; i++)
            {
                //Next point
                var p = (PointF)pts[i];

                //Get the new X position (Make sure we dont divide by 0)
                if((_mMaxValueX - _mMinValueX) != 0)
                    x = ((p.X - _mMinValueX) * axisX) / (_mMaxValueX - _mMinValueX);
                else
                    x = ((p.X - _mMinValueX) * axisX);

                //Get the new Y position (Make sure we dont divide by 0)
                if ((_mMaxValueY - _mMinValueY) != 0)
                    y = ((p.Y - _mMinValueY) * axisY) / (_mMaxValueY - _mMinValueY);
                else
                    y = ((p.Y - _mMinValueY) * axisY);

                //If it's the last point draw the last line
                if (i != size - 1)
                {
                    p = (PointF)pts[i + 1];

                    //Get the new X position (Make sure we dont divide by 0)
                    float xNext, yNext;
                    if ((_mMaxValueX - _mMinValueX) != 0)
                        xNext = ((p.X - _mMinValueX) * axisX) / (_mMaxValueX - _mMinValueX);
                    else
                        xNext = ((p.X - _mMinValueX) * axisX);

                    //Get the new Y position (Make sure we dont divide by 0)
                    if ((_mMaxValueY - _mMinValueY) != 0)
                        yNext = ((p.Y - _mMinValueY) * axisY) / (_mMaxValueY - _mMinValueY);
                    else
                        yNext = ((p.Y - _mMinValueY) * axisY);

                    g.DrawLine(new Pen(Color.Black), x + _offsetw, (_h - y) - _offseth, xNext + _offsetw, (_h - yNext) - _offseth);
                }

                g.FillEllipse(new SolidBrush(c), (int)(x + _offsetw - 4), (int)((_h - y) - _offseth - 4), 8, 8);
                g.DrawEllipse(Pens.Black, (int)(x + _offsetw - 4), (int)((_h - y) - _offseth - 4), 8, 8);
            }
        }


        //Draw all the texts
        private void DrawTexts(Graphics gfx)
        {
            var rect = new Rectangle((int)_offsetw, (int)(_offseth - 35), (int)(_w - _offsetw * 2), 20);

            //Title
            var stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;
            gfx.DrawString(_mSTitulo, new Font("Arial", 12), Brushes.Black, rect, stringFormat);

            //X-Axis
            rect = new Rectangle((int)_offsetw, (int)(_h - _offseth + 20), (int)(_w - _offsetw * 2), 35);
            gfx.DrawString(_mSAxisX, new Font("Arial", 12), Brushes.Black, rect, stringFormat);

            //Y-Axis
            rect = new Rectangle((int)(_offsetw - 50), (int)_offseth, 25, (int)(_h - _offseth * 2));
            SizeF size = gfx.MeasureString(_mSAxisY, new Font("Arial", 12));

            var tx = (int)(_offsetw / 2);
            var ty = (int)(_offseth + (_h - _offseth * 2) / 2);

            gfx.TranslateTransform(tx, ty, System.Drawing.Drawing2D.MatrixOrder.Append);
            
            gfx.RotateTransform(180.0F);
            gfx.DrawString(_mSAxisY, new Font("Arial", 12), Brushes.Black, -size.Height/2 + 20, -size.Width/2, new StringFormat(StringFormatFlags.DirectionVertical));
            gfx.RotateTransform(-180.0F);

            gfx.TranslateTransform(-tx, -ty, System.Drawing.Drawing2D.MatrixOrder.Append);
        }


        //Draws the legend
        private void DrawLegend(Graphics gfx)
        {
            var stringFormat = new StringFormat
	            {
		            LineAlignment = StringAlignment.Center,
		            Alignment = StringAlignment.Center
	            };

	        //Draw the elements in the plot, under the label
            int printed = 1;
            for (int i = 1; i <= _mINumDatos; i++)
            {
                //Only draw the datasets shown
                if (_mBShown[i - 1])
                {
                    //Draw the key colour
                    gfx.FillEllipse(new SolidBrush((Color)_mColours[i - 1]), Legend.Location.X + Legend.Size.Width / 2 - 4, 
														Legend.Location.Y + Legend.Size.Height + printed * 10, 8, 8);
                    gfx.DrawEllipse(Pens.Black, Legend.Location.X + Legend.Size.Width / 2 - 4, 
														Legend.Location.Y + Legend.Size.Height + printed * 10, 8, 8);

                    //Write the corresponding name
                    if (_mSNames != null && _mSNames.Count >= i)
                    {
                        gfx.DrawString((string)_mSNames[i - 1], new Font("Arial", 8), 
															Brushes.Black, new PointF(Legend.Location.X + Legend.Size.Width / 2 + 8, 
															Legend.Location.Y + Legend.Size.Height + printed * 10 - 2));
                    }

                    ++printed;
                }
            }

            //Write "Click to move text"
            if (_mBLeyendaOver)
                gfx.DrawString("click to move", new Font("Arial", 5), Brushes.Gray, new PointF(Legend.Location.X, Legend.Location.Y - 8));
        }

        //If window is resized recalculate sizes
        private void Grapher_Resize(object sender, EventArgs e)
        {
            _h = ClientSize.Height;
            _w = ClientSize.Width;

            _offseth = _h * 0.2f;
            _offsetw = _w * 0.09f;

            Refresh();
        }


        //Paint the window
        private void Grapher_Paint(object sender, PaintEventArgs e)
        {
            Graphics gfx = e.Graphics;
            gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            gfx.TranslateTransform(10, 0);

            //Draw axis
            gfx.DrawLine(new Pen(Color.Black), _offsetw, _offseth, _offsetw, _h - _offseth);
            gfx.DrawLine(new Pen(Color.Black), _offsetw, _h - _offseth, _w - _offsetw, _h - _offseth);

            float axisx = _w - _offsetw * 2;
            float axisy = _h - _offseth * 2;


            //Draw separators in X-Axis
            float nx1, ny1, nx2, ny2;
            float dist = (_w - _offsetw * 2) / _mCantsepX;
            float step = (_mMaxValueX - _mMinValueX) / _mCantsepX;
            for (int i = 0; i <= _mCantsepX; i++)
            {
                nx1 = _offsetw + dist * i;
                ny1 = _h - _offseth;
                nx2 = _offsetw + dist * i;
                ny2 = _h - _offseth + 5;
                gfx.DrawLine(new Pen(Color.Black), nx1, ny1, nx2, ny2);
                gfx.DrawString((_mMinValueX + step * i).ToString("N2"), new Font("Arial", 6), new SolidBrush(Color.Black), new PointF(nx2, ny2));
            }

            //Draw separators in Y-Axis
            dist = (_h - _offseth * 2) / _mCantsepY;
            step = (_mMaxValueY - _mMinValueY) / _mCantsepY;
            for (int i = 0; i <= _mCantsepY; i++)
            {
                nx1 = _offsetw;
                ny1 = _offseth + dist * i;
                nx2 = _offsetw - 5;
                ny2 = _offseth + dist * i;
                gfx.DrawLine(new Pen(Color.Black), nx1, ny1, nx2, ny2);
                SizeF strsize = gfx.MeasureString(((_mMaxValueY - step * i)).ToString("N2"), new Font("Arial", 6));

                gfx.DrawString(((_mMaxValueY - step * i)).ToString("N2"), new Font("Arial", 6), 
															new SolidBrush(Color.Black), new PointF(nx2 - strsize.Width, ny2));
            }

            //Graph data
            for (int i = 0; i < _mINumDatos; i++)
            {
                if (_mBShown[i])
                    Graph((ArrayList)_mData[i], gfx, (Color)_mColours[i]);
            }

            //Draw Texts
            DrawTexts(gfx);

            //Draw Legend
            DrawLegend(gfx);

        }

        //Update data (used when combo box is updated)
        private void UpdateData()
        {
            //Reset data values
            _mMaxValueX = 0;
            _mMaxValueY = 0;
            _mMinValueX = 99999999;
            _mMinValueY = 99999999;

            //Get the mix/max values for the data to be displayed
	        for (int i = 0; i < _mINumDatos; i++)
	        {
		        if (_mBShown[i])
		        {
							SetMinMax((ArrayList) _mData[i]);
							if (_mXAxisTexts != null) SetAxisNameX((string)_mXAxisTexts[i]);
							if (_mYAxisTexts != null) SetAxisNameY((string)_mYAxisTexts[i]);
						}
					}	

          SetPlotTitle((string)cmboDataShown.SelectedItem + "");
          //if(_mBUpdateAxisY) SetAxisNameY((string)cmboDataShown.SelectedItem);
					//if (_mBUpdateAxisY) SetAxisNameY((string)cmboDataShown.SelectedItem);
        }




        //***** Data File *****
        //Insert data that wants to be included in the text file with the X and Y
        public void AddExtraData(ArrayList newData, string dataName)
        {
            if (((ArrayList)_mData[0]).Count != newData.Count)
            {
                MessageBox.Show(Universal.Idioma("The number of extra data is different than the number of points being plotted.",
                    "El número de datos extra es distinto al de número de puntos."));
                return;
            }

            //Add data to data array
            _mExtraData.Add(newData);
            _mExtraDataName.Add(dataName);
        }



        //***** Events *****
        private void SaveData_Click(object sender, EventArgs e)
        {
            //Create a save file dialog
            var svdlg = new SaveFileDialog();
            svdlg.Filter = "Text Files (*.txt)|*.txt";
            if (svdlg.ShowDialog() == DialogResult.OK)
            {
                //Open the file for writing
                TextWriter tw = new StreamWriter(svdlg.FileName);

                //Write the title for each column
                tw.Write("{0,-20}", _mSAxisX);

                //Write the title for extra data
                if (_mExtraData != null)
                {
                    for (int i = 0; i < _mExtraData.Count; ++i)
                    {
                        tw.Write("{0,-20}", _mExtraDataName[i].ToString());
                    }
                }

                //Write title of the dataset(s)
                for (int i = 0; i < _mData.Count; ++i)
                {
                    if (_mBShown[i])
                    {
                        tw.Write("{0,-20}", _mSNames[i]);
                    }
                }
                
                tw.Write(tw.NewLine);
                

                //Write the data for each dataset
                int numData = ((ArrayList)_mData[0]).Count;
                for (int i = 0; i < numData; ++i)
                {
                    //Write the X-Axis data
                    tw.Write("{0,-20}", ((PointF)((ArrayList)_mData[0])[i]).X.ToString("N3"));

                    //Write the extra data
                    if (_mExtraData != null)
                    {
                        for (int j = 0; j < _mExtraData.Count; ++j)
                        {
                            tw.Write("{0,-20}", ((float)((double)((ArrayList)_mExtraData[j])[i])).ToString("N3")); 
                        }
                    }

                    //Write the Y-Axis data ... could be many
                    for (int j = 0; j < _mData.Count; ++j)
                    {
                        if (_mBShown[j])
                        {
                            tw.Write("{0,-20}", ((PointF)((ArrayList)_mData[j])[i]).Y.ToString("N3"));
                        }
                    }

                    tw.Write(tw.NewLine);
                }

                //Close writer
                tw.Close();
            }
        }

        //Update the data values shown in thestatus bar
        private void Grapher_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.X > _offsetw && e.X < _w - _offsetw &&
                e.Y > _offseth && e.Y < _h - _offseth)
            {
                float axisx = _w - _offsetw * 2;
                float axisy = _h - _offseth * 2;

                Posx.Text = System.Convert.ToString((((e.X - _offsetw) * (_mMaxValueX - _mMinValueX)) / axisx) + _mMinValueX);
                Posy.Text = System.Convert.ToString((((axisy - (e.Y - _offseth)) * (_mMaxValueY - _mMinValueY)) / axisy) + _mMinValueY);
								toolStripStatusLabel1.Text = _mStatusX;
								toolStripStatusLabel2.Text = _mStatusY;
            }
            else
            {
                Posx.Text = " -- ";
                Posy.Text = " -- ";
            }

        }


        //Mouse event on the legend
        private void Legend_MouseEnter(object sender, EventArgs e)
        {
            Legend.BackColor = Color.DarkOrange;
            _mBLeyendaOver = true;
            Refresh();
        }

        //Mouse event on the legend
        private void Legend_MouseDown(object sender, MouseEventArgs e)
        {
            _mBLeyendaMove = true;
        }

        //Mouse event on the legend
        private void Legend_MouseMove(object sender, MouseEventArgs e)
        {
            if (_mBLeyendaMove)
            {
                var p = new Point(Legend.Location.X + e.X, Legend.Location.Y + e.Y);
                Legend.Location = p;

                Refresh();
            }
        }

        //Mouse event on the legend
        private void Legend_MouseUp(object sender, MouseEventArgs e)
        {
            _mBLeyendaMove = false;
            Refresh();
        }

        //Mouse event on the legend
        private void Legend_MouseLeave(object sender, EventArgs e)
        {
            Legend.BackColor = Color.LightGray;
            _mBLeyendaOver = false;
            Refresh();
        }

        //Update grapher when a new dataset is selected
        private void cmboDataShown_SelectedIndexChanged(object sender, EventArgs e)
        {
	        try
	        {
						if (cmboDataShown.SelectedIndex == 0)
						{
							//"All" is selected
							for (int i = 0; i < _mINumDatos; ++i)
							{
								_mBShown[i] = true;
							}
						}
						else
						{
									//Set the specified dataset
									for (int i = 0; i < _mINumDatos; ++i)
									{
											_mBShown[i] = false;
									}
									_mBShown[cmboDataShown.SelectedIndex - 1] = true;
						}

						UpdateData();
						Refresh();
	        }
	        catch (Exception ex)
	        {
						MessageBox.Show(Universal.Idioma("ERROR 2003160956: error while selecting graph. ", "ERROR 2003160956: error seleccionando gráfico. ") +
                            ex.Message, "RiverFlow2D",
														MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
	        }
	
        }


    }
}
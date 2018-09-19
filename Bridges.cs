using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Simulation_Options
{
	public partial class Bridges : Form
	{
		
		private static readonly Brush Black = new SolidBrush(Color.Black);
		private static readonly Pen BlackPen = new Pen(Black, 1);
		private static readonly Brush Red = new SolidBrush(Color.Red);
		private static readonly Pen RedPen = new Pen(Red, 1);

		private static readonly List<Brush> Brush = new List<Brush>();
		private static readonly List<Pen> Pen = new List<Pen>();
		private static int _nextPen;

		private static readonly Point[] Points = new Point[]
			{
				new Point(10, 300), new Point(200, 200), new Point(250, 400), new Point(300, 100),
				new Point(340, 200), new Point(450, 450)
			};


		private static Point[] points2 = new Point[]
			{
				new Point(15, 315), new Point(220, 250), new Point(400, 450),
			};

		public class PolyLine
		{
			public string Name;
			public Pen Color;
			public  ArrayList Line;
		};

		public DataGridView MyDataBridgeGeometry { get; set; }

		private double _lastY;
		private int _last_eY;
		//private int _lastLineToDrag;
		//private int _lastPointToDrag;

		public List<PolyLine> CanvasPolyLines = new List<PolyLine>();

		private List<PolyLine> polyLines = new List<PolyLine>();
		private Boolean _dragingLine = false;
		private int _pointToDrag = -1;
		private int _lineToDrag = -1;
		private int _lineSelected = -1;
		private int _initialY = -1;
		private int _previousX = 0;
		private int _previousY = 0;
		private PointF[] _deck;
		private PointF[] _zLower;
		private PointF[] _zUpper;
		private PointF[] _bed;
		private Point[] _deckI;
		private Point[] _zLowerI;
		private Point[] _zUpperI;
		private Point[] _bedI;
		private const int bed = 0;
		private const int zLower = 1;
		private const int zUpper = 2;
		private const int deck = 3;

		private int _nBridgePoints ;
		//World coordinates.
		private double _worldX1 = Single.MaxValue;
		private double _worldY1 = Single.MaxValue;
		private double _worldX2 = Single.MinValue;
		private double _worldY2 = Single.MinValue;

		//private struct WorldCoordinates
		//{
		//	public double X0;
		//	public double Y0;
		//	public double X1;
		//	public double Y1;
		//}

		//private WorldCoordinates _world;

		public enum ActionState
		{
			Normal,
			AddNode,
			CreateLine,
			SelectLine
		};

		public ActionState CurrentState = ActionState.Normal;
		public Boolean _dragingNode = false;
		public int CurrentVertical;
		public Boolean UpdatePlotValues = true;
		//public bool cellValueChanged = false;

		public Bridges()
		{
			InitializeComponent();

			Brush.Add(new SolidBrush(Color.Black));
			Brush.Add(new SolidBrush(Color.Crimson));
			Brush.Add(new SolidBrush(Color.DeepSkyBlue));
			Brush.Add(new SolidBrush(Color.ForestGreen));
			Brush.Add(new SolidBrush(Color.BlueViolet));
			Brush.Add(new SolidBrush(Color.Chocolate));
			Brush.Add(new SolidBrush(Color.Brown));
			Brush.Add(new SolidBrush(Color.DarkGreen));
			Brush.Add(new SolidBrush(Color.DarkOrange));
			Brush.Add(new SolidBrush(Color.DarkOrchid));
			Brush.Add(new SolidBrush(Color.Violet));
			Brush.Add(new SolidBrush(Color.YellowGreen));

			Pen.Add(new Pen(Color.Black));
			Pen.Add(new Pen(Color.Crimson));
			Pen.Add(new Pen(Color.DeepSkyBlue));
			Pen.Add(new Pen(Color.ForestGreen));
			Pen.Add(new Pen(Color.BlueViolet));
			Pen.Add(new Pen(Color.Chocolate));
			Pen.Add(new Pen(Color.Brown));
			Pen.Add(new Pen(Color.DarkGreen));
			Pen.Add(new Pen(Color.DarkOrange));
			Pen.Add(new Pen(Color.DarkOrchid));
			Pen.Add(new Pen(Color.Violet));
			Pen.Add(new Pen(Color.YellowGreen));

			_nextPen = -1;

			//_world.X0 = 10000;
			//_world.Y0 = 200;
			//_world.X1 = 20000;
			//_world.Y1 = 1000;
		}

		public void SetBridgePoints(int nPoints, PointF[] bed,  PointF[] zLower, PointF[] zUpper,PointF[] deck)
		{
			_nBridgePoints = nPoints;
			_bed = bed;
			_zUpper = zUpper;
			_zLower = zLower;
			_deck = deck;
		}


		public void ChangeSingleBridgeCoordinate(string geometry, int index, float newValue)
		{
			//Transform new value to display coordinates.
			int x1; int y1; int x2; int y2;
			GetWindowRectangle(out x1, out y1, out x2, out y2);
			int iY = Convert.ToInt32(y2 + (y1 - y2) * (newValue - _worldY1) / (_worldY2 - _worldY1));

			//Change Y value of the corresponding geometry and index.
			switch (geometry)
			{

				case ("Bed"):
					_bed[index].Y = newValue;
					_bedI[index].Y = iY;
					break;

				case ("Zlower"):
					_zLower[index].Y = newValue;
					_zLowerI[index].Y = iY;
					break;

				case ("Zupper"):
					_zUpper[index].Y = newValue;
					_zUpperI[index].Y = iY;
					break;

				case ("Deck"):
					_deck[index].Y = newValue;
					_deckI[index].Y = iY;
					break;
			}

			 _worldY1 = Single.MaxValue;
			 _worldY2 = Single.MinValue;

			for (int i = 0; i < _nBridgePoints; ++i)
			{
				_worldY1 = Math.Min(_worldY1, _bed[i].Y);
				_worldY2 = Math.Max(_worldY2, _bed[i].Y);

				_worldY1 = Math.Min(_worldY1, _zUpper[i].Y);
				_worldY2 = Math.Max(_worldY2, _zUpper[i].Y);

				_worldY1 = Math.Min(_worldY1, _zLower[i].Y);
				_worldY2 = Math.Max(_worldY2, _zLower[i].Y);

				_worldY1 = Math.Min(_worldY1, _deck[i].Y);
				_worldY2 = Math.Max(_worldY2, _deck[i].Y);
			}
		}


		public void SetDisplayPoints( Point[] bedI,  Point[] zLowerI, Point[] zUpperI,Point[] deckI)
		{
			_bedI = bedI;
			_zLowerI = zLowerI;
			_zUpperI = zUpperI;
			_deckI = deckI;
			
			var thisLineBed = new PolyLine {Line = new ArrayList(), Name = "Bed", Color = Pen[1]};
			thisLineBed.Line.Add(_bedI);
			polyLines.Add(thisLineBed);

			var thisLineZLower = new PolyLine { Line = new ArrayList(), Name = "Zlower", Color = Pen[2] };
			thisLineZLower.Line.Add(_zLowerI);
			polyLines.Add(thisLineZLower);

			var thisLineZUpper = new PolyLine { Line = new ArrayList(), Name = "Zupper", Color = Pen[3] };
			thisLineZUpper.Line.Add(_zUpperI);
			polyLines.Add(thisLineZUpper);

			var thisLineDeck = new PolyLine { Line = new ArrayList(), Name = "Deck", Color = Pen[4] };
			thisLineDeck.Line.Add(_deckI);
			polyLines.Add(thisLineDeck);
		}

		public void SetWorldRectangle(float a1, float b1, float a2, float b2)
		{
			_worldX1 = a1;
			_worldY1 = b1;
			_worldX2 = a2;
			_worldY2 = b2;
		}

		public void RedrawCanvas()
		{
			Canvas.Invalidate();
		}

		private void Bridges_Load(object sender, EventArgs e)
		{
			lblCoordinates.Text = "";
		}

		private int GetNextPen()
		{
			if (_nextPen == Pen.Count - 1)
				_nextPen = 0;
			else
				_nextPen++;
			return _nextPen;
		}

		private void Canvas_Paint(object sender, PaintEventArgs e)
		{
			try
			{
				var onePolyPoints = new List<Point>();
				var anotherPolyPoints = new List<Point>();
				for (int i = 0; i < polyLines.Count; i++)
				{
					var pnts = (Point[])polyLines[i].Line[0];
					if (CurrentVertical >= pnts.Count())
						CurrentVertical = pnts.Count() - 1;
					float x = pnts[CurrentVertical].X;
					float y = pnts[CurrentVertical].Y;
					e.Graphics.FillEllipse(Brushes.Black, x - 5, y - 5, 10, 10);
					e.Graphics.FillEllipse(Brushes.Gold, x - 4, y - 4, 8, 8);

					//Draw Zupper y Deck as polygons.
					if (i == 2 )
					{
						for (int j = 0; j < pnts.Count(); j++)
						{
							onePolyPoints.Add(pnts[j]);
						}
					}

					if (i == 3)
					{
						for (int j = pnts.Count()-1; j>=0 ; j--)
						{
							onePolyPoints.Add(pnts[j]);
						}
					}

					//Draw Bed y Zlower as polygons.
					if (i == 0)
					{
						for (int j = 0; j < pnts.Count(); j++)
						{
							anotherPolyPoints.Add(pnts[j]);
						}
					}

					if (i == 1)
					{
						for (int j = pnts.Count() - 1; j >= 0; j--)
						{
							anotherPolyPoints.Add(pnts[j]);
						}
					}
				}
				e.Graphics.FillPolygon(Brushes.DarkGray, onePolyPoints.ToArray());
				e.Graphics.FillPolygon(Brushes.LightGray, anotherPolyPoints.ToArray());

				int k = -1;
				foreach (PolyLine t in polyLines)
				{
					foreach (Point[] pnts in t.Line)
					{
						k++;
						if (_lineSelected == k)
						{
							e.Graphics.DrawLines(t.Color, pnts);
							for (int j = 0; j < pnts.Count(); j++)
								e.Graphics.FillRectangle(Black, new Rectangle((int)(pnts[j].X - 2.5), (int)(pnts[j].Y - 2.5), 10, 10));
						}
						else
						{
							e.Graphics.DrawLines(t.Color, pnts);
							foreach (Point pnt in pnts)
							{
								float xx = pnt.X - 4;
								float yy = pnt.Y - 4;
								e.Graphics.FillEllipse(t.Color.Brush, xx, yy, 8, 8);
							}
						}
					}
				}


				using (var myFont = new Font("Arial", 14))
				{
					e.Graphics.DrawString("Y", myFont, Brushes.Black, new Point(10, Canvas.Height/2));
					e.Graphics.DrawString("X", myFont, Brushes.Black, new Point(Canvas.Width/2, Canvas.Height-myFont.Height));
				}

				using (var myFont = new Font("Arial", 10))
				{
					for (int i = 0; i < polyLines.Count; i++)
					{
						var pnts = (Point[]) polyLines[i].Line[0];
						switch (polyLines[i].Name)
						{
							case ("Bed"):				
								e.Graphics.DrawString("Bed", myFont, polyLines[i].Color.Brush,
								new Point(pnts[pnts.Count()-1].X+5, pnts[pnts.Count()-1].Y));
								break;

							case ("Zlower"):
								e.Graphics.DrawString("Z lower", myFont, polyLines[i].Color.Brush,
								new Point(pnts[pnts.Count()-1].X+5, pnts[pnts.Count()-1].Y));
								break;

							case ("Zupper"):
								e.Graphics.DrawString("Z upper", myFont, polyLines[i].Color.Brush,
								new Point(pnts[pnts.Count()-1].X+5, pnts[pnts.Count()-1].Y));
								break;

							case ("Deck"):
								e.Graphics.DrawString("Deck", myFont, polyLines[i].Color.Brush,
								new Point(pnts[pnts.Count()-1].X+5, pnts[pnts.Count()-1].Y));
								break;
						}
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(Universal.Idioma("ERROR 0204141429: error while drawing polylines. ", "ERROR 0204141429: error dibujando polilíneas. ") + ex.Message + ".",
												"RiverFlow2D", MessageBoxButtons.OK, MessageBoxIcon.Error);				
			}
		}

		private void actionsToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		private void addNodesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Cursor = Cursors.Cross;
			CurrentState = ActionState.AddNode;
		}

		private void createPolylineToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var pnts = new Point[]
				{
					new Point(Canvas.Left + Canvas.Width/4, Canvas.Top + Canvas.Height/6),
					new Point(Canvas.Left + Canvas.Width/3, Canvas.Top + Canvas.Height/6)
				};

			var otherLine = new PolyLine {Line = new ArrayList(), Name = "Mol", Color = Pen[GetNextPen()]};
			otherLine.Line.Add(pnts);
			polyLines.Add(otherLine);
			_lineSelected = polyLines.Count - 1;
			CurrentState = ActionState.Normal;
			//Canvas.Refresh();
		}

		private void moveNodesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Cursor = Cursors.Default;
			CurrentState = ActionState.Normal;
		}

		private void selectPolylineToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Cursor = Cursors.Default;
			CurrentState = ActionState.SelectLine;
			_lineSelected = -1;
		}

		private void showCoordinatesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (_lineSelected > -1)
			{
				int x1; int y1; int x2; int y2;
				GetWindowRectangle(out x1, out y1, out x2, out y2);

				//double xx = _worldX1 + (_worldX2 - _worldX1) * (e.X - x1) / (x2 - x1);
				//double yy = _worldY1 + (_worldY2 - _worldY1) * (e.Y - y2) / (y1 - y2);
				//lblCoordinates.Text = "X:  " + string.Format("{0:0.0000}", xx) + "         Y:  " + string.Format("{0:0.0000}", yy);

				var coords = new List<string>();

				foreach (Point[] pnts in polyLines[_lineSelected].Line)
				{
					foreach (Point pnt in pnts)
					{
						double xx = _worldX1 + (_worldX2 - _worldX1) * (pnt.X - x1) / (x2 - x1);
						double yy = _worldY1 + (_worldY2 - _worldY1) * (pnt.Y - y2) / (y1 - y2);

						string row = string.Format("{0:0.0000}", xx) + " " + string.Format("{0:0.0000}", yy);
						coords.Add(row);						
					}
				}

				var thisCoordinates = new PolylineCoordinates();
				thisCoordinates.SetData(coords);
				thisCoordinates.ShowDialog();
				
				if (thisCoordinates.ValueChanged)
				{
					//Retrieve points from table in dialog and replace points in selected line.
					List<string> newCoords = thisCoordinates.UpdatePoly();
					if (newCoords.Count > 1)
					{
						var newPoints = new Point[newCoords.Count - 1];
						for (int i = 0; i < newCoords.Count - 1; i++)
						{
							string[] nextRow = newCoords[i].Split(new[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);

							int iX = Convert.ToInt32(x1 + (x2 - x1) * (Convert.ToSingle( nextRow[0]) - _worldX1) / (_worldX2 - _worldX1));
							int iY = Convert.ToInt32(y2 + (y1 - y2) * (Convert.ToSingle( nextRow[1]) - _worldY1) / (_worldY2 - _worldY1));

							//var pnt = new Point {X = Convert.ToInt32(nextRow[0]), Y = Convert.ToInt32(nextRow[1])};
							newPoints[i].X = iX;
							newPoints[i].Y = iY;
						}
						polyLines[_lineSelected].Line.Clear();
						polyLines[_lineSelected].Line.Add(newPoints);

					}
					else
						polyLines.RemoveAt(_lineSelected);

					Canvas.Invalidate();
				}
			}
			else
				MessageBox.Show(Universal.Idioma("Select a polyline", "Seleccionar una polilínea") ,
                    "RiverFlow2D",MessageBoxButtons.OK,MessageBoxIcon.Information);
		}

		private void Canvas_MouseDown(object sender, MouseEventArgs e)
		{
			switch (CurrentState)
			{
				case ActionState.Normal:
					int k = -1;
					foreach (PolyLine t in polyLines)
					{
						k++;
						foreach (Point[] pnts in t.Line)
						{
							for (int i = 0; i < pnts.Count(); i++)
							{
								int xx = pnts[i].X ;
								int yy = pnts[i].Y;
								if (e.X > xx- 5 & e.X < xx + 5 & e.Y >  yy - 5 & e.Y < yy + 5)
								{
									_lineSelected = k;
									_dragingNode = true;
									_lineToDrag = k;
									_pointToDrag = i;
									CurrentVertical = i;
									Canvas.Invalidate();
									return;
								}
							}

							for (int i = 0; i < pnts.Count() - 1; i++)
							{
								double x1 = pnts[i].X;
								double y1 = pnts[i].Y;
								double x2 = pnts[i+1].X;
								double y2 = pnts[i+1].Y;
								double x0 = e.X;
								double y0 = e.Y	;

								if (x0 >= x1 && x0 <= x2)
								{
									double distance = Math.Abs((x2 - x1)*(y1 - y0) - (x1 - x0)*(y2 - y1))/
									                  Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
									if (distance < 5)
									{
										_lineSelected = k;
										_lineToDrag = k;
										_dragingLine = true;
										_previousX = e.X;
										_previousY = e.Y;
										_initialY = e.Y;
										Canvas.Invalidate();
										return;
									}
								}
							}
						}
					}
					NormalizeState();
					break;

				case ActionState.AddNode:

					int thisLine = -1;
					foreach (PolyLine t in polyLines)
					{
						foreach (Point[] pnts in t.Line)
						{
							thisLine++;
							for (int i = 1; i < pnts.Count(); i++)
							{
								double x1 = pnts[i - 1].X;
								double y1 = pnts[i - 1].Y;
								double x2 = pnts[i].X;
								double y2 = pnts[i].Y;
								double x0 = e.X;
								double y0 = e.Y;

								double d12 = Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
								double d10 = Math.Sqrt(Math.Pow(x1 - x0, 2) + Math.Pow(y1 - y0, 2));
								double d20 = Math.Sqrt(Math.Pow(x2 - x0, 2) + Math.Pow(y2 - y0, 2));
								double isOnSegment = Math.Abs(d10 + d20 - d12);

								if (isOnSegment < 0.1)
								{
									double distance = Math.Abs((x2 - x1)*(y1 - y0) - (x1 - x0)*(y2 - y1))/
									                  Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));

									double alpha = Math.Atan(y0/x0);
									double y = distance*Math.Sin(alpha) + y0;
									double x;
									x = x0 - Math.Sqrt(Math.Pow(distance, 2) - Math.Sqrt(y0 - y));

									if (Double.IsNaN(y))
										y = y0;
									if (Double.IsNaN(x))
										x = x0;

									var newPoints = new Point[pnts.Count() + 1];

									for (int j = 0; j < i; j++)
									{
										newPoints[j].X = pnts[j].X;
										newPoints[j].Y = pnts[j].Y;
									}

									newPoints[i].X = (int) x;
									newPoints[i].Y = (int) y;

									for (int j = i; j < pnts.Count(); j++)
									{
										newPoints[j + 1].X = pnts[j].X;
										newPoints[j + 1].Y = pnts[j].Y;
									}

									t.Line.Remove(pnts);
									t.Line.Add(newPoints);
									_lineSelected = thisLine;
									return;
								}
							}
						}
					}
					NormalizeState();
					break;

  		}
		}

		private void Canvas_MouseMove(object sender, MouseEventArgs e)
		{
			//if (! _justClick)
			//if (PointInsideValidBounds(_lineToDrag, _pointToDrag, e.Y))
			{
				int x1;
				int y1;
				int x2;
				int y2;
				GetWindowRectangle(out x1, out y1, out x2, out y2);

				//World coordinates of new position.
				double xx = _worldX1 + (_worldX2 - _worldX1)*(e.X - x1)/(x2 - x1);
				double yy = _worldY1 + (_worldY2 - _worldY1)*(e.Y - y2)/(y1 - y2);
				lblCoordinates.Text = "X:  " + string.Format("{0:0.0000}", xx) + "         Y:  " + string.Format("{0:0.0000}", yy);

				double xxx = e.X;
				double yyy = e.Y;
				lblCoordinatesI.Text = "X:  " + string.Format("{0:0}", xxx) + "         Y:  " + string.Format("{0:0}", yyy);

				if (e.Button == MouseButtons.Left)
				{
					if (_dragingNode)
					{
						//Get polyline of the vertex being moved, update its value.
						
						if (PointInsideValidBounds(_lineToDrag, _pointToDrag, e.Y))
						{
							var pnts = (Point[]) polyLines[_lineToDrag].Line[0];
							pnts[_pointToDrag].Y = e.Y;
							_lastY = yy;
							_last_eY = e.Y;
							Canvas.Invalidate();
						}
					}
					else if (_dragingLine)
					{
						//if (AllPointsInsideValidBounds(_lineSelected, _previousY + e.Y))
						//{
						//	var pnts = (Point[]) polyLines[_lineSelected].Line[0];
						//	for (int i = 0; i < pnts.Count(); i++)
						//		pnts[i].Y = pnts[i].Y - _previousY + e.Y;

						//	_previousX = e.X;
						//	_previousY = e.Y;
						//	Canvas.Invalidate();
						//}
					}
				}
			}
		}

		private void Canvas_MouseUp(object sender, MouseEventArgs e)
		{
			{
				if (_dragingNode)
				{
					if (_lineToDrag >= 0 && _pointToDrag >= 0)
					{
						MyDataBridgeGeometry[_lineToDrag + 1, _pointToDrag].Value = string.Format("{0:0.0000}", _lastY);
						MyDataBridgeGeometry.Rows[_pointToDrag].Selected = true;

						var pnts = (Point[])polyLines[_lineToDrag].Line[0];
						pnts[_pointToDrag].Y = _last_eY; //e.Y;

						//Change Y value of the corresponding geometry and index.
						int x1;
						int y1;
						int x2;
						int y2;
						GetWindowRectangle(out x1, out y1, out x2, out y2);

						//oat yy = Convert.ToSingle(_worldY1 + (_worldY2 - _worldY1) * (e.Y - y2) / (y1 - y2));
						//float yy = Convert.ToSingle(_worldY1 + (_worldY2 - _worldY1) * (e.Y- y2) / (y1 - y2));
						float yy = Convert.ToSingle(_worldY1 + (_worldY2 - _worldY1) * (_last_eY - y2) / (y1 - y2));
						switch (polyLines[_lineToDrag].Name)
						{
							case ("Bed"):
								_bed[_pointToDrag].Y = yy;
								_bedI[_pointToDrag].Y = _last_eY;
								break;

							case ("Zlower"):
								_zLower[_pointToDrag].Y = yy;
								_zLowerI[_pointToDrag].Y = _last_eY; //e.Y;
								break;

							case ("Zupper"):
								_zUpper[_pointToDrag].Y = yy;
								_zUpperI[_pointToDrag].Y = _last_eY;
								break;

							case ("Deck"):
								_deck[_pointToDrag].Y = yy;
								_deckI[_pointToDrag].Y = _last_eY;
								break;
						}
					}
				}
				else if (_dragingLine)
				{
					if (_lineToDrag >= 0)
					{
						var pnts = (Point[])polyLines[_lineToDrag].Line[0];

						int x1;
						int y1;
						int x2;
						int y2;
						GetWindowRectangle(out x1, out y1, out x2, out y2);

						//Disable recalculating the plot values from geometry table dataBridgeGeometry 
						//in UpdateBridgePlot routine called from CellValueChanged event.
						UpdatePlotValues = false;
						
						for (int i = 0; i < pnts.Count(); i++)
						{
							//Change Y value of the corresponding geometry and index.
							float yy = Convert.ToSingle(_worldY1 + (_worldY2 - _worldY1) * (pnts[i].Y - y2) / (y1 - y2));
							//float yy = Convert.ToSingle(_worldY1 + (_worldY2 - _worldY1) * (pnts[i].Y - _previousY + e.Y - y2) / (y1 - y2));

							switch (polyLines[_lineToDrag].Name)
							{

								case ("Bed"):
									_bed[i].Y = yy;
									_bedI[i].Y = pnts[i].Y;
									MyDataBridgeGeometry[_lineToDrag + 1, i].Value = string.Format("{0:0.0000}", yy);
									break;

								case ("Zlower"):
									_zLower[i].Y = yy;
									_zLowerI[i].Y =pnts[i].Y;
									MyDataBridgeGeometry[_lineToDrag + 1, i].Value = string.Format("{0:0.0000}", yy);
									break;

								case ("Zupper"):
									_zUpper[i].Y =  yy;
									_zUpperI[i].Y = pnts[i].Y;
									MyDataBridgeGeometry[_lineToDrag + 1, i].Value = string.Format("{0:0.0000}", yy);
									break;

								case ("Deck"):
									MyDataBridgeGeometry[_lineToDrag + 1, i].Value = string.Format("{0:0.0000}", yy);
									_deck[i].Y =  yy;
									_deckI[i].Y = pnts[i].Y;
									break;
							}
						}
					}			
				}

				//If part of the polyline was moved outside the container, update limits.
				var b1 = Single.MaxValue;
				var b2 = Single.MinValue;
				for (int i = 1; i < _bed.Count(); ++i)
				{
					b1 = Math.Min(b1, _bed[i].Y);
					b2 = Math.Max(b2, _bed[i].Y);
					b1 = Math.Min(b1, _zLower[i].Y);
					b2 = Math.Max(b2, _zLower[i].Y);
					b1 = Math.Min(b1, _zUpper[i].Y);
					b2 = Math.Max(b2, _zUpper[i].Y);
					b1 = Math.Min(b1, _deck[i].Y);
					b2 = Math.Max(b2, _deck[i].Y);
				}
				_worldY1 = b1;
				_worldY2 = b2;
				RedrawAfterResize();

				//_lastPointToDrag = -1;
				_dragingLine = false;
				_dragingNode = false;
				_pointToDrag = -1;
				_lineToDrag = -1;
				_previousX = 0;
				_previousY = 0;

				//Enable recalculating the plot values from geometry table dataBridgeGeometry 
				//in UpdateBridgePlot routine called from CellValueChanged event.
				UpdatePlotValues = true;

				Canvas.Invalidate();
			}
		}

		private void NormalizeState()
		{
			//_lastPointToDrag = -1;
			//_lastLineToDrag = -1;
			_dragingLine = false;
			_dragingNode = false;
			_pointToDrag = -1;
			_lineToDrag = -1;
			_lineSelected = -1;
			_previousX = 0;
			_previousY = 0;
			CurrentState = ActionState.Normal;
			Cursor = Cursors.Default;
		}

		private void Canvas_Click(object sender, EventArgs e)
		{

		}

		private void Bridges_KeyPress(object sender, KeyPressEventArgs e)
		{
		
		}

		private void Bridges_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case (Keys.Escape):
					NormalizeState();
					Canvas.Invalidate();
					break;

				case (Keys.Delete):
					if (_lineSelected >= 0)
					{
						if (polyLines.Count > 0)
						{
							polyLines.RemoveAt(_lineSelected);
							_lineSelected = -1;
							Canvas.Invalidate();
						}
					}
					break;
			}
		}

		private void Bridges_ResizeEnd(object sender, EventArgs e)
		{
			RedrawAfterResize();
		}

		private void Bridges_Resize(object sender, EventArgs e)
		{
			if (WindowState == FormWindowState.Maximized)
			{
				RedrawAfterResize();
			}
		}

		public void RedrawAfterResize()
		{
			int x1; int y1; int x2; int y2;
			GetWindowRectangle(out x1, out y1, out x2, out y2);

			for (int i = 0; i < _nBridgePoints; ++i)
			{
				_bedI[i].X = Convert.ToInt32(x1 + (x2 - x1) * (_bed[i].X - _worldX1) / (_worldX2 - _worldX1));
				_bedI[i].Y = Convert.ToInt32(y2 + (y1 - y2) * (_bed[i].Y - _worldY1) / (_worldY2 - _worldY1));
				_zLowerI[i].X = Convert.ToInt32(x1 + (x2 - x1) * (_zLower[i].X - _worldX1) / (_worldX2 - _worldX1));
				_zLowerI[i].Y = Convert.ToInt32(y2 + (y1 - y2) * (_zLower[i].Y - _worldY1) / (_worldY2 - _worldY1));
				_zUpperI[i].X = Convert.ToInt32(x1 + (x2 - x1) * (_zUpper[i].X - _worldX1) / (_worldX2 - _worldX1));
				_zUpperI[i].Y = Convert.ToInt32((y2 + (y1 - y2) * (_zUpper[i].Y - _worldY1) / (_worldY2 - _worldY1)));
				_deckI[i].X = Convert.ToInt32(x1 + (x2 - x1) * (_deck[i].X - _worldX1) / (_worldX2 - _worldX1));
				_deckI[i].Y = Convert.ToInt32(y2 + (y1 - y2) * (_deck[i].Y - _worldY1) / (_worldY2 - _worldY1));
			}

			polyLines.Clear();

			var thisLine = new PolyLine { Line = new ArrayList(), Name = "Bed", Color = Pen[1] };
			thisLine.Line.Add(_bedI);
			polyLines.Add(thisLine);

			thisLine = new PolyLine { Line = new ArrayList(), Name = "Zlower", Color = Pen[2] };
			thisLine.Line.Add(_zLowerI);
			polyLines.Add(thisLine);

			thisLine = new PolyLine { Line = new ArrayList(), Name = "Zupper", Color = Pen[3] };
			thisLine.Line.Add(_zUpperI);
			polyLines.Add(thisLine);

			thisLine = new PolyLine { Line = new ArrayList(), Name = "Deck", Color = Pen[4] };
			thisLine.Line.Add(_deckI);
			polyLines.Add(thisLine);

			Canvas.Invalidate();				
		}

		public void GetWindowRectangle(out int x1, out int y1, out int x2, out int y2)
		{
			x1 = +30;
			y1 = +30;
		  x2 = Canvas.Width - 60;
			y2 = Canvas.Height - 60;
		}

		private bool PointInsideValidBounds(int line, int point, int y)
		{
			var bedPoints = (Point[])polyLines[bed].Line[0];
			var zLowerPoints = (Point[])polyLines[zLower].Line[0];
			var zUpperPoints= (Point[])polyLines[zUpper].Line[0];
			var deckPoints = (Point[])polyLines[deck].Line[0];

			if (line == bed && zLowerPoints[point].Y < y && zUpperPoints[point].Y < y && deckPoints[point].Y < y)
	       return true;

			if (line == zLower && bedPoints[point].Y > y && zUpperPoints[point].Y < y && deckPoints[point].Y < y)
				return true;

			if (line == zUpper && bedPoints[point].Y > y && zLowerPoints[point].Y > y && deckPoints[point].Y < y)
				return true;

			if (line == deck && bedPoints[point].Y > y && zLowerPoints[point].Y > y && zUpperPoints[point].Y > y)
				return true;

			return false;
		}

		//private bool AllPointsInsideValidBounds(int line,  int y)
		//{
		//	var bedPoints = (Point[]) polyLines[bed].Line[0];
		//	var zLowerPoints = (Point[]) polyLines[zLower].Line[0];
		//	var zUpperPoints = (Point[]) polyLines[zUpper].Line[0];
		//	var deckPoints = (Point[]) polyLines[deck].Line[0];
		//	int nPoints = bedPoints.Count();

			//switch (line)
			//{
			//	case (bed):
			//		for (int i = 0; i < nPoints; i++)
			//		{
			//			for (int j = 0; j < nPoints; j++) 
			//			{
			//				if (bedPoints[i].Y - y < zLowerPoints[j].Y  && bedPoints[i].Y - y < zUpperPoints[i].Y  && bedPoints[i].Y - y < deckPoints[i].Y)
			//					//OK..Y
			//					continue;

			//				return false;
			//			}

			//		}
			//		return true;


			//				//if (line == zLower && bedPoints[i].Y > y && zUpperPoints[i].Y < y && deckPoints[i].Y < y)
			//				//	//OK.
			//				//	continue;
			//				//if (line == zUpper && bedPoints[i].Y > y && zLowerPoints[i].Y > y && deckPoints[i].Y < y)
			//				//	//OK.
			//				//	continue;
			//				//if (line == deck && bedPoints[i].Y > y && zLowerPoints[i].Y > y && zUpperPoints[i].Y > y)
			//				//	//OK.
			//				//	continue;



			//		break;

			//	case (zLower):

			//		break;

			//	case (zUpper):

			//		break;

			//	case (deck):

			//		break;
			//}



		//}

		private void Canvas_MouseClick(object sender, MouseEventArgs e)
		{
			//Maybe starting to drag or just hightlight.
			//_justClick = true;

		}

		private void Canvas_MouseEnter(object sender, EventArgs e)
		{

		}
	}
}

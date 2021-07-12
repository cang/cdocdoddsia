using System;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Data;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using SIA.Common;
using SIA.UI.Controls.Automation.Dialogs;
using SIA.UI.Controls.Automation.Commands;
using SIA.UI.Controls.Automation;

using SiGlaz.UI.CustomControls.XPTable;
using SiGlaz.UI.CustomControls.XPTable.Editors;
using SiGlaz.UI.CustomControls.XPTable.Models;
using SiGlaz.UI.CustomControls.XPTable.Events;

using SIA.UI.Controls;
using SIA.UI.Controls.Common;
using SIA.UI.Controls.Dialogs;
using SIA.UI.Controls.Utilities;

using SIA.UI.Controls.Automation.Steps;
using SIA.UI.Controls.Commands;

using SIA.Plugins.Common;

using SIA.Workbench.Common;


namespace SIA.Workbench.UserControls
{
	/// <summary>
	/// Summary description for ScriptViewer.
	/// </summary>
    internal class ScriptViewer 
        : ListBox
	{
		#region Fields

		protected static readonly ListBoxItem BeginStep = new ListBoxItem();
		protected static readonly ListBoxItem EndStep = new ListBoxItem();

		protected static Color nodeColor1 = Color.FromArgb(0xFF, 172, 187, 192);
		protected static Color nodeColor2 = Color.White;

		protected static Color selNodeColor1 = Color.FromArgb(0xFF, 166, 202, 216);
		protected static Color selNodeColor2 = Color.White;
		
		protected static Font nodeFont = null;

		protected static Pen inputLinkPen = null;
		protected static Pen outputLinkPen = null;

		protected int[] itemMargins = new int[] {10, 20, 20, 10};
		protected int itemPadding = 2;

		protected int itemHeight = 40;
        protected int itemWidth = 240;
        protected bool drawText = true;
		#endregion

		#region Properties

		public int StepCount
		{
			get 
			{
				return this.Items.Count - 2;				
			}
		}

		protected new ListBox.ObjectCollection Items
		{
			get {return base.Items;}
		}
		
		protected new bool Sorted
		{
			get {return base.Sorted;}
		}
		
		public ProcessStep this[int index]
		{
			get
			{
				int listIndex = index+1;
				ListBoxItem item = base.Items[listIndex] as ListBoxItem;
				return item.ProcessStep;
			}
		}


		#endregion

		#region Constructor and destructor

		static ScriptViewer()
		{
			nodeFont = new Font("Arial", 8.0F);
			//nodePen = new Pen(nodeColor1, 2.0F);
			
			outputLinkPen = new Pen(Color.Green, 4.0F);
			
			AdjustableArrowCap penCap = new AdjustableArrowCap(4, 5, true);
			penCap.MiddleInset = 2;
			inputLinkPen = new Pen(Color.Green, 4.0F);
			inputLinkPen.EndCap = LineCap.Custom;
			inputLinkPen.CustomEndCap = penCap;
		}

		public ScriptViewer()
		{
			this.InitializeComponent();

			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.DoubleBuffer, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.UserPaint, true);

			this.AllowDrop = true;
			this.DrawMode = DrawMode.OwnerDrawVariable;
			this.SelectionMode = SelectionMode.MultiExtended;			
			base.Sorted = false;
		}


		private void InitializeComponent()
		{

		}

		#endregion

		#region Methods

		public void Load(Script script)
		{
			this.BeginUpdate();

			// clear old items
			this.Items.Clear();

			// add begin step
			this.Items.Add(BeginStep);

			// add end step
			this.Items.Add(EndStep);
			
			int numSteps = script.ProcessSteps.Count;
			for (int i=0; i<numSteps; i++) 
			{
				ProcessStep step = script.ProcessSteps[i];
				ProcessStepInfo stepInfo = ProcessStepManager.GetRegistedProcessStep(step.ID);
				if (stepInfo == null)
				{
					ProcessStepInfo[] stepInfos = ProcessStepManager.GetRegistedProcessSteps();
					foreach(ProcessStepInfo si in stepInfos)
					{
						if (si.IsBuiltIn && si.Name == step.Name)
						{
							stepInfo = si;
							break;
						}
					}
				}
					

				ListBoxItem item = new ListBoxItem(stepInfo, step);
				this.Items.Insert(this.Items.Count-1, item);
			}

			
			this.EndUpdate();		
	
			// refresh internal items
			this.Refresh();
		}

		#endregion

		#region Override Routines

		protected override void OnMeasureItem(MeasureItemEventArgs e)
		{
			base.OnMeasureItem (e);

			// begin process step
			if (e.Index == 0) 
			{
				e.ItemWidth = 25;
				e.ItemHeight = 25;
			}
			else if (e.Index == this.Items.Count-1) // end process step
			{
				e.ItemWidth = 25;
				e.ItemHeight = 25;
			}
			else // process step
			{
				e.ItemWidth = itemWidth;
				e.ItemHeight = itemHeight;
			}

			e.ItemWidth = e.ItemWidth + itemMargins[0] + itemMargins[2];
			e.ItemHeight = e.ItemHeight + itemMargins[1] + itemMargins[3];
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaintBackground(e);
			
			using (Region iRegion = new Region(e.ClipRectangle))
			{
				e.Graphics.FillRegion(new SolidBrush(this.BackColor), iRegion);  
				if (this.Items.Count > 0)  
				{  
					for (int i = 0; i < this.Items.Count; ++i)  
					{  
						System.Drawing.Rectangle irect = this.GetItemRectangle(i);  
						if (e.ClipRectangle.IntersectsWith(irect))  
						{  
							if ((this.SelectionMode == SelectionMode.One && this.SelectedIndex == i)  
								|| (this.SelectionMode == SelectionMode.MultiSimple && this.SelectedIndices.Contains(i))  
								|| (this.SelectionMode == SelectionMode.MultiExtended && this.SelectedIndices.Contains(i)))  
							{  
								DrawItemEventArgs args = new DrawItemEventArgs(e.Graphics, this.Font, irect, i, 
									DrawItemState.Selected, this.ForeColor, this.BackColor);
								this.OnDrawItem(args);
							}  
							else  
							{  
								DrawItemEventArgs args = new DrawItemEventArgs(e.Graphics, this.Font, irect, i, 
									DrawItemState.Default, this.ForeColor, this.BackColor);
								this.OnDrawItem(args); 
							}  
							iRegion.Complement(irect);  
						}  
					}  
				}  
				base.OnPaint(e);  
			}
		}  

		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
			//base.OnPaintBackground (pevent);
		}

		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			base.OnDrawItem (e);

			// draw process step include begin and end steps
			this.DrawProcessStep(e);
		}

        #endregion

		#region Internal Helpers

        protected RectangleF textRect = RectangleF.Empty;
		protected virtual void DrawProcessStep(DrawItemEventArgs e)
		{
			ListBoxItem item = this.Items[e.Index] as ListBoxItem;
			if (item == null)
				return;

			ProcessStepInfo stepInfo = item.StepInfo;
			
			Graphics graph = e.Graphics;
			SmoothingMode smoothingMode = graph.SmoothingMode;
			graph.SmoothingMode = SmoothingMode.HighQuality;
			graph.InterpolationMode = InterpolationMode.Bicubic;

			Rectangle boundRect = e.Bounds;
			MeasureItemEventArgs args = new MeasureItemEventArgs(e.Graphics, e.Index);
			this.OnMeasureItem(args);
            			
			float width = args.ItemWidth - itemMargins[0] - itemMargins[2];
			float height = args.ItemHeight - itemMargins[1] - itemMargins[3];
			float x = boundRect.X + (boundRect.Width - width)*0.5F;
			float y = boundRect.Y + (boundRect.Height - height)*0.5F;
			float radius = 0.2F * Math.Min(width, height);

			RectangleF itemRect = new RectangleF(x, y, width, height);

			// draw process bounding
			if (e.Index == 0) // begin step
			{
				// draw begin step
				using (Brush br = new LinearGradientBrush(itemRect, Color.DarkGreen, Color.LightGreen, 135.0F))
					graph.FillEllipse(br, itemRect);
				graph.DrawEllipse(Pens.Black, itemRect);
			}
			else if (e.Index == this.Items.Count-1)
			{
				// draw begin step
				using (Brush br = new LinearGradientBrush(itemRect, Color.DarkRed, Color.Red, 135.0F))
					graph.FillEllipse(br, itemRect);
				graph.DrawEllipse(Pens.Black, itemRect);
			}
			else
			{
				// Step 1: draw process step background and border
				bool selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
				this.DrawProcessStepBackground(graph, itemRect, radius, selected);

				// Step 2: draw process step icon
				Image image = null;
				if (stepInfo.IsBuiltIn)
					image = Resource.GetProcessStepIcon(stepInfo.Type.Name);
				else 
					image = Resource.GetProcessStepIcon(stepInfo.ID);
				
				if (image == null)
					image = Resource.CustomProcessNodeIcon;
					
				int imageWidth = image.Width;
				int imageHeight = image.Height;

				//x = itemRect.X + (itemRect.Width - imageWidth)*0.5F;
                x = itemRect.X + 4 * itemPadding;
				y = itemRect.Y + 4*itemPadding;
                RectangleF imgDstRect = new RectangleF(x, y, 24, 24);
                RectangleF imgSrcRect = new RectangleF(0, 0, image.Width, image.Height);
				graph.DrawImage(image, x, y);

				// Step 3: draw process step settings state		
				RectangleF textRect = RectangleF.FromLTRB(itemRect.X + radius, 
					y + imageHeight + itemPadding, itemRect.Right - radius, itemRect.Bottom - radius);

                float xPos = itemRect.Width - 4 * itemPadding;
                float yPos = y;
				if (item.ProcessStep != null)
				{
					Image settingImage = item.ProcessStep.HasSettings ? Resource.SettingsImage : Resource.NoSettingsImage;
					xPos = (int)(itemRect.Right - settingImage.Width - itemPadding);
					//int yPos = (int)(y + image.Height + itemPadding + (textRect.Height - settingImage.Height)*0.5F);
                    yPos = (int)(itemRect.Y + 4*itemPadding);
					
                    //// update text rectangle
                    //textRect.Width = xPos - itemPadding - textRect.Left;

					graph.DrawImage(settingImage, xPos, yPos, settingImage.Width, settingImage.Height);
					                             
                    //graph.DrawRectangle(Pens.Black, xPos, yPos, settingImage.Width, settingImage.Height);
				}



                float textLeft = imgDstRect.Right + 4 * itemPadding;
                float textTop = yPos;
                float textWidth = xPos - 4 * itemPadding - textLeft;
                float textHeight = 24;

                textRect = new RectangleF(textLeft, textTop, textWidth, textHeight);

                if (drawText)
                {                   
                    // Step 3: draw process step display name
                    //graph.DrawRectangle(Pens.Black, textRect.X, textRect.Y, textRect.Width, textRect.Height);

                    StringFormat format = new StringFormat(StringFormat.GenericTypographic);
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;
                    format.Trimming = StringTrimming.EllipsisCharacter;

                    string str = this.Items[e.Index].ToString();
                    using (Brush textBrush = new SolidBrush(ControlPaint.Dark(Color.DarkGray)))
                        graph.DrawString(str, nodeFont, textBrush, textRect, format);
                }

				// Step X: draw process step focus border			
				if ((e.State & DrawItemState.Focus) == DrawItemState.Focus)
				{
					RectangleF frameRect = itemRect;
					//frameRect.Inflate(-2, -2);

					using (GraphicsPath path = this.GetRoundRectPath(frameRect, radius))
					{
						using (Pen pen = new Pen(Color.DarkGray))
						{
							pen.Color = ControlPaint.Dark(Color.DarkGray);
							pen.DashStyle = DashStyle.DashDot;
							graph.DrawPath(pen, path);
						}
					}
				}
			}

			// draw input link
			if (e.Index > 0) 
				this.DrawLink(graph, boundRect, itemRect, true);

			// draw output link
			if (e.Index < this.Items.Count - 1)
				this.DrawLink(graph, boundRect, itemRect, false);

			graph.SmoothingMode = smoothingMode;
		}

		protected virtual void DrawProcessStepBackground(Graphics graph, RectangleF itemRect, float radius, bool selected)
		{
			using (GraphicsPath path = this.GetRoundRectPath(itemRect, radius))
			{
				float penWidth = 1.5F;
				Color penColor = nodeColor1;
				Color clr1 = nodeColor1, clr2 = nodeColor2;
				if (selected)
				{
					clr1 = selNodeColor1;
					clr2 = selNodeColor2;
					penColor = Color.FromArgb(0xFF, 140, 199, 179);
				}

				using (Brush br = this.CreateProcessStepFillBrush(itemRect, clr1, clr2))
					graph.FillPath(br, path);
					
				using (Pen pen = new Pen(penColor, penWidth))
					graph.DrawPath(pen, path);
			}
		}

		protected virtual void DrawLink(Graphics graph, RectangleF boundRect, RectangleF itemRect, bool isInputLink)
		{
			if (isInputLink)
			{
				PointF pt1 = new PointF(itemRect.X + itemRect.Width*0.5F, boundRect.Y);
				PointF pt2 = new PointF(itemRect.X + itemRect.Width*0.5F, itemRect.Y);
				graph.DrawLine(inputLinkPen, pt1, pt2);
			}
			else
			{
				PointF pt1 = new PointF(itemRect.X + itemRect.Width*0.5F, itemRect.Bottom);
				PointF pt2 = new PointF(itemRect.X + itemRect.Width*0.5F, boundRect.Bottom);
				graph.DrawLine(outputLinkPen, pt1, pt2);
			}
		}

		protected Brush CreateProcessStepFillBrush(RectangleF itemRect, Color clr1, Color clr2)
		{
			float angle = 90.0F;
			LinearGradientBrush brush = new LinearGradientBrush(itemRect, clr1, clr2, angle, true);
			Blend blend = new Blend(3);
			blend.Positions = new float[]{0, 0.5F, 1.0F};
			blend.Factors = new float[]{0, 1, 0};
			brush.Blend = blend;
			brush.SetSigmaBellShape(0.5F, 1.0F);
			return brush;	
		}

		protected GraphicsPath GetRoundRectPath(RectangleF rect, float radius)
		{
			return GetRoundRectPath(rect.X, rect.Y, rect.Width, rect.Height, radius);
		}
	
		protected GraphicsPath GetRoundRectPath(float x, float y, float width, float height, float radius)
		{
			GraphicsPath path = new GraphicsPath();
			path.AddLine(x + radius, y, x + width - (radius*2), y);
			path.AddArc(x + width - (radius*2), y, radius*2, radius*2, 270, 90);
			path.AddLine(x + width, y + radius, x + width, y + height - (radius*2));
			path.AddArc(x + width - (radius*2), y + height - (radius*2), radius*2, radius*2,0,90);
			path.AddLine(x + width - (radius*2), y + height, x + radius, y + height);
			path.AddArc(x, y + height - (radius*2), radius*2, radius*2, 90, 90);
			path.AddLine(x, y + height - (radius*2), x, y + radius);
			path.AddArc(x, y, radius*2, radius*2, 180, 90);
			path.CloseFigure();

			return path;
		}


		#endregion

		
		[Serializable]
		internal class ListBoxItem : ISerializable
		{	
			public ProcessStepInfo StepInfo;
			
			[NonSerialized]
			public ProcessStep ProcessStep;

            [NonSerialized]
            public DateTime StartTime = DateTime.MinValue;

            [NonSerialized]
            public DateTime EndTime = DateTime.MinValue;

            public TimeSpan Duration
            {
                get
                {
                    return EndTime - StartTime;
                }
            }

			internal ListBoxItem()
			{
				StepInfo = null;
				ProcessStep = null;
			}

			public ListBoxItem(ProcessStepInfo stepInfo, ProcessStep step)
			{
				StepInfo = stepInfo;
				ProcessStep = step;
			}

			public ListBoxItem(SerializationInfo info, StreamingContext context)
			{
				this.StepInfo = info.GetValue("StepInfo", typeof(ProcessStepInfo)) as ProcessStepInfo;
				Type processStepType = info.GetValue("ProcessStepType", typeof(Type)) as Type;
				string xmlSettings = info.GetString("ProcessStepSettings");
				byte[] buffer = Encoding.UTF8.GetBytes(xmlSettings);
				using (MemoryStream stream = new MemoryStream(buffer))
				{
					XmlSerializerEx serializer = new XmlSerializerEx(processStepType);
					this.ProcessStep = serializer.Deserialize(stream) as ProcessStep;
				}
			}

			#region ISerializable Members

			public void GetObjectData(SerializationInfo info, StreamingContext context)
			{
				info.AddValue("StepInfo", this.StepInfo, typeof(ProcessStepInfo));
				info.AddValue("ProcessStepType", this.ProcessStep.GetType(), typeof(Type));

				using (MemoryStream stream = new MemoryStream())
				{
					ProcessStep.Serialize(stream);
					byte[] buffer = stream.GetBuffer();
					String xmlSettings = Encoding.UTF8.GetString(buffer);
					info.AddValue("ProcessStepSettings", xmlSettings);
				}
			}

			#endregion

			public override string ToString()
			{
				return StepInfo != null ? StepInfo.DisplayName : string.Empty;
			}
			
		};
	}
}

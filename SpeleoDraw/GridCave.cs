using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Corel.Interop.VGCore;
using System.Diagnostics;

namespace br.corp.bonus630.VSTA.SpeleoDraw
{
    public class GridCave
    {
        Cave cave;

        private double ySPosition=0;
        private double xSPosition=0;
        private double xPosition = 0;
        private double yPosition = 0;
        private double bigExtension = 0;
        private Rect gridArea = new Rect(0,0,0,0);

        public double BigExtension { get { return this.bigExtension; } }
        public GridCave(Cave cave)
        {
            this.cave = cave;
        }

        public Rect DrawGrid(double startX, double StartY)
        {
            xSPosition = startX;
            ySPosition = StartY;
            return DrawGrid();
        }
        
        public Rect DrawGrid()
        {
            foreach(BaseTopo baseTopo in this.cave.BaseList)
            {
                Color blue = new Color();
                blue.CMYKAssign(100, 0, 0, 0);
                if (cave.ExistReference(baseTopo) && baseTopo.IsStart)
                {

                    foreach (BaseTopo baseTopo1 in this.cave.BaseList)
                    {
                        double xPosition, yPosition;
                        if (baseTopo1.RefBase == baseTopo)
                        {
                            yPosition = (Math.Cos((-90 + baseTopo1.AngleCorrection) * Math.PI / 180)) * (baseTopo.LeftSide * (1000) / Scale.scale) + baseTopo.YPosition;
                            xPosition = (Math.Sin((-90 + baseTopo1.AngleCorrection) * Math.PI / 180)) * (baseTopo.LeftSide * (1000) / Scale.scale) + baseTopo.XPosition;


                            if (xPosition > this.gridArea.Rigth)
                                this.gridArea.Rigth = xPosition;
                            if (xPosition < this.gridArea.Left)
                                this.gridArea.Left = xPosition;
                            if (yPosition > this.gridArea.Bottom)
                                this.gridArea.Bottom = yPosition;
                            if (yPosition < this.gridArea.Top)
                                this.gridArea.Top = yPosition;

                            Shape line = DockerUI.corelApp.ActiveLayer.CreateLineSegment(baseTopo.XPosition, baseTopo.YPosition, xPosition, yPosition);
                            line.Outline.SetProperties(0.05, null, blue);

                            xPosition = (Math.Sin((90 + baseTopo1.AngleCorrection) * Math.PI / 180)) * (baseTopo.RightSide * (1000) / Scale.scale) + baseTopo.XPosition;
                            yPosition = (Math.Cos((90 + baseTopo1.AngleCorrection) * Math.PI / 180)) * (baseTopo.RightSide * (1000) / Scale.scale) + baseTopo.YPosition;
                            line = DockerUI.corelApp.ActiveLayer.CreateLineSegment(baseTopo.XPosition, baseTopo.YPosition, xPosition, yPosition);
                            line.Outline.SetProperties(0.05, null, blue);
                            if (xPosition > this.gridArea.Rigth)
                                this.gridArea.Rigth = xPosition;
                            if (xPosition < this.gridArea.Left)
                                this.gridArea.Left = xPosition;
                            if (yPosition > this.gridArea.Bottom)
                                this.gridArea.Bottom = yPosition;
                            if (yPosition < this.gridArea.Top)
                                this.gridArea.Top = yPosition;
                            

                        }
                    }
                }
                else
                {
                    BaseTopo auxBaseTopo = baseTopo;
                    double t = 0.0;
                    while(!auxBaseTopo.IsStart)
                    {
                        t += auxBaseTopo.Distance;
                        auxBaseTopo = auxBaseTopo.RefBase;
                    }
                    if (t > bigExtension)
                        bigExtension = t;
                    yPosition = (Math.Cos((-90 + baseTopo.AngleCorrection) * Math.PI / 180)) * (baseTopo.LeftSide * (1000) / Scale.scale) + baseTopo.YPosition;
                    xPosition = (Math.Sin((-90 + baseTopo.AngleCorrection) * Math.PI / 180)) * (baseTopo.LeftSide * (1000) / Scale.scale) + baseTopo.XPosition;
                    Shape line = DockerUI.corelApp.ActiveLayer.CreateLineSegment(baseTopo.XPosition, baseTopo.YPosition, xPosition, yPosition);
                    line.Outline.SetProperties(0.05, null, blue);
                    if (xPosition > this.gridArea.Rigth)
                        this.gridArea.Rigth = xPosition;
                    if (xPosition < this.gridArea.Left)
                        this.gridArea.Left = xPosition;
                    if (yPosition > this.gridArea.Bottom)
                        this.gridArea.Bottom = yPosition;
                    if (yPosition < this.gridArea.Top)
                        this.gridArea.Top = yPosition;
                   
                    yPosition = (Math.Cos((90 + baseTopo.AngleCorrection) * Math.PI / 180)) * (baseTopo.RightSide * (1000) / Scale.scale) + baseTopo.YPosition;
                    xPosition = (Math.Sin((90 + baseTopo.AngleCorrection) * Math.PI / 180)) * (baseTopo.RightSide * (1000) / Scale.scale) + baseTopo.XPosition;
                    line = DockerUI.corelApp.ActiveLayer.CreateLineSegment(baseTopo.XPosition, baseTopo.YPosition, xPosition, yPosition);
                    line.Outline.SetProperties(0.05, null, blue);
                  

                    if (xPosition > this.gridArea.Rigth)
                        this.gridArea.Rigth = xPosition;
                    if (xPosition < this.gridArea.Left)
                        this.gridArea.Left = xPosition;
                    if (yPosition > this.gridArea.Bottom)
                        this.gridArea.Bottom = yPosition;
                    if (yPosition < this.gridArea.Top)
                        this.gridArea.Top = yPosition;

                }
                  
            }
          
          // ShapeRange sr = DockerUI.corelApp.ActiveLayer.Shapes.All();
          // foreach (Shape s in sr)
          // {
          //     s.AddToSelection();
          // }
          //  Debug.WriteLine("Shapes no Grid:{0}", DockerUI.corelApp.ActiveLayer.Shapes.All().Count);
          // // double width, heigth,pageWidth,pageHeigth,positionX,positionY;
          // // DockerUI.corelApp.ActiveSelection.GetSize(out width, out heigth);
          //  // DockerUI.corelApp.ActiveSelection.GetPosition(out positionX, out positionY);
          ////  DockerUI.corelApp.ActivePage.GetSize(out pageWidth, out pageHeigth);
          //  //DockerUI.corelApp.ActiveSelection.SetPosition(0, pageHeigth);
          // // return new Ret(positionX, positionY, width, heigth);
            
          //  Rect rect = DockerUI.corelApp.ActiveSelection.BoundingBox;
          //  Debug.WriteLine("Shapes na seleçao:{0}", DockerUI.corelApp.ActiveSelection.Shapes.All().Count);
          //  DockerUI.corelApp.ActiveLayer.CreateRectangle2(rect.x,rect.y,rect.Width,rect.Height);
            return this.gridArea;
        }
        public void UpdateBigExtension()
        {
            foreach (BaseTopo baseTopo in this.cave.BaseList)
            {
                Color blue = new Color();
                blue.CMYKAssign(100, 0, 0, 0);
                if (cave.ExistReference(baseTopo))
                {

                    foreach (BaseTopo baseTopo1 in this.cave.BaseList)
                    {


                        BaseTopo auxBaseTopo = baseTopo;
                        double t = 0.0;
                        while (!auxBaseTopo.IsStart)
                        {
                            t += auxBaseTopo.Distance;
                            auxBaseTopo = auxBaseTopo.RefBase;
                        }
                        if (t > bigExtension)
                            bigExtension = t;
                    }
                }
            }
             

                
        }

    }
}

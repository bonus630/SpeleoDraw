using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Corel.Interop.VGCore;
using System.Runtime.Serialization;
using System.Diagnostics;


namespace br.corp.bonus630.VSTA.SpeleoDraw
{
    [Serializable]
    public class BaseTopo 
    {
        private double  incrination, distance,  leftSide, rightSide, topSide, bottomSide;
        [NonSerialized]
        private double xPosition, yPosition,zPosition, distanceScale,distance2D;
       
        private string baseName; 
        private string comments = "";
        private bool isStart = true;
        private int azymuti;
        private BaseTopo refBase;
        private BaseTopo closePolygonBase = null;
        private int angleCorrection = 0;
        public BaseTopo()
        {
            
        }
        public bool IsStart { get { return isStart; } set { isStart = value;  } }

        public double XPosition { get { return this.xPosition; } }
        public double YPosition { get { return this.yPosition; } }
        public double Distance2D { get { return this.distance2D; } }
        public double ZPosition { get { return this.zPosition; } }
        public string BaseName{ get { return baseName; }set { baseName = value; }}
        public int Azymuti{get { return azymuti; }set { azymuti = value; }}
        public double LeftSide{get { return leftSide; } set { leftSide = value; }}
        public double RightSide{get { return rightSide; } set { rightSide = value; }}
        public double TopSide { get { return topSide; } set { topSide = value; } }
        public double BottomSide{get { return bottomSide; }set { bottomSide = value; }}
        public double Distance {get { return distance; }set { 
                distance = value;
                //Converte metros para milimetros
                
            }
        }
      
        public double Incrination{get { return incrination; }set { incrination = value; }}
        public override string ToString()
        {
            return this.baseName;
        }
        public string Comments{get { return comments; } set { comments = value; }}
        public BaseTopo RefBase{get { return refBase; } set { refBase = value; }}
        public int AngleCorrection { get { return this.angleCorrection; } set { this.angleCorrection = this.azymuti - value; } }

        private void setDistance2D()
        {
            if (this.isStart)
                this.distance2D = 0;
            else
            {
                double inc;
                if (this.incrination < 0)
                    inc = this.incrination * -1;
                else
                    inc = this.incrination;
                this.distance2D = Math.Cos(inc * Math.PI / 180) * this.distanceScale;
                setZPosition();
            }
        }
       private void setXPosition()
        {
           double xprev;
           if (isStart)
           {
               xprev = 0;
              
           }
           else
           {
               xprev = this.refBase.xPosition;
               //Primeiro preciso calcular a distância 2d
               //Vou remover os 180 graus do calculo 180 - this.azymuty
               //this.xPosition = (Math.Sin((this.angleCorrection) * Math.PI / 180)) * this.distanceScale + xprev;
               //Calcular posição relativa a inclinação
               //cateto adjacento = cos * hipotenusa
               //this.xPosition = (Math.Sin((this.angleCorrection) * Math.PI / 180)) * (Math.Cos(inc) * Math.PI / 180 * this.distanceScale) + xprev;
               this.xPosition = (Math.Sin((this.angleCorrection) * Math.PI / 180)) * this.distance2D + xprev;
           }
        }
       private void setYPosition()
       {
           double yprev;
           if (isStart)
               yprev = 0;
           else
           {
               
               yprev = this.refBase.yPosition;
             
              // this.yPosition = (Math.Cos((this.angleCorrection) * Math.PI / 180)) * this.distanceScale + yprev;
              // this.yPosition = (Math.Cos((this.angleCorrection) * Math.PI / 180)) * (Math.Cos(inc) * Math.PI / 180 * this.distanceScale) + yprev;
               this.yPosition = (Math.Cos((this.angleCorrection) * Math.PI / 180)) * this.distance2D + yprev;
           }
       }
        private void setZPosition()
       {
          // double zprev;

           this.zPosition = Math.Sin(this.incrination * Math.PI / 180) * this.distance2D;
           
       }
       
        public void SetPositions()
        {
            this.distanceScale = this.distance * (1000) / Scale.scale;
            setDistance2D();
            setYPosition();
            setXPosition();

            Debug.WriteLine("SetPosition {0} - {1} - {2} - {3}", this.distanceScale, this.angleCorrection, this.xPosition, this.yPosition);
        }
        public void DrawBase()
        {
            DockerUI.corelApp.ActiveDocument.Unit = cdrUnit.cdrMillimeter;
            //Aqui vamos testar --Inicio--
            //this.distanceScale = this.distance * (1000) / Scale.scale;
            //setDistance2D();
            //setYPosition();
            //setXPosition();
            //Aqui vamos testar --Fim--
            Debug.WriteLine("Draw {0} - {1} - {2} - {3}", this.distanceScale, this.angleCorrection, this.xPosition, this.yPosition);

            Corel.Interop.VGCore.Color color = new Corel.Interop.VGCore.Color();
            color.CMYKAssign(0,100,100,0);
            Layer layer = DockerUI.corelApp.ActiveDocument.ActivePage.ActiveLayer;
            Shape triangle = layer.CreatePolygon2(this.xPosition, this.yPosition, 2.2, 3, 60.0);
            
           // triangle.Outline.PenWidth = 0.05;
            triangle.Outline.SetProperties(0.05);
            Shape ellipse = layer.CreateEllipse2(this.xPosition ,this.yPosition, 0.85);
            ellipse.Fill.ApplyUniformFill(color);
            ellipse.Outline.SetProperties(0.05);
            Corel.Interop.VGCore.Shape texto = DockerUI.corelApp.ActiveLayer.CreateArtisticText(this.xPosition+0.41, this.yPosition-0.24, baseName,
                                                                                           Corel.Interop.VGCore.cdrTextLanguage.cdrBrazilianPortuguese,
                                                                                           Corel.Interop.VGCore.cdrTextCharSet.cdrCharSetDefault, "Arial", 14.0f,
                                                                                           Corel.Interop.VGCore.cdrTriState.cdrFalse, Corel.Interop.VGCore.cdrTriState.cdrFalse,
                                                                                           Corel.Interop.VGCore.cdrFontLine.cdrNoFontLine, Corel.Interop.VGCore.cdrAlignment.cdrNoAlignment);
            triangle.AddToSelection();
            ellipse.AddToSelection();
            texto.AddToSelection();
            Shape group = DockerUI.corelApp.ActiveSelection.Group();
            group.Name = String.Format("Base {0}",this.baseName);
            //DataField dat = new DataField();
           // data
            //DataItem dat = new DataItem();
            //dat.DataField.DataType = cdrDataType.cdrDataTypeString;
           // dat.DataField.Name = "BaseName";
           // dat.DataType = cdrDataType.cdrDataTypeString;
          // dat.ElementName = "BaseName";
          //  group.ObjectData.Add(dat, this.baseName);

            //DataItems dats = group.ObjectData;
            //DataField d = new DataField();
            //d.ElementName = "T";
            //dats.Add(d, "c");
            //try
            //{
            //    DataItem distancia = new DataItem();
            //    distancia.DataField.Name = "Distancia";
            //   distancia.DataField.DataType = cdrDataType.cdrDataTypeString;
            //   distancia.Value = "20";
            //   dats.Add(distancia.DataField);
            //}
            //catch (Exception erro)
            //{
            //    Console.WriteLine(erro.Message);
            //}
            


                
        }
        public Rect DrawBasePerfil(double startX,double startY)
        {
            string text = "Perfil Base";
            if (this.isStart)
                text = "Pefil Entrada";
            DockerUI.corelApp.ActivePage.CreateLayer(String.Format("{0} {1}",text, this.baseName));
            double l = this.leftSide * 1000 / Scale.scale;
            double r = this.rightSide * 1000 / Scale.scale;
            double t = this.topSide * 1000 / Scale.scale;
            double b = this.bottomSide * 1000 / Scale.scale;
            startX = startX + l;
            Corel.Interop.VGCore.Shape texto = DockerUI.corelApp.ActiveLayer.CreateArtisticText(startX - l, startY + t + 2, String.Format("{0} {1}",text,this.baseName),
                                                                                          Corel.Interop.VGCore.cdrTextLanguage.cdrBrazilianPortuguese,
                                                                                          Corel.Interop.VGCore.cdrTextCharSet.cdrCharSetDefault, "Arial", 14.0f,
                                                                                          Corel.Interop.VGCore.cdrTriState.cdrFalse, Corel.Interop.VGCore.cdrTriState.cdrFalse,
                      Corel.Interop.VGCore.cdrFontLine.cdrNoFontLine, Corel.Interop.VGCore.cdrAlignment.cdrNoAlignment);
            Color blue = new Color();
            blue.CMYKAssign(100, 0, 0, 0);
            Shape top = DockerUI.corelApp.ActiveLayer.CreateLineSegment(startX, startY, startX, startY+t);
            top.Name = "Teto";
            top.Outline.SetProperties(0.05, null, blue);
            Shape bottom = DockerUI.corelApp.ActiveLayer.CreateLineSegment(startX, startY, startX,startY -b);
            bottom.Name = "Chão";
            bottom.Outline.SetProperties(0.05, null, blue);
            Shape right = DockerUI.corelApp.ActiveLayer.CreateLineSegment(startX, startY, startX+r, startY);
            right.Name = "Direito";
            right.Outline.SetProperties(0.05, null, blue);
            Shape left = DockerUI.corelApp.ActiveLayer.CreateLineSegment(startX, startY, startX-l, startY);
            left.Name = "Esquerdo";
            left.Outline.SetProperties(0.05, null, blue);
         //   top.AddToSelection();
           // bottom.AddToSelection();
          //  right.AddToSelection();
          //  left.AddToSelection();
          //  texto.AddToSelection();
            //double x, y, w, h;
            //DockerUI.corelApp.ActiveSelection.GetPosition(out x,out y);
            //DockerUI.corelApp.ActiveSelection.GetSize(out w,out h);
            Rect ret = new Rect(startX -l, startY+t, r + startX,  startY -b);
            
            return ret;     
        }

    }
}

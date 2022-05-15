using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Corel.Interop.VGCore;
 
namespace br.corp.bonus630.VSTA.SpeleoDraw
{
    [Serializable]
    public class Cave : DocumentManager
    {
     
        private List<BaseTopo> baseList = new List<BaseTopo>();
        [NonSerialized]
        private Document doc;
        [NonSerialized]
        private GridCave grid;
        private string caveName = "";
        private string filePath="";
        public string startBase;
        private DateTime dateTopo;
        public event EventHandler BaseRemove;
        public event EventHandler BaseAdd;
        public event EventHandler BaseModify;
        public event EventHandler BaseAlone;
        public event EventHandler CaveModify;
        public int AngleCorrection { get { return 0; } }

        //public string Estate { get; set; }
        //public string City { get; set; }
        //public string Region { get; set; }
        public DateTime DateTopo { get { return this.dateTopo; } set { this.dateTopo = value; } }
        public List<BaseTopo> BaseList
        {
            get { return baseList; }
            set 
            {
                baseList = value;
                caveModify();
            }
        }
         public string CaveName
        {
            get { return caveName; }
            set 
            {
                caveName = value;
                caveModify();
            }
        }
        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }
        public Cave()
        {
           // doc = topologia.corelApp.ActiveDocument;
            
        }
        public void caveModify()
        {
            if (grid != null)
                grid.UpdateBigExtension();
            if(CaveModify!=null)
                CaveModify(this,null);
        }
        public void AddBase(BaseTopo baseTopo)
        {
            this.baseList.Add(baseTopo);
           
            if (BaseAdd != null)
                BaseAdd(baseTopo, null);
            caveModify();
            if (baseTopo.RefBase == null && !this.ExistReference(baseTopo))
            {
                if(BaseAlone != null)
                BaseAlone(this,null);
            }
           
        }
        public bool ExistReference(BaseTopo baseTopo)
        {
            for (int i = 0; i < this.BaseList.Count; i++)
            {
                if (baseTopo == this.BaseList[i].RefBase)
                    return true;
            }
            return false;
        }
        public void NewCave()
        {
            this.caveName = "";
            this.filePath = "";
            //for(int i =0;i<this.baseList.Count;i++)
            //{
            //    this.RemoveBase(this.baseList[i]);
            //}
            this.baseList.Clear();
            caveModify();
        }
        public void EditBase(BaseTopo baseTopo, int index)
        {

            this.baseList[index] = baseTopo;
            //cb_baseAnt.Items.Remove(this.baseTopo.BaseName);
            // btn_add_Click(null, null);
            //  this.baseTopo = null;
            this.baseList[index] = baseTopo;
            if (BaseModify != null)
                BaseModify(baseTopo, null);
            caveModify();
            
        }
       public void RemoveBase(BaseTopo baseTopo)
       {
           if (this.baseList.Remove(baseTopo))
           {
               if (BaseRemove != null)
                   BaseRemove(baseTopo, null);
               caveModify();
           }
       }
        public BaseTopo GetBaseForName(string name)
        {
            for (int i = 0; i < baseList.Count; i++)
            {
                if (baseList[i].BaseName == name)
                    return baseList[i];
            }
            return null;
        }
        public BaseTopo GetBaseForName(string name,bool layername)
        {
            if(name.Contains("Base"))
                 return this.GetBaseForName(name.Remove(0, 5));
            return null;
        }
        public bool BaseNameCheck(string name)
        {
            for (int i = 0; i < baseList.Count; i++)
            {
                if (baseList[i].BaseName == name)
                    return true;
            }
            return false;
        }
        public void DrawAzymuti()
        {
            this.doc = DockerUI.corelApp.ActiveDocument;
            this.doc.Unit = cdrUnit.cdrMillimeter;
            Layer azymuti;
            if (!DocumentManager.checkLayerExist("Azimuti"))
                azymuti = DockerUI.corelApp.ActivePage.CreateLayer("Azimuti");
            else
            {
                azymuti = DockerUI.corelApp.ActivePage.Layers["Azimuti"];
                DocumentManager.deleteAllShapes(azymuti);
            }
            azymuti.Activate();
            double x = (Math.Sin((360-this.GetBaseForName(this.startBase).Azymuti) * Math.PI / 180)) * 100;
            double y = (Math.Cos((360-this.GetBaseForName(this.startBase).Azymuti) * Math.PI / 180)) * 100;
           Shape line = doc.ActiveLayer.CreateLineSegment(0,0,x,y);
           line.Outline.EndArrow = DockerUI.corelApp.ArrowHeads[2];
           Corel.Interop.VGCore.Shape texto = DockerUI.corelApp.ActiveLayer.CreateArtisticText(x-4, y+4, "NM",
                                                                                        Corel.Interop.VGCore.cdrTextLanguage.cdrBrazilianPortuguese,
                                                                                        Corel.Interop.VGCore.cdrTextCharSet.cdrCharSetDefault, "Arial", 10.0f,
                                                                                        Corel.Interop.VGCore.cdrTriState.cdrFalse, Corel.Interop.VGCore.cdrTriState.cdrFalse,

                    Corel.Interop.VGCore.cdrFontLine.cdrNoFontLine, Corel.Interop.VGCore.cdrAlignment.cdrNoAlignment);
           texto.Rotate(this.GetBaseForName(this.startBase).Azymuti);
          
                   
        }
        public void DrawScale()
        {
            this.DrawScale(0, 0);
        }
        public void DrawScale(double x, double y)
        {
            this.doc = DockerUI.corelApp.ActiveDocument;
            this.doc.Unit = cdrUnit.cdrMillimeter;
            Scale scale = new Scale();
            scale.Size = grid.BigExtension;

            //if (!DocumentManager.checkLayerExist("Bases Topograficas"))
            //    bases = this.doc.ActivePage.CreateLayer("Bases Topograficas");
            //else
            //{
            //    bases = this.doc.ActivePage.Layers["Bases Topograficas"];
            //    DocumentManager.deleteAllShapes(bases);
            //}
            try
            {
                scale.DrawScale(x, y);
            }
            catch(Exception e)
            {
                throw e;
            }

        }
        public void DrawBases()
        {
            this.doc = DockerUI.corelApp.ActiveDocument;
            this.doc.Unit = cdrUnit.cdrMillimeter;
            Layer bases;
            if (!DocumentManager.checkLayerExist("Bases Topograficas"))
                bases = DockerUI.corelApp.ActivePage.CreateLayer("Bases Topograficas");
            else
            {
                bases = DockerUI.corelApp.ActivePage.Layers["Bases Topograficas"];
                DocumentManager.deleteAllShapes(bases);
            }

           // Layer bases = doc.ActivePage.CreateLayer("Bases Topograficas");
            
            bases.Activate();
            for(int i=0;i<baseList.Count;i++)
            {
                baseList[i].AngleCorrection = this.GetBaseForName(this.startBase).Azymuti;
                baseList[i].DrawBase();
            }
           // bases.Editable = false;
        }
        public Rect DrawGrid()
        {
            Color blue = new Color();
            blue.CMYKAssign(100, 0, 0, 0);
             this.doc = DockerUI.corelApp.ActiveDocument;
             this.doc.Unit = cdrUnit.cdrMillimeter;
            Layer layer;
             if (!DocumentManager.checkLayerExist("Grid"))
                 layer = DockerUI.corelApp.ActivePage.CreateLayer("Grid");
             else
             {
                 layer = DockerUI.corelApp.ActivePage.Layers["Grid"];
                 DocumentManager.deleteAllShapes(layer);
             }
            // = doc.ActivePage.CreateLayer("Grid");
            layer.Color = blue;
            layer.Activate();
           
           
            layer.Editable = false;
            layer.Printable = false;
            return grid.DrawGrid();
        }
        public void DrawVisada()
        {
            this.doc = DockerUI.corelApp.ActiveDocument;
            this.doc.Unit = cdrUnit.cdrMillimeter;
            Layer layer;
            if (!DocumentManager.checkLayerExist("Visada"))
                layer = DockerUI.corelApp.ActivePage.CreateLayer("Visada");
            else
            {
                layer = DockerUI.corelApp.ActivePage.Layers["Visada"];
                DocumentManager.deleteAllShapes(layer);
            }
                //= doc.ActivePage.CreateLayer("Visada");
            layer.Activate();
            Visada visada = new Visada();
            Segment[] segments = new Segment[this.baseList.Count-1];
            int i = 0;
            foreach(BaseTopo baseT in baseList)
            {
                
                if(!baseT.IsStart){
                    Segment segment = new Segment(baseT);
                    segments[i] = segment;
                    i++;
                }
            }
            visada.Segments = segments;
            visada.DrawVisada();
            layer.Editable = false;
            layer.Printable = false;
        }
        public void DrawLong( string baseNameStart,string baseNameEnd,string baseOrientation)
        {
            this.doc = DockerUI.corelApp.ActiveDocument;
            this.doc.Unit = cdrUnit.cdrMillimeter;
            Layer layer;
            if (!DocumentManager.checkLayerExist("Perfil Longitudinal"))
                layer = DockerUI.corelApp.ActivePage.CreateLayer("Perfil Longitudinal");
            else
            {
                layer = DockerUI.corelApp.ActivePage.Layers["Perfil Longitudinal"];
                DocumentManager.deleteAllShapes(layer);
            }
          
            layer.Activate();
            try
            {
               
                List<BaseTopo> bases = this.retriveListBasesBetweenTwoBases(baseNameStart, baseNameEnd);
               
                //Vamos criar aqui nossa malha de pontos
                //Muita geometria

                Segment[] segments = new Segment[bases.Count-1];

                for (int i =0;i<bases.Count-1;i++)
                {
                    segments[i] = new Segment(bases[i]);
                    
                }
                Visada visada = new Visada();
                visada.Segments = segments;
                visada.DrawVisada();
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }
        private void Draw3d(int xAngle, int yAngle, int zAngle)
        {

        }
        public double CalculeTotalSize(string baseNameStart, string baseNameEnd)
        {
            try
            {
                double result = 0;
                List<BaseTopo> baseList = this.retriveListBasesBetweenTwoBases(baseNameStart, baseNameEnd);
                foreach (BaseTopo b in baseList)
                {
                    if(baseList.Contains(b.RefBase))
                        result += b.Distance;
                }
                return result;
            }
            catch(Exception erro)
            {
                throw erro;
            }
        }
        public List<BaseTopo> retriveListBasesBetweenTwoBases(string baseNameStart, string baseNameEnd)
        {
            

            BaseTopo firstBase = this.GetBaseForName(baseNameStart);
            BaseTopo lastBase = this.GetBaseForName(baseNameEnd);
            BaseTopo tempBase;
            List<BaseTopo> listBase = new List<BaseTopo>();
            bool searching = true;
            if (firstBase.RefBase != null)
            {
                tempBase = firstBase.RefBase;
                listBase.Add(firstBase);
                while (searching)
                {
                    if (tempBase.RefBase != null)
                    {
                        listBase.Add(tempBase);
                        
                       
                    }
                    else
                        searching = false;
                    if (tempBase == lastBase)
                        {
                            listBase.Add(lastBase);
                            return listBase;
                        }
                    tempBase = tempBase.RefBase;
                }
            }
            if(lastBase.RefBase != null)
            {
                searching = true;
                //listBase.Clear();
                tempBase = lastBase.RefBase;
                listBase.Add(lastBase);
                while (searching)
                {
                    if(listBase.Contains(tempBase))
                    {
                        searching = false;
                        return listBase;
                    }
                    if (tempBase.RefBase != null)
                    {
                        listBase.Add(tempBase);
                        
                       
                    }
                    else
                        searching = false; 
                    if (tempBase == firstBase)
                        {
                            listBase.Add(firstBase);
                            return listBase;
                        }
                    tempBase = tempBase.RefBase;
                }
            }
            else
            {
                listBase.Clear();
                return listBase;
            }
            throw new Exception("Não existe conexão entre essas duas bases");
        }



        internal void Generate3DPoints(string baseName = "")
        {
            if (!string.IsNullOrEmpty(baseName))
            {
                this.startBase = baseName;
            }
            else
            {
                if (this.baseList.Count > 1 && string.IsNullOrEmpty(this.startBase))
                {
                    if (this.baseList[0].RefBase == null)
                    {
                        this.startBase = this.baseList[1].BaseName;
                    }
                    else
                    {
                        this.startBase = this.baseList[0].BaseName;
                    }
                }
                else
                    return;
            }
            int angle = this.GetBaseForName(this.startBase).Azymuti;
            for (int i = 0; i < this.baseList.Count;i++ )
            {
                baseList[i].AngleCorrection = angle;
                baseList[i].SetPositions();
            }
            grid = new GridCave(this);
        }

        internal double CalculeCont()
        {
            double i = 0;
            foreach (BaseTopo baseT in baseList)
            {

                if (!baseT.IsStart)
                {
                    i += baseT.Distance;
                }
            }
            return i;
        }

        internal double CalculeDecli()
        {
            double i = 0;
            foreach (BaseTopo baseT in baseList)
            {

                if (!baseT.IsStart)
                {
                    i += baseT.Distance * Math.Sin(baseT.Incrination);
                }
            }
            return i;
        }
    }
}

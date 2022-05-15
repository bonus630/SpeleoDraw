using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Corel.Interop.VGCore;


namespace br.corp.bonus630.VSTA.SpeleoDraw
{
    public class SpeleoThemesManager
    {
        public Layer Layer { get; set; }
        private int scale = 1;
        public int Scale { set { scale = value; } }


        public void DrawTheme(int index,Point position)
        {
            string path = "";
            ImportFilter iFilter = Layer.ImportEx(path);
            iFilter.Finish();
            double w = 0;
            double h = 0;
            Layer.Application.ActiveShape.GetSize(out w, out h);
            Layer.Application.ActiveShape.SetSize(w * scale, h * scale);
            Layer.Application.ActiveShape.SetPosition(position.x, position.y);
        }
        public void DrawBox(int[] indexes)
        {
            for (int i = 0; i < indexes.Length; i++)
            {
                DrawTheme(indexes[i],null);
            }
        }
    }
}

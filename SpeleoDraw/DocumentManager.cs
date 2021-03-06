using Corel.Interop.VGCore;

namespace br.corp.bonus630.VSTA.SpeleoDraw
{
    public class DocumentManager
    {
        public static bool checkLayerExist(string layerName)
        {
            Layers layers = DockerUI.corelApp.ActiveDocument.ActivePage.Layers;
            foreach (Layer layer in layers)
            {
                if (layer.Name.Equals(layerName))
                    return true;
            }
            return false;
        }

        public static void deleteAllShapes(Layer layer)
        {
            Shapes shapes = layer.Shapes;
            
            foreach (Shape shape in shapes)
            {
                shape.Delete();
            }
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;

namespace Gem.Engine.Primitives.Providers
{
    public class JsonShapeProvider : IShapeProvider
    {
        private readonly GraphicsDevice device;
        private readonly string fileLocation;
        public JsonShapeProvider(GraphicsDevice device, string fileLocation)
        {
            this.device = device;
            this.fileLocation = fileLocation;
        }

        public bool Save(IList<IShape> shapes)
        {
            try
            {
                var json = JsonConvert.SerializeObject(shapes);
                File.WriteAllText(fileLocation, json);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public IList<IShape> Load()
        {
            try
            {
                List<IShape> loadedShapes = new List<IShape>();
                var json = File.ReadAllText(fileLocation);
                var jsonShapes = JArray.Parse(json);

                foreach (JToken shape in jsonShapes)
                {
                    var verticesPosition = new List<Vector2>();
                    var shapePosition = JArray.Parse(shape["VerticesPosition"].ToString());
                    foreach (var position in shapePosition)
                    {
                        verticesPosition.Add(
                            new Vector2(
                                ToFloat(position["X"]),
                                ToFloat(position["Y"])));
                    }
                    var offSetX = ToFloat(shape["ViewportOffsetX"]);
                    var offSetY = ToFloat(shape["ViewportOffsetY"]);
                    var color = new Color(
                        ToFloat(shape["Color"]["B"]),
                        ToFloat(shape["Color"]["G"]),
                        ToFloat(shape["Color"]["R"]),
                        ToFloat(shape["Color"]["A"]));

                    loadedShapes.Add(new FixedBoundsShape(verticesPosition, offSetX, offSetY, color, device));
                }
                return loadedShapes;
            }
            catch
            {
                return new List<IShape>();
            }
        }

        private float ToFloat(JToken token)
        {
            return float.Parse(token.ToString());
        }
    }
}

using System.IO;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Logic;
using LinearAlgebra;
using Geometry;

namespace IO
{
    public static class OpenFile
    {
        public static IDocument FromJSON(string filename)
        {
            FileValidator.CheckFileExists(filename);
            FileValidator.CheckExtension(filename, FileValidator.JSON_EXTENSION);

            string json_string;

            using (StreamReader reader = new StreamReader(filename))
            {
                json_string = reader.ReadToEnd();
            }

            var doc = DocumentFactory.CreateDocument("temp_doc", 1, 1);
            JsonConvert.PopulateObject(json_string, doc);

            var jobj = JObject.Parse(json_string);
            var vis_geom = jobj["VisualGeometries"];

            foreach (var vis_fig in vis_geom)
            {
                var name = vis_fig["Geometry"]["Name"].ToString();
                IVisualGeometry vis;
                switch(name)
                {
                    case "rectangle":
                        vis = JsonFiguresConverter.Rectangle(vis_fig);
                        break;
                    case "triangle":
                        vis = JsonFiguresConverter.Triangle(vis_fig);
                        break;
                    case "ellipse":
                        vis = JsonFiguresConverter.Ellipse(vis_fig);
                        break;
                    case "polyline":
                        vis = JsonFiguresConverter.Polyline(vis_fig);
                        break;
                    default:
                        throw new InvalidDataException("JSON-file isn't valid");
                }

                doc.AddVisualGeometry(vis);
            }

            return doc;
        }

        private static class JsonFiguresConverter
        {
            public static IVisualGeometry Rectangle(JToken rect)
            {
                var transform = rect["Geometry"]["Transform"];
                var rectangle = FigureFactory.CreateRectangle((double)rect["Geometry"]["Width"],
                                                              (double)rect["Geometry"]["Height"],
                                                              new Vector2((double)transform["LocalPosition"]["x"],
                                                                          (double)transform["LocalPosition"]["y"]));
                SetTransform(rectangle, transform);

                var vis_rectangle = VisualGeometryFactory.CreateVisualGeometry(rectangle);

                vis_rectangle.BackgroundBrush = Color(rect["BackgroundBrush"]);
                vis_rectangle.BorderBrush     = Color(rect["BorderBrush"]);
                vis_rectangle.BorderThickness = (double)rect["BorderThickness"];

                return vis_rectangle;

            }

            public static IVisualGeometry Triangle(JToken tri)
            {
                var transform = tri["Geometry"]["Transform"];
                var triangle = FigureFactory.CreateTriangle((double)tri["Geometry"]["Width"],
                                                            (double)tri["Geometry"]["Height"],
                                                            new Vector2((double)transform["LocalPosition"]["x"],
                                                                        (double)transform["LocalPosition"]["y"]));
                SetTransform(triangle, transform);

                var vis_triangle = VisualGeometryFactory.CreateVisualGeometry(triangle);

                vis_triangle.BackgroundBrush = Color(tri["BackgroundBrush"]);
                vis_triangle.BorderBrush     = Color(tri["BorderBrush"]);
                vis_triangle.BorderThickness = (double)tri["BorderThickness"];

                return vis_triangle;
            }

            public static IVisualGeometry Ellipse(JToken ell)
            {
                var transform = ell["Geometry"]["Transform"];
                var ellipse = FigureFactory.CreateEllipse((double)ell["Geometry"]["RadiusX"],
                                                          (double)ell["Geometry"]["RadiusY"],
                                                          new Vector2((double)transform["LocalPosition"]["x"],
                                                                      (double)transform["LocalPosition"]["y"]));
                SetTransform(ellipse, transform);

                var vis_ellipse = VisualGeometryFactory.CreateVisualGeometry(ellipse);

                vis_ellipse.BackgroundBrush = Color(ell["BackgroundBrush"]);
                vis_ellipse.BorderBrush     = Color(ell["BorderBrush"]);
                vis_ellipse.BorderThickness = (double)ell["BorderThickness"];

                return vis_ellipse;
            }

            public static IVisualGeometry Polyline(JToken poly)
            {
                var jsonpoints = poly["Geometry"]["Points"];
                var points = new List<Vector2>();
                foreach (var jsonpoint in jsonpoints)
                    points.Add(new Vector2((double)jsonpoint["x"], (double)jsonpoint["y"]));

                var polyline = FigureFactory.CreatePolyline(points);
                var transform = poly["Geometry"]["Transform"];
                SetTransform(polyline, transform);

                var vis_polyline = VisualGeometryFactory.CreateVisualGeometry(polyline);

                vis_polyline.BackgroundBrush = Color(poly["BackgroundBrush"]);
                vis_polyline.BorderBrush     = Color(poly["BorderBrush"]);
                vis_polyline.BorderThickness = (double)poly["BorderThickness"];

                return vis_polyline;
            }

            public static IBrush Color(JToken brush)
            {
                var color = brush["Color"];
                var brush_res = new Logic.SolidColorBrush((byte)color["r"], (byte)color["g"], (byte)color["b"]);
                brush_res.Opacity = (double)brush["Opacity"];

                return brush_res;
            }

            public static void SetTransform(IFigure fig, JToken transforms)
            {
                fig.Transform.Position = new Vector2((double)transforms["Position"]["x"], (double)transforms["Position"]["y"]);
                fig.Transform.Rotation = (double)transforms["Rotation"];
                fig.Transform.LocalScale = new Vector2((double)transforms["LocalScale"]["x"], (double)transforms["LocalScale"]["y"]);
            }
        }
    }
}

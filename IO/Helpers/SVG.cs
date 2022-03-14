using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

using Svg;
using Svg.Transforms;

using Logic;
using Geometry;
using LinearAlgebra;

namespace IO
{
    public static class SVG
    {
        public static SvgDocument GetSvgDocumentFromDocument(IDocument document)
        {
            SvgDocument svgDoc = ConvertToSvg.Document(document);

            foreach (var element in document.VisualGeometries)
                svgDoc.Children.Add(ConvertToSvg.Element(element, document));

            return svgDoc;
        }

        public static IDocument GetDocumentFromSvgDocument(SvgDocument svgString)
        {
            // План:
            // 1) Создать нужный нам IDocument из модуля Logic (файл DocumentFactory.cs)
            //       var document = DocumentFactory.CreateDocument(name, width, height);
            //      имя наверное нужно взять из имени файла и передать в эту функцию
            //
            // 2) Начать в цикле разбирать SVG файл и получать SVG фигуры
            //
            // 3) Нужно создать наши фигуры, эквивалентные SVG фигурам, которые мы определили.
            //       Фигуры создаются в модуле Geometry. В файле FigureFactory.cs
            //     Пример:
            //    if (element == elipse)
            //    {
            //        var elipse = FigureFactory.CreateEllipse(element.rx, element.ry, new Vector2(element.cx, element.cy));
            //        var visual_elipse = VisualGeometryFactory.CreateVisualGeometry(elipse);
            //        document.AddVisualGeometry(visual_elipse);
            //    }

            throw new NotSupportedException();
        }

        public static string GetSvgStringFromSvgDocument(SvgDocument svgDoc)
        {
            MemoryStream stream = new MemoryStream();
            svgDoc.Write(stream);
            return Encoding.UTF8.GetString(stream.GetBuffer());
        }
    }

    internal static class ConvertToSvg
    {
        public static SvgDocument Document(IDocument document)
        {
            return new SvgDocument
            {
                Width = document.Width,
                Height = document.Height
            };
        }

        public static SvgElement Element(IVisualGeometry element, IDocument parent)
        {
            switch (element.Geometry.Name)
            {
                case "rectangle":
                    return Rectangle(element, parent);
                case "ellipse":
                    return Ellipse(element, parent);
                case "polyline":
                    return Polyline(element, parent);
                case "triangle":
                case "polygon":
                    return Polygon(element, parent);
                default:
                    throw new NotImplementedException(element.Geometry.Name + " is Not supported");
            }
        }

        private static SvgRectangle Rectangle(IVisualGeometry v, IDocument parent)
        {
            var transforms = new SvgTransformCollection();

            var center_x = Convert.ToSingle(v.Geometry.Transform.Position.x);
            var center_y = Convert.ToSingle(v.Geometry.Transform.Position.y);
            var width = Convert.ToSingle(v.Geometry.GetParameters()["width"]);
            var height = Convert.ToSingle(v.Geometry.GetParameters()["height"]);

            ConvertToSvg.Coordinates(ref center_x, ref center_y, parent.Width, parent.Height);

            // В SVG позиция определяется координатами левого верхнего угла
            // а в ItCanDraw центром, поэтому нужно сделать замену
            float x = center_x, y = center_y;
            x -= width / 2;
            y -= height / 2;
            transforms.Add(new SvgTranslate(x, y));

            //float angle = (float)((180 / Math.PI) * v.Geometry.Transform.Rotation);
            //transforms.Add(new SvgRotate(angle, center_x, center_y));

            return new SvgRectangle
            {
                Width = width,
                Height = height,
                Transforms = transforms,
                Fill = SolidBrush(v.BackgroundBrush),
                Stroke = SolidBrush(v.BorderBrush),
                StrokeWidth = Convert.ToSingle(v.BorderThickness)
            };
        }

        private static SvgEllipse Ellipse(IVisualGeometry v, IDocument parent)
        {
            var transforms = new SvgTransformCollection();

            var x = Convert.ToSingle(v.Geometry.Transform.Position.x);
            var y = Convert.ToSingle(v.Geometry.Transform.Position.y);
            var radiusX = Convert.ToSingle(v.Geometry.GetParameters()["radiusX"]);
            var radiusY = Convert.ToSingle(v.Geometry.GetParameters()["radiusY"]);

            ConvertToSvg.Coordinates(ref x, ref y, parent.Width, parent.Height);

            transforms.Add(new SvgTranslate(x, y));

            //float angle = (float)((180 / Math.PI) * v.Geometry.Transform.Rotation);
            //transforms.Add(new SvgRotate(angle, x, y));

            return new SvgEllipse
            {
                RadiusX = radiusX,
                RadiusY = radiusY,
                Transforms = transforms,
                Fill = SolidBrush(v.BackgroundBrush),
                Stroke = SolidBrush(v.BorderBrush),
                StrokeWidth = Convert.ToSingle(v.BorderThickness)
            };
        }

        private static SvgPolygon Polygon(IVisualGeometry v, IDocument parent)
        {
            var transforms = new SvgTransformCollection();

            var pos_x = Convert.ToSingle(v.Geometry.Transform.Position.x);
            var pos_y = Convert.ToSingle(v.Geometry.Transform.Position.y);
            List<Vector2> points = (List<Vector2>)v.Geometry.GetParameters()["points"];

            ConvertToSvg.Coordinates(ref pos_x, ref pos_y, parent.Width, parent.Height);

            //float angle = (float)((180 / Math.PI) * v.Geometry.Transform.Rotation);
            //transforms.Add(new SvgRotate(angle, pos_x, pos_y));

            return new SvgPolygon
            {
                Points = Points(points, pos_x, pos_y),
                Transforms = transforms,
                Fill = SolidBrush(v.BackgroundBrush),
                Stroke = SolidBrush(v.BorderBrush),
                StrokeWidth = Convert.ToSingle(v.BorderThickness)
            };
        }

        private static SvgPolyline Polyline(IVisualGeometry v, IDocument parent)
        {
            var transforms = new SvgTransformCollection();

            var pos_x = Convert.ToSingle(v.Geometry.Transform.Position.x);
            var pos_y = Convert.ToSingle(v.Geometry.Transform.Position.y);
            List<Vector2> points = (List<Vector2>)v.Geometry.GetParameters()["points"];

            ConvertToSvg.Coordinates(ref pos_x, ref pos_y, parent.Width, parent.Height);

            //float angle = (float)((180 / Math.PI) * v.Geometry.Transform.Rotation);
            //transforms.Add(new SvgRotate(angle, pos_x, pos_y));

            return new SvgPolyline
            {
                Points = Points(points, pos_x, pos_y),
                Transforms = transforms,
                Fill = SvgColourServer.None,
                Stroke = SolidBrush(v.BorderBrush),
                StrokeWidth = Convert.ToSingle(v.BorderThickness)
            };
        }

        private static SvgPaintServer SolidBrush(IBrush brush)
        {
            if (!(brush is SolidColorBrush))
                throw new ArgumentException("brush must be SolidColorBrush");

            var logicBrush = (SolidColorBrush)brush;
            var logicColor = logicBrush.Color;
            var stdColor = System.Drawing.Color.FromArgb((int)(logicBrush.Opacity * 255),
                                                         logicColor.r, logicColor.g, logicColor.b);
            return new SvgColourServer(stdColor);
        }

        private static void Coordinates(ref float x, ref float y, float width, float height)
        {
            // В нашей программе стандартная координатная плоскость
            // А в SVG координатная плоскость начинается с 0,0 в левом верхем углу
            // Сделаем преобразование координат с учетом размера холста
            x = x + width / 2;
            y = -y + height / 2;
        }

        private static SvgPointCollection Points(List<Vector2> points, float x, float y)
        {
            var result = new SvgPointCollection();

            foreach (var point in points)
            {
                var svgPoint = new SvgPoint
                {
                    X = Convert.ToSingle( point.x + x),
                    Y = Convert.ToSingle(-point.y + y)
                };

                result.Add(Convert.ToSingle(svgPoint.X));
                result.Add(Convert.ToSingle(svgPoint.Y));
            }

            return result;
        }
    }
}

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

        public static IDocument GetDocumentFromSvgDocument(SvgDocument svgDoc)
        {
            IDocument doc = ConvertFromSvg.Document(svgDoc);

            foreach (var elem in svgDoc.Children)
                doc.AddVisualGeometry(ConvertFromSvg.Element(elem, svgDoc));

            return doc;
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

            var rotate = Convert.ToSingle(v.Geometry.Transform.RotationDegrees);
            transforms.Add(new SvgRotate(rotate, width / 2, height / 2));

            var scale = v.Geometry.Transform.LocalScale;
            float scale_x = Convert.ToSingle(scale.x);
            float scale_y = Convert.ToSingle(scale.y);
            transforms.Add(new SvgScale(scale_x, scale_y));

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

            var rotate = Convert.ToSingle(v.Geometry.Transform.RotationDegrees);
            transforms.Add(new SvgRotate(rotate));

            var scale = v.Geometry.Transform.LocalScale;
            float scale_x = Convert.ToSingle(scale.x);
            float scale_y = Convert.ToSingle(scale.y);
            transforms.Add(new SvgScale(scale_x, scale_y));

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

            transforms.Add(new SvgTranslate(pos_x, pos_y));

            var rotate = Convert.ToSingle(v.Geometry.Transform.RotationDegrees);
            transforms.Add(new SvgRotate(rotate));

            var scale = v.Geometry.Transform.LocalScale;
            float scale_x = Convert.ToSingle(scale.x);
            float scale_y = Convert.ToSingle(scale.y);
            transforms.Add(new SvgScale(scale_x, scale_y));

            return new SvgPolygon
            {
                Points = Points(points),
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

            transforms.Add(new SvgTranslate(pos_x, pos_y));

            var rotate = Convert.ToSingle(v.Geometry.Transform.RotationDegrees);
            transforms.Add(new SvgRotate(rotate));

            var scale = v.Geometry.Transform.LocalScale;
            float scale_x = Convert.ToSingle(scale.x);
            float scale_y = Convert.ToSingle(scale.y);
            transforms.Add(new SvgScale(scale_x, scale_y));

            return new SvgPolyline
            {
                Points = Points(points),
                Transforms = transforms,
                Fill = SvgPaintServer.None,
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

        private static SvgPointCollection Points(List<Vector2> points)
        {
            var result = new SvgPointCollection();

            foreach (var point in points)
            {
                var svgPoint = new SvgPoint
                {
                    X = Convert.ToSingle( point.x),
                    Y = Convert.ToSingle(-point.y)
                };

                result.Add(Convert.ToSingle(svgPoint.X));
                result.Add(Convert.ToSingle(svgPoint.Y));
            }

            return result;
        }
    }

    internal static class ConvertFromSvg
    {
        public static IDocument Document(SvgDocument svgDoc)
        {
            var name = Path.GetFileNameWithoutExtension(svgDoc.BaseUri.AbsolutePath);
            return DocumentFactory.CreateDocument(name, ((int)svgDoc.Width.Value), ((int)svgDoc.Height.Value));
        }

        public static IVisualGeometry Element(SvgElement element, SvgDocument parent)
        {
            switch (element)
            {
                case SvgRectangle rect:
                    return Rectangle(rect, parent);
                case SvgEllipse ellipse:
                    return Ellipse(ellipse, parent);
                case SvgPolyline polyline:
                    return Polyline(polyline, parent);
                case SvgPolygon polygon:
                    return Polygon(polygon, parent);
                default:
                    throw new NotImplementedException(element.ToString() + " is not supported");
            }
        }

        private static IVisualGeometry Rectangle(SvgRectangle rect, SvgDocument parent)
        {
            double x = 0, y = 0;
            double rotate = 0;
            Vector2 scale = new Vector2();

            foreach (var trans in rect.Transforms)
            {
                if (trans is SvgTranslate T)
                {
                    x = T.X;
                    y = T.Y;

                    // В SVG позиция определяется координатами левого верхнего угла
                    // а в ItCanDraw центром, поэтому нужно сделать замену
                    x += rect.Width / 2;
                    y += rect.Height / 2;

                    Coordinates(ref x, ref y, parent.Width, parent.Height);
                }

                if (trans is SvgRotate R)
                    rotate = R.Angle;

                if (trans is SvgScale S)
                    scale = new Vector2(S.X, S.Y);
            }

            var coord = new Vector2(x, y);
            var figure = FigureFactory.CreateRectangle(rect.Width, rect.Height, coord);
            figure.Transform.RotationDegrees = rotate;
            figure.Transform.LocalScale = scale;

            var vis_rect = VisualGeometryFactory.CreateVisualGeometry(figure);
            vis_rect.BackgroundBrush = ConvertFromSvg.SolidBrush(rect.Fill);
            vis_rect.BorderBrush = ConvertFromSvg.SolidBrush(rect.Stroke);
            vis_rect.BorderThickness = rect.StrokeWidth;

            return vis_rect;
        }

        private static IVisualGeometry Ellipse(SvgEllipse ellipse, SvgDocument parent)
        {
            double x = 0, y = 0;
            double rotate = 0;
            Vector2 scale = new Vector2();

            foreach (var trans in ellipse.Transforms)
            {
                if (trans is SvgTranslate T)
                {
                    x = T.X;
                    y = T.Y;

                    Coordinates(ref x, ref y, parent.Width, parent.Height);
                }

                if (trans is SvgRotate R)
                    rotate = R.Angle;

                if (trans is SvgScale S)
                    scale = new Vector2(S.X, S.Y);
            }

            var coord = new Vector2(x, y);
            var figure = FigureFactory.CreateEllipse(ellipse.RadiusX, ellipse.RadiusY, coord);
            figure.Transform.RotationDegrees = rotate;
            figure.Transform.LocalScale = scale;

            var vis_rect = VisualGeometryFactory.CreateVisualGeometry(figure);
            vis_rect.BackgroundBrush = ConvertFromSvg.SolidBrush(ellipse.Fill);
            vis_rect.BorderBrush = ConvertFromSvg.SolidBrush(ellipse.Stroke);
            vis_rect.BorderThickness = ellipse.StrokeWidth;

            return vis_rect;
        }

        private static IVisualGeometry Polygon(SvgPolygon polygon, SvgDocument parent)
        {
            double center_x = 0, center_y = 0;
            double rotate = 0;
            Vector2 scale = new Vector2();

            foreach (var trans in polygon.Transforms)
            {
                if (trans is SvgTranslate T)
                {
                    center_x = T.X;
                    center_y = T.Y;
                }

                if (trans is SvgRotate R)
                    rotate = R.Angle;

                if (trans is SvgScale S)
                    scale = new Vector2(S.X, S.Y);
            }

            List<Vector2> points = Points(polygon.Points);
            for (int i = 0; i < points.Count; i++)
            {
                double x = points[i].x + center_x;
                double y = points[i].y + center_y;
                Coordinates(ref x, ref y, parent.Width, parent.Height);
            }

            // заменить в будущем на полигон
            var figure = FigureFactory.CreatePolyline(points);
            figure.Transform.RotationDegrees = rotate;
            figure.Transform.LocalScale = scale;

            var vis_poly = VisualGeometryFactory.CreateVisualGeometry(figure);
            vis_poly.BackgroundBrush = ConvertFromSvg.SolidBrush(polygon.Fill);
            vis_poly.BorderBrush = ConvertFromSvg.SolidBrush(polygon.Stroke);
            vis_poly.BorderThickness = polygon.StrokeWidth;

            return vis_poly;
        }

        private static IVisualGeometry Polyline(SvgPolyline polyline, SvgDocument parent)
        {
            double center_x = 0, center_y = 0;
            double rotate = 0;
            Vector2 scale = new Vector2();

            foreach (var trans in polyline.Transforms)
            {
                if (trans is SvgTranslate T)
                {
                    center_x = T.X;
                    center_y = T.Y;
                }

                if (trans is SvgRotate R)
                    rotate = R.Angle;

                if (trans is SvgScale S)
                    scale = new Vector2(S.X, S.Y);
            }

            List<Vector2> points = Points(polyline.Points);
            for (int i = 0; i < points.Count; i++)
            {
                double x = points[i].x + center_x;
                double y = points[i].y + center_y;
                Coordinates(ref x, ref y, parent.Width, parent.Height);

                points[i] = new Vector2(x, y);
            }

            var figure = FigureFactory.CreatePolyline(points);
            figure.Transform.RotationDegrees = rotate;
            figure.Transform.LocalScale = scale;

            var vis_poly = VisualGeometryFactory.CreateVisualGeometry(figure);
            vis_poly.BorderBrush = ConvertFromSvg.SolidBrush(polyline.Stroke);
            vis_poly.BorderThickness = polyline.StrokeWidth;

            return vis_poly;
        }

        private static SolidColorBrush SolidBrush(SvgPaintServer color)
        {
            if (!(color is SvgColourServer))
                throw new ArgumentException("color must be SvgColourServer");

            var svgColor = ((SvgColourServer)color).Colour;

            return new SolidColorBrush(svgColor.R, svgColor.G, svgColor.B, svgColor.A);
        }

        private static void Coordinates(ref double x, ref double y, float width, float height)
        {
            // В нашей программе стандартная координатная плоскость
            // А в SVG координатная плоскость начинается с 0,0 в левом верхем углу
            // Сделаем преобразование координат с учетом размера холста
            x = x - width / 2;
            y = -y + height / 2;
        }

        private static List<Vector2> Points(SvgPointCollection points)
        {
            if (points.Count % 2 != 0)
                throw new ArgumentException("points must contains 2x even elements (x and y)");

            var result = new List<Vector2>();

            for (int i = 0; i < points.Count; i += 2)
                result.Add(new Vector2(points[i], points[i + 1]));

            return result;
        }
    }
}

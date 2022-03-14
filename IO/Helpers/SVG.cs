using System;
using System.IO;
using System.Text;

using Svg;
using Svg.Transforms;

using Logic;

namespace IO
{
    public static class SVG
    {
        /// <summary>
        /// Переводит Idocument в SVG-строку
        /// </summary>
        /// <param name="document">документ в формате IDocument</param>
        /// <returns>SvgDoc</returns>
        public static SvgDocument GetSvgDocumentFromDocument(IDocument document)
        {
            // Создадим документ
            SvgDocument svgDoc = GenerateDocument(document);

            // Добавим в него фигуры
            // В цикле перебираем всем объекты из IDocument
            // Разделяем их по группам
            // Накладываем на них трансформации
            // Возвращаем SVG строку

                                                         // исправить
            SvgGroup group = new SvgGroup();             // исправить
            svgDoc.Children.Add(group);                  // исправить
                                                         // исправить
            SvgRectangle asdf = new SvgRectangle {       // исправить
                X = 100,                                 // исправить
                Y = 100,                                 // исправить
                Width = 200,                             // исправить
                Height = 50,                             // исправить
                Fill = new SvgColourServer(System.Drawing.Color.Red),   // исправить
                Transforms = new SvgTransformCollection()// исправить
            };                                           // исправить
                                                         // исправить
            asdf.Transforms.Add(new SvgRotate(45));      // исправить
                                                         // исправить
            group.Children.Add(asdf);                    // исправить
                                                         // исправить
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

            return null;
        }

        /// <summary>
        /// Преобразует SvgDocument в SVG строку
        /// </summary>
        /// <param name="svgDoc"></param>
        /// <returns></returns>
        public static string GetSvgStringFromSvgDocument(SvgDocument svgDoc)
        {
            MemoryStream stream = new MemoryStream();
            svgDoc.Write(stream);
            return Encoding.UTF8.GetString(stream.GetBuffer());
        }

        /// <summary>
        /// Создает экземляр SVG document из IDocument
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        private static SvgDocument GenerateDocument(IDocument document)
        {
            return new SvgDocument
            {
                Width = 500,
                Height = 500,
                ViewBox = new SvgViewBox(-250, -250, 500, 500)
            };
        }
    }
}

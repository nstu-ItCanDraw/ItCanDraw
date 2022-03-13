﻿using System;
using System.Drawing;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Svg;
using Svg.Transforms;

namespace IO
{
    public static class SVG
    {
        /// <summary>
        /// Переводит Idocument в SVG-строку
        /// </summary>
        /// <param name="document">документ в формате IDocument</param>
        /// <returns>SvgDoc</returns>
        public static SvgDocument GetSvgDocumentFromDocument(object document)
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
                Fill = new SvgColourServer(Color.Red),   // исправить
                Transforms = new SvgTransformCollection()// исправить
            };                                           // исправить
                                                         // исправить
            asdf.Transforms.Add(new SvgRotate(45));      // исправить
                                                         // исправить
            group.Children.Add(asdf);                    // исправить
                                                         // исправить
            return svgDoc;
        }

        /// <summary>
        /// Преобразует SvgDocument в SVG строку
        /// </summary>
        /// <param name="svgDoc"></param>
        /// <returns></returns>
        public static string GetStringFromSvgDocument(SvgDocument svgDoc)
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
        private static SvgDocument GenerateDocument(object document)
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
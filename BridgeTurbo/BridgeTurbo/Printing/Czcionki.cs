using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc;
using MigraDoc.Rendering;
using PdfSharp.Pdf;
using System.Diagnostics;
using System.IO;
using MigraDoc.RtfRendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace BridgeTurbo
{
    class Czcionki
    {
        /// <summary>
        /// Cambria, 8pt, niebieska
        /// </summary>
        public static Font font_surnames
        {
            get { return AddFontSurnames(); }
        }
        /// <summary>
        /// Cambria, 10pt, niebieska, podkreślona
        /// </summary>
        public static Font font_header
        {
            get { return AddFontHeader(); }
        }
        /// <summary>
        /// Cambria, 8pt
        /// </summary>
        public static Font font_normal
        {
            get { return AddFontNormal(); }
        }
        /// <summary>
        /// Verdana, 10pt
        /// </summary>
        public static Font font_deep
        {
            get { return AddFontDeepFin(); }
        }
        /// <summary>
        /// Cambria, 8pt, BOLD
        /// </summary>
        public static Font font_normalBold
        {
            get { Font f = AddFontNormal(); f.Bold = true; return f; }
        }
        /// <summary>
        /// Cambria, 8pt, RED
        /// </summary>
        public static Font font_red
        {
            get { return AddFontRed(); }
        }
        /// <summary>
        /// Cambria, 6pt, bold
        /// </summary>
        public static Font font_smallbold
        {
            get { return AddFontBoldSmall(); }
        }
        /// <summary>
        /// Cambria, 6pt
        /// </summary>
        public static Font font_small
        {
            get { return AddFontSmall(); }
        }

        static Font AddFontSurnames()
        {
            Font font_ = new Font();

            font_.Bold = true;
            font_.Name = "Cambria";
            font_.Size = 8;
            font_.Color = new Color(79, 129, 189);

            return font_;
        }

        static Font AddFontHeader()
        {
            Font font_ = new Font();
            font_.Bold = true;

            font_.Color = Colors.Blue;
            font_.Name = "Cambria";
            font_.Size = 10;
            font_.Underline = Underline.Single;

            return font_;
        }

        static Font AddFontNormal()
        {
            Font font_ = new Font();

            font_.Name = "Cambria";
            font_.Size = 8;

            return font_;
        }

        static Font AddFontDeepFin()
        {
            Font font_ = new Font();

            font_.Name = "Verdana";
            font_.Size = 10;

            return font_;
        }

        static Font AddFontRed()
        {
            Font font_ = new Font();

            font_.Bold = true;
            font_.Name = "Cambria";
            font_.Size = 8;
            font_.Color = new Color(192, 80, 72);

            return font_;
        }

        static Font AddFontBoldSmall()
        {
            Font font_ = new Font();

            font_.Name = "Cambria";
            font_.Size = 6;
            font_.Bold = true;

            return font_;
        }
        static Font AddFontSmall()
        {
            Font font_ = new Font();

            font_.Name = "Cambria";
            font_.Size = 6;


            return font_;
        }

    }
}


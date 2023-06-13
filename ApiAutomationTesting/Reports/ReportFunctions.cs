using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using A = DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;
using System.Diagnostics;
using OpenPop.Mime;
using OpenPop.Pop3;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Message = OpenPop.Mime.Message;

namespace APITest
{
    /// <summary>
    /// Clase contains essential functions for printing reports and cleaning up evidence.
    /// </summary>
    public class ReportFunctions
    {
        public ReportFunctions()
        {

        }

        /// <summary>
        /// Create a Screenshot
        /// </summary>
        /// <returns></returns>
        public void Screenshot(string caseName, bool tag, string file)
        {
            Bitmap captureBitmap = new Bitmap(1600, 900, PixelFormat.Format32bppArgb);
            Rectangle captureRectangle = Screen.AllScreens[0].Bounds;
            Graphics captureGraphics = Graphics.FromImage(captureBitmap);
            captureGraphics.CopyFromScreen(captureRectangle.Left, captureRectangle.Top, 0, 0, captureRectangle.Size);
            string UrlImage = @"C:\Reportes\" + caseName + ".bmp";
            captureBitmap.Save(UrlImage, ImageFormat.Bmp);
            InsertAPicture(file, UrlImage, caseName, tag);
        }

        /// <summary>
        /// Create to Word document
        /// </summary>
        /// <returns></returns>
        public string CreteDocumentWordDinamic(string caseName)
        {
            string pat = Directory.GetCurrentDirectory().ToString();
            string path = string.Format(pat + "\\FastTrackReport");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string file = string.Format(@"{0}\FastTrackReport_{1}_{2}.docx", path, caseName, Hour());
            using (WordprocessingDocument wordDocument =
            WordprocessingDocument.Create(file, WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                mainPart.Document = new Document();
                Body body = mainPart.Document.AppendChild(new Body());
                Paragraph para = body.AppendChild(new Paragraph());
                Run run = para.AppendChild(new Run());
                run.AppendChild(new Text("Run test performed: " + caseName));
            }
            return file;
        }

        public string Hour()
        {
            DateTime dateAndTime = DateTime.Now;
            string date = dateAndTime.ToString("ddMMyyyy_HHmmss");
            return date;
        }

        /// <summary>
        /// Convert a word document to pdf
        /// </summary>
        /// <returns></returns>
        public void ConvertWordToPDF(string caseName)
        {
            string[] split = Directory.GetCurrentDirectory().Split('\\');
            string Path = "";
            for (int i = 0; i < (split.Length - 1); i++)
            {
                Path = Path + split[i] + @"\";
            }
            Path = string.Format(Path + @"FilePDF_{0}\", caseName);
            string file = split[split.Length - 1];
            Microsoft.Office.Interop.Word.Application word = new Microsoft.Office.Interop.Word.Application();
            object oMissing = System.Reflection.Missing.Value;
            word.Visible = false;
            word.ScreenUpdating = false;
            Object FileName = (Object)Path;
            Microsoft.Office.Interop.Word.Document doc = word.Documents.Open(ref FileName, ref oMissing,
                            ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                            ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                            ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            doc.Activate();
            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }
            object Filter = file.Replace(".docx", ".pdf");
            object outputFileName = Path + Filter;
            object fileFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF;
            doc.SaveAs(ref outputFileName,
                    ref fileFormat, ref oMissing, ref oMissing,
                    ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                    ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                    ref oMissing, ref oMissing, ref oMissing, ref oMissing);

            object saveChanges = Microsoft.Office.Interop.Word.WdSaveOptions.wdDoNotSaveChanges;
            ((Microsoft.Office.Interop.Word.Document)doc).Close(ref saveChanges, ref oMissing, ref oMissing);
            doc = null;
            ((Microsoft.Office.Interop.Word.Application)word).Quit(ref oMissing, ref oMissing, ref oMissing);
            word = null;
            Process[] processes2 = Process.GetProcessesByName("WINWORD");
            if (processes2.Length > 0)
            {
                for (int i = 0; i < processes2.Length; i++)
                {
                    processes2[i].Kill();
                }
            }
        }

        /// <summary>
        /// Cleans up processes that can block tests
        /// </summary>
        /// <returns></returns>
        public void CleanProcess()
        {
            Process[] processes = Process.GetProcessesByName("AcroRd32");
            if (processes.Length > 0)
            {
                for (int i = 0; i < processes.Length; i++)
                {
                    processes[i].Kill();
                }
            }
            Process[] processes1 = Process.GetProcessesByName("EXCEL");
            if (processes1.Length > 0)
            {
                for (int i = 0; i < processes1.Length; i++)
                {
                    processes1[i].Kill();
                }
            }
            Process[] processes2 = Process.GetProcessesByName("WINWORD");
            if (processes2.Length > 0)
            {
                for (int i = 0; i < processes2.Length; i++)
                {
                    processes2[i].Kill();
                }
            }

            Process[] processes3 = Process.GetProcessesByName("notepad");
            if (processes3.Length > 0)
            {
                for (int i = 0; i < processes3.Length; i++)
                {
                    processes3[i].Kill();
                }
            }
        }

        /// <summary>
        /// Add a picture to a document
        /// </summary>
        /// <returns></returns>
        public static void InsertAPicture(string document, string UrlImage, string caseName, bool tag)
        {
            using (WordprocessingDocument wordprocessingDocument =
                WordprocessingDocument.Open(document, true))
            {
                MainDocumentPart mainPart = wordprocessingDocument.MainDocumentPart;

                ImagePart imagePart = mainPart.AddImagePart(ImagePartType.Jpeg);

                using (FileStream stream = new FileStream(UrlImage, FileMode.Open))
                {
                    imagePart.FeedData(stream);
                }

                AddImageToBody(wordprocessingDocument, mainPart.GetIdOfPart(imagePart), caseName, tag, UrlImage);
            }
        }

        /// <summary>
        /// Add a picture to a Word document
        /// </summary>
        /// <returns></returns>
        private static void AddImageToBody(WordprocessingDocument wordDoc, string relationshipId, string maestro, bool bandera, string UrlImage)
        {
            int iWidth = 0;
            int iHeight = 0;
            using (Bitmap bmp = new Bitmap(UrlImage))
            {
                iWidth = bmp.Width;
                iHeight = bmp.Height;
            }
            iWidth = (int)Math.Round((decimal)iWidth * 4000);
            iHeight = (int)Math.Round((decimal)iHeight * 4000);

            var element =

                new Drawing(
                     new DW.Inline(
                         new DW.Extent() { Cx = iWidth, Cy = iHeight },
                         new DW.EffectExtent()
                         {
                             LeftEdge = 0L,
                             TopEdge = 0L,
                             RightEdge = 0L,
                             BottomEdge = 0L
                         },
                         new DW.DocProperties()
                         {
                             Id = (UInt32Value)1U,
                             Name = "Picture 1"
                         },
                         new DW.NonVisualGraphicFrameDrawingProperties(
                             new A.GraphicFrameLocks() { NoChangeAspect = true }),
                         new A.Graphic(
                             new A.GraphicData(
                                 new PIC.Picture(
                                     new PIC.NonVisualPictureProperties(
                                         new PIC.NonVisualDrawingProperties()
                                         {
                                             Id = (UInt32Value)0U,
                                             Name = "New Bitmap Image.bmp"
                                         },
                                         new PIC.NonVisualPictureDrawingProperties()),
                                     new PIC.BlipFill(
                                         new A.Blip(
                                             new A.BlipExtensionList(
                                                 new A.BlipExtension()
                                                 {
                                                     Uri =
                                                        "{28A0092B-C50C-407E-A947-70E740481C1C}"
                                                 })
                                         )
                                         {
                                             Embed = relationshipId,
                                             CompressionState =
                                             A.BlipCompressionValues.None
                                         },
                                         new A.Stretch(
                                             new A.FillRectangle())),
                                     new PIC.ShapeProperties(
                                         new A.Transform2D(
                                             new A.Offset() { X = 10000L, Y = 10000L },
                                             new A.Extents() { Cx = iWidth, Cy = iHeight }),
                                         new A.PresetGeometry(
                                             new A.AdjustValueList()
                                         )
                                         { Preset = A.ShapeTypeValues.Rectangle }))
                             )
                             { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" })
                     )
                     {
                         DistanceFromTop = (UInt32Value)0U,
                         DistanceFromBottom = (UInt32Value)0U,
                         DistanceFromLeft = (UInt32Value)0U,
                         DistanceFromRight = (UInt32Value)0U,
                         EditId = "50D07946"
                     });

            // Append the reference to body, the element should be in a Run.
            if (bandera)
            {
                wordDoc.MainDocumentPart.Document.Body.AppendChild(new Paragraph(new Run(new Text(maestro))));
            }
            wordDoc.MainDocumentPart.Document.Body.AppendChild(new Paragraph(new Run(element)));

        }      
    }
}

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;

//Upload af store filer. Indsættes i web.config:<httpRuntime maxRequestLength="2097151" executionTimeout="1200" />

namespace RepoMZ
{
    public class Uploader
    {
        private Random rnd = new Random();

        public string UploadFile(HttpPostedFile fileToUpload, string saveToPath, bool randomFileName,
            string allowExtentons)
        {
            string strFileName;
            string strLongFilePath = fileToUpload.FileName;
            bool done = false;
            strFileName = Path.GetFileName(strLongFilePath);


            if (allowExtentons.Length > 1)
            {
                string extentions = allowExtentons.Replace(" ", "").ToLower();
                extentions = allowExtentons.Replace(".", "");
                string[] arrExt = extentions.Split(',');

                foreach (string ext in arrExt)
                {
                    if ("." + ext == Path.GetExtension(strFileName))
                    {
                        if (randomFileName)
                        {
                            strFileName = GenRndName() + Path.GetExtension(strFileName);

                        }
                        else
                        {
                            strFileName = ReplaceChars(strFileName);
                        }

                        fileToUpload.SaveAs(saveToPath + strFileName);

                        done = true;
                    }

                }

            }
            else
            {
                if (randomFileName)
                {
                    strFileName = GenRndName() + Path.GetExtension(strFileName);
                }
                else
                {
                    strFileName = ReplaceChars(strFileName);
                }
                fileToUpload.SaveAs(saveToPath + strFileName);
                done = true;
            }

            if (done)
            {
                return saveToPath + strFileName;
            }
            else
            {
                return null;
            }
        }


        public string UploadImage(HttpPostedFileBase fileToUpload, string outputPath, int newWidth, bool randomFileName)
        {
            string strFileName;
            string strLongFilePath = fileToUpload.FileName;
            string ext = Path.GetExtension(strLongFilePath).ToLower();
            var imgFor = GetFormat(ext);

            if (imgFor != null)
            {

                strFileName = Path.GetFileName(strLongFilePath);

                if (randomFileName)
                {
                    strFileName = GenRndName() + Path.GetExtension(strFileName);
                }
                else
                {
                    strFileName = ReplaceChars(strFileName);
                }


                fileToUpload.SaveAs(outputPath + strFileName);

                if (newWidth > 0)
                {
                    this.ReSizeImage(outputPath + strFileName, outputPath, newWidth);
                }

                else
                {
                    return outputPath + strFileName;
                }
                return outputPath + strFileName;
            }
            else
            {
                return null;
            }
        }

        public string ReSizeImage(string imagePath, string outputPath, int newWidth)
        {
            System.Drawing.Image bm = System.Drawing.Image.FromFile(imagePath);
            string ext = Path.GetExtension(imagePath);
            string fileN = Path.GetFileName(imagePath);
            var imgFor = GetFormat(ext);
            int NewHeight = (bm.Height*newWidth)/bm.Width;

            Bitmap resized = new Bitmap(newWidth, NewHeight);

            Graphics g = Graphics.FromImage(resized);

            g.DrawImage(bm, new Rectangle(0, 0, resized.Width, resized.Height), 0, 0, bm.Width, bm.Height,
                GraphicsUnit.Pixel);

            g.Dispose();
            bm.Dispose();

            if (imgFor != null)
            {
                resized.Save(outputPath + fileN, imgFor);
                return outputPath + fileN;
            }
            else
            {
                return null;
            }
        }

        private string ReplaceChars(string text)
        {
            text = text.Replace(" ", "_");
            text = text.Replace("æ", "ae");
            text = text.Replace("ø", "oe");
            text = text.Replace("å", "aa");
            text = text.Replace(",", "");
            text = text.Replace("*", "");
            text = text.Replace("'", "");
            text = text.Replace("&", "");

            return text;
        }


        public string GenRnd(int antal)
        {

            string strKey =
                "a b c d e f g h i j k l m n o p q r s t u v x y z 1 2 3 4 5 6 7 8 9 A B C D E F G H I J K L M N O P Q R S T U V X Y Z";

            string[] arrKey = {};
            string rndString = "";

            arrKey = strKey.Split(' ');

            int intMax = arrKey.Length;
            int intMin = 1;

            for (int i = 0; i < antal; i++)
            {
                rndString += arrKey[rnd.Next(intMin, intMax)];
            }

            return rndString;
        }

        private string GenRndName()
        {
            DateTime d = DateTime.Now;
            string name = d.Day + GenRnd(7) + d.Year + GenRnd(7) + d.Month;

            return name;
        }

        private ImageFormat GetFormat(string ext)
        {
            switch (ext.ToLower())
            {
                case ".gif":
                    return ImageFormat.Gif;

                case ".jpg":
                    return ImageFormat.Jpeg;

                case ".png":
                    return ImageFormat.Png;

                default:
                    return null;
            }
        }
    }
}
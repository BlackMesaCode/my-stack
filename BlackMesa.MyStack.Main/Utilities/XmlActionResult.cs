using System.Web.Mvc;
using System.Xml.Linq;

namespace BlackMesa.MyStack.Main.Utilities
{
    public class XmlActionResult : ActionResult
    {
        public XmlActionResult(string xml, string fileName,
            EncodingType encoding = EncodingType.UTF16,
            LoadOptions loadOptions = System.Xml.Linq.LoadOptions.None)
        {
            XmlContent = xml;
            FileName = fileName;
            Encoding = encoding;
            LoadOptions = loadOptions;
        }

        public string FileName
        {
            get;
            set;
        }

        public string XmlContent
        {
            get;
            set;
        }

        public EncodingType Encoding
        {
            get;
            set;
        }

        public LoadOptions LoadOptions
        {
            get;
            set;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ContentType = "text/xml";
            context.HttpContext.Response.AddHeader("content-disposition",
              string.Format("attachment; filename={0}", FileName));

            switch (Encoding)
            {
                case EncodingType.UTF8:
                    context.HttpContext.Response.Charset = "utf-8";
                    context.HttpContext.Response.BinaryWrite(
                      System.Text.UTF8Encoding.Default.GetBytes(XmlContent));
                    break;
                case EncodingType.UTF16:
                    context.HttpContext.Response.Charset = "utf-16";
                    context.HttpContext.Response.BinaryWrite(
                      System.Text.UnicodeEncoding.Default.GetBytes(XmlContent));
                    break;
                case EncodingType.UTF32:
                    context.HttpContext.Response.Charset = "utf-32";
                    context.HttpContext.Response.BinaryWrite(
                      System.Text.UTF32Encoding.Default.GetBytes(XmlContent));
                    break;
            }

            context.HttpContext.Response.End();
        }
    }

    public enum EncodingType
    {
        UTF8,
        UTF16,
        UTF32
    }


}
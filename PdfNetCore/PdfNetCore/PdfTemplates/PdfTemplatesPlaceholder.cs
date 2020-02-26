using System.Reflection;

namespace PdfNetCore.PdfTemplates
{
    public class PdfTemplatesPlaceholder
    {
        public static Assembly Assembly => typeof(PdfTemplatesPlaceholder).GetTypeInfo().Assembly;
    }
}

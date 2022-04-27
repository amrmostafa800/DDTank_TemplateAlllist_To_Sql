using System;
using System.IO;
using System.Xml;
using System.Threading;
using System.Threading.Tasks;

var path = Path.Combine(Directory.GetCurrentDirectory(), "templatealllist_decoded.xml");
var path2 = Path.Combine(Directory.GetCurrentDirectory(), "TemplateAlllist.sql");

static void Main()
{

XmlReader xmlReader = XmlReader.Create("templatealllist_decoded.xml");
StreamWriter streamWriter = File.AppendText("TemplateAlllist.sql");
streamWriter.WriteLine("USE [Db_Tank]");
streamWriter.WriteLine("GO");
streamWriter.WriteLine("UPDATE dbo.Shop_Goods");
while (xmlReader.Read())
    {
  if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "Item" && xmlReader.HasAttributes )
 {
    string str1 = !(xmlReader.GetAttribute("TemplateID") == "") ? xmlReader.GetAttribute("TemplateID") : "null";
    string str2 = !(xmlReader.GetAttribute("Name") == "") ? xmlReader.GetAttribute("Name") : "null";
            if (str2.IndexOf("'") != -1)
                str2 = str2.Replace("'", "''");
                str2 = str2.Replace("?", "*");
            string str3 = !(xmlReader.GetAttribute("Description") == "") ? xmlReader.GetAttribute("Description") : "null";
            if (str3.IndexOf("'") != -1)
                str3 = str3.Replace("'", "''");
            if (str3 != "null")
            {
                streamWriter.WriteLine("SET Name = '{0}', Description = '{1}'", str2, str3);
            }
            else
            {
                streamWriter.WriteLine("SET Name = '{0}', Description = {1}", str2, str3);
            }
            streamWriter.WriteLine("WHERE TemplateID = '{0}'", str1);
    streamWriter.WriteLine("UPDATE dbo.Shop_Goods");
}
}
}


new Thread(new ThreadStart(Main)).Start();
Task.WaitAll();
Console.WriteLine("Done");

Console.ReadKey();

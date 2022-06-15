using System;
using System.IO;
using System.Xml;
using System.Threading;
using System.Threading.Tasks;

Console.WriteLine("1 - DDTank_TemplateAlllist");
Console.WriteLine("2 - NPCInfoList");
Console.WriteLine("3 - Questlist");
var Readline = Console.ReadLine();
if (Readline == "1")
{
    new Thread(new ThreadStart(Main)).Start();
    Task.WaitAll();
    Console.WriteLine("Finsh");
}
else if (Readline == "2")
{
    new Thread(new ThreadStart(NPCInfoList)).Start();
    Task.WaitAll();
    Console.WriteLine("Finsh");
}
else
{
    new Thread(new ThreadStart(QuestList)).Start();
    Task.WaitAll();
    Console.WriteLine("Finsh");
}



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
    Task.WaitAll();
    streamWriter.Close();
}


static void QuestList()
{
    XmlReader xmlReader = XmlReader.Create("questlist_Decoded.xml");
    StreamWriter streamWriter = File.AppendText("questlist.sql");
    streamWriter.WriteLine("USE [Db_Tank]");
    streamWriter.WriteLine("GO");
    streamWriter.WriteLine("UPDATE dbo.Quest");
    while (xmlReader.Read())
    {
        if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "Item" && xmlReader.HasAttributes)
        {
            string str10 = !(xmlReader.GetAttribute("ID") == "") ? xmlReader.GetAttribute("ID") : "null";
            string strCanRepeat = !(xmlReader.GetAttribute("CanRepeat") == "") ? xmlReader.GetAttribute("CanRepeat") : "null";
            string str20 = !(xmlReader.GetAttribute("Title") == "") ? xmlReader.GetAttribute("Title") : "null";
            if (str20.IndexOf("'") != -1)
                str20 = str20.Replace("'", "''");
            str20 = str20.Replace("?", "*");
            string str30 = !(xmlReader.GetAttribute("Detail") == "") ? xmlReader.GetAttribute("Detail") : "null";
            if (str30.IndexOf("'") != -1)
                str30 = str30.Replace("'", "''");
            if (str30 != "null")
            {
                streamWriter.WriteLine("SET Detail = '{0}', Title = '{1}', CanRepeat = '{2}'", str30, str20, strCanRepeat);
            }
            else
            {
                streamWriter.WriteLine("SET Detail = '{0}', Title = {1}, CanRepeat = '{2}'", str30, str20, strCanRepeat);
            }
            streamWriter.WriteLine("WHERE ID = '{0}'", str10);
            streamWriter.WriteLine("UPDATE dbo.Quest");
            
        }
    }
    Task.WaitAll();
    streamWriter.Close();
    new Thread(new ThreadStart(Quest_Condiction)).Start();
    streamWriter.Close();

}

static void Quest_Condiction()
{
    XmlReader xmlReader = XmlReader.Create("questlist_Decoded.xml");
    StreamWriter streamWriter = File.AppendText("Quest_Condiction.sql");
    streamWriter.WriteLine("USE [Db_Tank]");
    streamWriter.WriteLine("GO");
    streamWriter.WriteLine("UPDATE dbo.Quest_Condiction");
    while (xmlReader.Read())
    {
        if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "Item_Condiction" && xmlReader.HasAttributes)
        {
            string strC10 = !(xmlReader.GetAttribute("CondictionID") == "") ? xmlReader.GetAttribute("CondictionID") : "null";
            string strC100 = !(xmlReader.GetAttribute("QuestID") == "") ? xmlReader.GetAttribute("QuestID") : "null";
            string strC20 = !(xmlReader.GetAttribute("CondictionTitle") == "") ? xmlReader.GetAttribute("CondictionTitle") : "null";

            if (strC20.IndexOf("'") != -1)
                strC20 = strC20.Replace("'", "''");
            if (strC20 != "null")
            {
                streamWriter.WriteLine("SET CondictionTitle = '{0}'", strC20);
            }
            else
            {
                streamWriter.WriteLine("SET CondictionTitle = {0}", strC20);
            }
            streamWriter.WriteLine("WHERE CondictionID = '{0}' AND QuestID = '{1}'", strC10, strC100);
            streamWriter.WriteLine("UPDATE dbo.Quest_Condiction");

        }
    }
    Task.WaitAll();
    streamWriter.Close();
}
static void NPCInfoList()
{
    XmlReader xmlReader = XmlReader.Create("NPCInfoList_decoded.xml");
    StreamWriter streamWriter = File.AppendText("NPCInfoList.sql");
    streamWriter.WriteLine("USE [Db_Tank]");
    streamWriter.WriteLine("GO");
    streamWriter.WriteLine("UPDATE dbo.NPC_Info");
    if (xmlReader.NodeType == XmlNodeType.Attribute && xmlReader.Name == "Item" && xmlReader.HasAttributes)
    {
        string str1 = !(xmlReader.GetAttribute("ID") == "") ? xmlReader.GetAttribute("ID") : "null";
        string str2 = !(xmlReader.GetAttribute("Name") == "") ? xmlReader.GetAttribute("Name") : "null";
        if (str2.IndexOf("'") != -1)
        { 
            str2 = str2.Replace("'", "''");
        }
        streamWriter.WriteLine("SET Name = {0}", str2);
        streamWriter.WriteLine("WHERE ID = '{0}'", str1);
        streamWriter.WriteLine("UPDATE dbo.NPC_Info");
    }
    Task.WaitAll();
    streamWriter.Close();
}

Console.ReadKey();

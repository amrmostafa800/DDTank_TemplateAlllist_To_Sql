using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Threading;
using System.Threading.Tasks;

Console.WriteLine("1 - DDTank_TemplateAlllist");
Console.WriteLine("2 - NPCInfoList");
Console.WriteLine("3 - Questlist");
Console.WriteLine("4 - Questlist (full insert)");
Console.WriteLine("5 - Quest_Condiction (full insert)");
Console.WriteLine("6 - Quest_Goods (full insert)");
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
else if (Readline == "3")
{
    new Thread(new ThreadStart(QuestList)).Start();
    Task.WaitAll();
    Console.WriteLine("Finsh");
}
else if (Readline == "4")
{
    new Thread(new ThreadStart(QuestListFull)).Start();
    Task.WaitAll();
    Console.WriteLine("Finsh");
}
else if (Readline == "5")
{
    new Thread(new ThreadStart(Quest_CondictionFull)).Start();
    Task.WaitAll();
    Console.WriteLine("Finsh");
}
else
{
    new Thread(new ThreadStart(Quest_GoodsFull)).Start();
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
        if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "Item" && xmlReader.HasAttributes)
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
    XDocument xDoc = XDocument.Load("NPCInfoList_decoded.xml");
    StreamWriter streamWriter = File.AppendText("NPCInfoList.sql");
    streamWriter.WriteLine("USE [Db_Tank]");
    streamWriter.WriteLine("GO");
    foreach (XElement elem in xDoc.Descendants("Item"))
    {
        string str1 = (elem.Attribute("ID")?.Value);
        string str2 = (elem.Attribute("Name")?.Value);
        if (str1 != null || str2 != null)
        {
            streamWriter.WriteLine("UPDATE dbo.NPC_Info");
            if (str2.Contains("'"))
            {
                str2 = str2.Replace("'", "''");
            }
            streamWriter.WriteLine("SET Name = '{0}'", str2);
            streamWriter.WriteLine("WHERE ID = '{0}'", str1);
        }
    }
    Task.WaitAll();
    streamWriter.Close();
}
static void QuestListFull()
{
    XmlReader xmlReader = XmlReader.Create("questlist_Decoded.xml");
    StreamWriter streamWriter = File.AppendText("questlist_Quest.sql");
    streamWriter.WriteLine("USE [Db_Tank]");
    streamWriter.WriteLine("GO");
    while (xmlReader.Read())
    {
        if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "Item" && xmlReader.HasAttributes)
        {
            string ID = !(xmlReader.GetAttribute("ID") == "") ? xmlReader.GetAttribute("ID") : "null";
            string QuestID = !(xmlReader.GetAttribute("QuestID") == "") ? xmlReader.GetAttribute("QuestID") : "null";
            string Title = !(xmlReader.GetAttribute("Title") == "") ? xmlReader.GetAttribute("Title") : "null";
            if (Title.IndexOf("'") != -1)
                Title = Title.Replace("'", "''");
            string Detail = !(xmlReader.GetAttribute("Detail") == "") ? xmlReader.GetAttribute("Detail") : "null";
            if (Detail.IndexOf("'") != -1)
                Detail = Detail.Replace("'", "''");
            string Objective = !(xmlReader.GetAttribute("Objective") == "") ? xmlReader.GetAttribute("Objective") : "null";
            string NeedMinLevel = !(xmlReader.GetAttribute("NeedMinLevel") == "") ? xmlReader.GetAttribute("NeedMinLevel") : "null";
            string NeedMaxLevel = !(xmlReader.GetAttribute("NeedMaxLevel") == "") ? xmlReader.GetAttribute("NeedMaxLevel") : "null";
            string PreQuestID = !(xmlReader.GetAttribute("PreQuestID") == "") ? xmlReader.GetAttribute("PreQuestID") : "null";
            string NextQuestID = !(xmlReader.GetAttribute("NextQuestID") == "") ? xmlReader.GetAttribute("NextQuestID") : "null";
            string IsOther = !(xmlReader.GetAttribute("IsOther") == "") ? xmlReader.GetAttribute("IsOther") : "null";
            string CanRepeat = !(xmlReader.GetAttribute("CanRepeat") == "") ? xmlReader.GetAttribute("CanRepeat") : "null";
            string RepeatInterval = !(xmlReader.GetAttribute("RepeatInterval") == "") ? xmlReader.GetAttribute("RepeatInterval") : "null";
            string RepeatMax = !(xmlReader.GetAttribute("RepeatMax") == "") ? xmlReader.GetAttribute("RepeatMax") : "null";
            string RewardGP = !(xmlReader.GetAttribute("RewardGP") == "") ? xmlReader.GetAttribute("RewardGP") : "null";
            string RewardGold = !(xmlReader.GetAttribute("RewardGold") == "") ? xmlReader.GetAttribute("RewardGold") : "null";
            string RewardGiftToken = !(xmlReader.GetAttribute("RewardGiftToken") == "") ? xmlReader.GetAttribute("RewardGiftToken") : "null";
            string RewardOffer = !(xmlReader.GetAttribute("RewardOffer") == "") ? xmlReader.GetAttribute("RewardOffer") : "null";
            string RewardRiches = !(xmlReader.GetAttribute("RewardRiches") == "") ? xmlReader.GetAttribute("RewardRiches") : "null";
            string RewardBuffID = !(xmlReader.GetAttribute("RewardBuffID") == "") ? xmlReader.GetAttribute("RewardBuffID") : "null";
            string RewardBuffDate = !(xmlReader.GetAttribute("RewardBuffDate") == "") ? xmlReader.GetAttribute("RewardBuffDate") : "null";
            string RewardMoney = !(xmlReader.GetAttribute("RewardMoney") == "") ? xmlReader.GetAttribute("RewardMoney") : "null";
            string Rands = !(xmlReader.GetAttribute("Rands") == "") ? xmlReader.GetAttribute("Rands") : "null";
            string RandDouble = !(xmlReader.GetAttribute("RandDouble") == "") ? xmlReader.GetAttribute("RandDouble") : "null";
            string TimeMode = !(xmlReader.GetAttribute("TimeMode") == "") ? xmlReader.GetAttribute("TimeMode") : "null";
            string StartDate = !(xmlReader.GetAttribute("StartDate") == "") ? xmlReader.GetAttribute("StartDate") : "null";
            string EndDate = !(xmlReader.GetAttribute("EndDate") == "") ? xmlReader.GetAttribute("EndDate") : "null";
            string MapID = !(xmlReader.GetAttribute("MapID") == "") ? xmlReader.GetAttribute("MapID") : "null";
            string AutoEquip = !(xmlReader.GetAttribute("AutoEquip") == "") ? xmlReader.GetAttribute("AutoEquip") : "null";
            string RewardMedal = !(xmlReader.GetAttribute("RewardMedal") == "") ? xmlReader.GetAttribute("RewardMedal") : "null";
            string Rank = !(xmlReader.GetAttribute("Rank") == "") ? xmlReader.GetAttribute("Rank") : "null";
            string StarLev = !(xmlReader.GetAttribute("StarLev") == "") ? xmlReader.GetAttribute("StarLev") : "null";
            string NotMustCount = !(xmlReader.GetAttribute("NotMustCount") == "") ? xmlReader.GetAttribute("NotMustCount") : "null";

            string Str = ",";
            streamWriter.WriteLine("INSERT INTO dbo.Quest (ID,QuestID,Title,Detail,Objective,NeedMinLevel,NeedMaxLevel,PreQuestID,NextQuestID,IsOther,CanRepeat,RepeatInterval,RepeatMax,RewardGP,RewardGold,RewardGiftToken,RewardOffer,RewardRiches,RewardBuffID,RewardBuffDate,RewardMoney,Rands,RandDouble,TimeMode,StartDate,EndDate,MapID,AutoEquip,RewardMedal,Rank,StarLev,NotMustCount)" + "VALUES (" + "'" + ID + "'" + Str + "'" + QuestID + "'" + Str + "'" + Title + "'" + Str + "'" + Detail + "'" + Str + "'" + Objective + "'" + Str + "'" + NeedMinLevel + "'" + Str + "'" + NeedMaxLevel + "'" + Str + "'" + PreQuestID + "'" + Str + "'" + NextQuestID + "'" + Str + "'" + IsOther + "'" + Str + "'" + CanRepeat + "'" + Str + "'" + RepeatInterval + "'" + Str + "'" + RepeatMax + "'" + Str + "'" + RewardGP + "'" + Str + "'" + RewardGold + "'" + Str + "'" + RewardGiftToken + "'" + Str + "'" + RewardOffer + "'" + Str + "'" + RewardRiches + "'" + Str + "'" + RewardBuffID + "'" + Str + "'" + RewardBuffDate + "'" + Str + "'" + RewardMoney + "'" + Str + "'" + Rands + "'" + Str + "'" + RandDouble + "'" + Str + "'" + TimeMode + "'" + Str + "'" + StartDate + "'" + Str + "'" + EndDate + "'" + Str + "'" + MapID + "'" + Str + "'" + AutoEquip + "'" + Str + "'" + RewardMedal + "'" + Str + "'" + Rank + "'" + Str + "'" + StarLev + "'" + Str + "'" + NotMustCount + "'" + ");");

        }
    }
    Task.WaitAll();
    streamWriter.Close();
}

static void Quest_CondictionFull()
{
    XmlReader xmlReader = XmlReader.Create("questlist_Decoded.xml");
    StreamWriter streamWriter = File.AppendText("questlist_Condiction.sql");
    streamWriter.WriteLine("USE [Db_Tank]");
    streamWriter.WriteLine("GO");
    while (xmlReader.Read())
{
if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "Item_Condiction" && xmlReader.HasAttributes)
{
            string QuestID = !(xmlReader.GetAttribute("QuestID") == "") ? xmlReader.GetAttribute("QuestID") : "null";
            string CondictionID = !(xmlReader.GetAttribute("CondictionID") == "") ? xmlReader.GetAttribute("CondictionID") : "null";
            string CondictionTitle = !(xmlReader.GetAttribute("CondictionTitle") == "") ? xmlReader.GetAttribute("CondictionTitle") : "null";
            if (CondictionTitle.IndexOf("'") != -1)
                CondictionTitle = CondictionTitle.Replace("'", "''");
            string CondictionType = !(xmlReader.GetAttribute("CondictionType") == "") ? xmlReader.GetAttribute("CondictionType") : "null";
            string Para1 = !(xmlReader.GetAttribute("Para1") == "") ? xmlReader.GetAttribute("Para1") : "null";
            string Para2 = !(xmlReader.GetAttribute("Para2") == "") ? xmlReader.GetAttribute("Para2") : "null";
            string isOpitional = !(xmlReader.GetAttribute("isOpitional") == "") ? xmlReader.GetAttribute("isOpitional") : "null";

            string Str = ",";
            streamWriter.WriteLine("INSERT INTO dbo.Quest_Condiction (QuestID, CondictionID, CondictionTitle, CondictionType, Para1, Para2, isOpitional) VALUES (" + "'" + QuestID + "'" + Str + "'" + CondictionID + "'" + Str + "'" + CondictionTitle + "'" + Str + "'" + CondictionType + "'" + Str + "'" + Para1 + "'" + Str + "'" + Para2 + "'" + Str + "'" + isOpitional + "'" + ");");
        }
    }
    Task.WaitAll();
    streamWriter.Close();
}
static void Quest_GoodsFull()
{
    XmlReader xmlReader = XmlReader.Create("questlist_Decoded.xml");
    StreamWriter streamWriter = File.AppendText("questlist_Goods.sql");
    streamWriter.WriteLine("USE [Db_Tank]");
    streamWriter.WriteLine("GO");
    while (xmlReader.Read())
    {
        if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "Item_Good" && xmlReader.HasAttributes)
        {
            string QuestID = !(xmlReader.GetAttribute("QuestID") == "") ? xmlReader.GetAttribute("QuestID") : "null";
            string RewardItemID = !(xmlReader.GetAttribute("RewardItemID") == "") ? xmlReader.GetAttribute("RewardItemID") : "null";
            string IsSelect = !(xmlReader.GetAttribute("IsSelect") == "") ? xmlReader.GetAttribute("IsSelect") : "null";
            string RewardItemValid = !(xmlReader.GetAttribute("RewardItemValid") == "") ? xmlReader.GetAttribute("RewardItemValid") : "null";
            string RewardItemCount = !(xmlReader.GetAttribute("RewardItemCount") == "") ? xmlReader.GetAttribute("RewardItemCount") : "null";
            string StrengthenLevel = !(xmlReader.GetAttribute("StrengthenLevel") == "") ? xmlReader.GetAttribute("StrengthenLevel") : "null";
            string AttackCompose = !(xmlReader.GetAttribute("AttackCompose") == "") ? xmlReader.GetAttribute("AttackCompose") : "null";
            string DefendCompose = !(xmlReader.GetAttribute("DefendCompose") == "") ? xmlReader.GetAttribute("DefendCompose") : "null";
            string AgilityCompose = !(xmlReader.GetAttribute("AgilityCompose") == "") ? xmlReader.GetAttribute("AgilityCompose") : "null";
            string LuckCompose = !(xmlReader.GetAttribute("LuckCompose") == "") ? xmlReader.GetAttribute("LuckCompose") : "null";
            string IsCount = !(xmlReader.GetAttribute("IsCount") == "") ? xmlReader.GetAttribute("IsCount") : "null";
            string IsBind = !(xmlReader.GetAttribute("IsBind") == "") ? xmlReader.GetAttribute("IsBind") : "null";

            string Str = ",";
            streamWriter.WriteLine("INSERT INTO dbo.Quest_Goods (QuestID, RewardItemID, IsSelect, RewardItemValid, RewardItemCount, StrengthenLevel, AttackCompose, DefendCompose, AgilityCompose, LuckCompose, IsCount, IsBind) VALUES (" + "'" + QuestID + "'" + Str + "'" + RewardItemID + "'" + Str + "'" + IsSelect + "'" + Str + "'" + RewardItemValid + "'" + Str + "'" + RewardItemCount + "'" + Str + "'" + StrengthenLevel + "'" + Str + "'" + AttackCompose + "'" + Str + "'" + DefendCompose + "'" + Str + "'" + AgilityCompose + "'" + Str + "'" + LuckCompose + "'" + Str + "'" + IsCount + "'" + Str + "'" + IsBind + "'" + ");");
        }
    }
    Task.WaitAll();
    streamWriter.Close();
}

Console.ReadKey();

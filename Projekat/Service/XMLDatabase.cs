using Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Service.FileHelper;

namespace Service
{
    public class XMLDatabase
    {
         private readonly ICheckFile fInUseCheck;
         private readonly string loadsPath;
         private readonly string auditPath;

         public XMLDatabase(ICheckFile fileInUseChecker)
         {
             this.fInUseCheck = fileInUseChecker;
             loadsPath = ConfigurationManager.AppSettings["loadsPath"];
             auditPath = ConfigurationManager.AppSettings["auditPath"];

             EnsureDirectoryExists(Path.GetDirectoryName(loadsPath));
             EnsureDirectoryExists(Path.GetDirectoryName(auditPath));
             EnsureFileExists(loadsPath);
             EnsureFileExists(auditPath);
         }

         public Result GetLoads(DateTime date)
         {
             Result result = new Result();

             XDocument xml = TryLoadXml(loadsPath, result.Audits);

             if (xml == null)
             {
                 result.Audits.Add(new Audit { Message = "Nevalidan XML", MessageType = MessageType.ERROR });
             }
             else
             {
                 foreach (var row in xml.Descendants("row"))
                 {
                     List<Audit> audits = new List<Audit>();

                     if (!TryParseLong(row.Element("ID").Value, out long id, audits, "ID")
                         || !TryParseDateTime(row.Element("TIME_STAMP").Value, out DateTime timestamp, audits, "TIME_STAMP")
                         || !TryParseDouble(row.Element("FORECAST_VALUE").Value, out double forecastValue, audits, "FORECAST_VALUE")
                         || !TryParseDouble(row.Element("MEASURED_VALUE").Value, out double measuredValue, audits, "MEASURED_VALUE"))
                     {
                         result.Audits.AddRange(audits);
                         continue;
                     }

                     if (audits.Count == 0)
                         result.Loads.Add(new Load(id, timestamp, forecastValue, measuredValue));
                 }

                 result.Loads = result.Loads.Where(x => x.Timestamp.Date == date).ToList();
             }

             return result;
         }

         public void AddAudits(List<Audit> audits)
         {
             XDocument xDocument = TryLoadXml(auditPath, null);

             if (xDocument == null)
             {
                 xDocument = new XDocument(new XElement("STAVKE"));
             }

             var xmlAudits = xDocument.Descendants("row").ToList();

             foreach (Audit audit in audits)
             {
                 XElement row = new XElement("row");
                 row.Add(new XElement("Timestamp", audit.Timestamp.ToString()));
                 row.Add(new XElement("MessageType", audit.MessageType.ToString()));
                 row.Add(new XElement("Message", audit.Message.ToString()));
                 xmlAudits.Add(row);
             }

             xDocument.Root.Add(xmlAudits);

             TrySaveXml(xDocument, auditPath);
         }

         private XDocument TryLoadXml(string path, List<Audit> audits)
         {
             if (fInUseCheck.IsFileInUse(path))
             {
                 audits?.Add(new Audit { Message = $"Cannot process the file {Path.GetFileName(path)}. It's being in use by another process or it has been deleted.", MessageType = MessageType.ERROR });
                 return null;
             }

             using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
             {
                 try
                 {
                    fileStream.Position = 0;
                     XDocument xdoc = XDocument.Load(fileStream);
                     return xdoc;
                 }
                 catch (Exception e)
                 {
                     audits?.Add(new Audit { Message = e.Message, MessageType = MessageType.ERROR });
                     return null;
                 }
             }
         }

         private bool TryParseLong(string value, out long result, List<Audit> audits, string fieldName)
         {
             if (string.IsNullOrEmpty(value))
             {
                 audits?.Add(new Audit { Message = $"{fieldName} ne postoji u XML bazi", MessageType = MessageType.ERROR });
                 result = 0;
                 return false;
             }

             if (!long.TryParse(value, out result))
             {
                 audits?.Add(new Audit { Message = $"{fieldName} nije ceo broj u XML bazi", MessageType = MessageType.ERROR });
                 return false;
             }

             return true;
         }

         private bool TryParseDateTime(string value, out DateTime result, List<Audit> audits, string fieldName)
         {
             if (string.IsNullOrEmpty(value))
             {
                 audits?.Add(new Audit { Message = $"{fieldName} ne postoji u XML bazi", MessageType = MessageType.ERROR });
                 result = DateTime.Now;
                 return false;
             }

             if (!DateTime.TryParse(value, out result))
             {
                 audits?.Add(new Audit { Message = $"{fieldName} nije datum u XML bazi", MessageType = MessageType.ERROR });
                 return false;
             }

             return true;
         }

         private bool TryParseDouble(string value, out double result, List<Audit> audits, string fieldName)
         {
             if (string.IsNullOrEmpty(value))
             {
                 audits?.Add(new Audit { Message = $"{fieldName} ne postoji u XML bazi", MessageType = MessageType.ERROR });
                 result = 0.0;
                 return false;
             }

             if (!double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out result))
             {
                 audits?.Add(new Audit { Message = $"{fieldName} nije realan broj u XML bazi", MessageType = MessageType.ERROR });
                 return false;
             }

             return true;
         }

         private void EnsureDirectoryExists(string directoryPath)
         {
             if (!Directory.Exists(directoryPath))
             {
                 Directory.CreateDirectory(directoryPath);
             }
         }

         private void EnsureFileExists(string filePath)
         {
             if (!File.Exists(filePath))
             {
                 File.Create(filePath).Close();
             }
         }

         private void TrySaveXml(XDocument xDocument, string path)
         {
             try
             {
                 xDocument.Save(path);
             }
             catch (Exception)
             {
                 // Handle any potential exceptions here.
             }
         }








    }
}

using Fiddler;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace JMeterExporterExtension
{
    [ProfferFormat("Cognizant JMeter", "Fiddler to JMeter .jmx Format")]
    public class JMeterExporter : ISessionExporter, IDisposable
    {
               public bool ExportSessions(
          string sFormat,
          Session[] oSessions,
          Dictionary<string, object> dictOptions,
          EventHandler<ProgressCallbackEventArgs> evtProgressNotifications)
        {
            bool flag = true;
            string saveFilename = Utilities.ObtainSaveFilename("Export As " + sFormat, "JMeter Files (*.jmx)|*.jmx");
            if (string.IsNullOrEmpty(saveFilename))
                return false;
            if (!Path.HasExtension(saveFilename))
                saveFilename += ".jmx";
            Encoding encoding = (Encoding)new UTF8Encoding(false);
            JMeterTestPlan jmeterTestPlan = new JMeterTestPlan(oSessions, saveFilename);
            StreamWriter streamWriter = new StreamWriter(saveFilename, false, encoding);
            try
            {
                //streamWriter.Write(jmeterTestPlan.Jmx);
                streamWriter.Write(jmeterTestPlan.Jmx.Replace("&lt;", "<").Replace("&gt;", ">"));
                streamWriter.Close();
                FiddlerApplication.Log.LogString("Successfully exported sessions to JMeter Test Plan");
                FiddlerApplication.Log.LogString(string.Format("\t{0}", (object)saveFilename));
            }
            catch (Exception ex)
            {
                FiddlerApplication.Log.LogString(ex.Message);
                FiddlerApplication.Log.LogString(ex.StackTrace);
                flag = false;
                streamWriter.Close();
            }
            return flag;
        }

        public void Dispose()
        {
        }

        
    }
}

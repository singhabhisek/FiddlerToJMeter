using Fiddler;
using System;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace JMeterExporterExtension
{
    public class JMeterTestPlan
    {
        private SessionList sessionList;
        private Session[] sessions;
        private string filename;

        public JMeterTestPlan()
        {
            this.sessions = new Session[0];
            this.sessionList = new SessionList(this.sessions);
        }

        public JMeterTestPlan(Session[] oSessions, string outputFilename)
        {
            this.filename = outputFilename;
            this.sessions = oSessions;
            this.sessionList = new SessionList(oSessions);
            using (StreamWriter streamWriter = new StreamWriter(Path.GetDirectoryName(this.filename) + "\\" + Path.GetFileNameWithoutExtension(this.filename) + "_Raw_Request_Response.txt", true))
            {
                for (int index = 0; index < this.sessions.Length; ++index)
                    
                    streamWriter.WriteLine((object)this.sessions[index]);
            }
        }

        public string Jmx
        {
            get
            {
                try
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");
                    try
                    {
                        String var = new System.Xml.Linq.XText(this.Xml).ToString();
                        //XDocument xdocument = XDocument.Parse(var);
                        stringBuilder.Append(var);
                    }
                    catch(Exception ex)
                    {
                        FiddlerApplication.Log.LogString("Error_XML:" + this.Xml);
                        //stringBuilder.Append(xdocument.ToString());
                    }
                    
                    return stringBuilder.ToString();
                }
               
                    catch (Exception ex)
                {
                    FiddlerApplication.Log.LogString(this.Xml);
                    FiddlerApplication.Log.LogString(ex.Message);
                    FiddlerApplication.Log.LogString(ex.StackTrace);
                    return "";
                }
            }
            
        }

        private string Xml
        {
            get
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("<jmeterTestPlan version=\"5.0\" properties=\"2.3\">");
                stringBuilder.Append("<hashTree>");
                stringBuilder.Append("<ThreadGroup guiclass=\"ThreadGroupGui\" testclass=\"ThreadGroup\" testname=\"Thread Group\" enabled=\"true\">");
                stringBuilder.Append("<stringProp name=\"ThreadGroup.on_sample_error\">continue</stringProp>");
                stringBuilder.Append("<elementProp name=\"ThreadGroup.main_controller\" elementType=\"LoopController\" guiclass=\"LoopControlPanel\" testclass=\"LoopController\" testname=\"Loop Controller\" enabled=\"true\">");
                stringBuilder.Append("<boolProp name=\"LoopController.continue_forever\">false</boolProp>");
                stringBuilder.Append("<stringProp name=\"LoopController.loops\">1</stringProp>");
                stringBuilder.Append("</elementProp>");
                stringBuilder.Append("<stringProp name=\"ThreadGroup.num_threads\">1</stringProp>");
                stringBuilder.Append("<stringProp name=\"ThreadGroup.ramp_time\">1</stringProp>");
                stringBuilder.Append("<boolProp name=\"ThreadGroup.scheduler\">false</boolProp>");
                stringBuilder.Append("<stringProp name=\"ThreadGroup.duration\"></stringProp>");
                stringBuilder.Append("<stringProp name=\"ThreadGroup.delay\"></stringProp>");
                stringBuilder.Append("</ThreadGroup>");
                stringBuilder.Append(this.sessionList.Xml);
                stringBuilder.Append("<ResultCollector guiclass = \"TableVisualizer\" testclass = \"ResultCollector\" testname = \"View Results in Table\" enabled = \"true\" >");
                stringBuilder.Append("</ResultCollector>");
                stringBuilder.Append("<hashTree/>");
                stringBuilder.Append("<ResultCollector guiclass = \"ViewResultsFullVisualizer\" testclass = \"ResultCollector\" testname = \"View Results Tree\" enabled = \"true\" >");
                stringBuilder.Append("</ResultCollector>");
                stringBuilder.Append("<hashTree/>");
                stringBuilder.Append("<HeaderManager guiclass=\"HeaderPanel\" testclass=\"HeaderManager\" testname=\"HTTP Header Manager\" enabled=\"true\">");
                stringBuilder.Append("<collectionProp name=\"HeaderManager.headers\"/>");
                stringBuilder.Append("</HeaderManager>");
                stringBuilder.Append("<hashTree/>");
                stringBuilder.Append("<CookieManager guiclass=\"CookiePanel\" testclass=\"CookieManager\" testname=\"HTTP Cookie Manager\" enabled=\"true\">");
                stringBuilder.Append("<collectionProp name=\"CookieManager.cookies\"/>");
                stringBuilder.Append("<boolProp name=\"CookieManager.clearEachIteration\">false</boolProp>");
                stringBuilder.Append("</CookieManager>");
                stringBuilder.Append("<hashTree/>");
                stringBuilder.Append("<CacheManager guiclass=\"CacheManagerGui\" testclass=\"CacheManager\" testname=\"HTTP Cache Manager\" enabled=\"true\">");
                stringBuilder.Append("<boolProp name=\"clearEachIteration\">false</boolProp>");
                stringBuilder.Append("<boolProp name=\"useExpires\">true</boolProp>");
                stringBuilder.Append("</CacheManager>");
                stringBuilder.Append("<hashTree/>");
                stringBuilder.Append("</hashTree>");
                stringBuilder.Append("</jmeterTestPlan>");
                return stringBuilder.ToString();
            }
        }
    }
}
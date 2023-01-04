using Fiddler;
using System.Collections.Specialized;
using System.Text;

namespace JMeterExporterExtension
{
    public class SessionList
    {
        private Session[] sessions;

        public SessionList()
        {
            this.sessions = new Session[0];
        }

        public SessionList(Session[] oSessions)
        {
            this.sessions = oSessions;
        }

        public string Xml
        {
            get
            {
                StringBuilder stringBuilder = new StringBuilder();
                string str1 = "";
                int num1;
                int num2 = num1 = 0;
                bool flag = false;
                if ((uint)this.sessions.Length > 0U)
                {
                    stringBuilder.Append("<hashTree>");
                    int index;
                    for (index = 0; index < this.sessions.Length; ++index)
                    {
                        

                        //put a check for host,content-type
                        if (((StringDictionary)this.sessions[index].oFlags).ContainsKey("ui-comments"))
                        {
                            flag = true;
                            string str2 = this.sessions[index].oFlags["ui-comments"];
                            if (str2 != str1)
                            {
                                ++num2;
                                if (num2 > 1)
                                    stringBuilder.Append("</hashTree>");
                                stringBuilder.Append("<TransactionController guiclass=\"TransactionControllerGui\" testclass=\"TransactionController\" testname=\"" + str2 + "\" enabled=\"true\">");
                                stringBuilder.Append("<boolProp name=\"TransactionController.includeTimers\">false</boolProp>");
                                stringBuilder.Append("<boolProp name=\"TransactionController.parent\">false</boolProp>");
                                stringBuilder.Append("</TransactionController>");
                                stringBuilder.Append("<hashTree>");
                                HTTPSamplerProxy httpSamplerProxy = new HTTPSamplerProxy(this.sessions[index]);
                                stringBuilder.Append(httpSamplerProxy.Xml);
                            }
                            else
                            {
                                HTTPSamplerProxy httpSamplerProxy = new HTTPSamplerProxy(this.sessions[index]);
                                stringBuilder.Append(httpSamplerProxy.Xml);
                            }
                            str1 = this.sessions[index].oFlags["ui-comments"];
                        }
                        else
                        {
                            if (num2 >= 1 & flag && index == this.sessions.Length)
                            {
                                stringBuilder.Append("</hashTree>");
                                flag = false;
                            }
                            HTTPSamplerProxy httpSamplerProxy = new HTTPSamplerProxy(this.sessions[index]);
                            stringBuilder.Append(httpSamplerProxy.Xml);
                        }
                    }
                    if (index == this.sessions.Length & flag)
                        stringBuilder.Append("</hashTree>");
                    stringBuilder.Append("</hashTree>");
                }
                return stringBuilder.ToString();
            }
        }
    }
}

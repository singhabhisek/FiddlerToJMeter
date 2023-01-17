using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Fiddler;

namespace JMeterExporterExtension
{
    public class HTTPSamplerProxy
    {
        private Session session;

        public HTTPSamplerProxy(Session session)
        {
            this.session = session;
        }

        public string Xml
        {
            get
            {
                if (session.host == "IMlicensing.mp.microsoft.com")
                {
                    FiddlerApplication.Log.LogString("Host was" + session.host);

                    return "";
                }
                else
                {
                    //FiddlerApplication.Log.LogString("PRINT: METHOD was - " + this.session.oRequest.headers.HTTPMethod);

                    if (this.session.oRequest.headers.HTTPMethod == "CONNECT")
                    {
                        FiddlerApplication.Log.LogString("SKIPPED_REQUEST: METHOD was - " + (object)((string)((ClientChatter)this.session.oRequest).headers.HTTPMethod).ToUpper());
                        return "";
                    }
                    if (this.session.oRequest.headers.ExistsAndContains("Content-Type", "application/x-protobuf"))
                    {
                        FiddlerApplication.Log.LogString("SKIPPED_REQUEST: Content Type was - application/x-protobuf");
                        return "";
                    }
                    else
                    {
                        //FiddlerApplication.Log.LogString("WRITE: METHOD was - " + (object)((string)((ClientChatter)this.session.oRequest).headers.HTTPMethod).ToUpper());

                        StringBuilder stringBuilder = new StringBuilder();
                        stringBuilder.Append(string.Format("<HTTPSamplerProxy guiclass=\"HttpTestSampleGui\" testclass=\"HTTPSamplerProxy\" testname=\"{0}\" enabled=\"true\">", (object)this.Path));
                        stringBuilder.Append("<boolProp name=\"HTTPSampler.postBodyRaw\">true</boolProp>");
                        stringBuilder.Append("<elementProp name=\"HTTPsampler.Arguments\" elementType=\"Arguments\">");
                        stringBuilder.Append("<collectionProp name=\"Arguments.arguments\">");
                        stringBuilder.Append("<elementProp name=\"\" elementType=\"HTTPArgument\">");
                        stringBuilder.Append("<boolProp name=\"HTTPArgument.always_encode\">false</boolProp>");
                        stringBuilder.Append(string.Format("<stringProp name=\"Argument.value\">{0}</stringProp>", (object)this.RequestBody));
                        stringBuilder.Append("<stringProp name=\"Argument.metadata\">=</stringProp>");
                        stringBuilder.Append("</elementProp>");
                        stringBuilder.Append("</collectionProp>");
                        stringBuilder.Append("</elementProp>");
                        stringBuilder.Append(string.Format("<stringProp name=\"HTTPSampler.domain\">{0}</stringProp>", (object)this.session.host));
                        stringBuilder.Append(string.Format("<stringProp name=\"HTTPSampler.port\">{0}</stringProp>", (object)this.Port));
                        stringBuilder.Append("<stringProp name=\"HTTPSampler.connect_timeout\"></stringProp>");
                        stringBuilder.Append("<stringProp name=\"HTTPSampler.response_timeout\"></stringProp>");
                        stringBuilder.Append(string.Format("<stringProp name=\"HTTPSampler.protocol\">{0}</stringProp>", (object)((ClientChatter)this.session.oRequest).headers.UriScheme));
                        stringBuilder.Append("<stringProp name=\"HTTPSampler.contentEncoding\"></stringProp>");
                        stringBuilder.Append(string.Format("<stringProp name=\"HTTPSampler.path\">{0}</stringProp>", (object)this.Path));
                        stringBuilder.Append(string.Format("<stringProp name=\"HTTPSampler.method\">{0}</stringProp>", (object)((string)((ClientChatter)this.session.oRequest).headers.HTTPMethod).ToUpper()));
                        stringBuilder.Append("<boolProp name=\"HTTPSampler.follow_redirects\">true</boolProp>");
                        stringBuilder.Append("<boolProp name=\"HTTPSampler.auto_redirects\">false</boolProp>");
                        stringBuilder.Append("<boolProp name=\"HTTPSampler.use_keepalive\">true</boolProp>");
                        stringBuilder.Append("<boolProp name=\"HTTPSampler.DO_MULTIPART_POST\">false</boolProp>");
                        stringBuilder.Append("<boolProp name=\"HTTPSampler.monitor\">false</boolProp>");
                        stringBuilder.Append("<stringProp name=\"HTTPSampler.embedded_url_re\"></stringProp>");
                        stringBuilder.Append("</HTTPSamplerProxy>");
                        stringBuilder.Append("<hashTree>");
                        stringBuilder.Append("<HeaderManager guiclass=\"HeaderPanel\" testclass=\"HeaderManager\" testname=\"HTTP Header Manager\" enabled=\"true\">");
                        stringBuilder.Append("<collectionProp name=\"HeaderManager.headers\">");
                        using (IEnumerator<HTTPHeaderItem> enumerator = ((ClientChatter)this.session.oRequest).headers.GetEnumerator())
                        {
                            while (((IEnumerator)enumerator).MoveNext())
                            {
                                HTTPHeaderItem current = enumerator.Current;
                                stringBuilder.Append("<elementProp name=\"" + (string)current.Name + "\" elementType=\"Header\">");
                                stringBuilder.Append("<stringProp name=\"Header.name\">" + (string)current.Name + "</stringProp>");
                                stringBuilder.Append("<stringProp name=\"Header.value\">" + (string)current.Value + "</stringProp>");
                                stringBuilder.Append("</elementProp>");
                            }
                        }
                        stringBuilder.Append("</collectionProp>");
                        stringBuilder.Append("</HeaderManager>");
                        stringBuilder.Append("<hashTree/>");
                        stringBuilder.Append("<ConstantTimer guiclass=\"ConstantTimerGui\" testclass=\"ConstantTimer\" testname=\"Constant Timer\" enabled=\"true\">");
                        stringBuilder.Append("<stringProp name=\"ConstantTimer.delay\">300</stringProp>");
                        stringBuilder.Append("</ConstantTimer>");
                        stringBuilder.Append("<hashTree/>");
                        stringBuilder.Append("</hashTree>");
                        return stringBuilder.ToString();
                    }
                }
            }
        }

        private string Path
        {
            get
            {
                return WebUtility.HtmlEncode(this.session.PathAndQuery);
            }
        }

        private string getPort()
        {
            int port = this.session.port;
            string uriScheme = ((ClientChatter)this.session.oRequest).headers.UriScheme;
            return uriScheme.ToLower() == "https" && port == 443 || uriScheme.ToLower() == "http" && port == 80 ? "" : port.ToString();
        }

        private string Port
        {
            get
            {
                return this.getPort();
            }
        }

        private string RequestBody
        {
            get
            {
                return WebUtility.HtmlEncode(this.session.GetRequestBodyAsString());
            }
        }
    }
}

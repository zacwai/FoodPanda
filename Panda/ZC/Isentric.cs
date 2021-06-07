using Panda.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Panda.ZC
{
    public class Isentric
    {
        public async Task Send(string to, string text)
        {
            var responseContent = "";
            SMSLog SmsOutput = new SMSLog() { Text = text, To = to };

            try
            {
                if (Cfg.GetCfg("IsentricSentWay") == "TM")
                    responseContent = await SendViaTmApi(to, text);
                else
                    responseContent = await SendDirectLink(to, text);

                #region process content
                SmsOutput.RawResp = responseContent;

                string[] resp1 = responseContent.Split(':');
                string[] resp2 = resp1[1].Split('-');
                string[] resp3 = resp2[0].Split(',');

                string[] codeArr = resp3[0].Split('=');
                string[] msgIdArr = resp3[1].Split('=');
                string[] phoneArr = resp3[2].Split('=');
                string[] msgArr = resp3[3].Split('=');

                string code = codeArr[1].Trim();
                string msgId = msgIdArr[1].Trim();
                string phone = phoneArr[1].Trim();
                string msg = msgArr[1].Trim();
                string statusCode = resp2[1].Trim();
                string statusText = resp2[2].Trim();
                SmsOutput.MsgId = msgId;

                if (statusCode == "0")
                {
                    if (code == "0")
                    {
                        SmsOutput.OK = true;
                        SmsOutput.Msg = msg;
                        SmsOutput.MsgId = msgId;
                    }
                    else
                    {
                        SmsOutput.OK = false;
                        SmsOutput.ErrMsg = msg;
                    }
                }
                else if (statusCode == "1")
                {
                    SmsOutput.OK = false;
                    SmsOutput.ErrMsg = "Failed to send SMS. Please try again.";
                }
                else if (statusCode == "4")
                {
                    SmsOutput.OK = false;
                    SmsOutput.ErrMsg = "Invalid Phone Number";
                }
                else if (statusCode == "10")
                {
                    SmsOutput.OK = false;
                    SmsOutput.ErrMsg = "Blacklisted MSISDN due to user send in STOP <keyword> or STOP ALL before.Please contact your respective telco and request remove your number from 66399";
                }
                else if (statusCode == "11")
                {
                    SmsOutput.OK = false;
                    SmsOutput.ErrMsg = "Insufficient Credit";
                }
                else
                {
                    SmsOutput.OK = false;
                    SmsOutput.ErrMsg = statusText;
                }
                #endregion
            }
            catch (Exception e)
            {
                SmsOutput.ErrMsg = e.Message;
                SmsOutput.OK = false;
            }

            using (var db = new Core.Schema())
            {
                db.smslogs.Add(SmsOutput);
                db.SaveChanges();
            }

        }
        private async Task<string> SendViaTmApi(string to, string text)
        {
            string responseContent = "";
            string _isentricUrl = Cfg.GetCfg("IsentricUrlTM");
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("key", "080ab4d7da985e01d14b9d02529ea30e"),
                new KeyValuePair<string, string>("to", to),
                new KeyValuePair<string, string>("text", text),
            });

            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage response = await httpClient.PostAsync(_isentricUrl, content);
                response.EnsureSuccessStatusCode();
                responseContent = await response.Content.ReadAsStringAsync();
            }
            return responseContent;
        }

        private async Task<string> SendDirectLink(string to, string text)
        {
            string responseContent = "";

            string _isentricUrl = Cfg.GetCfg("IsentricUrlOri");
            text = HttpUtility.UrlEncode(text);
            string msgId = Guid.NewGuid().ToString("N");
            string url = _isentricUrl + "&rmsisdn=" + to + "&mtid=" + msgId + "&dataStr=" + text;

            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                responseContent = await response.Content.ReadAsStringAsync();
            }

            return responseContent;
        }

        private async Task<SMSLog> _OriginalSend(string to, string text)
        {
            SMSLog SmsOutput = new SMSLog() { Text = text, To = to };

            string _isentricUrl = Cfg.GetCfg("IsentricUrlOri");
            text = HttpUtility.UrlEncode(text);
            string msgId = Guid.NewGuid().ToString("N");
            string url = _isentricUrl + "&rmsisdn=" + to + "&mtid=" + msgId + "&dataStr=" + text;

            using (var httpClient = new HttpClient())
            {
                try
                {
                    string responseContent;
                    HttpResponseMessage response = await httpClient.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    responseContent = await response.Content.ReadAsStringAsync();
                    SmsOutput.RawResp = responseContent;

                    string[] resp1 = responseContent.Split(':');
                    string[] resp2 = resp1[1].Split('-');
                    string[] resp3 = resp2[0].Split(',');

                    string[] codeArr = resp3[0].Split('=');
                    string[] msgIdArr = resp3[1].Split('=');
                    string[] phoneArr = resp3[2].Split('=');
                    string[] msgArr = resp3[3].Split('=');

                    string code = codeArr[1].Trim();
                    msgId = msgIdArr[1].Trim();
                    string phone = phoneArr[1].Trim();
                    string msg = msgArr[1].Trim();
                    string statusCode = resp2[1].Trim();
                    string statusText = resp2[2].Trim();
                    SmsOutput.MsgId = msgId;

                    if (statusCode == "0")
                    {
                        if (code == "0")
                        {
                            SmsOutput.OK = true;
                            SmsOutput.Msg = msg;
                            SmsOutput.MsgId = msgId;
                        }
                        else
                        {
                            SmsOutput.OK = false;
                            SmsOutput.ErrMsg = msg;
                        }
                    }
                    else if (statusCode == "1")
                    {
                        SmsOutput.OK = false;
                        SmsOutput.ErrMsg = "Failed to send SMS. Please try again.";
                    }
                    else if (statusCode == "4")
                    {
                        SmsOutput.OK = false;
                        SmsOutput.ErrMsg = "Invalid Phone Number";
                    }
                    else if (statusCode == "10")
                    {
                        SmsOutput.OK = false;
                        SmsOutput.ErrMsg = "Blacklisted MSISDN due to user send in STOP <keyword> or STOP ALL before.Please contact your respective telco and request remove your number from 66399";
                    }
                    else if (statusCode == "11")
                    {
                        SmsOutput.OK = false;
                        SmsOutput.ErrMsg = "Insufficient Credit";
                    }
                    else
                    {
                        SmsOutput.OK = false;
                        SmsOutput.ErrMsg = statusText;
                    }

                }
                catch (Exception e)
                {
                    SmsOutput.ErrMsg = e.Message;
                    SmsOutput.OK = false;
                }
                try
                {
                    string balnaceUrl = "http://203.223.130.115/ExtMTPush/CheckSMSUserCredit?custid=xoftcode";
                    string responseContent;
                    HttpResponseMessage response = await httpClient.GetAsync(balnaceUrl);
                    response.EnsureSuccessStatusCode();
                    responseContent = await response.Content.ReadAsStringAsync();
                    string[] resp1 = responseContent.Split(':');
                    string[] resp2 = resp1[1].Split(',');
                    string[] resp3 = resp2[1].Split(':');
                    string[] resp4 = resp3[0].Split('=');

                    if (!double.TryParse(resp4[1].Trim(), out double bal))
                    {
                        bal = 0;
                    }

                    SmsOutput.Balance = bal / 10;
                }
                catch (Exception e)
                {
                    SmsOutput.Balance = 0;
                }
            }

            using(var db = new Core.Schema())
            {
                db.smslogs.Add(SmsOutput);
                db.SaveChanges();
            }

            return SmsOutput;
        }
    }
}

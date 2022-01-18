using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using URSAPI.Models;
 

namespace URSAPI.DataAccessLayer
{
    public class EmailDAL
    {
        public static string Emailtest(Int64 userid, string sub, string status, string levelno, string reqno)
        {
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    string success = "";
                    //Int32 userid = 1;
                    //string sub = "Peer Review Request#FR-202110-000333 ";
                    //Int32 orgid = 0; string status = "approved"; string levelno = "1";
                    string reason = "ok"; string remark = "tested";
                    string frmAddress = "thiruvenkatachalam@adventsys.in";
                    string password = "9944029787123";
                     
                    {
                       
                        var credentials = new NetworkCredential(frmAddress, password);

                        var myMail = new MailMessage()
                        {
                            From = new MailAddress(frmAddress, "UserAccess Mailing Service"),
                            Subject = sub,
                        };

                        string strBodyDetail = "";
 
                        string itemuser = "thiruvenkatachalam@adventsys.in";
                       // string mail2 = "sananth.kumar@adventsys.in";
                       // myMail.To.Add(mail2);
                        myMail.To.Add(itemuser);
                      

                        myMail.IsBodyHtml = true;
                        string strHeadr = "";
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("<table style='width: 800px; margin: 0 auto; background-color: #f8f9f9; font-size: 12px; font-family: Open Sans,Helvetica,Arial,sans-serif;' border='0' cellpadding='0' cellspacing='0'>");
                        sb.AppendLine("        <tr style='background-color: #082654; color: white; text-align: center;'>");
                        sb.AppendLine("            <td colspan='3'>");
                        sb.AppendLine("                <p><b>Firewal Request</b></p>");
                        sb.AppendLine("            </td>");
                        sb.AppendLine("        </tr>");
                        sb.AppendLine("        <tr>");
                        sb.AppendLine("            <td colspan='3'>&nbsp;</td>");
                        sb.AppendLine("        </tr>");
                        sb.AppendLine("        <tr>");
                        sb.AppendLine("            <td style='width: 20px;'>&nbsp;</td>");
                        sb.AppendLine("            <td>");
                        sb.AppendLine("                <table style='width: 100%; background-color: white;' border='0' cellpadding='0' cellspacing='0'>");
                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td colspan='4' style='text-align: center;'>");
                        sb.AppendLine("                            <p>Task Assigned</p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");
                        

                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 40px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td style='width: 100px;' colspan='3'>");
                        sb.AppendLine("                            <p>Kindly review the request below and provide feedback </p>");
                        sb.AppendLine("                        </td>");
                       
                        sb.AppendLine("                    </tr>");


                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 40px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td style='width: 100px;'>");
                        sb.AppendLine("                         <p>   Request No</p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td style='width: 20px;'>");
                        sb.AppendLine("                           <p>:</p> ");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td>");
                        sb.AppendLine("                         <p>   "+reqno.ToString()+ " </p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");

                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 40px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td style='width: 100px;'>");
                        sb.AppendLine("                         <p>     Status </p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td style='width: 20px;'>");
                        sb.AppendLine("                        <p>:</p>   ");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td>");
                        sb.AppendLine("                          "+status+"");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");

                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 40px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td style='width: 100px;'>");
                        sb.AppendLine("                           <p> Valid till</P>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td style='width: 20px;'>");
                        sb.AppendLine("                          <p>:</p>    ");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td>");
                        sb.AppendLine("                         <p>   " + Convert.ToString(DateTime.Now.AddDays(3).ToString("MM'/'dd'/'yyyy")) + "</P>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");


                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td colspan='4' style='text-align: center;'><a style='background-color: #082654; color: white; padding: 10px;' href='http://localhost:4200/urs/userview'>View Task</a></td>");
                        sb.AppendLine("                    </tr>");
                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 20px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td colspan='3' style='text-align: left;'>");
                        sb.AppendLine("                            <p>Note: This is auto generated mail</p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");
                        sb.AppendLine("                </table>");
                        sb.AppendLine("            </td>");
                        sb.AppendLine("            <td style='width: 20px;'>&nbsp;</td>");
                        sb.AppendLine("        </tr>");
                        sb.AppendLine("        <tr>");
                        sb.AppendLine("            <td colspan='3'>&nbsp;</td>");
                        sb.AppendLine("        </tr>");
                        sb.AppendLine("    </table>");
                        
                                                
                        myMail.Body = sb.ToString();
                        // }

                        var client = new SmtpClient()
                        {
                            Port = 587,
                            DeliveryMethod = SmtpDeliveryMethod.Network,
                            UseDefaultCredentials = true,
                            Host = "smtp.gmail.com",
                            EnableSsl = true,
                            Credentials = credentials
                        };
                        client.Send(myMail);
                        success = "Success";
                    }
                    return success;
                }

            }
            catch (Exception ex)
            {

                return "Fail";
            }
        }

        public static string EmailPeerreviewcomplete(Int64 userid, string sub, string status, string levelno, string reqno, string email, Int64 reqId, string url, string username,string ccmail,string time)
        {
            TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById(time);
            DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
            try
            {


                using (dbURSContext db = new dbURSContext())
                {
                    string success = "";
                    //Int32 userid = 1;
                    //string sub = "Peer Review Request#FR-202110-000333 ";
                    //Int32 orgid = 0; string status = "approved"; string levelno = "1";
                    string reason = "ok"; string remark = "tested";
                    //string frmAddress = "thiruvenkatachalam@adventsys.in";
                    //string password = "9944029787123";
                    string frmAddress = "";
                    string password = "";
                    var templateNames = (from MD in db.Maildetails
                                         select new
                                         {
                                             username = MD.Mailaddress,
                                             password = MD.Password,
                                         }).ToList();
                    if (templateNames != null)
                    {
                        foreach (var item in templateNames)
                        {
                            frmAddress = Convert.ToString(item.username);
                            password = Convert.ToString(item.password);
                        }

                        // Credentials
                        var credentials = new NetworkCredential(frmAddress, password);

                        var myMail = new MailMessage()
                        {
                            From = new MailAddress(frmAddress, "UserAccess Mailing Service"),
                            Subject = sub,
                        };

                        string strBodyDetail = "";

                        myMail.To.Add(new MailAddress("thiruvenkatachalam@adventsys.in"));
                        myMail.To.Add(new MailAddress(email));
                        myMail.CC.Add(new MailAddress(ccmail));
          

                        myMail.IsBodyHtml = true;

                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("<table style='width: 1000px;  padding:0; margin: 0 auto; background-color: #f8f9f9; font-size: 12px; font-family: Open Sans,Helvetica,Arial,sans-serif;' border='0' cellpadding='0' cellspacing='0' >");
                        sb.AppendLine("        <tr style='background-color: #082654; color: white; text-align: center; height: 40px'>");
                        sb.AppendLine("            <td colspan='3'>");
                        sb.AppendLine("                <p><b>Risk Assessment Tool</b></p>");
                        sb.AppendLine("            </td>");
                        sb.AppendLine("        </tr>");
                        sb.AppendLine("        <tr>");
                        sb.AppendLine("            <td colspan='3'>&nbsp;</td>");
                        sb.AppendLine("        </tr>");
                        sb.AppendLine("        <tr>");
                        sb.AppendLine("            <td style='width: 20px;'>&nbsp;</td>");
                        sb.AppendLine("            <td>");
                        sb.AppendLine("                <table style='width: 100%; background-color: white;' border='0' cellpadding='0' cellspacing='0'>");
                        //sb.AppendLine("                    <tr>");
                        //sb.AppendLine("                        <td colspan='4' style='text-align: center;'>");
                        //sb.AppendLine("                            <p><b>Request Assigned</b></p>");
                        //sb.AppendLine("                        </td>");
                        //sb.AppendLine("                    </tr>");
                        
                        sb.AppendLine("                    <tr style='height: 40px'>");
                        sb.AppendLine("                        <td style='width: 40px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td  style='width: 100px;' colspan='3'>");
                        sb.AppendLine("                            <p>Peer review of following request is completed:</p>");
                        sb.AppendLine("                        </td>");

                        sb.AppendLine("                    </tr>");

                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 40px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td style='width: 100px;'>");
                        sb.AppendLine("                         <p><b>Request No</p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td style='width: 20px;'>");
                        sb.AppendLine("                           <p><b>:</p> ");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td>");
                        sb.AppendLine("                         <p>   " + reqno.ToString() + " </p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");

                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 40px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td style='width: 100px;'>");
                        sb.AppendLine("                         <p> <b>  Created by</p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td style='width: 20px;'>");
                        sb.AppendLine("                           <p><b>:</p> ");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td>");
                        sb.AppendLine("                         <p>   " + username + " </p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");

                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 40px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td style='width: 100px;'>");
                        sb.AppendLine("                         <p> <b>  Created on</p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td style='width: 20px;'>");
                        sb.AppendLine("                           <p><b>:</p> ");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td>");
                        sb.AppendLine("                         <p>   " + Convert.ToString(dateTime_Indian.ToString("MM'/'dd'/'yyyy")) + " </p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");


                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 40px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td style='width: 100px;'>");
                        sb.AppendLine("                         <p> <b>Status</p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td style='width: 20px;'>");
                        sb.AppendLine("                        <p><b>:</p>   ");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td>");
                        sb.AppendLine("                          " + status + "");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");

                        sb.AppendLine("                    <tr> <td style='width: 20px;height:30px'>&nbsp;</td>");

                        sb.AppendLine("                    </tr>");
                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <tr ><td>    </td>  <td>    </td> <td>    </td> <td>  <table><tr><td> <td colspan='4' style='text-align: left;'><a style='background-color: #082654; color: white; margin-left: 40px; padding:10px;' href='" + url + "urs/request?requestId=" + reqId + "&&userId=" + userid + "'>View Request</a></td> </table></tr></td>    </td> <td>  </td></tr> ");
                        sb.AppendLine("                    </tr>");

                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 20px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td colspan='3' style='text-align: left;'>");
                        sb.AppendLine("                            <p>Regards,</p>  ");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");
                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 20px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td colspan='3' style='text-align: left;'>");
                        sb.AppendLine("                            <p>Admin</p><p><b>Note:</b> This is an auto generated mail. Please do not reply</p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");


                        sb.AppendLine("        <tr>");
                        sb.AppendLine("            <td colspan='3'>&nbsp;</td>");
                        sb.AppendLine("        </tr>");
                        sb.AppendLine("    </table>");

                        myMail.Body = sb.ToString();
                        // }
                        var client = new SmtpClient()
                        {
                            Port = 587,
                            DeliveryMethod = SmtpDeliveryMethod.Network,
                            UseDefaultCredentials = true,
                            Host = "smtp.gmail.com",
                            EnableSsl = true,
                            Credentials = credentials
                        };
                        client.Send(myMail);
                        success = "Success";
                    }
                    return success;
                }
            }
            catch (Exception ex)
            {

                return "Fail";
            }
        }


        public static string EmailRequestCreation(Int64 userid,string sub,string status, string levelno,string reqno,string email,Int64 reqId,string url,string username,string time)
        {
            TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById(time);
            DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
            try
            {
               

                using (dbURSContext db = new dbURSContext())
                {
                    string success = "";
                    //Int32 userid = 1;
                    //string sub = "Peer Review Request#FR-202110-000333 ";
                    //Int32 orgid = 0; string status = "approved"; string levelno = "1";
                    string reason = "ok"; string remark = "tested";
                    //string frmAddress = "thiruvenkatachalam@adventsys.in";
                    //string password = "9944029787123";
                    string frmAddress = "";
                    string password = "";
                    var templateNames = (from MD in db.Maildetails
                                         select new
                                         {
                                             username = MD.Mailaddress,
                                             password = MD.Password,
                                         }).ToList();
                     if (templateNames != null)
                    {
                        foreach (var item in templateNames)
                        {
                            frmAddress = Convert.ToString(item.username);
                            password = Convert.ToString(item.password);
                        }

                        // Credentials
                        var credentials = new NetworkCredential(frmAddress, password);

                        var myMail = new MailMessage()
                        {
                            From = new MailAddress(frmAddress, "UserAccess Mailing Service"),
                            Subject = sub,
                        };

                        string strBodyDetail = "";

                        //foreach (DataRow documentyperow in datarows.Rows)
                        //{
                        //foreach (MailAddress itemuser in lst)
                        //{
                        myMail.To.Add(new MailAddress("thiruvenkatachalam@adventsys.in"));
                        myMail.To.Add(new MailAddress(email));
                        //   string itemuser = "thiruvenkatachalam@adventsys.in";
                        //  string mail2 = "sananth.kumar@adventsys.in";
                       // myMail.To.Add(email);
                      //  myMail.To.Add(mail2);
                     //   myMail.To.Add(itemuser);
                        // }

                        myMail.IsBodyHtml = true;
                       
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("<table style='width: 1000px;  padding:0; margin: 0 auto; background-color: #f8f9f9; font-size: 12px; font-family: Open Sans,Helvetica,Arial,sans-serif;' border='0' cellpadding='0' cellspacing='0' >");
                        sb.AppendLine("        <tr style='background-color: #082654; color: white; text-align: center; height: 40px'>");
                        sb.AppendLine("            <td colspan='3'>");
                        sb.AppendLine("                <p><b>Risk Assessment Tool</b></p>");
                        sb.AppendLine("            </td>");
                        sb.AppendLine("        </tr>");
                        sb.AppendLine("        <tr>");
                        sb.AppendLine("            <td colspan='3'>&nbsp;</td>");
                        sb.AppendLine("        </tr>");
                        sb.AppendLine("        <tr>");
                        sb.AppendLine("            <td style='width: 20px;'>&nbsp;</td>");
                        sb.AppendLine("            <td>");
                        sb.AppendLine("                <table style='width: 100%; background-color: white;' border='0' cellpadding='0' cellspacing='0'>");
                        //sb.AppendLine("                    <tr>");
                        //sb.AppendLine("                        <td colspan='4' style='text-align: center;'>");
                        //sb.AppendLine("                            <p><b>Request Assigned</b></p>");
                        //sb.AppendLine("                        </td>");
                        //sb.AppendLine("                    </tr>");


                        sb.AppendLine("                    <tr style='height: 40px'>");
                        sb.AppendLine("                        <td style='width: 40px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td  style='width: 100px;' colspan='3'>");
                        sb.AppendLine("                            <p>New Request created as per below details:</p>");
                        sb.AppendLine("                        </td>");

                        sb.AppendLine("                    </tr>");

                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 40px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td style='width: 100px;'>");
                        sb.AppendLine("                         <p><b>Request No</p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td style='width: 20px;'>");
                        sb.AppendLine("                           <p><b>:</p> ");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td>");
                        sb.AppendLine("                         <p>   " + reqno.ToString() + " </p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");

                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 40px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td style='width: 100px;'>");
                        sb.AppendLine("                         <p> <b>  Created by</p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td style='width: 20px;'>");
                        sb.AppendLine("                           <p><b>:</p> ");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td>");
                        sb.AppendLine("                         <p>   " + username + " </p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");

                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 40px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td style='width: 100px;'>");
                        sb.AppendLine("                         <p> <b>  Created on</p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td style='width: 20px;'>");
                        sb.AppendLine("                           <p><b>:</p> ");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td>");
                        sb.AppendLine("                         <p>   " + Convert.ToString (dateTime_Indian.ToString("MM'/'dd'/'yyyy")) + " </p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");


                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 40px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td style='width: 100px;'>");
                        sb.AppendLine("                         <p> <b>Status</p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td style='width: 20px;'>");
                        sb.AppendLine("                        <p><b>:</p>   ");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td>");
                        sb.AppendLine("                          " + status + "");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");
                      
                        sb.AppendLine("                    <tr> <td style='width: 20px;height:30px'>&nbsp;</td>");
                         
                        sb.AppendLine("                    </tr>");
                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <tr ><td>    </td>  <td>    </td> <td>    </td> <td>  <table><tr><td> <td colspan='4' style='text-align: left;'><a style='background-color: #082654; color: white; margin-left: 40px; padding:10px;' href='" + url + "urs/request?requestId=" + reqId + "&&userId="+ userid + "'>View Request</a></td> </table></tr></td>    </td> <td>  </td></tr> ");
                        sb.AppendLine("                    </tr>");

                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 20px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td colspan='3' style='text-align: left;'>");
                        sb.AppendLine("                            <p>Regards,</p>  ");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");
                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 20px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td colspan='3' style='text-align: left;'>");
                        sb.AppendLine("                            <p>Admin</p><p><b>Note:</b> This is an auto generated mail. Please do not reply</p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");

                       
                        sb.AppendLine("        <tr>");
                        sb.AppendLine("            <td colspan='3'>&nbsp;</td>");
                        sb.AppendLine("        </tr>");
                        sb.AppendLine("    </table>");

                        myMail.Body = sb.ToString();
                        // }
                        var client = new SmtpClient()
                        {
                            Port = 587,
                            DeliveryMethod = SmtpDeliveryMethod.Network,
                            UseDefaultCredentials = true,
                            Host = "smtp.gmail.com",
                            EnableSsl = true,
                            Credentials = credentials
                        };
                        client.Send(myMail);
                        success = "Success";
                    }
                    return success;
                    }
            }
            catch (Exception ex)
            {
                
                return "Fail";
            }
        }



        //user to manager
        public static string EmailRequestpublishToManager(Int64 userid, string sub, string status, string reqno, dynamic email, Int64 reqId, string url, string ccusercreator,string username,string time)
        {
            TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById(time);
            DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    string success = "";
                    string frmAddress = "";
                    string password = "";
                    var templateNames = (from MD in db.Maildetails
                                         select new
                                         {
                                             username = MD.Mailaddress,
                                             password = MD.Password,
                                         }).ToList();
                    if (templateNames != null)
                    {
                        foreach (var item in templateNames)
                        {
                            frmAddress = Convert.ToString(item.username);
                            password = Convert.ToString(item.password);
                        }

                        // Credentials
                        var credentials = new NetworkCredential(frmAddress, password);

                        var myMail = new MailMessage()
                        {
                            From = new MailAddress(frmAddress, "UserAccess Mailing Service"),
                            Subject = sub,
                        };

                        string strBodyDetail = "";

                         
                        myMail.To.Add(new MailAddress("thiruvenkatachalam@adventsys.in"));
                        foreach (string itemuser in email)
                        {
                            myMail.To.Add(new MailAddress(itemuser));
                        }
                        myMail.CC.Add(ccusercreator);
                        
                        myMail.IsBodyHtml = true;

                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("<table style='width: 1000px;  padding:0; margin: 0 auto; background-color: #f8f9f9; font-size: 12px; font-family: Open Sans,Helvetica,Arial,sans-serif;' border='0' cellpadding='0' cellspacing='0' >");
                        sb.AppendLine("        <tr style='background-color: #082654; color: white; text-align: center; height: 40px'>");
                        sb.AppendLine("            <td colspan='3'>");
                        sb.AppendLine("                <p><b>Risk Assessment Tool</b></p>");
                        sb.AppendLine("            </td>");
                        sb.AppendLine("        </tr>");
                        sb.AppendLine("        <tr>");
                        sb.AppendLine("            <td colspan='3'>&nbsp;</td>");
                        sb.AppendLine("        </tr>");
                        sb.AppendLine("        <tr>");
                        sb.AppendLine("            <td style='width: 20px;'>&nbsp;</td>");
                        sb.AppendLine("            <td>");
                        sb.AppendLine("                <table style='width: 100%; background-color: white;' border='0' cellpadding='0' cellspacing='0'>");
                     
                        sb.AppendLine("                    <tr style='height: 40px'>");
                        sb.AppendLine("                        <td style='width: 40px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td  style='width: 100px;' colspan='3'>");
                        sb.AppendLine("                            <p> The below request has been published and is submitted for manager approval:</p>");
                        sb.AppendLine("                        </td>");

                        sb.AppendLine("                    </tr>");
                        sb.AppendLine("                    <tr> <td style='width: 20px;height:30px'>&nbsp;</td>");
                        sb.AppendLine("                    </tr>");

                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 40px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td style='width: 100px;'>");
                        sb.AppendLine("                         <p><b>Request No</p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td style='width: 20px;'>");
                        sb.AppendLine("                           <p><b>:</p> ");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td>");
                        sb.AppendLine("                         <p>   " + reqno.ToString() + " </p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");

                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 40px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td style='width: 100px;'>");
                        sb.AppendLine("                         <p> <b>  Created by</p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td style='width: 20px;'>");
                        sb.AppendLine("                           <p><b>:</p> ");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td>");
                        sb.AppendLine("                         <p>   " + username + " </p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");

                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 40px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td style='width: 100px;'>");
                        sb.AppendLine("                         <p> <b>  Created on</p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td style='width: 20px;'>");
                        sb.AppendLine("                           <p><b>:</p> ");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td>");
                        sb.AppendLine("                         <p>   " + Convert.ToString(dateTime_Indian.ToString("MM'/'dd'/'yyyy")) + " </p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");


                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 40px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td style='width: 100px;'>");
                        sb.AppendLine("                         <p> <b>Status</p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td style='width: 20px;'>");
                        sb.AppendLine("                        <p><b>:</p>   ");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td>");
                        sb.AppendLine("                          " + status + "");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");

                        sb.AppendLine("                    <br>");
                        sb.AppendLine("                    <tr> <td style='width: 20px;height:30px'>&nbsp;</td>");
                        sb.AppendLine("                    </tr>");
                       
                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <tr ><td>    </td>  <td>    </td> <td>    </td> <td>  <table><tr><td> <td colspan='4' style='text-align: left;'><a style='background-color: #082654; color: white; margin-left: 40px; padding:10px;' href='" + url + "urs/viewrequest?requestId=" + reqId + "&view=manager'>View Request</a></td> </table></tr></td>    </td> <td>  </td></tr> ");
                        sb.AppendLine("                    </tr>");

                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 20px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td colspan='3' style='text-align: left;'>");
                        sb.AppendLine("                            <p>Regards,</p>  ");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");
                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 20px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td colspan='3' style='text-align: left;'>");
                        sb.AppendLine("                            <p>Admin</p><p><b>Note:</b> This is an auto generated mail. Please do not reply</p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");
                        sb.AppendLine("        <tr>");
                        sb.AppendLine("            <td colspan='3'>&nbsp;</td>");
                        sb.AppendLine("        </tr>");
                        sb.AppendLine("    </table>");

                        myMail.Body = sb.ToString();
                        // }
                        var client = new SmtpClient()
                        {
                            Port = 587,
                            DeliveryMethod = SmtpDeliveryMethod.Network,
                            UseDefaultCredentials = true,
                            Host = "smtp.gmail.com",
                            EnableSsl = true,
                            Credentials = credentials
                        };
                        client.Send(myMail);
                        success = "Success";
                    }
                    return success;
                }
            }
            catch (Exception ex)
            {

                return "Fail";
            }
        }

        public static string EmailRequestToPeerreview(Int64 userid, string sub, string status, string levelno,
            string reqno, dynamic email, Int64 reqId, string url, string username,string emailcctocreator,Int64 peerreviewid,string time)
        {
            TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById(time);
            DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    string success = "";
                    string frmAddress = "";
                    string password = "";
                    var templateNames = (from MD in db.Maildetails
                                         select new
                                         {
                                             username = MD.Mailaddress,
                                             password = MD.Password,
                                         }).ToList();
                    if (templateNames != null)
                    {
                        foreach (var item in templateNames)
                        {
                            frmAddress = Convert.ToString(item.username);
                            password = Convert.ToString(item.password);
                        }

                        // Credentials
                        var credentials = new NetworkCredential(frmAddress, password);

                        var myMail = new MailMessage()
                        {
                            From = new MailAddress(frmAddress, "UserAccess Mailing Service"),
                            Subject = sub,
                        };
                        
                        myMail.To.Add(new MailAddress("thiruvenkatachalam@adventsys.in"));
                        foreach (string itemuser in email)
                        {
                            myMail.To.Add(new MailAddress(itemuser));
                        }
                        myMail.CC.Add(new MailAddress(emailcctocreator));

                        myMail.IsBodyHtml = true;

                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("<table style='width: 1000px; margin: 0 auto; background-color: #f8f9f9; font-size: 12px; font-family: Open Sans,Helvetica,Arial,sans-serif;' border='0' cellpadding='0' cellspacing='0'>");
                        sb.AppendLine("        <tr style='background-color: #082654; color: white; text-align: center; height: 40px'>");
                        sb.AppendLine("            <td colspan='3'>");
                        sb.AppendLine("                <p><b>Risk Assessment Tool</b></p>");
                        sb.AppendLine("            </td>");
                        sb.AppendLine("        </tr>");
                        sb.AppendLine("        <tr>");
                        sb.AppendLine("            <td colspan='3'>&nbsp;</td>");
                        sb.AppendLine("        </tr>");
                        sb.AppendLine("        <tr>");
                        sb.AppendLine("            <td style='width: 20px;'>&nbsp;</td>");
                        sb.AppendLine("            <td>");
                        sb.AppendLine("                <table style='width: 100%; background-color: white;' border='0' cellpadding='0' cellspacing='0'>");
                  

                        sb.AppendLine("                    <tr style='height: 40px'>");
                        sb.AppendLine("                        <td style='width: 40px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td  style='width: 100px;' colspan='3'>");
                        sb.AppendLine("                            <p>Please review the request created as per below details:</p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");
                        sb.AppendLine("                    <tr> <td style='width: 20px;height:30px'>&nbsp;</td>");
                        sb.AppendLine("                    </tr>");

                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 40px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td style='width: 100px;'>");
                        sb.AppendLine("                         <p>  <b> Request No</p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td style='width: 20px;'>");
                        sb.AppendLine("                           <p><b>:</p> ");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td>");
                        sb.AppendLine("                         <p>   " + reqno.ToString() + " </p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");

                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 40px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td style='width: 100px;'>");
                        sb.AppendLine("                         <p>  <b> Created by</p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td style='width: 20px;'>");
                        sb.AppendLine("                           <p><b>:</p> ");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td>");
                        sb.AppendLine("                         <p>   " + username + " </p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");

                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 40px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td style='width: 100px;'>");
                        sb.AppendLine("                         <p>  <b> Created on</p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td style='width: 20px;'>");
                        sb.AppendLine("                           <p><b>:</p> ");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td>");
                        sb.AppendLine("                         <p>   " + Convert.ToString(dateTime_Indian.ToString("MM'/'dd'/'yyyy")) + " </p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");


                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 40px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td style='width: 100px;'>");
                        sb.AppendLine("                         <p> <b> Status </p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td style='width: 20px;'>");
                        sb.AppendLine("                        <p><b>:</p>   ");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td>");
                        sb.AppendLine("                          " + status + "");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");

                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 40px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td style='width: 100px;'>");
                        sb.AppendLine("                           <p> <b> Valid till</P>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td style='width: 20px;'>");
                        sb.AppendLine("                          <p><b>:</p>    ");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td>");
                        sb.AppendLine("                         <p>   " + Convert.ToString(dateTime_Indian.AddDays(3).ToString("MM'/'dd'/'yyyy")) + "</P>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");

                        sb.AppendLine("                    <br>");
                        sb.AppendLine("                    <tr> <td style='width: 20px;height:30px'>&nbsp;</td>");
                        sb.AppendLine("                    </tr>");
                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <tr ><td>    </td> <td> <td colspan='4' style='text-align: left;'><a style='background-color: #082654; color: white; margin-left: 40px; padding:10px;' href='" + url + "urs/request?requestId=" + reqId + "&&userId="+ peerreviewid + "'>View Request</a></td> </td> <td>  </td></tr> ");
                        sb.AppendLine("                    </tr>");

                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 20px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td colspan='3' style='text-align: left;'>");
                        sb.AppendLine("                            <p>Regards,</p>  ");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");
                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 20px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td colspan='3' style='text-align: left;'>");
                        sb.AppendLine("                            <p>Admin</p><p><b>Note:</b> This is an auto generated mail. Please do not reply</p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");

                        sb.AppendLine("        <tr>");
                        sb.AppendLine("            <td colspan='3'>&nbsp;</td>");
                        sb.AppendLine("        </tr>");
                        sb.AppendLine("    </table>");

                        
                        myMail.Body = sb.ToString();
                        // }
                        var client = new SmtpClient()
                        {
                            Port = 587,
                            DeliveryMethod = SmtpDeliveryMethod.Network,
                            UseDefaultCredentials = true,
                            Host = "smtp.gmail.com",
                            EnableSsl = true,
                            Credentials = credentials
                        };
                        client.Send(myMail);
                        success = "Success";
                    }
                    return success;
                }
            }
            catch (Exception ex)
            {

                return "Fail";
            }
        }
        //Manager to network
        public static string EmailRequestToPublish(Int64 userid, string sub, string status, string levelno, string reqno,dynamic lst,string url,Int64 requestid,string username,string emailcctocreator,Int64 reqId,string comment,string time)
        {
            TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById(time);
            DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    string success = "";
                                      
                    string frmAddress = "";
                    string password = "";
                    var templateNames = (from MD in db.Maildetails
                                         select new
                                         {
                                             username = MD.Mailaddress,
                                             password = MD.Password,
                                         }).ToList();
                      if (templateNames != null)
                      {
                        foreach (var item in templateNames)
                        {
                            frmAddress = Convert.ToString(item.username);
                            password = Convert.ToString(item.password);
                        }

                        // Credentials
                        var credentials = new NetworkCredential(frmAddress, password);
                        var myMail = new MailMessage()
                        {
                            From = new MailAddress(frmAddress, "UserAccess Mailing Service"),
                            Subject = sub,
                        };
                         
                        string strBodyDetail = "";
                        foreach (string itemuser in lst)
                        {
                           myMail.To.Add(new MailAddress(itemuser));
                        }
                        myMail.CC.Add(emailcctocreator);

                        myMail.IsBodyHtml = true;
                        string strHeadr = "";

                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("<table style='width: 1000px; margin: 0 auto; background-color: #f8f9f9; font-size: 12px; font-family: Open Sans,Helvetica,Arial,sans-serif;' border='0' cellpadding='0' cellspacing='0' height: 40px'>");
                        sb.AppendLine("        <tr style='background-color: #082654; color: white; text-align: center;'>");
                        sb.AppendLine("            <td colspan='3'>");
                        sb.AppendLine("                <p><b>Risk Assessment Tool</b></p>");
                        sb.AppendLine("            </td>");
                        sb.AppendLine("        </tr>");
                        sb.AppendLine("        <tr>");
                        sb.AppendLine("            <td colspan='3'>&nbsp;</td>");
                        sb.AppendLine("        </tr>");
                        sb.AppendLine("        <tr>");
                        sb.AppendLine("            <td style='width: 20px;'>&nbsp;</td>");
                        sb.AppendLine("            <td>");
                        sb.AppendLine("                <table style='width: 100%; background-color: white;' border='0' cellpadding='0' cellspacing='0'>");
                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 40px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td style='width: 100px;' colspan='3'>");
                        sb.AppendLine("                            <p>The below request has been approved and is submitted for network admin groups: </p>");
                        sb.AppendLine("                        </td>");

                        sb.AppendLine("                    </tr>");

                        sb.AppendLine("                    <tr> <td style='width: 20px;height:30px'>&nbsp;</td>");
                        sb.AppendLine("                    </tr>");


                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 40px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td style='width: 100px;'>");
                        sb.AppendLine("                         <p>  <b> Request No</p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td style='width: 20px;'>");
                        sb.AppendLine("                           <p><b>:</p> ");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td>");
                        sb.AppendLine("                         <p>   " + reqno.ToString() + " </p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");

                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 40px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td style='width: 100px;'>");
                        sb.AppendLine("                         <p> <b>  Created by</b></p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td style='width: 20px;'>");
                        sb.AppendLine("                           <p><b>:</p> ");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td>");
                        sb.AppendLine("                         <p>   " + username + " </p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");

                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 40px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td style='width: 100px;'>");
                        sb.AppendLine("                         <p>  <b> Created on</p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td style='width: 20px;'>");
                        sb.AppendLine("                           <p><b>:</p> ");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td>");
                        sb.AppendLine("                         <p>   " + Convert.ToString(dateTime_Indian.ToString("MM'/'dd'/'yyyy")) + " </p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");

                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 40px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td style='width: 100px;'>");
                        sb.AppendLine("                         <p> <b>  Status</p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td style='width: 20px;'>");
                        sb.AppendLine("                        <p><b>:</p>   ");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td>");
                        sb.AppendLine("                          Manager Approval");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");

                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 40px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td style='width: 100px;'>");
                        sb.AppendLine("                           <p><b> Valid till</b></P>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td style='width: 20px;'>");
                        sb.AppendLine("                          <p><b>:</p>    ");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td>");
                        sb.AppendLine("                         <p>   " + Convert.ToString(dateTime_Indian.AddDays(3).ToString("MM'/'dd'/'yyyy")) + "</P>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");

                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 40px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td style='width: 100px;'>");
                        sb.AppendLine("                         <p> <b> Comment</p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td style='width: 20px;'>");
                        sb.AppendLine("                        <p><b>:</p>   ");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td>");
                        sb.AppendLine("                          " + comment + "");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");


                        sb.AppendLine("                    <br>");
                        sb.AppendLine("                    <tr> <td style='width: 20px;height:30px'>&nbsp;</td>");
                        sb.AppendLine("                    </tr>");
                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <tr ><td>    </td> <td> <td colspan='4' style='text-align: left;'><a style='background-color: #082654; color: white; margin-left: 40px; padding:10px;'href='" + url + "urs/networkrequest?requestId=" + reqId + "&view=network'>View Request</a></td> </td> <td>  </td></tr> ");
                        sb.AppendLine("                    </tr>");

                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 20px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td colspan='3' style='text-align: left;'>");
                        sb.AppendLine("                            <p>Regards,</p>  ");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");
                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 20px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td colspan='3' style='text-align: left;'>");
                        sb.AppendLine("                            <p>Admin</p><p><b>Note:</b> This is an auto generated mail. Please do not reply</p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");

                        sb.AppendLine("        <tr>");
                        sb.AppendLine("            <td colspan='3'>&nbsp;</td>");
                        sb.AppendLine("        </tr>");
                        sb.AppendLine("    </table>");

                      
                        myMail.Body = sb.ToString();
                    // }

                        var client = new SmtpClient()
                        {
                            Port = 587,
                            DeliveryMethod = SmtpDeliveryMethod.Network,
                            UseDefaultCredentials = true,
                            Host = "smtp.gmail.com",
                            EnableSsl = true,
                            Credentials = credentials
                        };
                        client.Send(myMail);
                        success = "Success";
                      }
                    return success;
                }

            }
            catch (Exception ex)
            {

                return "Fail";
            }
        }

        public static string EmailManagerToIT(Int64 userid, string sub, string status, string comment, string reqno, dynamic lst,Int64 reqId,string url,string emailtocccreator,string username,string time)
        {
            TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById(time);
            DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    string success = "";
                    //Int32 userid = 1;
                    //string sub = "Peer Review Request#FR-202110-000333 ";
                    //Int32 orgid = 0; string status = "approved"; string levelno = "1";
                    string reason = "ok"; string remark = "tested";
                    string frmAddress = "";
                    string password = "";
                    var templateNames = (from MD in db.Maildetails

                                         select new
                                         {
                                             username = MD.Mailaddress,
                                             password = MD.Password,
                                         }).ToList();
                    if (templateNames != null)
                    {
                        foreach (var item in templateNames)
                        {
                            frmAddress = Convert.ToString(item.username);
                            password = Convert.ToString(item.password);
                        }

                        // Credentials
                        var credentials = new NetworkCredential(frmAddress, password);
                        var myMail = new MailMessage()
                        {
                            From = new MailAddress(frmAddress, "UserAccess Mailing Service"),
                            Subject = sub,
                        };

                        string strBodyDetail = "";
                        foreach (string itemuser in lst)
                        {
                            myMail.To.Add("thiruvenkatachalam@adventsys.in");
                            myMail.To.Add(new MailAddress(itemuser));
                        }
                        myMail.CC.Add(emailtocccreator);
                        myMail.IsBodyHtml = true;
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("<table style='width: 1000px; margin: 0 auto; background-color: #f8f9f9; font-size: 12px; font-family: Open Sans,Helvetica,Arial,sans-serif;' border='0' cellpadding='0' cellspacing='0' height: 40px'>"); 
                        sb.AppendLine("<tr style='background-color: #082654; color: white; text-align: center;'>");
                        sb.AppendLine("    <td colspan='3'>");
                        sb.AppendLine("     <p><b>Risk Assessment Tool</b></p>");
                        sb.AppendLine("   </td>");
                        sb.AppendLine("</tr>");
                        sb.AppendLine("        <tr>");
                        sb.AppendLine("            <td colspan='3'>&nbsp;</td>");
                        sb.AppendLine("        </tr>");
                        sb.AppendLine("        <tr>");
                        sb.AppendLine("            <td style='width: 20px;'>&nbsp;</td>");
                        sb.AppendLine("            <td>");
                        sb.AppendLine("                <table style='width: 100%; background-color: white;' border='0' cellpadding='0' cellspacing='0'>");
                     


                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 40px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td style='width: 100px;' colspan='3'>");
                        sb.AppendLine("                            <p>The below request has been approved by Network admin groups and is submitted for Security admin groups: </p>");
                        sb.AppendLine("                        </td>");

                        sb.AppendLine("                    </tr>");

                        sb.AppendLine("                    <tr> <td style='width: 20px;height:30px'>&nbsp;</td>");
                        sb.AppendLine("                    </tr>");

                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 40px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td style='width: 100px;'>");
                        sb.AppendLine("                         <p> <b>  Request No</p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td style='width: 20px;'>");
                        sb.AppendLine("                           <p><b>:</p> ");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td>");
                        sb.AppendLine("                         <p>   " + reqno.ToString() + " </p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");

                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 40px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td style='width: 100px;'>");
                        sb.AppendLine("                         <p> <b>  Created by</p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td style='width: 20px;'>");
                        sb.AppendLine("                           <p><b>:</p> ");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td>");
                        sb.AppendLine("                         <p>   " + username + " </p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");

                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 40px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td style='width: 100px;'>");
                        sb.AppendLine("                         <p>  <b> Created on</p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td style='width: 20px;'>");
                        sb.AppendLine("                           <p><b>:</b></p> ");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td>");
                        sb.AppendLine("                         <p>   " + Convert.ToString(dateTime_Indian.ToString("MM'/'dd'/'yyyy")) + " </p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");

                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 40px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td style='width: 100px;'>");
                        sb.AppendLine("                         <p> <b> Status</b> </p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td style='width: 20px;'>");
                        sb.AppendLine("                        <p><b>:</p>   ");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td>");
                        sb.AppendLine("                          " + status + "");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");
                        
                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 40px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td style='width: 100px;'>");
                        sb.AppendLine("                           <p><b> Comments</b></P>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td style='width: 20px;'>");
                        sb.AppendLine("                          <p><b>:</p>    ");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td>");
                        sb.AppendLine("                         <p>   " + comment + "</P>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");

                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 40px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td style='width: 100px;'>");
                        sb.AppendLine("                           <p> <b>Valid till</b></P>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td style='width: 20px;'>");
                        sb.AppendLine("                          <p><b>:</p>    ");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td>");
                        sb.AppendLine("                         <p>   " + Convert.ToString(dateTime_Indian.AddDays(3).ToString("MM'/'dd'/'yyyy")) + "</P>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");
                        
                        sb.AppendLine("                    <br>");
                        sb.AppendLine("                    <tr> <td style='width: 20px;height:30px'>&nbsp;</td>");
                        sb.AppendLine("                    </tr>");
                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <tr ><td>    </td> <td> <td colspan='4' style='text-align: left;'><a style='background-color: #082654; color: white; margin-left: 40px; padding:10px;'href='" + url + "urs/itrequest?requestId=" + reqId + "&&view=IT'>View Request</a></td> </td> <td>  </td></tr> ");
                        sb.AppendLine("                    </tr>");

                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 20px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td colspan='3' style='text-align: left;'>");
                        sb.AppendLine("                            <p>Regards,</p>  ");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");
                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 20px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td colspan='3' style='text-align: left;'>");
                        sb.AppendLine("                            <p>Admin</p><p><b>Note:</b> This is an auto generated mail. Please do not reply</p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");
                        sb.AppendLine("        <tr>");
                        sb.AppendLine("            <td colspan='3'>&nbsp;</td>");
                        sb.AppendLine("        </tr>");
                        sb.AppendLine("    </table>");


                        myMail.Body = sb.ToString();
                       
                        // }

                        var client = new SmtpClient()
                        {
                            Port = 587,
                            DeliveryMethod = SmtpDeliveryMethod.Network,
                            UseDefaultCredentials = true,
                            Host = "smtp.gmail.com",
                            EnableSsl = true,
                            Credentials = credentials
                        };
                        client.Send(myMail);
                        success = "Success";
                    }
                    return success;
                }
            }
            catch (Exception ex)
            {
                return "Fail";
            }
        }

        public static string EmailRejectManagerandIT(Int64 userid, string sub, string status,  string reqno, string comment, dynamic lst, Int64 reqId, string url,string managerorIT,string username,string time,Int64? createuserId)
        {
            TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById(time);
            DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    string success = "";
               
                    string frmAddress = "";
                    string password = "";
                    var templateNames = (from MD in db.Maildetails

                                         select new
                                         {
                                             username = MD.Mailaddress,
                                             password = MD.Password,
                                         }).ToList();
                    if (templateNames != null)
                    {
                        foreach (var item in templateNames)
                        {
                            frmAddress = Convert.ToString(item.username);
                            password = Convert.ToString(item.password);
                        }

                        // Credentials
                        var credentials = new NetworkCredential(frmAddress, password);
                        var myMail = new MailMessage()
                        {
                            From = new MailAddress(frmAddress, "UserAccess Mailing Service"),
                            Subject = sub,
                        };

                        string strBodyDetail = "";
                        foreach (string itemuser in lst)
                        {
                            myMail.To.Add("thiruvenkatachalam@adventsys.in");
                            myMail.To.Add(new MailAddress(itemuser));
                        }
                       
                        myMail.IsBodyHtml = true;
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("<table style='width: 1000px; margin: 0 auto; background-color: #f8f9f9; font-size: 12px; font-family: Open Sans,Helvetica,Arial,sans-serif;' border='0' cellpadding='0' cellspacing='0' height: 40px'>");
                        sb.AppendLine("<tr style='background-color: #082654; color: white; text-align: center;'>");
                        sb.AppendLine("    <td colspan='3'>");
                        sb.AppendLine("     <p><b>Risk Assessment Tool</b></p>");
                        sb.AppendLine("   </td>");
                        sb.AppendLine("</tr>");
                        sb.AppendLine("        <tr>");
                        sb.AppendLine("            <td colspan='3'>&nbsp;</td>");
                        sb.AppendLine("        </tr>");
                        sb.AppendLine("        <tr>");
                        sb.AppendLine("            <td style='width: 20px;'>&nbsp;</td>");
                        sb.AppendLine("            <td>");
                        sb.AppendLine("                <table style='width: 100%; background-color: white;' border='0' cellpadding='0' cellspacing='0'>");
               

                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 40px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td style='width: 100px;' colspan='3'>");
                        if (managerorIT == "Manager")
                        {
                            sb.AppendLine("                            <p>The below request has been rejected by manager with comments. </p>");
                        }
                        else
                        {
                            sb.AppendLine("                            <p>The below request has been rejected by IT team with comments. </p> ");
                        }
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");

                        sb.AppendLine("                    <tr> <td style='width: 20px;height:30px'>&nbsp;</td>");
                        sb.AppendLine("                    </tr>");

                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 40px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td style='width: 100px;'>");
                        sb.AppendLine("                         <p> <b>  Request No</b></p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td style='width: 20px;'>");
                        sb.AppendLine("                           <p><b>:</b></p> ");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td>");
                        sb.AppendLine("                         <p>   " + reqno.ToString() + " </p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");

                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 40px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td style='width: 100px;'>");
                        sb.AppendLine("                         <p>  <b> Created by </b></p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td style='width: 20px;'>");
                        sb.AppendLine("                           <p><b>:</p> ");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td>");
                        sb.AppendLine("                         <p>   " + username + " </p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");

                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 40px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td style='width: 100px;'>");
                        sb.AppendLine("                         <p> <b>  Created on</b></p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td style='width: 20px;'>");
                        sb.AppendLine("                           <p><b>:</p> ");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td>");
                        sb.AppendLine("                         <p>" + Convert.ToString(dateTime_Indian.ToString("MM'/'dd'/'yyyy")) + " </p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");

                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 40px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td style='width: 100px;'>");
                        sb.AppendLine("                         <p>  <b>   Status</b></p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td style='width: 20px;'>");
                        sb.AppendLine("                        <p><b>:</b></p>   ");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td>");
                        sb.AppendLine("                          " + status + "");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");

                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 40px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td style='width: 100px;'>");
                        sb.AppendLine("                           <p><b> Comments</b></P>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td style='width: 20px;'>");
                        sb.AppendLine("                          <p><b>:</p>    ");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td>");
                        sb.AppendLine("                         <p>   " + comment + "</P>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");

                        sb.AppendLine("                    <br>");
                        sb.AppendLine("                    <tr> <td style='width: 20px;height:30px'>&nbsp;</td>");
                        sb.AppendLine("                    </tr>");
                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <tr ><td>    </td> <td> <td colspan='4' style='text-align: left;'><a style='background-color: #082654; color: white; margin-left: 40px; padding:10px;'href='" + url + "urs/request?requestId=" + reqId + "&&userId="+createuserId+"'>View Request</a></td> </td> <td>  </td></tr> ");
                        sb.AppendLine("                    </tr>");

                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 20px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td colspan='3' style='text-align: left;'>");
                        sb.AppendLine("                            <p>Regards,</p>  ");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");
                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 20px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td colspan='3' style='text-align: left;'>");
                        sb.AppendLine("                            <p>Admin</p><p><b>Note:</b> This is an auto generated mail. Please do not reply</p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");
                        sb.AppendLine("        <tr>");
                        sb.AppendLine("            <td colspan='3'>&nbsp;</td>");
                        sb.AppendLine("        </tr>");
                        sb.AppendLine("    </table>");
                                                                      

                        myMail.Body = sb.ToString();

                        // }

                        var client = new SmtpClient()
                        {
                            Port = 587,
                            DeliveryMethod = SmtpDeliveryMethod.Network,
                            UseDefaultCredentials = true,
                            Host = "smtp.gmail.com",
                            EnableSsl = true,
                            Credentials = credentials
                        };
                        client.Send(myMail);
                        success = "Success";
                    }
                    return success;
                }

            }
            catch (Exception ex)
            {

                return "Fail";
            }
        }

        public static string EmailClosedManagerorIT(Int64 userid, string sub, string status, string comment, string reqno, dynamic lst, Int64 reqId, string url, string managerorIT,string emailtoname,string time,Int64? createuserid)
        {
            TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById(time);
            DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    string success = "";

                    string frmAddress = "";
                    string password = "";
                    var templateNames = (from MD in db.Maildetails

                                         select new
                                         {
                                             username = MD.Mailaddress,
                                             password = MD.Password,
                                         }).ToList();
                    if (templateNames != null)
                    {
                        foreach (var item in templateNames)
                        {
                            frmAddress = Convert.ToString(item.username);
                            password = Convert.ToString(item.password);
                        }

                        // Credentials
                        var credentials = new NetworkCredential(frmAddress, password);
                        var myMail = new MailMessage()
                        {
                            From = new MailAddress(frmAddress, "UserAccess Mailing Service"),
                            Subject = sub,
                        };

                        string strBodyDetail = "";
                        foreach (string itemuser in lst)
                        {
                            myMail.To.Add("thiruvenkatachalam@adventsys.in");
                            myMail.To.Add(new MailAddress(itemuser));
                        }

                        myMail.IsBodyHtml = true;
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("<table style='width: 1000px; margin: 0 auto; background-color: #f8f9f9; font-size: 12px; font-family: Open Sans,Helvetica,Arial,sans-serif;' border='0' cellpadding='0' cellspacing='0' height: 40px'>");
                        sb.AppendLine("<tr style='background-color: #082654; color: white; text-align: center;'>");
                        sb.AppendLine("    <td colspan='3'>");
                        sb.AppendLine("     <p><b>Risk Assessment Tool</b></p>");
                        sb.AppendLine("   </td>");
                        sb.AppendLine("</tr>");
                        sb.AppendLine("        <tr>");
                        sb.AppendLine("            <td colspan='3'>&nbsp;</td>");
                        sb.AppendLine("        </tr>");
                        sb.AppendLine("        <tr>");
                        sb.AppendLine("            <td style='width: 20px;'>&nbsp;</td>");
                        sb.AppendLine("            <td>");
                        sb.AppendLine("                <table style='width: 100%; background-color: white;' border='0' cellpadding='0' cellspacing='0'>");
                     

                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 40px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td style='width: 100px;' colspan='3'>");
                        if (managerorIT == "Manager")
                        {
                            sb.AppendLine("                            <p>The below request has been closed by manager with comments. </p> <p>Kindly make changes and resubmit.</p>");
                        }
                        else
                        {
                            sb.AppendLine("                            <p>The below request has been rejected by IT team with comments. </p>");
                        }
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");

                        sb.AppendLine("                    <tr> <td style='width: 20px;height:30px'>&nbsp;</td>");
                        sb.AppendLine("                    </tr>");

                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 40px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td style='width: 100px;'>");
                        sb.AppendLine("                         <p> <b>  Request No</b></p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td style='width: 20px;'>");
                        sb.AppendLine("                           <p><b>:</p> ");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td>");
                        sb.AppendLine("                         <p>   " + reqno.ToString() + " </p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");

                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 40px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td style='width: 100px;'>");
                        sb.AppendLine("                         <p>  <b> Created by</b></p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td style='width: 20px;'>");
                        sb.AppendLine("                           <p><b>:</b></p> ");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td>");
                        sb.AppendLine("                         <p>   "+ emailtoname + " </p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");

                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 40px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td style='width: 100px;'>");
                        sb.AppendLine("                         <p> <b>   Created on</b></p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td style='width: 20px;'>");
                        sb.AppendLine("                           <p><b>:</p> ");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td>");
                        sb.AppendLine("                         <p>   " + Convert.ToString(dateTime_Indian.ToString("MM'/'dd'/'yyyy")) + " </p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");

                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 40px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td style='width: 100px;'>");
                        sb.AppendLine("                         <p>   <b>  Status</b></p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td style='width: 20px;'>");
                        sb.AppendLine("                        <p><b>:</b></p>   ");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td>");
                        sb.AppendLine("                          " + status + "");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");
                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 40px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td style='width: 100px;'>");
                        sb.AppendLine("                           <p><b> Comments</b></P>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td style='width: 20px;'>");
                        sb.AppendLine("                          <p><b>:</p>    ");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td>");
                        sb.AppendLine("                         <p>   " + comment + "</P>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");
                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                    <br>");
                        sb.AppendLine("                    <tr> <td style='width: 20px;height:30px'>&nbsp;</td>");
                        sb.AppendLine("                    </tr>");

                        sb.AppendLine("                        <tr ><td>    </td> <td> <td colspan='4' style='text-align: left;'><a style='background-color: #082654; color: white; margin-left: 40px; padding:10px;'href='" + url + "urs/request?requestId=" + reqId + "&&userId="+createuserid+"'>View Request</a></td> </td> <td>  </td></tr> ");
                        sb.AppendLine("                    </tr>");

                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 20px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td colspan='3' style='text-align: left;'>");
                        sb.AppendLine("                            <p>Regards,</p>  ");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");
                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 20px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td colspan='3' style='text-align: left;'>");
                        sb.AppendLine("                            <p>Admin</p><p><b>Note:</b> This is an auto generated mail. Please do not reply</p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");

                        sb.AppendLine("        <tr>");
                        sb.AppendLine("            <td colspan='3'>&nbsp;</td>");
                        sb.AppendLine("        </tr>");
                        sb.AppendLine("    </table>");


                      

                        myMail.Body = sb.ToString();

                        // }

                        var client = new SmtpClient()
                        {
                            Port = 587,
                            DeliveryMethod = SmtpDeliveryMethod.Network,
                            UseDefaultCredentials = true,
                            Host = "smtp.gmail.com",
                            EnableSsl = true,
                            Credentials = credentials
                        };
                        client.Send(myMail);
                        success = "Success";
                    }
                    return success;
                }

            }
            catch (Exception ex)
            {

                return "Fail";
            }
        }
        public static string EmailITDecesion(Int64 userid, string sub, string status, string levelno, string reqno, dynamic lst,string url,string comment,string username,Int64 reqId,Int64 creatoruserid,string time)
        {
            TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById(time);
            DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
            try
            {
                using (dbURSContext db = new dbURSContext())
                {
                    string success = "";
                    //Int32 userid = 1;
                    //string sub = "Peer Review Request#FR-202110-000333 ";
                    //Int32 orgid = 0; string status = "approved"; string levelno = "1";
                    string reason = "ok"; string remark = "tested";
                    string frmAddress = "";
                    string password = "";
                    var templateNames = (from MD in db.Maildetails

                                         select new
                                         {
                                             username = MD.Mailaddress,
                                             password = MD.Password,
                                         }).ToList();
                    if (templateNames != null)
                    {
                        foreach (var item in templateNames)
                        {
                            frmAddress = Convert.ToString(item.username);
                            password = Convert.ToString(item.password);
                        }

                        // Credentials
                        var credentials = new NetworkCredential(frmAddress, password);
                        var myMail = new MailMessage()
                        {
                            From = new MailAddress(frmAddress, "UserAccess Mailing Service"),
                            Subject = sub,
                        };

                        string strBodyDetail = "";
                        foreach (string itemuser in lst)
                        {
                            myMail.To.Add(new MailAddress(itemuser));
                        }

                        myMail.IsBodyHtml = true;
                        string strHeadr = "";


                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("<table style='width: 1000px; margin: 0 auto; background-color: #f8f9f9; font-size: 12px; font-family: Open Sans,Helvetica,Arial,sans-serif;' border='0' cellpadding='0' cellspacing='0'  height: 40px'>");
                        sb.AppendLine("        <tr style='background-color: #082654; color: white; text-align: center;'>");
                        sb.AppendLine("            <td colspan='3'>");
                        sb.AppendLine("                <p><b>Risk Assessment Tool</b></p>");
                        sb.AppendLine("            </td>");
                        sb.AppendLine("        </tr>");
                        sb.AppendLine("        <tr>");
                        sb.AppendLine("            <td colspan='3'>&nbsp;</td>");
                        sb.AppendLine("        </tr>");
                        sb.AppendLine("        <tr>");
                        sb.AppendLine("            <td style='width: 20px;'>&nbsp;</td>");
                        sb.AppendLine("            <td>");
                        sb.AppendLine("                <table style='width: 100%; background-color: white;' border='0' cellpadding='0' cellspacing='0'>");
                  
                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 40px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td style='width: 100px;' colspan='3'>");
                        sb.AppendLine("                            <p>The below request has been approved by Security admin groups and will be soon completed.  </p>");
                        sb.AppendLine("                        </td>");

                        sb.AppendLine("                    </tr>");

                        sb.AppendLine("                    <tr> <td style='width: 20px;height:30px'>&nbsp;</td>");
                        sb.AppendLine("                    </tr>");


                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 40px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td style='width: 100px;'>");
                        sb.AppendLine("                         <p>  <b> Request No</b></p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td style='width: 20px;'>");
                        sb.AppendLine("                           <p><b>:</p> ");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td>");
                        sb.AppendLine("                         <p>   " + reqno.ToString() + " </p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");

                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 40px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td style='width: 100px;'>");
                        sb.AppendLine("                         <p>  <b> Created by</b></p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td style='width: 20px;'>");
                        sb.AppendLine("                           <p><b>:</p> ");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td>");
                        sb.AppendLine("                         <p>   " + username + " </p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");

                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 40px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td style='width: 100px;'>");
                        sb.AppendLine("                         <p> <b>  Created on</b></p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td style='width: 20px;'>");
                        sb.AppendLine("                           <p><b>:</b></p> ");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td>");
                        sb.AppendLine("                         <p>   " + Convert.ToString(dateTime_Indian.ToString("MM'/'dd'/'yyyy")) + " </p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");
                        
                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 40px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td style='width: 100px;'>");
                        sb.AppendLine("                         <p>  <b>   Status</b></p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td style='width: 20px;'>");
                        sb.AppendLine("                        <p><b>:</p>   ");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td>");
                        sb.AppendLine("                          " + status + "");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");

                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 40px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td style='width: 100px;'>");
                        sb.AppendLine("                         <p>  <b>   Comment</b></p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td style='width: 20px;'>");
                        sb.AppendLine("                        <p><b>:</p>   ");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td>");
                        sb.AppendLine("                          " + comment + "");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");

                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 40px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td style='width: 100px;'>");
                        sb.AppendLine("                           <p> <b>Valid till</P>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td style='width: 20px;'>");
                        sb.AppendLine("                          <p><b>:</b></p>    ");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                        <td>");
                        sb.AppendLine("                         <p>   " + Convert.ToString(dateTime_Indian.AddDays(3).ToString("MM'/'dd'/'yyyy")) + "</P>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");

                        sb.AppendLine("                    <br>");
                        sb.AppendLine("                    <tr> <td style='width: 20px;height:30px'>&nbsp;</td>");
                        sb.AppendLine("                    </tr>");
                        sb.AppendLine("                        <tr ><td>    </td> <td> <td colspan='4' style='text-align: left;'><a style='background-color: #082654; color: white; margin-left: 40px; padding:10px;'href='" + url + "urs/itsummary?requestId=" + reqId + "&&userId="+ creatoruserid + "'>View Request</a></td> </td> <td>  </td></tr> ");
                        sb.AppendLine("                    </tr>");

                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 20px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td colspan='3' style='text-align: left;'>");
                        sb.AppendLine("                            <p>Regards,</p>  ");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");
                        sb.AppendLine("                    <tr>");
                        sb.AppendLine("                        <td style='width: 20px;'>&nbsp;</td>");
                        sb.AppendLine("                        <td colspan='3' style='text-align: left;'>");
                        sb.AppendLine("                            <p>Admin</p><p><b>Note:</b> This is an auto generated mail. Please do not reply</p>");
                        sb.AppendLine("                        </td>");
                        sb.AppendLine("                    </tr>");

                        sb.AppendLine("        <tr>");
                        sb.AppendLine("            <td colspan='3'>&nbsp;</td>");
                        sb.AppendLine("        </tr>");
                        sb.AppendLine("    </table>");
                        myMail.Body = sb.ToString();
                        
                        // }

                        var client = new SmtpClient()
                        {
                            Port = 587,
                            DeliveryMethod = SmtpDeliveryMethod.Network,
                            UseDefaultCredentials = true,
                            Host = "smtp.gmail.com",
                            EnableSsl = true,
                            Credentials = credentials
                        };
                        client.Send(myMail);
                        success = "Success";
                    }
                    return success;
                }
            }
            catch (Exception ex)
            {
                return "Fail";
            }
        }

    }
}

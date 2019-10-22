﻿using EddarsCms.Dto.BasicDtos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace EddarsCms.UserSides
{
    public class FrontendSenders
    {
        public static int SendBlogComment(BlogCommentDto dto)
        {

            var result = 0;
            try
            {
                using (SqlProgress sql = new SqlProgress())
                {
                    result = sql.SetExecuteNonQuery("insert into BlogComments (BlogId,UserName,UserEmail,Comment,WebSite,Date,LanguageId) values (@BlogId,@UserName,@UserEmail,@Comment,@WebSite,@Date,@LanguageId)", CommandType.Text, new SqlParameter("@BlogId", dto.BlogId), new SqlParameter("@UserName", dto.UserName), new SqlParameter("@UserEmail", dto.UserEmail), new SqlParameter("@Comment", dto.Comment), new SqlParameter("@WebSite", dto.WebSite), new SqlParameter("@Date", DateTime.Now), new SqlParameter("@LanguageId", LanguageOperation.GetLang().Id));

                    if (result>0)
                    {
                         var result2 = sql.SetExecuteNonQuery("insert into notifications (Caption,Description,Date,Icon) values (@Caption,@Description,@Date,@Icon)", CommandType.Text, new SqlParameter("@Caption", "Bir blog yorumu var"), new SqlParameter("@Description", "Kullanıcı Adı: " + dto.UserName + ", BlogId: " + dto.BlogId + ", Yorum:" + dto.Comment), new SqlParameter("@Icon", "bg-orange icon-notification glyph-icon icon-user"),new SqlParameter("@Date",DateTime.Now));
                    }
                }
            }
            catch (Exception e)
            {

                //service result dönülürse eğer hata mesajı da uı a kadar iletilebilir  
            }

            return result;
   
        }


        public static int SendInfoFromUs(InformationFromUsDto dto)
        {

            var result = 0;
            try
            {
                using (SqlProgress sql = new SqlProgress())
                {
                    result = sql.SetExecuteNonQuery("insert into InformationFromUs (Mail,Date) values (@Mail,@Date)", CommandType.Text, new SqlParameter("@Mail", dto.Mail), new SqlParameter("@Date", DateTime.Now));

                    if (result > 0)
                    {
                        var result2 = sql.SetExecuteNonQuery("insert into notifications (Caption,Description,Date,Icon) values (@Caption,@Description,@Date,@Icon)", CommandType.Text, new SqlParameter("@Caption", "Yeni Bizden Haberdar Olun maili var"), new SqlParameter("@Description", "Kullanıcı Mail: " + dto.Mail + ", Tarih: " + DateTime.Now.ToString("dd MMMM yyyy")), new SqlParameter("@Icon", "bg-purple icon-notification glyph-icon icon-user"), new SqlParameter("@Date", DateTime.Now));
                    }
                }
            }
            catch (Exception e)
            {

                //service result dönülürse eğer hata mesajı da uı a kadar iletilebilir  
            }
            return result;
        }


        //eklenecek alanlara göre düzenlenecek

        //public static int SendMail(BlogCommentDto dto)
        //{

        //    var result = 0;
        //    try
        //    {
        //        using (SqlProgress sql = new SqlProgress())
        //        {
        //            result = sql.SetExecuteNonQuery("insert into BlogComments (BlogId,UserName,UserEmail,Comment,WebSite,Date,LanguageId) values (@BlogId,@UserName,@UserEmail,@Comment,@WebSite,@Date,@LanguageId)", CommandType.Text, new SqlParameter("@BlogId", dto.BlogId), new SqlParameter("@UserName", dto.UserName), new SqlParameter("@UserEmail", dto.UserEmail), new SqlParameter("@Comment", dto.Comment), new SqlParameter("@WebSite", dto.WebSite), new SqlParameter("@Date", DateTime.Now), new SqlParameter("@LanguageId", LanguageOperation.GetLang().Id));

        //            if (result > 0)
        //            {
        //                var result2 = sql.SetExecuteNonQuery("insert into notifications (Caption,Description,Date,Icon) values (@Caption,@Description,@Date,@Icon)", CommandType.Text, new SqlParameter("@Caption", "Bir blog yorumu var"), new SqlParameter("@Description", "Kullanıcı Adı: " + dto.UserName + ", BlogId: " + dto.BlogId + ", Yorum:" + dto.Comment), new SqlParameter("@Icon", "bg-orange icon-notification glyph-icon icon-user"), new SqlParameter("@Date", DateTime.Now));


        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {

        //        //service result dönülürse eğer hata mesajı da uı a kadar iletilebilir  
        //    }

        //    return result;

        //}


        public static MailInfoDto GetMailInfo()
        {
            MailInfoDto mailInfo = new MailInfoDto();
            using (SqlProgress sql = new SqlProgress())
            {
                var dt = sql.GetDataTable("select * from MailInfoes", CommandType.Text);
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        mailInfo = new MailInfoDto()
                        {
                              Caption = dr.IsNull("Caption") ? "" : dr["Caption"].ToString(),
                              Host = dr.IsNull("Host") ? "" : dr["Host"].ToString(),
                              Port = dr.IsNull("Port") ? "" : dr["Port"].ToString(),
                              ReceiverMail = dr.IsNull("ReceiverMail") ? "" : dr["ReceiverMail"].ToString(),
                              SenderMail = dr.IsNull("SenderMail") ? "" : dr["SenderMail"].ToString(),
                              SenderMailPass = dr.IsNull("SenderMailPass") ? "" : dr["SenderMailPass"].ToString(),
                        };
                    }
                }
            }

            return mailInfo;
        }


        public static string SendPassword(string email,string subject,string message)
        {
            var contact = GetMailInfo();
            string returnMessage = "<script>jsError()</script>";
            //info @modafen.com.tr
            try
            {

                var senderEmail = new MailAddress(contact.SenderMail, contact.Caption);
                var receiverEmail = new MailAddress(email, "Receiver");
                var password = contact.SenderMailPass;
                var sub = subject;
                var body = message;
                var smtp = new SmtpClient
                {
                    //test edilecek
                    Host = contact.Host,
                    Port = Convert.ToInt32( contact.Port),
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(senderEmail.Address, password)
                //Credentials =  CredentialCache.DefaultNetworkCredentials,

            };
                using (var mess = new MailMessage(senderEmail, receiverEmail)
                {
                    Subject = sub,
                    Body = body
                })
                {
                    smtp.Send(mess);
                    returnMessage = "<script>jsSuccess('Hesap şifreniz başarıyla mail adresinize gönderilmiştir. Giriş ekranına dönmek için lütfen Giriş Ekranına Dön butonuna basınız');$('.form-control').val('')</script>";
                }
                return returnMessage;

            }
            catch (Exception e)
            {
                returnMessage="<script>jsError('" + e.Message + "')</script>";
            }
            return returnMessage;
        }

        public static string SendMail(ContactMailDto dto)
        {
            var contact = GetMailInfo();
            string returnMessage = "<script>jsError()</script>";
            //info @modafen.com.tr
            try
            {

                var senderEmail = new MailAddress(contact.SenderMail, contact.Caption);
                var receiverEmail = new MailAddress(contact.ReceiverMail, "Receiver");
                var password = contact.SenderMailPass;
                var sub = dto.Caption;
                var body = "Kullanıcı Adı:"+dto.Name+", "+"Konu:"+dto.Caption+", "+ "Telefon:" + dto.Phone + ", " + "E-Mail:" + dto.Mail + ", " + "Tarih:" + dto.Date + ", " + "Mesaj:" + dto.Mail;
                var smtp = new SmtpClient
                {
                    //test edilecek
                    Host = contact.Host,
                    Port = Convert.ToInt32(contact.Port),
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(senderEmail.Address, password)
                    //Credentials =  CredentialCache.DefaultNetworkCredentials,

                };
                using (var mess = new MailMessage(senderEmail, receiverEmail)
                {
                    Subject = sub,
                    Body = body
                })
                {
                    smtp.Send(mess);
                    returnMessage = "<script>jsSuccess('Göndermiş olduğunuz mail tarafımıza iletiliştir.');$('.form-control').val('')</script>";
                }
                return returnMessage;

            }
            catch (Exception e)
            {
                returnMessage = "<script>jsError('" + e.Message + "')</script>";

            }

            return returnMessage;

        }

    }
}
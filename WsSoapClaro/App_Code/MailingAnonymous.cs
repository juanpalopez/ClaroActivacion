using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Xml;
using System.Net.Mime;


public class MailingAnonymous
{
    private const string openTag = "<style type=\"text/css\">";
    private const string closeTag = "</style>";

    public static string Estilo
    {
        get
        {
            return
                string.Format("{1}{0}{0}{2}{0}{0}{3}{0}{0}{4}"
                    , "\r\n\r\n"
                    , openTag
                    , GetStyle("estandar", false)
                    , GetStyle("2009", false)
                    , closeTag);
        }
    }

    private static string GetStyle(string name, bool addTags)
    {
        StringBuilder sb = new StringBuilder();
        FileStream fs = null;
        StreamReader sr = null;

        try
        {
            String ruta = string.Format("/css/{0}.css", name);
            String linea = String.Empty;

            fs = new FileStream(HttpContext.Current.Server.MapPath(ruta), FileMode.Open, FileAccess.Read);
            sr = new StreamReader(fs);

            if (addTags) sb.AppendLine(openTag);

            linea = sr.ReadLine();

            while (linea != null)
            {
                sb.AppendLine(linea);
                linea = sr.ReadLine();
            }

            if (addTags) sb.AppendLine(closeTag);
        }
        catch
        {
            return string.Empty;
        }
        finally
        {
            if (fs != null) fs.Close(); fs = null;
            if (sr != null) sr.Close(); sr = null;
        }

        return sb.ToString();
    }

    private static void SetNull(ref List<string> mails)
    {
        if (mails == null) mails = new List<string>();
    }

    /// <summary>
    /// Envia un mensaje de correo a los correos asignados.
    /// </summary>
    /// <param name="mails_to">Listado de correos de destino.</param>
    /// <param name="mails_cc">Listado de correos de copia.</param>
    /// <param name="mails_co">Listado de correos de copia oculta.</param>
    /// <param name="contenido">Contenido del mensaje de correo a enviar.</param>
    public static void SendMail
        (
        List<string> mails_to,
        List<string> mails_cc,
        List<string> mails_co,
        String titulo,
        StringBuilder contenido,
        String displayName
        )
    {
        SendMail(mails_to, mails_cc, mails_co, titulo, contenido, new List<string>(), displayName);
    }

    /// <summary>
    /// Envia un mensaje de correo a los correos asignados.
    /// </summary>
    /// <param name="mails_to">Listado de correos de destino.</param>
    /// <param name="mails_cc">Listado de correos de copia.</param>
    /// <param name="mails_co">Listado de correos de copia oculta.</param>
    /// <param name="contenido">Contenido del mensaje de correo a enviar.</param>
    public static void SendMail
        (
        List<string> mails_to,
        List<string> mails_cc,
        List<string> mails_co,
        String titulo,
        StringBuilder contenido
        )
    {
        SendMail(mails_to, mails_cc, mails_co, titulo, contenido, new List<String>(), String.Empty);
    }


    public static void SendMail
        (
        List<string> mails_to,
        List<string> mails_cc,
        List<string> mails_co,
        String titulo,
        StringBuilder contenido,
        List<string> archivos,
        String displayName
        )
    {
        SetNull(ref archivos);

        List<Attachment> files = new List<Attachment>();

        if (archivos.Count > 0)
        {
            foreach (string archivo in archivos)
            {
                if (System.IO.File.Exists(archivo))
                {
                    files.Add(new Attachment(archivo));
                }
            }
        }

        SendMail(mails_to, mails_cc, mails_co, titulo, contenido, files, displayName);
    }

    public static void SendMail
      (
      List<string> mails_to,
      List<string> mails_cc,
      List<string> mails_co,
      String titulo,
      StringBuilder contenido,
      List<Attachment> archivos
      )
    {
        MailMessage correo = new MailMessage();

        SetNull(ref mails_to);
        SetNull(ref mails_cc);
        SetNull(ref mails_co);

        if (archivos == null) archivos = new List<Attachment>();

        foreach (string mail in mails_to) correo.To.Add(mail);
        foreach (string mail in mails_cc) correo.CC.Add(mail);
        foreach (string mail in mails_co) correo.Bcc.Add(mail);

        correo.Subject = titulo;
        correo.Body = contenido.ToString();
        correo.IsBodyHtml = true;
        correo.Priority = MailPriority.Normal;

        if (archivos != null && archivos.Count > 0)
            foreach (Attachment archivo in archivos)
                correo.Attachments.Add(archivo);

        SendMailMessage(correo, false);
    }

    public static void SendMail
        (
        List<string> mails_to,
        List<string> mails_cc,
        List<string> mails_co,
        String titulo,
        StringBuilder contenido,
        List<Attachment> archivos,
        String displayName
        )
    {
        MailMessage correo = new MailMessage();

        SetNull(ref mails_to);
        SetNull(ref mails_cc);
        SetNull(ref mails_co);

        if (archivos == null) archivos = new List<Attachment>();

        foreach (string mail in mails_to) correo.To.Add(mail);
        foreach (string mail in mails_cc) correo.CC.Add(mail);
        foreach (string mail in mails_co) correo.Bcc.Add(mail);

        correo.Subject = titulo;
        correo.Body = contenido.ToString();
        correo.IsBodyHtml = true;
        correo.Priority = MailPriority.Normal;

        if (archivos != null && archivos.Count > 0)
            foreach (Attachment archivo in archivos)
                correo.Attachments.Add(archivo);

        SendMailMessage(correo, false, displayName);
    }

    public static SmtpClient smtpSendMessage(MailMessage correo)
    {
        String smtp_ruta = Util.GetConfigValue(true, "smtp_ruta");//smtp.gmail.com
        String smtp_port = Util.GetConfigValue(true, "smtp_port");//587
        String smtp_user = Util.GetConfigValue(true, "smtp_user");//contacto.kimsasoft@gmail.com
        String smtp_pwd = Util.GetConfigValue(true, "smtp_pwd");//#echo@Password1**#
        String smtp_enable_ssl = Util.GetConfigValue(true, "smtp_enable_ssl");//smtp.gmail.com

        SmtpClient smtp = int.Parse(smtp_port) == 0 ? new SmtpClient(smtp_ruta) : new SmtpClient(smtp_ruta, int.Parse(smtp_port));
        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
        smtp.UseDefaultCredentials = false;
        smtp.Credentials = new System.Net.NetworkCredential(smtp_user, smtp_pwd);
        smtp.EnableSsl = String.IsNullOrEmpty(smtp_enable_ssl) ? false : Convert.ToBoolean(smtp_enable_ssl);//gmail = true

        correo.Priority = MailPriority.Normal;
        smtp.Send(correo);
        return smtp;
    }

    private static void SendMailMessage(MailMessage correo, bool useTo)
    {
        try
        {
            string from1 = "contacto.kimsasoft@gmail.com";
            string from2 = "KimsaSoft Notifications";

            try
            {
                string mFrom = Util.GetConfigValue(true, "mail_from");

                if (!string.IsNullOrEmpty(mFrom))
                {
                    if (mFrom.IndexOf('|') != -1)
                    {
                        from1 = mFrom.Split('|')[0];
                        from2 = mFrom.Split('|')[1];
                    }
                    else
                    {
                        from1 = from2 = mFrom;
                    }
                }
            }
            catch
            {
            }

            correo.From = new MailAddress(from1, from2);
            correo.Sender = new MailAddress(from1, from2);
            correo.BodyEncoding = Encoding.UTF8;
            correo.SubjectEncoding = Encoding.UTF8;

            smtpSendMessage(correo);
        }
        catch
        {
            throw;
        }
        finally
        {
            correo.Dispose();
        }
    }

    private static void SendMailMessage(MailMessage correo, bool useTo, String displayName)
    {
        try
        {
            string from1 = "contacto.kimsasoft@gmail.com";
            string from2 = displayName.Length == 0 ? "KimsaSoft Notifications" : displayName;

            try
            {
                string mFrom = Util.GetConfigValue(true, "mail_from");

                if (!string.IsNullOrEmpty(mFrom))
                {
                    if (mFrom.IndexOf('|') != -1)
                    {
                        from1 = mFrom.Split('|')[0];
                        from2 = mFrom.Split('|')[1];
                    }
                    else
                    {
                        from1 = from2 = mFrom;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            correo.From = new MailAddress(from1, from2);
            correo.Sender = new MailAddress(from1, from2);
            correo.BodyEncoding = Encoding.UTF8;
            correo.SubjectEncoding = Encoding.UTF8;

            smtpSendMessage(correo);
        }
        catch (Exception ex)
        {
            throw;
        }
        finally
        {
            correo.Dispose();
        }
    }
}
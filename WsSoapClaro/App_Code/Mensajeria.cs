using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Messaging;
using System.Xml.Serialization;
using System.IO;
using System.Text;

/// <summary>
/// Summary description for Mensajeria
/// </summary>
public class Mensajeria
{
    public static String COLA = @".\Private$\CLARITO";

    public Mensajeria()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static LogMessage ReceiveMessage(string queueName)
    {
        if (!MessageQueue.Exists(queueName))
            return null;

        MessageQueue messageQueue = new MessageQueue(queueName);
        LogMessage logMessage = null;

        try
        {
            messageQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(LogMessage) });
            Message Mymessage = messageQueue.Receive();
            logMessage = (LogMessage)Mymessage.Body;
            //MailingAnonymous.SendMail(to, cc, co, titulo, sb);
        }
        catch { throw; }
        finally
        {
            messageQueue.Close();
        }
        return logMessage;

    }

    public static CorreoMessage ReceiveMessageActivacion(string queueName)
    {
        if (!MessageQueue.Exists(queueName))
            return null;

        MessageQueue messageQueue = new MessageQueue(queueName);
        CorreoMessage emailMessage = null;

        try
        {
            messageQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(CorreoMessage) });
            Message Mymessage = messageQueue.Receive();
            emailMessage = (CorreoMessage)Mymessage.Body;

            List<String> to = emailMessage.Destinatario.Split(';').ToList<String>();
            List<String> cc = new List<String>();
            List<String> co = new List<String>();
            StringBuilder sb = new StringBuilder();
            String html = File.ReadAllText(@"c:\inetpub\wwwroot\WsSoapClaro\Template\template.html");

            sb.Append(html);
            sb.Replace("[CLIENTE]", emailMessage.Nombre);
            sb.Replace("[MENSAJE]", emailMessage.Mensaje);
            sb.Replace("[MONTO]", emailMessage.Monto);
            sb.Replace("[LINEA]", emailMessage.NroLinea);

            MailingAnonymous.SendMail(to, cc, co, "Test-MSMQ - Confirmación de línea: " + emailMessage.NroLinea, sb);
        }
        catch { throw; }
        finally
        {
            messageQueue.Close();
        }
        return emailMessage;
    }

    public static void SendMessage(string queueName, LogMessage msg)
    {
        MessageQueue messageQueue = null;

        if (!MessageQueue.Exists(queueName))
        {
            messageQueue = MessageQueue.Create(queueName);
        }
        else
        {
            messageQueue = new MessageQueue(queueName);
        }

        try
        {
            messageQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(LogMessage) });
            messageQueue.Send(msg, "Test");
        }
        catch (Exception)
        {
            throw;
            //Write code here to do the necessary error handling.
        }
        finally
        {
            messageQueue.Close();
        }
    }

    public static void SendMessage(string queueName, CorreoMessage msg)
    {
        MessageQueue messageQueue = null;

        if (!MessageQueue.Exists(queueName))
        {
            messageQueue = MessageQueue.Create(queueName);
        }
        else
        {
            messageQueue = new MessageQueue(queueName);
        }

        try
        {
            messageQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(CorreoMessage) });
            messageQueue.Send(msg, "Activacion");
        }
        catch (Exception)
        {
            throw;
            //Write code here to do the necessary error handling.
        }
        finally
        {
            messageQueue.Close();
        }
    }

}

public class CorreoMessage
{
    public CorreoMessage() { }
    public string Ruc { get; set; }
    public string Destinatario { get; set; }
    public string Nombre { get; set; }
    public string NroLinea { get; set; }
    public string Monto { get; set; }
    public string Imei { get; set; }
    public string Mensaje { get; set; }
}

public class LogMessage
{
    public string MessageText { get; set; }

    public DateTime MessageTime { get; set; }
}

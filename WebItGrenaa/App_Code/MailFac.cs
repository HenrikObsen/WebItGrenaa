using System.Net;
using System.Net.Mail;

/// <summary>
/// 
//Brug af skolen SMTP
//MailFac mf = new MailFac("vid233.vid.net.local", "hto@videndjurs.dk", "Henrik Obsen");

//Brug din egen gmail og Googles SMTP
//MailFac mf = new MailFac("smtp.gmail.com", "email@gmail.com", "DitNavne", "password", 587);

//mf.Send("Emne","Body teksten.","hto@djes.dk");
/// </summary>
public class MailFac
{
    private string _SMTP = "";
    private string _fromEmail = "";
    private string _fromName = "";
    private string _password = "";
    private int _port = 25;


	public MailFac(string SMTP, string fromEmail, string fromName)
	{
        _SMTP = SMTP;
	    _fromEmail = fromEmail;
	    _fromName = fromName;
	}

    public MailFac(string SMTP, string fromEmail, string fromName, string password, int port)
    {
        _SMTP = SMTP;
        _fromEmail = fromEmail;
        _fromName = fromName;
        _password = password;
        _port = port;
    }

    public void Send(string Subject, string Text, string Receiver)
    {
        MailMessage mail = new MailMessage();

        mail.From = new MailAddress(_fromEmail, _fromName);

        mail.To.Add(Receiver);
        mail.Subject = Subject;
        mail.Body = Text;
        mail.IsBodyHtml = true;
        
        SmtpClient smtpCl = new SmtpClient(_SMTP, _port);

        if (_password != "")
        {
        smtpCl.EnableSsl = true;
        smtpCl.UseDefaultCredentials = false;
        smtpCl.Credentials = new NetworkCredential(_fromEmail, _password);
        smtpCl.DeliveryMethod = SmtpDeliveryMethod.Network;  
        }
       
        smtpCl.Send(mail); 
        
    }
    




}
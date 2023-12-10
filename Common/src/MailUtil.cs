using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Resto.Common
{
    public class MailSettings
    {
        public MailSettings(string from, List<string> toList, string host, int port, bool auth, string login, string pass, bool enableSSL)
        {
            EmailFrom = from;
            EmailsTo = toList;
            Host = host;
            Port = port;
            UseDefaultCredentials = auth;
            CredentialLogin = login;
            CredentialPassword = pass;
            EnableSSL = enableSSL;
        }
        /// <summary>
        /// Адрес отправителя
        /// </summary>
        public string EmailFrom { get; set; }

        /// <summary>
        /// Список адресов получателей.
        /// Первый адрес из списка - кому посылается письмо, последующие адреса - кому отправляются копии.
        /// </summary>
        public List<string> EmailsTo { get; set; }

        /// <summary>
        /// Имя или IP-адресс сервера, который используется для отправки сообщений по SMTP.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Порт сервера, который используется для отправки сообщений по SMTP.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Аутентификация для доступа сервер.
        /// </summary>
        public bool UseDefaultCredentials { get; set; }

        /// <summary>
        /// Логин для доступа на сервер.
        /// </summary>
        public string CredentialLogin { get; set; }

        /// <summary>
        /// Пароль доступа на сервер.
        /// </summary>
        public string CredentialPassword { get; set; }

        /// <summary>
        /// Использовать SSL аутентификацию.
        /// </summary>
        public bool EnableSSL { get; set; }

        public bool IsCorrect()
        {
            return !string.IsNullOrEmpty(EmailFrom) && EmailsTo.Count > 0 && !string.IsNullOrEmpty(EmailsTo[0])
                    && !string.IsNullOrEmpty(Host);
        }

        /// <summary>
        /// Возвращает список адресов получателей в виде строки разделённых символом/строкой.
        /// </summary>
        /// <param name="delimiter">символ разделитель</param>
        public string GetEmailsToAsString(string delimiter)
        {
            string listAsString = string.Empty;

            if (EmailsTo.Count > 0)
            {
                listAsString = EmailsTo[0];

                for (int i = 1; i < EmailsTo.Count; i++)
                {
                    if (!string.IsNullOrEmpty(EmailsTo[i]))
                    {
                        listAsString += string.Format("{0} {1}", delimiter, EmailsTo[i]);
                    }
                }
            }
            return listAsString;
        }
    }

    public class MailUtil
    {
        public static void SendMail(string subject, string body, MailSettings settings)
        {
            //отсылка сообщения по емейлу                
            MailAddress from = new MailAddress(settings.EmailFrom, settings.EmailFrom);
            MailAddress to = new MailAddress(settings.EmailsTo[0]);
            MailMessage message = new MailMessage(from, to)
                                      {
                                          Subject = subject,
                                          Body = body,
                                          Priority = MailPriority.High,
                                          BodyEncoding = Encoding.UTF8
                                      };
            message.Priority = MailPriority.Normal;

                for (int i = 1; i < settings.EmailsTo.Count; i++)
                {
                    if (!string.IsNullOrEmpty(settings.EmailsTo[i]))
                    {
                        to = new MailAddress(settings.EmailsTo[i]);
                        message.CC.Add(to);
                    }
                }

                SmtpClient client = new SmtpClient(settings.Host, settings.Port);
                if (settings.UseDefaultCredentials)
                {
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(settings.CredentialLogin, settings.CredentialPassword);
                }
                else
                {
                    client.UseDefaultCredentials = true;
                }
                client.EnableSsl = settings.EnableSSL;                
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                client.Send(message);
        }
    }
}

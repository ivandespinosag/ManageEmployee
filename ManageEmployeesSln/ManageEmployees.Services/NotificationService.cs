using ManageEmployees.Infraestructure.Interfaces;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace ManageEmployees.Services
{
    public class NotificationService : INotificationService
    {
        private readonly SmtpSettings _smtpSettings;
        private readonly IEmployeeData _employeeData;

        public NotificationService(IOptions<SmtpSettings> smtpSettings, IEmployeeData employeeData)
        {
            _smtpSettings = smtpSettings.Value;
            _employeeData = employeeData;
        }

        public async Task SendNotificationEmail(string toEmail, string subject, string body)
        {
            using (var client = new SmtpClient(_smtpSettings.Server, _smtpSettings.Port))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password);
                client.EnableSsl = true;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_smtpSettings.FromEmail),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(toEmail);

                try
                {
                    await client.SendMailAsync(mailMessage);
                }
                catch (SmtpException ex)
                {
                    Console.WriteLine($"Error en el envío del correo: {ex.Message}");
                    throw;
                }
            }
        }

        public async Task NotifyContractEnd()
        {
            var employees = await _employeeData.EmployeesList();
            foreach (var employee in employees)
            {
                var monthsSinceHire = (DateTime.Now - employee.HireDate).TotalDays / 30;

                if (monthsSinceHire >= 3 && monthsSinceHire < 4)
                {
                    // Notificación de periodo de prueba
                    await SendNotificationEmail(
                        employee.Email,
                        "Periodo de Prueba Superado",
                        $"Hola {employee.FirstName}, has superado el periodo de prueba."
                    );
                }
                else if (monthsSinceHire >= 11 && monthsSinceHire < 12)
                {
                    // Notificación de vencimiento de contrato
                    await SendNotificationEmail(
                        employee.Email,
                        "Vencimiento de Contrato",
                        $"Hola {employee.FirstName}, tu contrato está próximo a vencer el {employee.ContractEndDate}."
                    );
                }
                else if (monthsSinceHire >= 11.5 && monthsSinceHire < 12)
                {
                    // Notificación de renovación de contrato
                    await SendNotificationEmail(
                        employee.Email,
                        "Renovación de Contrato",
                        $"Hola {employee.FirstName}, tu contrato será renovado por un año más."
                    );
                }
            }
        }
    }
}

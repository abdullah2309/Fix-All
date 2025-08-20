using Fix_All.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using MailKit.Net.Smtp;
using MimeKit;


namespace Fix_All.Controllers
{
    public class AdminController : Controller
    {
        private readonly mydbcontext _context;

        public AdminController(mydbcontext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> applyforlabar()
        {
            var providers = await _context.ServiceProviders
                .Include(s => s.LaborField)
                .OrderByDescending(s => s.LarberId) // ✅ Newest first
                .ToListAsync();
            return View(providers);
        }     
        public async Task<IActionResult> Labers()
        {
            var providers = await _context.approve_labers
                .Include(s => s.LaborField)
                .OrderByDescending(s => s.ApproveLarberId) // ✅ Newest first
                .ToListAsync();
            return View(providers);
        }
        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            var provider = await _context.ServiceProviders.FindAsync(id);

            if (provider == null)
                return NotFound();

            // ✅ Transfer Data to approve_laber table
            var approved = new approve_laber
            {
                FirstName = provider.FirstName,
                LastName = provider.LastName,
                Email = provider.Email,
                Phone = provider.Phone,
                CNIC = provider.CNIC,
                Address = provider.Address,
                Education = provider.Education,
                FieldId = provider.FieldId,
                addmorefield = provider.addmorefield,
                IsDiploma = provider.IsDiploma,
                Experience = provider.Experience,
                PasswordHash = provider.PasswordHash,
                CVFilePath = provider.CVFilePath,
                ProfileImagePath = provider.ProfileImagePath,
                Status = "Approved"
            };

            _context.approve_labers.Add(approved);

            // ✅ Optionally: remove from ServiceProvider table
            _context.ServiceProviders.Remove(provider);

            await _context.SaveChangesAsync();

            // ✅ Send Email Confirmation
            await SendApprovalEmail(approved.Email, approved.FirstName);

            TempData["Message"] = "Labor approved successfully and confirmation email sent.";
            return RedirectToAction("Labers");
        }

        private async Task SendApprovalEmail(string toEmail, string name)
        {
            try
            {
                using (var client = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587))
                {
                    client.Credentials = new System.Net.NetworkCredential("abdullah2309r@gmail.com", "gwtonnsduolnuzwx");
                    client.EnableSsl = true;

                    var mailMessage = new System.Net.Mail.MailMessage();
                    mailMessage.From = new System.Net.Mail.MailAddress("abdullah2309r@gmail.com", "Fix All Team");
                    mailMessage.To.Add(toEmail);
                    mailMessage.Subject = "Approval Confirmation";
                    mailMessage.Body = $"Dear {name},\n\nCongratulations! Your account has been approved by our admin. You can now log in and start working.\n\nRegards,\nFix All Team";

                    await client.SendMailAsync(mailMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Email sending failed: " + ex.Message);
            }
        }
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Reject(int id)
        {
            var provider = await _context.ServiceProviders.FindAsync(id);
            if (provider != null)
            {
                // ✉️ Email send before removing
                await SendRejectionEmail(provider.Email, provider.FirstName);

                _context.ServiceProviders.Remove(provider);
                await _context.SaveChangesAsync();

                TempData["Message"] = "Service Provider rejected and email sent successfully.";
            }
            else
            {
                TempData["Message"] = "Service Provider not found.";
            }

            return RedirectToAction("applyforlabar");
        }

        private async Task SendRejectionEmail(string toEmail, string name)
        {
            try
            {
                using (var client = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587))
                {
                    client.Credentials = new System.Net.NetworkCredential("abdullah2309r@gmail.com", "gwtonnsduolnuzwx");
                    client.EnableSsl = true;

                    var mailMessage = new System.Net.Mail.MailMessage();
                    mailMessage.From = new System.Net.Mail.MailAddress("abdullah2309r@gmail.com", "Fix All Admin");
                    mailMessage.To.Add(toEmail);
                    mailMessage.Subject = "Application Rejected - Fix All";
                    mailMessage.Body = $@"
                <h3>Dear {name},</h3>
                <p>We regret to inform you that your application has been <b>rejected</b>.</p>
                <p>If you have any questions, please contact our support team.</p>
                <br>
                <p>Best Regards,</p>
                <p><b>Fix All Team</b></p>";
                    mailMessage.IsBodyHtml = true;

                    await client.SendMailAsync(mailMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Email sending failed: " + ex.Message);
            }
        }







    }
}

//
//
//
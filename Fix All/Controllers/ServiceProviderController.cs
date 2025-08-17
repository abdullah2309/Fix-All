using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
using System.IO;
using System;
using Fix_All.Models;

public class ServiceProviderController : Controller
{
    private readonly mydbcontext _context;
    private readonly IPasswordHasher<ServiceProvider> _passwordHasher;

    public ServiceProviderController(mydbcontext context, IPasswordHasher<ServiceProvider> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    [HttpGet]
    public IActionResult Register()
    {
        ViewBag.LaborFields = _context.LaborFields.ToList();
        return View();
    }

    [HttpPost]
    public IActionResult SendOtp(string email)
    {
        var otp = new Random().Next(100000, 999999).ToString();

        // Use Session instead of TempData for OTP to persist across multiple requests
        HttpContext.Session.SetString("Otp", otp);
        HttpContext.Session.SetString("OtpEmail", email);

        using (var smtp = new SmtpClient("smtp.gmail.com", 587))
        {
            smtp.Credentials = new NetworkCredential("abdullah2309r@gmail.com", "gwtonnsduolnuzwx");
            smtp.EnableSsl = true;
            smtp.Send("abdullah2309r@gmail.com", email, "Fix All - Email Verification", $"Your OTP is: {otp}");
        }

        return Json(new { success = true });
    }

    [HttpPost]
    public IActionResult VerifyOtp(string otp)
    {
        var storedOtp = HttpContext.Session.GetString("Otp");

        if (storedOtp != null && storedOtp == otp)
        {
            HttpContext.Session.SetString("OtpVerified", "true");
            return Json(new { success = true });
        }
        return Json(new { success = false });
    }

    [HttpPost]
    public IActionResult Register(ServiceProvider model, IFormFile cv, IFormFile image)
    {
        var otpVerified = HttpContext.Session.GetString("OtpVerified");

        if (otpVerified != "true")
        {
            ModelState.AddModelError("", "Please verify your email before submitting.");
            ViewBag.LaborFields = _context.LaborFields.ToList();
            return View(model);
        }

        if (cv != null)
        {
            var cvPath = Path.Combine("wwwroot/uploads/cv", cv.FileName);
            using var stream = new FileStream(cvPath, FileMode.Create);
            cv.CopyTo(stream);
            model.CVFilePath = "/uploads/cv/" + cv.FileName;
        }

        if (image != null)
        {
            var imgPath = Path.Combine("wwwroot/uploads/images", image.FileName);
            using var stream = new FileStream(imgPath, FileMode.Create);
            image.CopyTo(stream);
            model.ProfileImagePath = "/uploads/images/" + image.FileName;
        }

        // Hash password
        model.PasswordHash = _passwordHasher.HashPassword(model, model.PasswordHash);
        model.EmailVerified = true;

        _context.ServiceProviders.Add(model);
        _context.SaveChanges();

        return RedirectToAction("Success");
    }

    public IActionResult Success()
    {
        return View();
    }
}

using System.ComponentModel.Design;
using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineHelpDesk.Data;
using OnlineHelpDesk.Models;

namespace OnlineHelpDesk.Controllers
{
    public class HomeController : Controller
    {
        private readonly HelpdeskContext db;

        private readonly IHttpContextAccessor CONTX;
        private readonly HelpdeskContext context;
        private readonly IWebHostEnvironment _webHostEnvironment;



        public HomeController(HelpdeskContext db, IHttpContextAccessor contx, HelpdeskContext context, IWebHostEnvironment webHostEnvironment)//contact add
        {
            this.db = db;
            this.CONTX = contx;
            this.context = context;
            _webHostEnvironment = webHostEnvironment;

        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult About()
        {
            var about = db.Aboutus.FirstOrDefault(); // Or LastOrDefault if you want latest
            return View(about);
        }

        ///contact add <summary>
        // GET: Display the contact form
        public IActionResult Contact()
        {
            // Pass a new ContactMessage to the view for the form
            return View(new ContactMessage());
        }

        // POST: Handle form submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Contact(ContactMessage contact)
        {
            if (ModelState.IsValid)
            {
                // Set the SubmittedAt timestamp
                contact.SubmittedAt = DateTime.Now;

                // Add the contact message to the database
                db.ContactMessages.Add(contact);
                int result = db.SaveChanges();  // Save changes to the database

                if (result > 0)
                {
                    // Store success message in TempData
                    TempData["success"] = "Form submitted successfully!";
                    // Redirect to avoid re-submission on page refresh
                    return RedirectToAction("Contact");
                }
                else
                {
                    // Handle the case where the form wasn't saved successfully
                    TempData["error"] = "There was an issue submitting your form. Please try again.";
                    return View(contact); // Return to the form with validation errors
                }
            }

            // If validation fails, return the same view with validation messages
            return View(contact);
        }

        public IActionResult Facilities()
        {
            return View();
        }

        public IActionResult ActiveFacility()
        {
            // Sirf active facilities fetch karo
            var activeFacilities = db.Facilities
                                     .Where(f => f.IsActive)
                                     .ToList();

            return View(activeFacilities);
        }
        // GET: Show request facility form
        [HttpGet]
        public IActionResult RequestFacility()
        {
            return View(new Facility());
        }

        // POST: Handle user-submitted facility request
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RequestFacility(Facility facility)
        {
            if (ModelState.IsValid)
            {
                facility.CreatedDate = DateTime.Now;
                facility.IsActive = false; // Because admin has to approve

                db.Facilities.Add(facility);
                db.SaveChanges();

                TempData["success"] = "Your facility request has been sent to admin.";
                return RedirectToAction("ActiveFacility");
            }

            return View(facility);
        }


        public IActionResult Reviews()
        {
            var reviews = db.Reviews.ToList(); // Make sure 'db' is your actual DbContext
            return View(reviews);
        }

        [HttpGet]
        public IActionResult SubmitReview()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SubmitReview(Review model)
        {
            if (ModelState.IsValid)
            {
                if (model.ImageFile != null)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(model.ImageFile.FileName);
                    string extension = Path.GetExtension(model.ImageFile.FileName);
                    model.ImagePath = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwRootPath + "/uploads/", fileName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(fileStream);
                    }
                }

                db.Reviews.Add(model);
                await db.SaveChangesAsync();

                TempData["Success"] = "Thank you for your feedback!";
                return RedirectToAction("SubmitReview");
            }

            return View(model);
        }

        //Registration
        //[HttpGet]
        //public IActionResult Registration()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public IActionResult Registration(UserRegistration newSTD)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        newSTD.UserRole = "customer";

        //        db.UserRegistrations.Add(newSTD);
        //        db.SaveChanges();

        //        // ? Success message + redirect to Login
        //        TempData["LoginMessage"] = "You have been registered successfully.";
        //        return RedirectToAction("Login");
        //    }


          
        //    return View(); // If validation fails
        //}


        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registration(UserRegistration newSTD)
        {
            if (ModelState.IsValid)
            {
                // Check if the email already exists
                bool emailExists = db.UserRegistrations.Any(u => u.UserEmail == newSTD.UserEmail);
                if (emailExists)
                {
                    ModelState.AddModelError("UserEmail", "This email is already registered.");
                    return View(newSTD);
                }

                newSTD.UserRole = "customer";
                db.UserRegistrations.Add(newSTD);
                db.SaveChanges();

                // ? Success message + redirect to Login
                TempData["LoginMessage"] = "You have been registered successfully.";
                return RedirectToAction("Login");
            }

            // Return view with validation messages
            return View(newSTD);
        }


        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(UserRegistration USERVALIDATION)
        {
            if (!ModelState.IsValid)
            {
                var uservalidity = db.UserRegistrations.Where(user => user.UserEmail == USERVALIDATION.UserEmail &&
                user.UserPassword == USERVALIDATION.UserPassword).ToList();

                if (uservalidity[0].UserRole == "customer")
                {
                    CONTX.HttpContext.Session.SetString("UserName", uservalidity[0].UserName);
                    CONTX.HttpContext.Session.SetString("UserEmail", uservalidity[0].UserEmail);
                    CONTX.HttpContext.Session.SetString("UserRole", uservalidity[0].UserRole);
                   
                    return RedirectToAction("Index", "Home");
                }

                else if (uservalidity[0].UserRole == "ADMIN")
                {
                    CONTX.HttpContext.Session.SetString("UserName", uservalidity[0].UserName);
                    CONTX.HttpContext.Session.SetString("UserEmail", uservalidity[0].UserEmail);
                    CONTX.HttpContext.Session.SetString("UserRole", uservalidity[0].UserRole);

                    return RedirectToAction("Index", "Admin");
                }
            }
            return View();
        }



        // GET: Change Password Page
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        // POST: Handle Change Password Submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Invalid data.";
                return View(model);
            }

            string userEmail = CONTX.HttpContext.Session.GetString("UserEmail");

            if (string.IsNullOrEmpty(userEmail))
            {
                TempData["error"] = "Session expired. Please log in again.";
                return RedirectToAction("Login");
            }

            var user = db.UserRegistrations.FirstOrDefault(u => u.UserEmail == userEmail);

            if (user == null || user.UserPassword != model.CurrentPassword)
            {
                TempData["error"] = "Current password is incorrect.";
                return View(model);
            }

            if (model.NewPassword != model.ConfirmPassword)
            {
                TempData["error"] = "New passwords do not match.";
                return View(model);
            }

            // Update password
            user.UserPassword = model.NewPassword;
            db.SaveChanges();

            TempData["success"] = "Password changed successfully.";
            return RedirectToAction("ChangePassword");
        }


        // GET: Show user account details
        [HttpGet]
        public IActionResult MyAccount()
        {
            // ? Correct: Fetching session value
            string userEmail = CONTX.HttpContext.Session.GetString("UserEmail");

            // ? Correct: Handling if session is expired or missing
            if (string.IsNullOrEmpty(userEmail))
            {
                TempData["error"] = "Session expired. Please login again.";
                return RedirectToAction("Login");
            }

            // ? Correct: Querying the user from the database using email
            var user = db.UserRegistrations.FirstOrDefault(u => u.UserEmail == userEmail);

            // ? Correct: Passing the user model to the view
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MyAccount(UserRegistration model)
        {
            if (!ModelState.IsValid)
            {
                // ? Log model state errors
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        Console.WriteLine($"{state.Key}: {error.ErrorMessage}");
                    }
                }

                TempData["error"] = "Invalid data submitted.";
                return View(model);
            }

            var user = db.UserRegistrations.FirstOrDefault(u => u.UserId == model.UserId);
            if (user == null)
            {
                TempData["error"] = "User not found.";
                return RedirectToAction("Login");
            }

            // ? Update fields
            user.UserName = model.UserName;
            user.UserEmail = model.UserEmail;

            db.SaveChanges();

            TempData["success"] = "Account updated successfully!";
            return RedirectToAction("MyAccount");
        }

       //Logout
        public IActionResult Logout()
        {
            CONTX.HttpContext.Session.Clear();
            TempData["LogoutMessage"] = "You have been logged out successfully.";
            return RedirectToAction("Registration");
        }





    }
}

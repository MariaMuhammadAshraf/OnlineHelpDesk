using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnlineHelpDesk.Data;
using OnlineHelpDesk.Models;


namespace OnlineHelpDesk.Controllers
{
    public class AdminController : Controller
    {
        private readonly HelpdeskContext db;
        private readonly IHttpContextAccessor CONTX;
      

        public AdminController(HelpdeskContext db, IHttpContextAccessor cONTX)
        {
            this.db = db;
            CONTX = cONTX;
            

        }


        public IActionResult Index()
        {
            var userRole = CONTX.HttpContext.Session.GetString("UserRole");

            //Agar session mein UserRole empty hai ya Admin nahi hai, toh redirect kar do Home page par

            // Redirect if UserRole is empty OR userRole is NOT "admin"

            if (string.IsNullOrEmpty(userRole) || userRole.ToLower() != "admin")
            {
                return RedirectToAction("Index", "Home");
            }

            // Agar Admin hai, tabhi dashboard dikhao, counts bhejo View ko
            ViewBag.FacilityCount = db.Facilities.Count();
            ViewBag.UserCount = db.UserRegistrations.Count();
            ViewBag.ContactCount = db.ContactMessages.Count();

            return View();
        }

      // Facilities: List all
        public IActionResult Facilities()
        {
          
            var facilities = db.Facilities.ToList();
            return View(facilities);
        }

        // Add Facility - GET
        [HttpGet]
        public IActionResult AddFacility()
        {
          

            return View();
        }

        // Add Facility - POST
        [HttpPost]
        public IActionResult AddFacility(Facility facility)
        {
           

            if (ModelState.IsValid)
            {
                db.Facilities.Add(facility);
                db.SaveChanges();
                return RedirectToAction("Facilities");
            }
            return View(facility);
        }

        // Edit Facility - GET
        // GET: Edit Facility
        [HttpGet]
        public IActionResult EditFacility(int id)
        {
            var facility = db.Facilities.Find(id);
            if (facility == null)
            {
                return NotFound();
            }
            return View(facility);
        }

        // POST: Update Facility
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditFacility(int id, Facility facility)
        {
            if (id != facility.FacilityId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existing = db.Facilities.Find(id);
                    if (existing == null)
                    {
                        return NotFound();
                    }

                    // Manually update fields
                    existing.FacilityName = facility.FacilityName;
                    existing.Description = facility.Description;
                    existing.IsActive = facility.IsActive;

                    db.SaveChanges();
                    return RedirectToAction("Facilities");
                }
                catch (DbUpdateException)
                {
                    return View(facility);
                }
            }

            return View(facility);
        }

        // Delete Facility
        public IActionResult DeleteFacility(int id)
        {
           

            var facility = db.Facilities.Find(id);
            if (facility == null) return NotFound();

            db.Facilities.Remove(facility);
            db.SaveChanges();
            return RedirectToAction("Facilities");
        }

        // User Management - List all users
        public IActionResult Users()
        {
           

            var users = db.UserRegistrations.ToList();
            return View(users);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var user = db.UserRegistrations.FirstOrDefault(u => u.UserId == id);
            if (user != null)
            {
                db.UserRegistrations.Remove(user);
                db.SaveChanges();
            }

            return RedirectToAction("Users"); // Or "UserManagement", depending on your view name
        }

        [HttpGet]
        public IActionResult ContactList()
        {
            var contacts = db.ContactMessages.ToList();
            return View(contacts);
        }
      public IActionResult DeleteContact(int id)
        {


            var msg = db.ContactMessages.Find(id);
            if (ContactList == null) return NotFound();

            db.ContactMessages.Remove(msg);
            db.SaveChanges();
            return RedirectToAction("ContactList");//redirct page here after delete
        }

       //Logout
        public IActionResult Logout()
        {
            CONTX.HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }


        //DYNAMIC ABOUT US

        // Show all entries
        public IActionResult AboutUsList()
        {
            var abouts = db.Aboutus.ToList();
            return View(abouts);
        }

        // GET: Add new About Us entry
        [HttpGet]
        public IActionResult AddAboutUs()
        {
            return View();
        }

        // POST: Add new entry
        [HttpPost]
        public async Task<IActionResult> AddAboutUs(Aboutu model, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    var fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    model.ImagePath = "/uploads/" + fileName;
                }

                db.Aboutus.Add(model);
                db.SaveChanges();
                return RedirectToAction("AboutUsList");
            }

            return View(model);
        }

        // GET: Edit
        [HttpGet]
        public IActionResult EditAboutUs(int id)
        {
            var entry = db.Aboutus.Find(id);
            return entry == null ? NotFound() : View(entry);
        }

        // POST: Edit
        [HttpPost]
        public async Task<IActionResult> EditAboutUs(int id, Aboutu model, IFormFile image)
        {
            var existing = db.Aboutus.Find(id);
            if (existing == null) return NotFound();

            existing.Content = model.Content;

            if (image != null)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                existing.ImagePath = "~/uploads/" + fileName;
            }

            db.SaveChanges();
            return RedirectToAction("AboutUsList");
        }

        // Delete
        public IActionResult DeleteAboutUs(int id)
        {
            var entry = db.Aboutus.Find(id);
            if (entry == null) return NotFound();

            db.Aboutus.Remove(entry);
            db.SaveChanges();
            return RedirectToAction("AboutUsList");
        }




    }
}

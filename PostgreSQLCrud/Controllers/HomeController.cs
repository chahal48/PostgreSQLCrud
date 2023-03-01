using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using PostgreSQLCrud.Models;
using PostgreSQLCrudBAL;
using PostgreSQLCrudEntity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PostgreSQLCrud.Controllers
{
    public class HomeController : Controller
    {
        private readonly ContactBAL _ContactBll;
        private readonly ProfessionBAL _ProfessionBll;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(ContactBAL contactBll, ProfessionBAL professionBll, IWebHostEnvironment webHostEnvironment)
        {
            _ContactBll = contactBll;
            _ProfessionBll = professionBll;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            ModelState.Clear();
            return View(_ContactBll.GetAllContact());
        }
        /// <summary>
        /// Auto generated
        /// </summary>
        /// <returns></returns>
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Add()
        {
            ViewBag.itemlist = new SelectList(_ProfessionBll.GetAllProfession().OrderBy(p => p.Profession), "ProfessionID", "Profession");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Add")]
        public IActionResult PostAdd(ContactEntity contactEntity)
        {
            try
            {
                ViewBag.itemlist = new SelectList(_ProfessionBll.GetAllProfession().OrderBy(p => p.Profession), "ProfessionID", "Profession");
                if (ModelState.IsValid)
                {
                    contactEntity.ContactImage = UploadImage(HttpContext.Request.Form.Files);

                    if (_ContactBll.AddContact(contactEntity))
                    {
                        TempData["AlertMsg"] = "Contact details added successfully.";
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.Message = "Error adding contact!!";
                    }
                }
            }
            catch
            {
                ViewBag.Message = "Unknown error!!";
            }
            return View();
        }
        public IActionResult Edit(int id)
        {
            ViewBag.itemlist = new SelectList(_ProfessionBll.GetAllProfession().OrderBy(p => p.Profession), "ProfessionID", "Profession");
            return View(_ContactBll.GetContactByID(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Edit")]
        public IActionResult PostEdit(int id, ContactEntity contactEntity)
        {
            string FileName = string.Empty;
            try
            {
                ViewBag.itemlist = new SelectList(_ProfessionBll.GetAllProfession().OrderBy(p => p.Profession), "ProfessionID", "Profession");

                if (ModelState.IsValid)
                {
                    contactEntity.ContactImage = UploadImage(HttpContext.Request.Form.Files);

                    if (_ContactBll.UpdateContact(contactEntity))
                    {
                        TempData["AlertMsg"] = "Contact details edited successfully.";
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.Message = "Unable to update contact!!";
                    }
                }
            }
            catch
            {
                ViewBag.Message = "Unknown error!!";
            }
            return View(contactEntity);
        }
        public IActionResult Details(int id)
        {
            return View(_ContactBll.GetContactByID(id));
        }
        public IActionResult Delete(int id)
        {
            try
            {
                if (_ContactBll.DeleteContact(id))
                {
                    TempData["AlertMsg"] = "Contact details deleted successfully.";
                }
            }
            catch { }
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// To mentain uniqueness of Email
        /// </summary>
        /// <param name="emailAddr"></param>
        /// <param name="initialEmail"></param>
        /// <returns></returns>
        [AcceptVerbs("Get", "Post")]
        public IActionResult IsEmailAvailable([Bind(Prefix = "emailAddr")] string emailAddr, string initialEmail)
        {
            if (initialEmail != "" && initialEmail != null)
            {
                if (emailAddr.ToLower() == initialEmail.ToLower())
                {
                    return new JsonResult(true);
                }
            }
            bool result = _ContactBll.CheckEmailAlreadyExists(emailAddr);
            if (result)
            {
                return new JsonResult(true);
            }
            return new JsonResult(false);
        }

        /// <summary>
        /// To validate Date fields
        /// </summary>
        /// <param name="DOB"></param>
        /// <returns></returns>
        [AcceptVerbs("GET", "POST")]
        public IActionResult isDateOfTimeInUse(string DOB)
        {
            DateTime Inputdate;
            if (DateTime.TryParse(DOB, out Inputdate))
            {
                int minimumAge = 5;
                int maximumAge = 70;
                if (Inputdate.Date < DateTime.Now.AddYears(minimumAge * -1))
                {
                    if (Inputdate.Date > DateTime.Now.AddYears(maximumAge * -1))
                    {
                        return new JsonResult(true);
                    }
                    else
                    {
                        return new JsonResult($"Error!! Date of birth " + Inputdate.ToShortDateString() + " greater then " + maximumAge + " years");
                    }
                }
                else
                {
                    return new JsonResult($"Error!! Date of birth " + Inputdate.ToShortDateString() + " less then " + minimumAge + " years");
                }
            }
            return new JsonResult("You must enter a valid date!!");
        }

        /// <summary>
        /// To get next record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult NextContact(int id)
        {
            var Result = _ContactBll.GetAllContact().SkipWhile(c => c.ContactID != id).Skip(1).FirstOrDefault();

            if (Result != null)
            {
                return RedirectToAction("Details", new RouteValueDictionary(new { controller = "Home", action = "Details", id = Result.ContactID }));
            }
            else
            {
                int FirstRecord = _ContactBll.GetAllContact().First().ContactID;
                return RedirectToAction("Details", new RouteValueDictionary(new { controller = "Home", action = "Details", id = FirstRecord }));
            }
        }

        /// <summary>
        /// To get previous record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult PreviousContact(int id)
        {
            var Result = _ContactBll.GetAllContact().OrderBy(c => c.ContactID).OrderBy(c => c.LastModified).SkipWhile(c => c.ContactID != id).Skip(1).FirstOrDefault();

            if (Result != null)
            {
                return RedirectToAction("Details", new RouteValueDictionary(new
                {
                    controller = "Home",
                    action = "Details",
                    id = Result.ContactID
                }));
            }
            else
            {
                int LastRecord = _ContactBll.GetAllContact().OrderBy(c => c.ContactID).OrderBy(c => c.LastModified).First().ContactID;
                return RedirectToAction("Details", new RouteValueDictionary(new { controller = "Home", action = "Details", id = LastRecord }));
            }
        }

        #region Upload Image
        public string UploadImage(dynamic files)
        {
            string ReturnFile = string.Empty;
            if (files.Count > 0)
            {
                //string contentRootPath = _webHostEnvironment.ContentRootPath;
                string webRootPath = _webHostEnvironment.WebRootPath;
                Guid guid = Guid.NewGuid();
                string NewImageName = guid.ToString() + Path.GetExtension(files[0].FileName);

                string path = string.Empty;
                path = Path.Combine(webRootPath, "Upload", NewImageName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                    ReturnFile = NewImageName;
                }
            }
            return ReturnFile;
        }
        #endregion

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

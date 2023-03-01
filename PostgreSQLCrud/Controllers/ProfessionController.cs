using Microsoft.AspNetCore.Mvc;
using PostgreSQLCrudBAL;
using PostgreSQLCrudEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostgreSQLCrud.Controllers
{
    public class ProfessionController : Controller
    {
        private readonly ProfessionBAL _professionBll;

        public ProfessionController(ProfessionBAL professionBll)
        {
            _professionBll = professionBll;
        }

        public IActionResult Index()
        {
            ModelState.Clear();
            return View(_professionBll.GetAllProfession());
        }
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Add")]
        public IActionResult PostAdd(ProfessionEntity professionEntity)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_professionBll.AddProfession(professionEntity))
                    {
                        TempData["AlertMsg"] = "Profession details added successfully.";
                        return RedirectToAction("Index", "Profession");
                    }
                    else
                    {
                        ViewBag.Message = "Error adding profession!!";
                    }
                }
            }
            catch
            {
                ViewBag.Message = "Unknown error!!";
            }
            return View();
        }
        public IActionResult EditProfessionDetails(int id)
        {
            return View(_professionBll.GetProfessionById(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("EditProfessionDetails")]
        public IActionResult PostEdit(int id, ProfessionEntity professionEntity)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_professionBll.UpdateProfession(professionEntity))
                    {
                        TempData["AlertMsg"] = "Profession details edited successfully.";
                        return RedirectToAction("Index", "Profession");
                    }
                    else
                    {
                        ViewBag.Message = "Unable to update profession!!";
                    }
                }
            }
            catch
            {
                ViewBag.Message = "Unknown error!!";
            }
            return View(professionEntity);
        }

        public IActionResult DeleteProfession(int id)
        {
            if (_professionBll.DeleteProfession(id))
            {
                TempData["AlertMsg"] = "Profession details deleted successfully.";
            }
            return RedirectToAction("Index", "Profession");
        }
        /// <summary>
        /// To mentain uniqueness of Profession
        /// </summary>
        /// <param name="Profession"></param>
        /// <param name="initialProfession"></param>
        /// <returns></returns>
        [AcceptVerbs("Get", "Post")]
        public IActionResult IsProfessionAvailable([Bind(Prefix = "Profession")] string Profession, string initialProfession)
        {
            if (initialProfession != "" && initialProfession != null)
            {
                if (Profession.ToLower() == initialProfession.ToLower())
                {
                    return new JsonResult(true);
                }
            }
            bool result = _professionBll.CheckProfessionAlreadyExists(Profession);
            if (result)
            {
                return new JsonResult(true);
            }
            return new JsonResult(false);
        }
    }
}

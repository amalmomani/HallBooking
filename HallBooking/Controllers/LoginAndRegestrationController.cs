using HallBooking.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace HallBooking.Controllers
{
    public class LoginAndRegestrationController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment webHostEnviroment;

        public LoginAndRegestrationController(ModelContext context, IWebHostEnvironment webHostEnviroment)
        {
            _context = context;
            this.webHostEnviroment = webHostEnviroment;
        }
        public async Task<IActionResult> Register(decimal id, [Bind("Userid,Fullname,Phonenumber,Image,Email,Password,Roleid,ImageFile")] Useraccount useraccount)
        {

            if (ModelState.IsValid)
            {
                HttpContext.Session.SetInt32("flag", 1);
                ViewBag.flag = HttpContext.Session.GetInt32("flag");

                var exist = _context.Useraccounts.Where(data => data.Email == useraccount.Email).SingleOrDefault();
                if (exist == null)
                {
                    if (useraccount.ImageFile != null)
                    {
                        //1- get w3rootpath
                        string w3rootpath = webHostEnviroment.WebRootPath;
                        //Guid.NewGuid : generate unique string before image name ;
                        ////2- generate image name and add unique string
                        string fileName = Guid.NewGuid().ToString() + "_" + useraccount.ImageFile.FileName;
                        string path = Path.Combine(w3rootpath + "/Image/" + fileName);
                        //4-create Image inside image file in w3root folder
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await useraccount.ImageFile.CopyToAsync(fileStream);
                        }

                        useraccount.Image = fileName;
                    }
                    useraccount.Roleid = 1;
                    _context.Add(useraccount);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Login", "LoginRegister");


                }
                else
                {
                    HttpContext.Session.SetInt32("flag", 0);
                    ViewBag.flag = HttpContext.Session.GetInt32("flag");
                }
            }
            return View(useraccount);
        }



   
            public IActionResult Register()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }

    }
}

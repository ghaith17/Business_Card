using ClosedXML.Excel;
using Newtonsoft.Json;
using ProgressSoft.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using ZXing;

namespace ProgressSoft.Controllers
{
    public class HomeController : Controller
    {
        ModelContext entities = new ModelContext();

        public ActionResult View_business_cards()
        {
            return View(entities.BusinessCards.ToList());
        }
        public ActionResult New_business_cards()
        {
            return View();
        }
        [HttpPost]
        public ActionResult View_business_cards(FormCollection formCollection)
        {
            string[] ids = formCollection["ID"].Split(new char[] { ',' });
           
            foreach (string id in ids)
            {
                int CustomerId = int.Parse(id);
                var item = (from obj in entities.BusinessCards
                        where obj.CustomerId == CustomerId
                            select obj).FirstOrDefault();
                entities.BusinessCards.Remove(item);
            }
            entities.SaveChanges();
            return RedirectToAction("View_business_cards");
        }
        public ActionResult submition()
        {
           
            List<BusinessCard> businessCards = new List<BusinessCard>();
            businessCards = (List<BusinessCard>)TempData["businessCards"];
            return View(businessCards);
        }
        [HttpPost]
        public ActionResult save(List<BusinessCard> businessCards)
        {
           
            using (entities)
            {
                foreach (var i in businessCards)
                {
                    
                        entities.BusinessCards.Add(i);

                }

                entities.SaveChanges();
            }
            return RedirectToAction("View_business_cards");
        }

        public ActionResult UI()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UI(BusinessCard businessCard)
        {
            HttpPostedFileBase poImgFile = Request.Files["ProfileImage"];
            if(poImgFile.ContentLength> 1048576) 
            {
                TempData["Message"] =  "Too Large image size";
                return View("UI");

            }
            businessCard.Gender=Request.Form["Gender"].ToString();
            businessCard.Photo = poImgFile.FileName;
            businessCard.CustomerId = (from BusinessCards in entities.BusinessCards select BusinessCards).ToList().Count() + 1;
          

            List<BusinessCard> businessCards = new List<BusinessCard>();
            businessCards.Add(businessCard);
            TempData["businessCards"] = businessCards;



            return RedirectToAction("submition");
        }
        public ActionResult xml()
        {
            return View();
        }
        [HttpPost]
        public ActionResult xml(HttpPostedFileBase xmlFile)
        {
            if (xmlFile.ContentType.Equals("application/xml") || xmlFile.ContentType.Equals("text/xml"))
            {
                try
                {
                    var xmlPath = Server.MapPath("~/FileUpload" + xmlFile.FileName);
                    xmlFile.SaveAs(xmlPath);
                    XDocument xDoc = XDocument.Load(xmlPath);
                    List<BusinessCard> businessCards = xDoc.Descendants("BusinessCard").Select
                        (customer => new BusinessCard
                        {
                            CustomerId = entities.BusinessCards.Count() + 1,
                            Name = customer.Element("Name").Value,
                            Gender = customer.Element("Gender").Value,
                            DOB = DateTime.ParseExact(customer.Element("DOB").Value, "dd/MM/yyyy", null),
                            Email = customer.Element("Email").Value,
                            Phone = customer.Element("Phone").Value,
                            Photo = customer.Element("Photo").Value,
                            Address = customer.Element("Address").Value

                        }).ToList();


                    ViewBag.Success = "File uploaded successfully..";
                    TempData["businessCards"] = businessCards;
                    return RedirectToAction("submition");
                }
                catch(Exception ex)
                {
                    ViewBag.Error = "Invalid XML file data";
                    return View("New_business_cards");
                }
            }
            else
            {
                ViewBag.Error = "Invalid file(Upload xml file only)";
            }
            return View("New_business_cards");


        }
        public ActionResult csv()
        {
            return View();
        }
        [HttpPost]
        public ActionResult csv(HttpPostedFileBase csvFile)
        {
            List<BusinessCard> businessCards = new List<BusinessCard>();
            string filePath = string.Empty;
            if (csvFile != null)
            {
                try
                {
                    string path = Server.MapPath("~/Uploads/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    filePath = path + Path.GetFileName(csvFile.FileName);
                    string extension = Path.GetExtension(csvFile.FileName);
                    csvFile.SaveAs(filePath);

                    //Read the contents of CSV file.
                    string csvData = System.IO.File.ReadAllText(filePath);

                    //Execute a loop over the rows.
                    foreach (string row in csvData.Split('\n'))
                    {
                        if (!string.IsNullOrEmpty(row))
                        {
                            try
                            {
                                businessCards.Add(new BusinessCard
                                {
                                    CustomerId = entities.BusinessCards.Count() + 1,
                                    Name = row.Split(',')[0],
                                    Gender = row.Split(',')[1],
                                    DOB = DateTime.ParseExact(row.Split(',')[2], "dd/MM/yyyy", null),
                                    Email = row.Split(',')[3],
                                    Phone = row.Split(',')[4],
                                    Photo = row.Split(',')[5],
                                    Address = row.Split(',')[6]
                                });
                            }
                            catch (System.FormatException ex)
                            {
                                continue;
                            }
                        }
                    }

                    TempData["businessCards"] = businessCards;
                    return RedirectToAction("submition");
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Invalid CSV file data";
                    return View("New_business_cards");
                }
            }

            return View(businessCards);
        }

        // Info   return View();

        public ActionResult QR()
        {
            return View();
        }
        [HttpPost]
        public ActionResult QR(HttpPostedFileBase QRFile)
        {
            
            string filePath = string.Empty;
            if (QRFile != null)
            {
                try
                {
                    string path = Server.MapPath("~/Uploads/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }


                    string barcodeText = "";
                    string imagePath = "~/Uploads/";
                    filePath = imagePath + Path.GetFileName(QRFile.FileName);
                    string barcodePath = Server.MapPath(filePath);
                    var barcodeReader = new BarcodeReader();

                    var result = barcodeReader.Decode(new Bitmap(barcodePath));
                    if (result != null)
                    {
                        var businessCards = JsonConvert.DeserializeObject<List<BusinessCard>>(result.ToString());
                        barcodeText = result.Text;

                        TempData["businessCards"] = businessCards;
                        return RedirectToAction("submition");

                    }

                    ViewBag.Error = "Invalid QR image data" ;
                    return View("New_business_cards");

                }
                catch(Exception ex)
                {
                    ViewBag.Error = "Invalid QR image data";
                    return View("New_business_cards");
                }
             }

            return View();
        }
   
        public FileResult ExportCSV()
        {
            
            List<object> customers = (from businessCard in entities.BusinessCards.ToList().Take(10)
                                      select new[] { businessCard.CustomerId.ToString(),
                                                             businessCard.Name,
                                                             businessCard.Address,
                                                             businessCard.DOB.ToString(),
                                                             businessCard.Gender,
                                                             businessCard.Email,
                                                             businessCard.Phone,
                                                             businessCard.Photo
                                 }).ToList<object>();
           

            //Insert the Column Names.
            customers.Insert(0, new string[8] { "Customer ID", "Customer Name", "Address", "DOB", "Gender", "Email", "Phone", "Photo" });

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < customers.Count; i++)
            {
                
                string[] customer = (string[])customers[i];
                for (int j = 0; j < customer.Length; j++)
                {
                    //Append data with separator.
                    
                    sb.Append(customer[j] + ',');
                }

                //Append new line character.
                sb.Append("\r\n");

            }

            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "BusinessCards.csv");
        }

        public FileResult ExportXML()
        {
            var data = entities.BusinessCards.ToList();
            var serializer = new System.Xml.Serialization.XmlSerializer(data.GetType());
            serializer.Serialize(Response.OutputStream, data);
            return File(Encoding.UTF8.GetBytes(data.ToString()), "text/xml", "BusinessCards.xml");

        }

    }
}

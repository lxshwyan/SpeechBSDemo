using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SpeechBSDemo.Controllers
{
    public class TestSpeechController : Controller
    {
        // GET: TestSpeech
        public ActionResult Index()
        {   
            return View();
        }
        public async Task<ActionResult> PlayWav(string id,string spText)
        {    
            string strPath = Server.MapPath("~\\MP4\\" + id + ".wav");
            SpeechService.SaveMp3(strPath, spText);
            try
            {
                //FTP 直接返回Stream，需要自己实现
                using (FileStream fileStream = new FileStream(strPath, FileMode.Open))
                {
                    byte[] fileByte = new byte[fileStream.Length];
                    fileStream.Seek(0, SeekOrigin.Begin);
                    fileStream.Read(fileByte, 0, (int)fileStream.Length);
                    long fSize = fileStream.Length;
                    long startbyte = 0;
                    long endbyte = fSize - 1;
                    int statusCode = 200;
                    if ((Request.Headers["Range"] != null))
                    {
                        //Get the actual byte range from the range header string, and set the starting byte.
                        string[] range = Request.Headers["Range"].Split(new char[] { '=', '-' });
                        startbyte = Convert.ToInt64(range[1]);
                        if (range.Length > 2 && range[2] != "") endbyte = Convert.ToInt64(range[2]);
                        //If the start byte is not equal to zero, that means the user is requesting partial content.
                        if (startbyte != 0 || endbyte != fSize - 1 || range.Length > 2 && range[2] == "")
                        { statusCode = 206; }//Set the status code of the response to 206 (Partial Content) and add a content range header.                                    
                    }
                    long desSize = endbyte - startbyte + 1;
                    //Headers
                    Response.StatusCode = statusCode;
                    Response.ContentType = "audio/mpeg";
                    Response.AddHeader("Content-Accept", Response.ContentType);
                    Response.AddHeader("Content-Length", desSize.ToString());
                    Response.AddHeader("Content-Range", string.Format("bytes {0}-{1}/{2}", startbyte, endbyte, fSize));
                    return File(fileByte, Response.ContentType);
                    //return new FileStreamResult(fileByte, Response.ContentType);  这个方法不行
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
       
    }
}
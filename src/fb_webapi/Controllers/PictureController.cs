using fb_webapi.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using ImageProcessorCore;
using ImageProcessorCore.Processors;
using ImageProcessorCore.Formats;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace  fb_webapi.Controllers {
    public class PictureController : Controller {
        private ApplicationDbContext dbContext;
        public PictureController (ApplicationDbContext context) {
            this.dbContext = context;
        }

        [Route("api/picture/full/{id}")]
        public IActionResult GetFull(int id) {
            var image = dbContext.Pictures
                .Where(p => p.Id == id)
                .Include(p => p.Full)
                .FirstOrDefault()
                .Full;

            MemoryStream ms = new MemoryStream(image);
            
            return new FileStreamResult(ms, "image/png");
        }

        [Route("api/picture/small/{id}")]
        public IActionResult GetSmall(int id) {
            System.Console.WriteLine("Picture id is: {0}", id);

            var image = dbContext.Pictures
                .Where(p => p.Id == id)
                .Include(p => p.Thumbnail)
                .FirstOrDefault()
                .Thumbnail;

            MemoryStream ms = new MemoryStream(image);
            
            return new FileStreamResult(ms, "image/png");
        }

        [Route("api/pictures/add/{mealId:int}")]
        public async Task<IActionResult> AddPicture(int mealId) {
            var pictures = new List<Picture>();

            var meal = dbContext.Meals
                .Where(m => m.Id == mealId)
                .FirstOrDefault();

            if (meal == null) {
                return new BadRequestResult();
            }

            foreach (var file in Request.Form.Files) {
                var picture = new Picture();
                picture.MealId = meal.Id;

                dbContext.Pictures.Add(picture);
                await dbContext.SaveChangesAsync();
                
                //Read the file from the stream and convert it to PNG
                using (var tempStream = new MemoryStream())
                using (var outStream = new MemoryStream()) {
                    await file.CopyToAsync(tempStream);

                    var image = new Image(tempStream);
                    image.SaveAsPng(outStream);
                    
                    picture.Full = new byte[outStream.Length];
                    picture.Full = outStream.ToArray();
                }

                //Load the picture, resize and save as thumbail.
                using (var inStream = new MemoryStream(picture.Full))
                using (var outStream = new MemoryStream()) {
                    var image = new Image (inStream);

                    image.Resize(144,144)
                        .SaveAsPng(outStream);

                    picture.Thumbnail = new byte[outStream.Length];
                    picture.Thumbnail = outStream.ToArray();
                }

                pictures.Add(picture);
            }

            meal.Pictures.AddRange(pictures);
            await dbContext.SaveChangesAsync();

            return new OkResult();
        }
    }
}
using System.ComponentModel.DataAnnotations.Schema;

namespace fb_webapi.Entities {
    public class Picture {
        public int Id { get; set; }
        public int MealId { get; set; }

        public byte[] Full { get; set; }
        public byte[] Thumbnail { get; set; }
    }
}
using System.Collections.Generic;

namespace fb_webapi.Entities {
    public class Meal {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<Picture> Pictures { get; set; }

        public Meal () {
            Pictures = new List<Picture>();
        }
    }
}
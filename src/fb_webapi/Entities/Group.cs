using System.Collections.Generic;

namespace fb_webapi.Entities {
    public class Group {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<User> Members { get; set; }

        public Group () {
            Members = new List<User>();
        }
    }
}
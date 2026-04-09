using DAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public enum MediaSortBy { Title, PublishDate, Likes }

    public class Media : Record
    {
        public string Title { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string YoutubeId { get; set; }
        public DateTime PublishDate { get; set; } = DateTime.Now;

        public int OwnerId { get; set; } = 1;
        public bool Shared { get; set; } = true;
        [JsonIgnore]
        public User Owner => DB.Users.Get(OwnerId).Copy();
        public int Likes => DB.Likes.ToList().Where(l => l.MediaId == Id).Count();

        [JsonIgnore]
        public List<User> LikedByUsers
        {
            get
            {
                var likesList = DB.Likes.ToList().Where(l => l.MediaId == Id);
                List<User> users = new List<User>();
                foreach (Like like in likesList)
                {
                    var user = DB.Users.Get(like.UserId);
                    if (user != null)
                        users.Add(user);
                }
                return users;
            }
        }

        public bool IsLikedByConnectedUser()
        {
            if (User.ConnectedUser == null) return false;
            return DB.Likes.ToList().Exists(l => l.MediaId == Id && l.UserId == User.ConnectedUser.Id);
        }

      public override bool IsValid()
        {
            if (!HasRequiredLength(Title, 1)) return false;
            if (!HasRequiredLength(Category, 1)) return false;
            if (!HasRequiredLength(Description, 1)) return false;
            if (DB.Medias.ToList().Where(m => m.YoutubeId == YoutubeId && m.Id != Id).Any()) return false;
            return true;
        }
    }
}
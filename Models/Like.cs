using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
   public class Like : Record
   {
       public int MediaId { get; set; }
       public int UserId { get; set; }

       public override bool IsValid()
       {
         if (DB.Medias.Get(MediaId) == null)
         {
            return false;

         }
         if (DB.Users.Get(UserId) == null)
         {
            return false;
         }
         if (DB.Likes.ToList().Exists(l => l.MediaId == MediaId && l.UserId == UserId && l.Id != Id))
         {
            return false;
         }

         return true;
       }
   }
}
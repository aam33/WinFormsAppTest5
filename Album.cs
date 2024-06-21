using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsAppTest5
{
    internal class Album
    {
        public int ID {  get; set; }
        public int Artist {  get; set; }
        // make sure to capitalize String so that you use String class
        public String AlbumTitle { get; set; }
        public int ReleaseYear { get; set; }
        public String ImageURL { get; set; }

        /* TODO: make a List<Track> songs
         later connect to the database */
    }
}

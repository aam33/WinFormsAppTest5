using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using System.Windows.Forms;

namespace WinFormsAppTest5
{
    internal class AlbumsDAO
    {
        // version 1 only contains fake data. No connection to actual db yet.
        // TODO: Connect to database

        /* Initialize with new List<Album>() aka a list of 0 items
        automatically private unless specified */

        //public List<Album> albums = new List<Album>();

        // version 2 connects to the database
        // MAMP server is local host; otherwise could use aws or some such
        String connectionString = "datasource=localhost;port=3306;username=root;password=root;database=music;";

        // methods that perform actions for main program
        public List<AlbumBand> getAllAlbums()
        {
            Console.WriteLine("Getting all album titles...");
            // start with an empty list
            List<AlbumBand> returnAlbumsBands = new List<AlbumBand>();

            // connect to mysql server (MAMP)
            // by default, Microsoft expects you to be talking to SQL server (why are we using C# again?), so we need a dependency
            //install package MySql.Data (dependency)
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open(); // log in to server

            // define the sql statement to fetch all albums
            MySqlCommand command = new MySqlCommand("SELECT bands.name, albums.name, albums.year_released, albums.image_url, albums.album_url FROM albums INNER JOIN bands ON albums.band_id = bands.id ORDER BY albums.year_released ASC", connection);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    AlbumBand ab = new AlbumBand
                    {
                        Artist = reader.IsDBNull(0) ? null : reader.GetString(0),
                        AlbumTitle = reader.IsDBNull(1) ? null : reader.GetString(1),
                        ReleaseYear = reader.IsDBNull(2) ? 0 : reader.GetInt32(2),
                        ImageURL = reader.IsDBNull(3) ? null : reader.GetString(3),
                        AlbumURL = reader.IsDBNull(4) ? null : reader.GetString(4)
                        /*AlbumID = reader.GetInt32(0),
                        Artist = reader.GetString(0),
                        AlbumTitle = reader.GetString(1),
                        ReleaseYear = reader.GetInt32(2),
                        ImageURL = reader.GetString(3),  // TEST STEP 3
                        //BandID = reader.GetInt32(5),
                        //BandName = reader.GetString(6),
                        //YearFounded = reader.GetInt32(7),
                        //SingerID = reader.GetInt32(8)
                        AlbumURL = reader.GetString(4)*/
                    };
                    returnAlbumsBands.Add(ab);    // attach as an item to our list
                }
            }

            // after querying
            connection.Close();

            Console.WriteLine("All album titles retrieved");
            // replace returnAlbums with return AlbumsBands
            return returnAlbumsBands;   // TEST STEP 5
        }

        // TEST CHANGE TO LIST OF TYPE ALBUM
        // works - YAY! :D
        public List<AlbumBand> searchAlbumTitles(String searchTerm)
        {
            Console.WriteLine("Searching album titles...");
            // start with an empty list
            List<AlbumBand> returnAlbumsBands = new List<AlbumBand>();

            // connect to mysql server (MAMP)
            // by default, Microsoft expects you to be talking to SQL server, so we need a dependency
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open(); // log in to server

            String searchWildPhrase = "%" + searchTerm + "%";


            // define the sql statement to fetch all albums
            // protect against SQL injection
            MySqlCommand command = new MySqlCommand();
            command.CommandText = "SELECT bands.name, albums.name, albums.year_released, albums.image_url, albums.album_url FROM albums INNER JOIN bands ON albums.band_id = bands.id WHERE albums.name LIKE @search OR bands.name LIKE @search OR CAST(albums.year_released AS CHAR) LIKE @search ORDER BY albums.year_released ASC";   // TEST STEP 1
            command.Parameters.AddWithValue("@search", searchWildPhrase);
            command.Connection = connection;    // defined at beginning of class
            

            // TEST
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    AlbumBand ab = new AlbumBand
                    {
                        Artist = reader.IsDBNull(0) ? null : reader.GetString(0),
                        AlbumTitle = reader.IsDBNull(1) ? null : reader.GetString(1),
                        ReleaseYear = reader.IsDBNull(2) ? 0 : reader.GetInt32(2),
                        ImageURL = reader.IsDBNull(3) ? null : reader.GetString(3),
                        AlbumURL = reader.IsDBNull(4) ? null : reader.GetString(4)
                        /*AlbumID = reader.GetInt32(0),
                        Artist = reader.GetString(0),
                        AlbumTitle = reader.GetString(1),
                        ReleaseYear = reader.GetInt32(2),
                        ImageURL = reader.GetString(3),  // TEST STEP 3
                        //BandID = reader.GetInt32(5),
                        //BandName = reader.GetString(6),
                        //YearFounded = reader.GetInt32(7),
                        //SingerID = reader.GetInt32(8)
                        AlbumURL = reader.GetString(4)*/
                    };
                    returnAlbumsBands.Add(ab);    // attach as an item to our list
                }
            }
            // END TEST

            
            // after querying
            connection.Close();

            Console.WriteLine("Album titles search complete");
            return returnAlbumsBands;
        }

        public List<Track> retrieveTracksFromAlbum(String searchTerm2)
        {
            Console.WriteLine("Getting all tracks from album...");
            List<Track> returnTracklist = new List<Track>();

            // connect to mysql server (MAMP)
            // install package MySql.Data (dependency)
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            // define the sql statement to fetch tracks
            MySqlCommand command = new MySqlCommand();
            command.CommandText = "SELECT tracks.number_in_album, tracks.name, tracks.video_url FROM tracks INNER JOIN albums ON albums.id = tracks.albums_ID WHERE albums.name LIKE @search ORDER BY tracks.number_in_album ASC";
            command.Parameters.AddWithValue("@search", searchTerm2);
            command.Connection = connection;

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Track t = new Track
                    {
                        TrackNumber = reader.IsDBNull(0) ? null : reader.GetInt32(0),
                        TrackTitle = reader.GetString(1),
                        MusicURL = reader.IsDBNull(2) ? null : reader.GetString(2)
                    };
                    returnTracklist.Add(t);    // attach as an item to our list
                }
            }

            // after querying
            connection.Close();

            Console.WriteLine("All tracks in album retrieved");
            return returnTracklist;
        }



        // TODO: New form and addOneAlbum works in it
        /*internal int addOneAlbum(Album album)
        {
            // connect to the MySQL server
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            // define the sql statement to fetch all albums
            MySqlCommand cmd = new MySqlCommand("INSERT INTO albums(`BAND_ID`, `NAME`, `YEAR_RELEASED`, `IMAGE_URL`) VALUES (@artistname, @albumtitle, @year, @imageURL)", connection);

            // associate parameters with actual values
            // why red underline under command?
            command.Parameters.AddWithValue("@artistname", album.Artist);
            command.Parameters.AddWithValue("@albumtitle", album.AlbumTitle);
            command.Parameters.AddWithValue("@year", album.ReleaseYear);
            command.Parameters.AddWithValue("@imageURL", album.ImageURL);

            int newRows = command.ExecuteNonQuery();  // returns how many rows are affected
            connection.Close();

            return newRows;
        }*/
    }
}










/* extra stuff
 * INSERT INTO albums (band_id, name, year_released, image_url) VALUES ((SELECT bands.ID FROM bands WHERE name = "The Killers"), "Imploding the Mirage", 2020, "https://upload.wikimedia.org/wikipedia/en/b/b9/The_Killers_-_Imploding_the_Mirage.png"), ((SELECT bands.ID FROM bands WHERE name = "The Killers"), "Pressure Machine", 2021, "https://upload.wikimedia.org/wikipedia/en/3/39/The_Killers_-_Pressure_Machine.png");
 * 
 * 
 * TODO:
 * add all TK albums (even if empty)
 * fix if click in bottom dataGridView, update image
 * handle that error when clicking edge cell
 */

using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public List<Album> getAllAlbums()
        {
            Console.WriteLine("Getting all album titles...");
            // start with an empty list
            List<Album> returnAlbums = new List<Album>();

            // connect to mysql server (MAMP)
            // by default, Microsoft expects you to be talking to SQL server (why are we using C# again?), so we need a dependency
            //install package MySql.Data (dependency)
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open(); // log in to server

            // define the sql statement to fetch all albums
            MySqlCommand command = new MySqlCommand("SELECT * FROM albums ORDER BY year_released ASC", connection);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Album a = new Album
                    {
                        // at position 0, expect to see an integer
                        ID = reader.GetInt32(0),
                        // at position 1, expect to see an integer
                        Artist = reader.GetInt32(1),
                        // at position 2, expect to see a string
                        AlbumTitle = reader.GetString(2),
                        // at position 3, expect to see an int
                        ReleaseYear = reader.GetInt32(3),
                        // at position 4, expect to see a string
                        ImageURL = reader.GetString(4)
                    };
                    returnAlbums.Add(a);    // attach as an item to our list
                }
            }

            // after querying
            connection.Close();

            Console.WriteLine("All album titles retrieved");
            return returnAlbums;
        }

        public List<Album> searchAlbumTitles(String searchTerm)
        {
            Console.WriteLine("Searching album titles...");
            // start with an empty list
            List<Album> returnAlbums = new List<Album>();

            // connect to mysql server (MAMP)
            // by default, Microsoft expects you to be talking to SQL server, so we need a dependency
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open(); // log in to server

            String searchWildPhrase = "%" + searchTerm + "%";

            // define the sql statement to fetch all albums
            // protect against SQL injection
            MySqlCommand command = new MySqlCommand();
            //(test sql statement) //MySqlCommand command = new MySqlCommand("SELECT * FROM albums WHERE year_released = 2004 ORDER BY name ASC", connection);
            command.CommandText = "SELECT * FROM albums WHERE name LIKE @search OR image_url LIKE @search ORDER BY year_released ASC";
            command.Parameters.AddWithValue("@search", searchWildPhrase);
            command.Connection = connection;    // defined at beginning of class

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Album a = new Album
                    {
                        ID = reader.GetInt32(0),
                        Artist = reader.GetInt32(1),
                        AlbumTitle = reader.GetString(2),
                        ReleaseYear = reader.GetInt32(3),
                        ImageURL = reader.GetString(4)
                    };
                    returnAlbums.Add(a);    // attach as an item to our list
                }
            }

            // after querying
            connection.Close();

            Console.WriteLine("Album titles search complete");
            return returnAlbums;
        }
    }
}

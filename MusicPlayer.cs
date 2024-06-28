using System.Windows.Forms;

namespace WinFormsAppTest5;

public partial class MusicPlayer : Form
{
    // connect a list of items (albums) to grid control
    BindingSource topBindingSource = new BindingSource();
    BindingSource bottomBindingSource = new BindingSource();
    public MusicPlayer()
    {
        InitializeComponent();
    }

    private void button1_Click(object sender, EventArgs e)
    {
        Console.WriteLine("Button 1 clicked. Now retrieving albums...");
        // Version 1 temporary code. Notice private method and on Click event name
        // creates new list every time we click the button - I don't think we want this!

        //AlbumsDAO myAlbumsDAO = new AlbumsDAO();  // Version 1
        /*Album a1 = new Album
        {
            ID = 1,
            Artist = 1,
            AlbumTitle = "Test",
            ReleaseYear = 2024,
            ImageURL = "Nothing yet"
        };
        Album a2 = new Album
        {
            ID = 2,
            Artist = 2,
            AlbumTitle = "Test 2",
            ReleaseYear = 2022,
            ImageURL = "Gonna fail"
        };

        // albums list in AlbumsDAO class must be public
        myAlbumsDAO.albums.Add(a1);
        myAlbumsDAO.albums.Add(a2);*/

        // Version 2 actually connects to the database
        AlbumsDAO myAlbumsDAO = new AlbumsDAO();    // Version 2

        // connect the list to the grid view control
        //top.DataSource = myAlbumsDAO.albums;   // Version 1
        topBindingSource.DataSource = myAlbumsDAO.getAllAlbums(); // Version 2

        // tell the grid view that the binding source is associated with it
        dataGridView1.DataSource = topBindingSource;
    }

    private void button2_Click(object sender, EventArgs e)
    {
        // TODO: same functionality if ENTER key pressed and search box is not empty
        Console.WriteLine("Button 2 clicked. Now searching...");
        AlbumsDAO myAlbumsDAO = new AlbumsDAO();

        // connect the list to the grid view control
        topBindingSource.DataSource = myAlbumsDAO.searchAlbumTitles(textBox1.Text);
        //topBindingSource.DataSource = myAlbumsDAO.searchAlbumTitles(contents);

        // tell the grid view that the binding source is associated with it
        dataGridView1.DataSource = topBindingSource;
    }

    private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        // add explicit cast
        DataGridView dataGridView = (DataGridView)sender;

        // Check if the click was on a header cell
        if (e.RowIndex == -1)
        {
            // handle header click
            MessageBox.Show("Column header clicked");
            // if column index is valid
            if (e.ColumnIndex >= 0 && e.ColumnIndex < dataGridView.Columns.Count)
            {
                // Gets or sets the caption text on the column's header cell
                String headerText = dataGridView.Columns[e.ColumnIndex].HeaderText;
                MessageBox.Show("Header contents: " + headerText);
            }
            // TODO: Sort by that column by calling method in AlbumsDAO???
            return;
        }

        // trying to handle case if someone clicks on that bottom row
        if (e.RowIndex >= dataGridView.RowCount - 1 || dataGridView.CurrentCell.Value == null)
        {
            return; // Do nothing if an invalid cell or the new row placeholder is clicked
        }

        // get the row number clicked
        int rowClicked = dataGridView.CurrentRow.Index;
        String contents = dataGridView.CurrentCell.Value.ToString();
        MessageBox.Show("Clicked row " + rowClicked);
        // cell contents works!! :D
        MessageBox.Show("Cell contents: " + contents);

        // Check if column index is valid
        if (e.ColumnIndex >= 0 && e.ColumnIndex < dataGridView.Columns.Count)
        {
            String imageURL = dataGridView.Rows[rowClicked].Cells[4].Value.ToString();

            // test code starts here -- appears to be working as intended
            if (imageURL == null || imageURL.Length == 0)
            {
                pictureBox1.Hide();
                //return;
            }
            else
            {
                MessageBox.Show("Image URL=" + imageURL);
                //pictureBox1.Load(imageURL);   // had to handle according to Wikipedia's User-Agent policy: https://meta.wikimedia.org/wiki/User-Agent_policy
                LoadImageFromUrlAsync(imageURL, pictureBox1);
                pictureBox1.Show();
            }

            // TODO: Get rid of message boxes
            // TODO: what if cells(column) 4 isn't always the URL?

            String forNewQuery = "";
            // set headerText if there is any
            String headerText2 = "";
            if (dataGridView.Columns[e.ColumnIndex].HeaderText != null)
            {
                headerText2 = dataGridView.Columns[(int)e.ColumnIndex].HeaderText;
            }

            //if ((headerText2 == "Artist") || (headerText2 == "AlbumTitle"))
            //{
            MessageBox.Show(contents + " " + headerText2);

            /*if (forNewQuery == null)
                {
                    return;
                }*/
            //else
            if ((headerText2 == "AlbumTitle") || (headerText2 == "AlbumID") || (headerText2 == "ReleaseYear") || (headerText2 == "ImageURL"))
            {
                // forNewQuery needs to be AlbumTitle unless you click in Artist column
                forNewQuery = dataGridView.Rows[(int)rowClicked].Cells[2].Value?.ToString();
                MessageBox.Show(forNewQuery);

                AlbumsDAO myAlbumsDAO2 = new AlbumsDAO();

                bottomBindingSource.DataSource = myAlbumsDAO2.retrieveTracksFromAlbum(forNewQuery);

                // tell the grid view that the binding source is associated with it
                dataGridView2.DataSource = bottomBindingSource;
            }
            else if (headerText2 == "Artist")
            {
                // forNewQuery needs to be AlbumTitle unless you click in Artist column
                forNewQuery = contents;
                MessageBox.Show(forNewQuery);

                AlbumsDAO myAlbumsDAO2 = new AlbumsDAO();
                bottomBindingSource.DataSource = myAlbumsDAO2.searchAlbumTitles(forNewQuery);

                // tell the grid view that the binding source is associated with it
                // different binding source for different gridviews
                dataGridView2.DataSource = bottomBindingSource;
            }
            //}
        }
    }

    private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        // add explicit cast
        DataGridView dataGridView2 = (DataGridView)sender;

        // Check if the click was on a header cell
        if (e.RowIndex == -1)
        {
            MessageBox.Show("Column header clicked");
            if (e.ColumnIndex >= 0 && e.ColumnIndex < dataGridView2.Columns.Count)
            {
                // Gets or sets the caption text on the column's header cell
                String headerText = dataGridView2.Columns[e.ColumnIndex].HeaderText;
                MessageBox.Show("Header contents: " + headerText);
            }
            // TODO: Sort by that column by calling method in AlbumsDAO???
            return;
        }

        // click on bottom row
        if (e.RowIndex >= dataGridView2.RowCount - 1 || dataGridView2.CurrentCell.Value == null)
        {
            return;
        }

        // get the row number clicked
        int rowClicked = dataGridView2.CurrentRow.Index;
        String contents = dataGridView2.CurrentCell.Value?.ToString();
        MessageBox.Show("Clicked row " + rowClicked);
        MessageBox.Show("Cell contents: " + contents);


        // TODO: Get rid of message boxes
        // TODO: what if cells(column) 2 isn't always the URL?

        // Check if column index is valid
        if (e.ColumnIndex >= 0 && e.ColumnIndex < dataGridView2.Columns.Count)
        {
            // if on the filtered album view, switch to track view when user clicks on album
            // else (on the tracklist), pull up video player for given URL
            String forNewQuery = "";
            // TODO: fix error when clicking on border cell
            String headerText3 = dataGridView2.Columns[e.ColumnIndex].HeaderText;
            if ((headerText3 == "AlbumTitle") || (headerText3 == "AlbumID") || (headerText3 == "Artist") || (headerText3 == "ReleaseYear") || (headerText3 == "ImageURL"))    // on album view for particular artist
            {
                MessageBox.Show(contents + " " + headerText3);

                // always update image
                String imageURL = dataGridView2.Rows[rowClicked].Cells[4].Value?.ToString();

                // test code starts here -- appears to be working as intended
                if (imageURL == null || imageURL.Length == 0)
                {
                    pictureBox1.Hide();
                    //MessageBox.Show("RETURN");
                    return;
                }
                else
                {
                    MessageBox.Show("Image URL=" + imageURL);
                    //pictureBox1.Load(imageURL);   // had to handle according to Wikipedia's User-Agent policy: https://meta.wikimedia.org/wiki/User-Agent_policy
                    LoadImageFromUrlAsync(imageURL, pictureBox1);
                    pictureBox1.Show();
                }


                // switch screen if headerText3 == AlbumTitle
                forNewQuery = contents;
                MessageBox.Show(forNewQuery);
                if (forNewQuery == null)
                {
                    return;
                }
                else if (headerText3 == "AlbumTitle")
                {
                    // create new AlbumsDAO
                    AlbumsDAO myAlbumsDAO3 = new AlbumsDAO();

                    bottomBindingSource.DataSource = myAlbumsDAO3.retrieveTracksFromAlbum(forNewQuery);

                    // tell the grid view that the binding source is associated with it
                    dataGridView2.DataSource = bottomBindingSource;
                }
            }
            else if ((headerText3 == "TrackTitle") || (headerText3 == "TrackNumber") || (headerText3 == "VideoURL"))    // showing tracklist
            {
                String videoURL = dataGridView2.Rows[rowClicked].Cells[2].Value?.ToString();

                if (string.IsNullOrEmpty(videoURL))
                {
                    MessageBox.Show("headerText3: " + headerText3);
                    MessageBox.Show("<ERROR: VIDEO NOT FOUND>");
                    return;
                }
                else
                {
                    MessageBox.Show("headerText3: " + headerText3);
                    MessageBox.Show("Video URL=" + videoURL);
                    // TODO: pull up embedded video player
                }
            }
        }
    }

    // beginning of ChatGPT code (handles Wikipedia User-Agent policy)
    private async void LoadImageFromUrlAsync(string imageUrl, PictureBox pictureBox)
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                // Set the User-Agent header
                client.DefaultRequestHeaders.UserAgent.ParseAdd("WinFormsAppTest5/1.0 (yourname@example.com)");

                HttpResponseMessage response = await client.GetAsync(imageUrl);
                response.EnsureSuccessStatusCode();

                using (Stream stream = await response.Content.ReadAsStreamAsync())
                {
                    pictureBox.Image = Image.FromStream(stream);
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error loading image: " + ex.Message);
        }
    }   // end LoadImageFromUrlAsync
    // end of ChatGPT code

}   // end public partial class MusicPlayer : Form

// TODO: (MOVE THIS BACK INSIDE NAMESPACE) Make new form, connect forms, make "Add new album" box and button
/*private void button3_Click(object sender, EventArgs e)
{
    // TODO: Error checking
    // add a new item to the database
    Album albumFromForm = new Album
    {
        // check to see if artist exists in bands table, and if not add and assign new id
        Artist = txt_albumArtist.Text,
        AlbumTitle = txt_albumName.Text,
        ReleaseYear = Int32.Parse(txt_albumYear.Text),
        ImageURL = txt_ImageURL.Text
    };

    AlbumsDAO albumsDAO = new AlbumsDAO();
    int result = albumsDAO.addOneAlbum(albumFromForm);
    MessageBox.Show(result + " new row(s) inserted");
}*/

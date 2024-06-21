using System.Windows.Forms;

namespace WinFormsAppTest5;

public partial class MusicPlayer : Form
{
    // connect a list of items (albums) to grid control
    BindingSource albumBindingSource = new BindingSource();
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
        //albumBindingSource.DataSource = myAlbumsDAO.albums;   // Version 1
        albumBindingSource.DataSource = myAlbumsDAO.getAllAlbums(); // Version 2

        // tell the grid view that the binding source is associated with it
        dataGridView1.DataSource = albumBindingSource;
    }

    private void button2_Click(object sender, EventArgs e)
    {
        Console.WriteLine("Button 2 clicked. Now searching...");
        AlbumsDAO myAlbumsDAO = new AlbumsDAO();

        // connect the list to the grid view control
        albumBindingSource.DataSource = myAlbumsDAO.searchAlbumTitles(textBox1.Text);

        // tell the grid view that the binding source is associated with it
        dataGridView1.DataSource = albumBindingSource;
    }

    private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        // add explicit cast
        DataGridView dataGridView = (DataGridView)sender;

        // get the row number clicked
        int rowClicked = dataGridView.CurrentRow.Index;
        String contents = dataGridView.CurrentCell.Value.ToString();
        MessageBox.Show("Clicked row " + rowClicked);
        // cell contents works!! :D
        MessageBox.Show("Cell contents: " + contents);

        String imageURL = dataGridView.Rows[rowClicked].Cells[4].Value.ToString();
        MessageBox.Show("URL=" + imageURL);
        //pictureBox1.Load(imageURL);   // had to handle according to Wikipedia's User-Agent policy: https://meta.wikimedia.org/wiki/User-Agent_policy
        LoadImageFromUrlAsync(imageURL, pictureBox1);

        // TODO: Get rid of message boxes
        // TODO: what if cells(column) 4 isn't always the URL?
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
    }
    // end of ChatGPT code




}

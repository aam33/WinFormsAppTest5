# MusicPlayerApp

Started by following [tutorial found here](https://youtu.be/VX4wl7qIcbA?si=aSEgqtDoROr6ldDV). However, this tutorial was missing a very crucial part: how to put a WebView2 onto the form and embed a YouTube video within it.

Many tutorials are available for the WebBrowser component, but this is not an available component for the `Windows Forms App` (as compared with the misleadingly-named `Windows Forms App (.NET Framework)`). The Windows Forms App supports WebView2, not WebBrowser, so I used this [tutorial](https://www.youtube.com/embed/tMRyQ_zAb2A?si=M-uwnvBFWyZ4VJWx) to learn the capabilities of the WebView2 component and to embed a YouTube video using HTML.

However, this still didn't return the results I wanted. Due to YouTube's content embedding policies, YouTube music videos cannot be embedded when the server is localhost. Content must be served from a remote server, else the video player will display "Video unavailable". I overcame this issue by electing to switch to the Spotify embedded player instead of YouTube.

namespace ObjectDetector;

public class VisualizationTemplate
{
    private static readonly string Template = """
                                              "
                                              <!DOCTYPE html>
                                              <html lang="en">
                                              <head>
                                                  <meta charset="UTF-8">
                                                  <meta name="viewport" content="width=device-width, initial-scale=1.0">
                                                  <title>Image Animation</title>
                                                  <style>
                                                      body {
                                                          display: flex;
                                                          justify-content: center;
                                                          align-items: center;
                                                          height: 100vh;
                                                          margin: 0;
                                                          background-color: #f0f0f0;
                                                      }
                                                      img {
                                                          max-width: 100%;
                                                          max-height: 100%;
                                                      }
                                                  </style>
                                              </head>
                                              <body>
                                                  <img id="animation" src="" alt="Animation Frame">
                                              
                                                  <script>
                                                      const filenames = [
                                                          %%FILENAMES%%
                                                      ]; // Replace with your filenames
                                                      const frameInterval = 500; // Time in milliseconds between frames
                                                      let currentIndex = 0;
                                              
                                                      const imgElement = document.getElementById("animation");
                                              
                                                      function updateFrame() {
                                                        if (currentIndex < filenames.length) {
                                                          imgElement.src = filenames[currentIndex] + "?t=" + new Date().getTime(); // Prevent caching
                                                          currentIndex++;
                                                          setTimeout(updateFrame, frameInterval);
                                                        } else {
                                                          currentIndex = 0;
                                                          setTimeout(updateFrame, 3000); // 3-second delay before restarting the loop
                                                        }
                                                      }
                                                      
                                                      updateFrame();
                                                  </script>
                                              </body>
                                              </html>

                                              """;
    
    public static string GenerateVisualization(List<string> filenames)
    {
        var formattedFilenames = string.Join(", ", filenames.Select(f => $"\"{f}\""));
        return Template.Replace("%%FILENAMES%%", formattedFilenames);
    }
}
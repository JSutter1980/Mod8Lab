using NLog;

// See https://aka.ms/new-console-template for more information
string path = Directory.GetCurrentDirectory() + "\\nlog.config";

// create instance of Logger
var logger = LogManager.LoadConfiguration(path).GetCurrentClassLogger();
logger.Info("Program started");

string scrubbedFile = FileScrubber.ScrubMovies("movies.csv");
logger.Info(scrubbedFile);
MovieFile movieFile = new MovieFile(scrubbedFile);

string choice = "";
do
{
  // display choices to user
  Console.WriteLine("1) Add Movie");
  Console.WriteLine("2) Display All Movies");
  Console.WriteLine("3) Find Movie");
  Console.WriteLine("Enter to quit");
  // input selection
  choice = Console.ReadLine();
  logger.Info("User choice: {Choice}", choice);

  
  if (choice == "1")
  {
    // Add movie
       Movie movie = new Movie();
    // ask user to input movie title
    Console.WriteLine("Enter movie title");
    // input title
    movie.title = Console.ReadLine();
    // verify title is unique
    if (movieFile.isUniqueTitle(movie.title)){
            // input genres
      string input;
      do
      {
        // ask user to enter genre
        Console.WriteLine("Enter genre (or done to quit)");
        // input genre
        input = Console.ReadLine();
        // if user enters "done"
        // or does not enter a genre do not add it to list
        if (input != "done" && input.Length > 0)
        {
          movie.genres.Add(input);
        }
      } while (input != "done");

      Console.WriteLine("Enter Director name");
        movie.director = Console.ReadLine();


      Console.WriteLine("Enter movie runtime (hr:min:sec)"); 
        string[] timeValues = Console.ReadLine().Split(":");
        movie.runningTime = new TimeSpan(Int32.Parse(timeValues[0]), Int32.Parse(timeValues[1]), Int32.Parse(timeValues[2]));
        

      // specify if no genres are entered
      if (movie.genres.Count == 0)
      {
        movie.genres.Add("(no genres listed)");
      }
       // add movie
      movieFile.AddMovie(movie);
    }
  } else if (choice == "2")
  {
    // Display All Movies
    foreach(Movie m in movieFile.Movies)
    {
      Console.WriteLine(m.Display());
    }
  } else if (choice == "3")
  {
     Console.WriteLine("Enter search criteria:");
     string name = Console.ReadLine();

     //int num = movieFile.Movies.Where(m => m.title.Contains(name)).Count();

    var movieList = movieFile.Movies.Where(m => m.title.Contains(name, StringComparison.OrdinalIgnoreCase));
    Console.WriteLine($"There are {movieList.Count()} movies that contain {name}");
    foreach(Movie m in movieList)
{
    Console.WriteLine($"  {m.title}");
}

  }

} while (choice == "1" || choice == "2" || choice == "3");

logger.Info("Program ended");
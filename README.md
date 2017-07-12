# SoundexProcessor
Soundex processor to sort through records to group them into possible duplicates.

## How to install:

Download and load the solution into Visual Studio (this was compiled on Visual Studio Community 2017). 
Alternatively if you don't want to add another solution to your project, download the [SoundexProcessor.dll](https://github.com/omar-ebrahim/SoundexProcessor/tree/master/DLLs) and add as a reference to your project instead.

Feel free to add this to your own projects, either private or commercial.

## How to use

### Method calls

The main method call to use, and the only one you really need to use, is 

```
var list = new List<Tuple<int, string, string, DateTime>>();
list.Add(new Tuple<int, string, string, DateTime>(1, "mark", "jacobs", new DateTime(2000, 1, 1)));
list.Add(new Tuple<int, string, string, DateTime>(2, "david", "hollister", new DateTime(2000, 1, 1)));
list.Add(new Tuple<int, string, string, DateTime>(3, "marc", "jacobson", new DateTime(2000, 1, 1)));
list.Add(new Tuple<int, string, string, DateTime>(4, "dave", "hollister", new DateTime(2000, 1, 1)));

var processor = new Processor(MatchSensitivity.Medium);
var result = processor.Process(list);
```
This will process the list of names and produce a Dictionary of possible duplicates, using the forenames soundex and surname soundex rounded down to the nearest whole number (set by the MatchSensitivity Enum) and date of birth as the key. For example, processing the list above with the sensitivity set to MatchSensitivity.Medium (50) produces
```
Key (string): M600 J200 01/01/2000
Value (list of Tuples)
  1: mark jacobs (M620 J212). DOB: 01/01/2000
  2: marc jacobson (M620 J212). DOB: 01/01/2000
Key: D100 H400 01/01/2000
Value (list of tuples)
  3: david hollister (D130 H423). DOB: 01/01/2000
  3: dave hollister (D100 H423). DOB: 01/01/2000
```

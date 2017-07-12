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
Dictionary<string, List<Tuple<int, string, string, string, string, DateTime>>> result = processor.Process(list);

### Using the results
Below show how to use the results returned from the soundex processor, assuming the dictionary contains an entry with a non-empty list of tuples.
```
#### Item1: person ID
```
var personId = result[0][0].Item1; // 1
```
#### Item2: person forenames
```
var forenames = result[0][0].Item2; // mark
```
#### Item3: person surname
```
var surname = result[0][0].Item3; // jacobs
```
#### Item4: person forenames soundex
```
var forenamesSoundex = result[0][0].Item4; // M620
```
#### Item5: person surname soundex
```
var surnameSoundex = result[0][0].Item5; // J212
```
#### Item3: person date of birth
```
var surname = result[0][0].Item6.ToString("dd/MM/yyyy"); // 01/01/2000
```
### Dictionary key
The dictionary key rounds down the integer part of the soundex value to the nearest 25, 50 or 100 depending on the sensitivity in the MatchSensitivity enum. If the sensitivity is set to MatchSensitivity of VeryHigh, then the soundex values will not be adjusted at all.

The processor will process the list of names and produce a Dictionary of possible duplicates, using the adjusted forenames and surname soundexes and date of birth as the key. For example, processing the list above with the sensitivity set to MatchSensitivity.Medium (50) produces a key to use as the grouping value.
```
M600 J200 01/01/2000
```
Nicely formatted, it will display as
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

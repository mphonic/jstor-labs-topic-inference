# Jstor Labs Topic Inference
A .NET Console app that queries Jstor Labs' Topic Inferencer API to get a list of topics relevant to a given document.

### Usage
JstorLabsInferTopics.dll URL_OR_PATH_TO_FILE

You could also compile as an executable and just use JstorLabsInferTopics.

### Jstor API Key
You will need to acquire a free API key to use Jstor's API. See [Jstor Labs API docs](http://labs.jstor.org/api/docs/) for more info.

### Notes
When developing from Visual Studio, use ctrl-F5 to keep the console open after the app executes. The application will run with the default argument provided in Properties->Debug, which points to a [pdf online](http://labs.jstor.org/api/docs/).

When run, the app will return something like this:
```
Extracting text.
Extract Text Result: OK
Text extracted.
Retrieving topics.
Get Topics Result: OK

The following topics were returned:
Western civilization (weight 10)
Chinese history (weight 10)
Information technology (weight 10)
Heideggerianism (weight 9)
History (weight 8)
Philosophy (weight 7)
Cultural psychology (weight 7)
Sociology (weight 7)
Knowledge (weight 6)
Anti intellectualism (weight 6)
Chinese nationalism (weight 6)
Religious persecution (weight 6)
Technology (weight 5)
Computer technology (weight 5)
Best available technology (weight 5)
Appropriate technology (weight 5)
Technological independence (weight 4)
Hunting gear (weight 4)
High cost technology (weight 4)
Technology transfer (weight 4)
Cultural globalization (weight 4)
Pulp and paper industry (weight 4)
Educational technology (weight 4)
Geological museums (weight 4)
Capital saving technology (weight 4)
Press any key to continue . . .
```

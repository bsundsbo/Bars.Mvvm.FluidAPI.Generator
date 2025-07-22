# FluentApi.Common

## TO DO
### Do proper project references
This project contains the common code for the different generators. There is some complexity with including project
references for generators where NotFoundException is thrown when running the generator.

This is temporarily worked around by Linking files into the projects rather than project reference, and this can be resolved at a later point.

### Evaluate EquatableList class

This class may not be truly necessary as of now, as we are not using it for caching IncrementalGenerator results
 But I am keeping it here for future use, as it can be useful for comparing lists of items in a value-based manner,
 to boost performance when extending with new kinds of generators based on SyntaxNodes.
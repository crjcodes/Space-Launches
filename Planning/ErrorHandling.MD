# Error Handling Thoughts

## Expected Errors

1. Stop waiting on GET request 
	- Times out
	- Unknown cancellation
2.  GET Request result
	- GET does not complete successfully for unknown reason
	- Content wrong
		1. Not present
		2. Not as expected
3. Content does not convert successfully
4. Index page
	- Times out
	- Unknown exception
5. Routing 
	- To Index failure
	- Unknown routing problem
6. Standard Http Error Pages
	- 404


## Goals
Quoted from Red-Gate[1]: 

1.  Wrap in try/catch blocks any calls that can possibly generate an “expected” exception. In the catch block, you may log and/or swallow in some way the exception or, when allowed, just implement some compensation logic right in the block.
2.  Always have an Application_Error method, so that unexpected exceptions are stopped before they reach the outermost shell of ASP.NET MVC code and render as yellow screens of death.
3.  Use large chunks of validation logic to prevent errors as much as possible. As mentioned, errors originate from violated business rules and you are supposed to know them very well. When users find a way to bypass validation and the outlined user interface (i.e., they type an invalid URL on the browser address bar) you throw yourself an exception and redirect the application flow to Application_Error.

## Implement
- #2, above, first



# Resources
1. https://www.red-gate.com/simple-talk/dotnet/asp-net/pragmatic-web-error-handling-asp-net-mvc/
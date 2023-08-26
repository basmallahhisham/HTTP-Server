# HTTP-Server
Partial implementation of HTTP, utilizing OOP Principles.
- Threaded (multiple clients)
- GET only.
- Error handling:
  - Redirection
  - Page Not Found
  - Bad Request
  - Internal Server Error<br><br>
----------------
### Starting the Server
- Accept multiple clients by starting a thread for each accepted connection.
- Keep on accepting requests from the remote client until the client closes the socket (sends a zero length message).
- For each received request, the server must reply with a response.
----------------
### Receiving Request
The received request must be a valid HTTP request, else return 400 Bad Request.<br><br>
What is valid HTTP request?
- Check single space separating the request line parameters.	
	- Method URI HTTP-Version
- Check blank line separating the header lines and the content, even if the content is empty.
- Check valid URI
- Check at least request line and host header and blank lines exist.
----------------
### Response Headers
The response includes the following headers:
- **Content-Type:**
     We used only **text/html**
- **Content-Length:**
     The length of the content sent in the response.
  - This can be useful for the client to know how much data to expect and for the server to know how much data to send.<br>
    Example: When downloading a file from internet, the broweser looks at this header to determine how big the file is.
- **Date:**
     Current DateTime of the server.
- **Location:**
     Only if there is redirection.
----------------
### Handling Request
Using Configuration.RootPath, map the URI to the physical path.<br><br>
Example:<br> **configuration.RootPath**= “c:\intepub\wwwroot\fcis1” and **URI** = “/aboutus.html” <br>then **physical path**= “c:\intepub\wwwroot\fcis1\aboutus.html”

----------------
### Error handling

| Redirection | 
| ------------- | 
 If the URI exists in the configuration.RedirectionRules, <br>then return 301 Redirection Error and add location header with the new redirected URI.
 The content should be the content of the static page “redirect.html”  
 
| Not Found | 
| ------------- | 
If the physical file is not found,<br> return 404 Not Found error.
The content should be the content of the static page “Notfound.html”

| Bad Request | 
| ------------- |
If there is any parsing error in the request,<br> return 400 Bad Request Error.
The content should be loaded with the content of the static page “BadRequest.html”

| Internal Server Error | 
| ------------- |
If there is any unknown exception,<br> return 500 Internal Server Error.
The content should be the content of the static page “InternalError.html”


-----------

### Run the server

Try the following URIs in the web browser:<br>
<br>
http://localhost:1000/aboutus2.html
<br>should display aboutus2.html page<br>
<br>
http://localhost:1000/aboutus.html
<br>should display aboutus2.html (Redirection)<br>
<br>
http://localhost:1000/main.html
<br>should display main page<br>
<br>
http://localhost:1000/blabla.html
<br>should display 404 page.<br>
<br>

---------------

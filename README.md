# HTTP-Server
Partial implementation of HTTP, utilizing OOP Principles.
- Threaded (multiple clients)
- GET only.
- Error handling:
  - Page Not found
  - Bad Request
  - Redirection
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
     We used only text/html
- **Content-Length:**
     The length of the content sent in the response.
  - This can be useful for the client to know how much data to expect and for the server to know how much data to send.<br>
    Example: When downloading a file from internet, the broweser looks at this header to determine how big the file is.
- **Date:**
     Current DateTime of the server.
- **Location:**
     Only if there is redirection.
----------------




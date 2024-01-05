namespace APITests;

public class Helpers
{
    /// <summary>
    /// Turns the JSON string into an HttpContent object with the fitting 'content-type' header.
    /// </summary>
    /// <param name="body">The JSON string for the body.</param>
    /// <returns>An HttpContent the JSON string as its body and with 'content-type' set to 'application/json'</returns>
    public static HttpContent GetJSONContent(string body)
    {
        StringContent content = new StringContent(body);
        content.Headers.Remove("Content-Type");
        content.Headers.Add("Content-Type", "application/json");
        return content;
    }
}
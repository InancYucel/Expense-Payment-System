namespace Base.Encryption;

public class Md5Extension //Extension required for md5 encryption
{
    public static string Create(string input)
    {
        using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
        {
            var inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            var hashBytes = md5.ComputeHash(inputBytes);

            return Convert.ToHexString(hashBytes).ToLower();

        }
    }
    public static string GetHash(string input)
    {
        var hash = Create(input);
        return Create(hash);
    }
}
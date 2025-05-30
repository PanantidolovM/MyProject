using System;
using System.Security.Cryptography;
using System.Text;

public static class PasswordHelper
{
    /// <summary>
    /// Xác thực mật khẩu bằng cách tách chuỗi "salt:hash" được lưu, tính toán lại hash với salt,
    /// sau đó so sánh với giá trị hash đã lưu.
    /// Đồng thời ghi log các giá trị salt, stored hash và computed hash để debug.
    /// </summary>
    /// <param name="password">Mật khẩu người dùng nhập (plaintext)</param>
    /// <param name="storedPassword">Chuỗi được lưu theo định dạng "salt:hash"</param>
    /// <returns>true nếu mật khẩu hợp lệ, false nếu không.</returns>
    public static bool VerifyPassword(string password, string storedPassword)
    {
        // Tách chuỗi dựa vào dấu ':' để lấy salt và hash.
        var parts = storedPassword.Split(':');
        if (parts.Length != 2)
        {
            Console.WriteLine("Lỗi định dạng: Stored password không theo định dạng 'salt:hash'");
            return false;
        }
        
        string saltString = parts[0];
        string storedHash = parts[1];

        // Chuyển salt từ Base64 về mảng byte
        byte[] saltBytes;
        try
        {
            saltBytes = Convert.FromBase64String(saltString);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Không chuyển đổi salt thành byte được: {ex.Message}");
            return false;
        }
        
        // Tính toán hash của mật khẩu nhập dựa trên salt đã lấy
        using (var hmac = new HMACSHA512(saltBytes))
        {
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            string computedHashString = Convert.ToBase64String(computedHash);

            // Ghi log các giá trị để debug
            Console.WriteLine($"[DEBUG] Salt: {saltString}");
            Console.WriteLine($"[DEBUG] Stored Hash: {storedHash}");
            Console.WriteLine($"[DEBUG] Computed Hash: {computedHashString}");

            return computedHashString.Equals(storedHash, StringComparison.Ordinal);
        }
    }
}

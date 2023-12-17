namespace Trit.DemoConsole._6_Reflection;

public static class Demo
{
    // FEATURE: Zero-overhead member access with suppressed visibility checks
    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_combinationCode")]
    private static extern ref int GetCombinationCode(BankVault @this);

    [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "Decrypt")]
    private static extern string Decrypt(BankVault @this);

    [UnsafeAccessor(UnsafeAccessorKind.StaticField, Name = "MinValue")]
    private static extern ref DateTime DateTimeMinValue(DateTime @this);

    public static Task Main()
    {
        var bankVault = new BankVault();

        // BEFORE FEATURE: Zero-overhead member access with suppressed visibility checks
        FieldInfo field = typeof(BankVault)
            .GetField("_combinationCode",
                BindingFlags.Instance | BindingFlags.NonPublic)!;
        // Alternatively, this code is generated with Reflection.Emit or Linq.Expressions
        // which isn't AOT-friendly
        var oldCombinationCode = field.GetValue(bankVault);
        WriteLine($"The (old) combination code is: {oldCombinationCode}");

        // FEATURE: Zero-overhead member access with suppressed visibility checks
        WriteLine($"The combination code is: {GetCombinationCode(bankVault)}");
        WriteLine($"The decrypted password is: {Decrypt(bankVault)}");

        DateTimeMinValue(new DateTime()) = DateTime.UnixEpoch;
        WriteLine($"The new DateTime.MinValue is: {DateTime.MinValue}");

        return Task.CompletedTask;
    }

    public class BankVault
    {
        private readonly int _combinationCode;
        private readonly byte[] _secretPassword;

        public BankVault()
        {
            _combinationCode = RandomNumberGenerator.GetInt32(1_000_000, 42_000_000);
            var passwordBytes = "VwAxAG4AZAAwAHcAcwBTAHUAYwBrAHMAIQA="u8;
            var encryptedBytes = new byte[passwordBytes.Length];
            var codeBytes = BitConverter.GetBytes(_combinationCode);

            for(var i = 0; i < passwordBytes.Length; i++)
            {
                var e = (byte)(passwordBytes[i] ^ codeBytes[i % codeBytes.Length]);
                encryptedBytes[i] = e;
            }

            _secretPassword = encryptedBytes;
        }

        private string Decrypt()
        {
            var codeBytes = BitConverter.GetBytes(_combinationCode);
            var decryptedBytes = new char[_secretPassword.Length];

            for(var i = 0; i < _secretPassword.Length; i++)
            {
                var d = (char)(_secretPassword[i] ^ codeBytes[i % codeBytes.Length]);
                decryptedBytes[i] = d;
            }

            var base64Decoded = Convert
                .FromBase64CharArray(decryptedBytes, 0, decryptedBytes.Length);
            return Encoding.UTF8.GetString(base64Decoded);
        }
    }
}
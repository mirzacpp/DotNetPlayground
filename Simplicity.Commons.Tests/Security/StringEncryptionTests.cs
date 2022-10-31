using Simplicity.Commons.Security;

namespace Simplicity.Commons.Tests.Security
{
	public class StringEncryptionTests
	{
		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData("The quick brown fox jumps over the lazy dog")]		
		public void ShouldEncryptAndDecrypt(string plainText)
		{
			SimpleStringCipher stringCipher = new();

			var encryptedText = stringCipher.Encrypt(plainText);

			stringCipher.Decrypt(encryptedText!).ShouldBe(plainText);
		}
	}
}
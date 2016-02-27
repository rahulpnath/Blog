import java.io.UnsupportedEncodingException;
import java.net.URISyntaxException;
import java.util.concurrent.ExecutionException;
import java.util.concurrent.Future;
import com.microsoft.azure.keyvault.KeyVaultClient;
import com.microsoft.azure.keyvault.KeyVaultClientService;
import com.microsoft.azure.keyvault.KeyVaultConfiguration;
import com.microsoft.azure.keyvault.authentication.KeyVaultCredentials;
import com.microsoft.azure.keyvault.models.KeyOperationResult;
import com.microsoft.azure.keyvault.webkey.JsonWebKeyEncryptionAlgorithm;
import com.microsoft.windowsazure.Configuration;

public class Program {

	public static void main(String[] args)
			throws InterruptedException, ExecutionException, URISyntaxException, UnsupportedEncodingException {
		
		KeyVaultCredentials kvCred = new ClientSecretKeyVaultCredential("AD Application ID", "AD Application Secret");
		Configuration config = KeyVaultConfiguration.configure(null, kvCred);
		KeyVaultClient vc = KeyVaultClientService.create(config);

		System.out.println(vc.getBaseUri());
		String keyIdentifier = "https://rahulkeyvault.vault.azure.net:443/keys/NewKey";
		String textToEncrypt = "This is a test";

		byte[] byteText = textToEncrypt.getBytes("UTF-16");
		Future<KeyOperationResult> result = vc.encryptAsync(keyIdentifier, JsonWebKeyEncryptionAlgorithm.RSAOAEP, byteText); 
		
		KeyOperationResult keyoperationResult = result.get();
		System.out.println(keyoperationResult);
		result = vc.decryptAsync(keyIdentifier, "RSA-OAEP", keyoperationResult.getResult());

		String decryptedResult = new String(result.get().getResult(), "UTF-16");
		System.out.println(decryptedResult);
	}
}


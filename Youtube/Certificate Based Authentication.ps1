Login-AzureRmAccount

$certificateFilePath = "<Path to cer File>"
$certificate = New-Object System.Security.Cryptography.X509Certificates.X509Certificate2
$certificate.Import($certificateFilePath)
$rawCertificateData = $certificate.GetRawCertData()
$credential = [System.Convert]::ToBase64String($rawCertificateData)

$startDate= $certificate.GetEffectiveDateString()
$endDate = $certificate.GetExpirationDateString()
$adApplication = New-AzureRmADApplication `
-DisplayName "<Display Name>" `
-HomePage  "<URL>" `
-IdentifierUris "<URL>" `
# if you are on Azure 2.0 powershell use -CertValue (https://docs.microsoft.com/en-us/azure/azure-resource-manager/resource-group-authenticate-service-principal#create-service-principal-with-certificate)
-KeyValue  $credential `
-KeyType "AsymmetricX509Cert" `
-KeyUsage "Verify" `
-StartDate $startDate `
-EndDate $endDate


$servicePrincipal = New-AzureRmADServicePrincipal -ApplicationId $adApplication.ApplicationId
Set-AzureRmKeyVaultAccessPolicy `
-ResourceGroupName '<Resource Group Name>' `
-VaultName '<Key Vault Name>' `
-ObjectId  $servicePrincipal.Id `
-PermissionsToKeys all `
-PermissionsToSecrets all

$ServicePrincipal.ApplicationId

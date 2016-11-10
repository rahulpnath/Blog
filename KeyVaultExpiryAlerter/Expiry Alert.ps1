# Params - VaultName, KeyVersions, SecretVersion, AlertBeforeDays
$VaultName = ''
$IncludeAllKeyVersions = $true
$IncludeAllSecretVersions = $true
$AlertBeforeDays = 3

# Get Keys (flag to specify all versions)
   
# Convert keys common model of Object (Id, Name, Version,Expiry, Enabled)

Function New-KeyVaultObject
{
    param
    (
        [string]$Id,
        [string]$Name,
        [string]$Version,
        [System.Nullable[DateTime]]$Expires
    )

    $server = New-Object -TypeName PSObject
    $server | Add-Member -MemberType NoteProperty -Name Id -Value $Id
    $server | Add-Member -MemberType NoteProperty -Name Name -Value $Name
    $server | Add-Member -MemberType NoteProperty -Name Version -Value $Version
    $server | Add-Member -MemberType NoteProperty -Name Expires -Value $Expires
    
    return $server
}

function Get-AzureKeyVaultObjectKeys
{
  param
  (
   [string]$VaultName,
   [bool]$IncludeAllVersions
  )

  $vaultObjects = [System.Collections.ArrayList]@()
  $allKeys = Get-AzureKeyVaultKey -VaultName $VaultName
  foreach ($key in $allKeys) {
    if($IncludeAllVersions){
     $allSecretVersion = Get-AzureKeyVaultKey -VaultName $VaultName -IncludeVersions -Name $key.Name
     foreach($key in $allSecretVersion){
         $vaultObject = New-KeyVaultObject -Id $key.Id -Name $key.Name -Version $key.Version -Expires $key.Expires
         $vaultObjects.Add($vaultObject)
     }
     
    } else {
      $vaultObject = New-KeyVaultObject -Id $key.Id -Name $key.Name -Version $key.Version -Expires $key.Expires
      $vaultObjects.Add($vaultObject)
    }
  }
  
  return $vaultObjects
}

function Get-AzureKeyVaultObjectSecrets
{
  param
  (
   [string]$VaultName,
   [bool]$IncludeAllVersions
  )

  $vaultObjects = [System.Collections.ArrayList]@()
  $allSecrets = Get-AzureKeyVaultSecret -VaultName $VaultName
  foreach ($secret in $allSecrets) {
    if($IncludeAllVersions){
     $allSecretVersion = Get-AzureKeyVaultSecret -VaultName $VaultName -IncludeVersions -Name $secret.Name
     foreach($secret in $allSecretVersion){
         $vaultObject = New-KeyVaultObject -Id $secret.Id -Name $secret.Name -Version $secret.Version -Expires $secret.Expires
         $vaultObjects.Add($vaultObject)
     }
     
    } else {
      $vaultObject = New-KeyVaultObject -Id $secret.Id -Name $secret.Name -Version $secret.Version -Expires $secret.Expires
      $vaultObjects.Add($vaultObject)
    }
  }
  
  return $vaultObjects
}

$allKeyVaultObjects = [System.Collections.ArrayList]@()
$allKeyVaultObjects.AddRange((Get-AzureKeyVaultObjectKeys -VaultName $VaultName -IncludeAllVersions $IncludeAllKeyVersions))
$allKeyVaultObjects.AddRange((Get-AzureKeyVaultObjectSecrets -VaultName $VaultName -IncludeAllVersions $IncludeAllSecretVersions))


# Get expired Objects
$today = (Get-Date).Date
$expiredKeyVaultObjects = [System.Collections.ArrayList]@()
foreach($vaultObject in $allKeyVaultObjects){
if($vaultObject.Expires -and $vaultObject.Expires.AddDays(-$AlertBeforeDays).Date -lt $today)
{
  # add to expiry list
  $expiredKeyVaultObjects.Add($vaultObject) | Out-Null
  Write-Output "Expiring" $vaultObject.Id
}
}
    
# Pass to Alerter
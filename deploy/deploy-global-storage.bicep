@description('Name of the storage account to create.')
param resumeStorageAccountName string

@description('The name of the CosmosDB account to create.')
param cosmosUserDatabaseName string

@description('Location to deploy storage to.')
param primaryLocation string = resourceGroup().location

@description('Location to replicate data to.')
param secondaryLocation string

resource resumeStorage 'Microsoft.Storage/storageAccounts@2021-09-01' = {
  kind: 'StorageV2'
  location: primaryLocation
  name: resumeStorageAccountName
  sku: {
    name: 'Standard_LRS'
  }
}

resource userStorage 'Microsoft.DocumentDB/databaseAccounts@2022-05-15' = {
  name: cosmosUserDatabaseName
  kind: 'GlobalDocumentDB'
  location: primaryLocation
  properties: {
    databaseAccountOfferType: 'Standard'
    consistencyPolicy: {
      defaultConsistencyLevel: 'Session'
    }
    enableAutomaticFailover: true
    locations: [
      {
        locationName: primaryLocation
        failoverPriority: 0
        isZoneRedundant: false
      }
      {
        locationName: secondaryLocation
        failoverPriority: 1
        isZoneRedundant: false
      }
    ]
  }
}

targetScope = 'subscription'

@description('Which environment to deploy to.')
@allowed([
  'dev'
  'qa'
  'prod'
])
param environmentName string
var allEnvSettingInfo = {
  dev: {
    envIndicator: 'd'
    SKU_ASP: 'Y1'
    SKU_APIM: 'Developer'
    SKU_APIM_Capacity: 1
  }
  qa: {
    envIndicator: 'q'
    SKU_ASP: 'Y1'
    SKU_APIM: 'Developer'
    SKU_APIM_Capacity: 1
  }
  prod: {
    envIndicator: 'p'
    SKU_ASP: 'Y1'
    SKU_APIM: 'Developer'
    SKU_APIM_Capacity: 1
  }
}
var envSettings = allEnvSettingInfo[environmentName]

@description('Prefix to use when creating resource groups. Will be combined with environment indicator.')
param rgPrefix string = 'mjl'

@description('Azure locations to create resource groups in.')
param regionLocations array = ['centralus','eastus2','westus2']

@description('Azure location to create a global resource group in.')
param globalLocation string

var locationSettings = {
  eastus: {
    locIndicator: 'e'
  }
  westus: {
    locIndicator: 'w'
  }
  centralus: {
    locIndicator: 'w'
  }
  eastus2: {
    locIndicator: 'e2'
  }
  westus2: {
    locIndicator: 'w2'
  }
  centralus2: {
    locIndicator: 'c2'
  }
}

// Create Global Resource Group
resource globalGroup 'Microsoft.Resources/resourceGroups@2021-04-01' = {
  location: globalLocation
  name: '${rgPrefix}${envSettings.envIndicator}${locationSettings[globalLocation].locIndicator}global'
}

// Create Regional Resource Groups
resource groups 'Microsoft.Resources/resourceGroups@2021-04-01' = [for location in regionLocations: {
  name: '${rgPrefix}${envSettings.envIndicator}${locationSettings[location].locIndicator}'
  location: location
}]

// Storage Module to Create Global Storage
module storage './deploy-global-storage.bicep' = {
  name: 'storageDeployment'
  scope: globalGroup
  params: {
    resumeStorageAccountName: '${envSettings.envIndicator}globresumestor'
    cosmosUserDatabaseName: '${envSettings.envIndicator}globuserdb'
    primaryLocation: globalLocation
    secondaryLocation: 'eastus2'
  }
}

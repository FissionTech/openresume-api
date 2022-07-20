@description('Which environment to deploy to.')
@allowed([
  'dev'
  'qa'
  'prod'
])
param environmentName string

@description('Location for all resources.')
param location string = resourceGroup().location

@description('The storage account type to use when creating storage for function apps.')
@allowed([
  'Standard_LRS'
  'Standard_GRS'
  'Standard_RAGRS'
])
param funcAppStorageAccountType string = 'Standard_LRS'

var environmentSettings = {
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

var locationSettings = {
  eastus: {
    locIndicator: 'eus'
  }
  westus: {
    locIndicator: 'wus'
  }
  centralus: {
    locIndicator: 'wus'
  }
  eastus2: {
    locIndicator: 'eus2'
  }
  westus2: {
    locIndicator: 'wus2'
  }
  centralus2: {
    locIndicator: 'cus2'
  }
}

var deploymentEnv = environmentSettings[environmentName]
var locationInfo = locationSettings[location]

// General Variables
var primaryContactName = 'Michael Lindsay'
var primaryContactEmail = 'michael.lindsay.j@gmail.com'
var webappName = 'openresume-api'

// App Service Plan / Function App Variables
var aspName = 'mjl${deploymentEnv.envIndicator}${locationInfo.locIndicator}asp-${webappName}'
var functionAppName = 'mjl${deploymentEnv.envIndicator}${locationInfo.locIndicator}-${webappName}-func'
var functionAppRuntime = '.NET'

// Function App Storage Acct.
var functionAppStorageAccountName = '${deploymentEnv.envIndicator}${locationInfo.locIndicator}funcstor'

// APIM Variables
var apimName = 'mjl${deploymentEnv.envIndicator}${locationInfo.locIndicator}apim-${webappName}'
var apiName = 'openresume'

resource functionAppStorageAccount 'Microsoft.Storage/storageAccounts@2021-09-01' = {
  name: functionAppStorageAccountName
  location: location
  sku: {
    name: funcAppStorageAccountType
  }
  kind: 'Storage'
}

resource regionASP 'Microsoft.Web/serverfarms@2022-03-01' = {
  name: aspName
  location: location
  sku: {
    name: deploymentEnv.SKU_ASP
  }
  kind: 'functionapp'
}

resource functionApp 'Microsoft.Web/sites@2022-03-01' = {
  name: functionAppName
  kind: 'functionapp'
  location: location
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: regionASP.id
    siteConfig: {
      appSettings: [
        {
          name: 'AzureWebJobsStorage'
          value: 'DefaultEndpointsProtocol=https;AccountName=${functionAppStorageAccountName};EndpointSuffix=${environment().suffixes.storage};AccountKey=${functionAppStorageAccount.listKeys().keys[0].value}'
        }
        {
          name: 'FUNCTIONS_EXTENSION_VERSION'
          value: '~2'
        }
        {
          name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
          value: 'placeholder'
        }
        {
          name: 'FUNCTIONS_WORKER_RUNTIME'
          value: functionAppRuntime
        }
      ]
      minTlsVersion: '1.2'
      ftpsState: 'FtpsOnly'
    }
    httpsOnly: true
  }
}

resource regionAPIM 'Microsoft.ApiManagement/service@2021-08-01' = {
  location: location
  name: apimName
  properties:{
    publisherName: primaryContactName
    publisherEmail: primaryContactEmail
    notificationSenderEmail: 'apimgmt-noreply@mail.windowsazure.com'
    hostnameConfigurations: [
      {
        type: 'Proxy'
        hostName: '${apimName}.azure-api.net'
        defaultSslBinding: true
        certificateSource: 'BuiltIn'
      }
    ]
    virtualNetworkType: 'None'
    disableGateway: false
    apiVersionConstraint: {}
    publicNetworkAccess: 'Enabled'
  }
  sku: {
    name: deploymentEnv.SKU_APIM
    capacity: deploymentEnv.SKU_APIM_Capacity
  }

  resource api 'apis' = {
    name: apiName
    properties: {
      displayName: 'APIs'
      path: apimName
      protocols: [
        'http'
        'https'
      ]
    }
  }

  // resource portalSettingsDelegation 'portalsettings' = {
  //   name: 'delegation'
  //   properties: {
  //     subscriptions: {
  //       enabled: false
  //     }
  //     userRegistration: {
  //       enabled: false
  //     }
  //   }
  // }

  resource portalSettingsSignin 'portalsettings' = {
    name: 'signin'
    properties: {
      enabled: true
    }
  }

  resource portalSettingsSignUp 'portalsettings' = {
    name: 'signup'
    properties: {
      enabled: true
      termsOfService: {
        enabled: false
        consentRequired: false
      }
    }
  }

  resource defaultProduct 'products' = {
    name: 'default'
    properties: {
      displayName: 'Default'
      description: 'Default product limit. 5 calls/min.'
      subscriptionRequired: true
      approvalRequired: false
      subscriptionsLimit: 1
      state: 'published'
    }
  }
}



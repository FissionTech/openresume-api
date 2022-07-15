@description('Which environment to deploy to.')
@allowed([
  'dev'
  'qa'
  'prod'
])
param environmentName string

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

@description('Location for all resources.')
param location string = resourceGroup().location

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

var environment = environmentSettings[environmentName]
var locationInfo = locationSettings[location]

var primaryContactName = 'Michael Lindsay'
var primaryContactEmail = 'michael.lindsay.j@gmail.com'
var webappName = 'openresume-api'
var aspName = 'mjl${environment.envIndicator}${locationInfo.locIndicator}asp-${webappName}'
var apimName = 'mjl${environment.envIndicator}${locationInfo.locIndicator}apim-${webappName}'
var apiName = 'openresume'

resource regionASP 'Microsoft.Web/serverfarms@2022-03-01' = {
  name: aspName
  location: location
  sku: {
    name: environment.SKU_ASP
  }
  kind: 'functionapp'
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
    name: environment.SKU_APIM
    capacity: environment.SKU_APIM_Capacity
  }

  resource api 'apis' = {
    name: apiName
    properties: {
      displayName: 'APIs'
      path: './${apiName}'
    }
  }

  resource administratorsGroup 'groups' = {
    name: 'Administrators'
    properties: {
      displayName: 'Administrators'
      type: 'system'
    }
  }

  resource developersGroup 'groups' = {
    name: 'Developers'
    properties: {
      displayName: 'Developers'
      type: 'system'
    }
  }

  resource guestsGroup 'groups' = {
    name: 'Guests'
    properties: {
      displayName: 'Guests'
      type: 'system'
    }
  }

  resource portalSettingsDelegation 'portalsettings' = {
    name: 'delegation'
    properties: {
      subscriptions: {
        enabled: false
      }
      userRegistration: {
        enabled: false
      }
    }
  }

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



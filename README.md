# openresume-api

## Deployments
Deployments are handled via Bicep template files and powershell scripts.

### Region Deployments
APIs and resources associated directly with those APIs are deployed regionally as per Azure's recommended High-Availability protocols.

These regional deployments include various resources such as:
- API Management
- App Service Plans
- App Services / Function Apps
- Application Insights

To run a local regional deployment:

`deploy-region.ps1 -Environment <env> -RegionResourceGroups <string[]> -IsLocal`

Possible acceptable values for the `Environment` switch include:
- "dev"
- "qa"
- "prod"

Any valid regional resource group may be provided as a value in an array of resource group names.

#### Example:
To run a local deployment in the "dev" environment for three regions:
`.\deploy-region.ps1 -Environment dev -RegionResourceGroups mjldeus2,mjldcus2,mjldwus2 -IsLocal`

In order for a deployment to be successful, it is advised to run `Connect-AzAccount` beforehand.
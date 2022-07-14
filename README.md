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
`deploy-region.ps1 -Environment "dev" -IsLocal
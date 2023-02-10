
# .NET MVC Web app, Azure SQL
## Introduction

Contoso University is a sample application that demonstrates how to use Entity Framework Core in an ASP.NET Core Razor Pages web app. This app can be deployed to Microsoft's Azure cloud platform that allows you to build, deploy, and scale web apps.

This sample application is structured in a way to be compatible with [Azure Developer CLI (azd) ](https://github.com/Azure/azure-dev/) which are currently in preview.

> Refer to the [App Templates](https://github.com/microsoft/App-Templates) repo Readme for more samples that are compatible with [`azd`](https://github.com/Azure/azure-dev/).

## Tech stack:

- Azure
- Azure-sql-db
- .NET MVC Web
- Github Actions
- App Insights
- Log Analytics
- Bicep

## Application Architecture
This application utilizes the following Azure resources:

Azure App Services to host the Web frontend and API backend

Azure SQL DB for storage

Azure App Insights for monitoring 

Log Analystics for logging

Here's a high level architecture diagram that illustrates these components. Notice that these are all contained within a single resource group, that will be created for you when you create the resources.

![alt text](https://github.com/Azure-Samples/app-templates-dotnet-azuresql-appservice/blob/main/assets/ASP.NET%20SQL%20app%20template.jpg)

## Prerequisites
- Local shell with Azure CLI installed or [Azure Cloud Shell](https://ms.portal.azure.com/#cloudshell/)
- Azure Subscription, on which you are able to create resources and assign permissions
  - View your subscription using ```az account show``` 
  - If you don't have an account, you can [create one for free](https://azure.microsoft.com/free).  
  

Use your favorite terminal to run the az cli commands

## Getting Started
### Fork the repository

Fork the repository by clicking the 'Fork' button on the top right of the page.
This creates a local copy of the repository for you to work in. 

### Create Resource Group
Run the below az cli command to create a resource group named "rg-ContosoUniversityDemo". Change resourcegroup and location variable as required. If you are changing the resourcegroupname, you must update the resourcegroup name under ContosoUniversity - Infra.yml as well after the az group command is run.

```
resourceGroup='rg-ContosoUniversityDemo'
location='eastus'
az login
az group create --name $resourceGroup --location $location
```

 Navigate to [ContosoUniversity - Infra.yml](https://github.com/microsoft/csu-digiapps-p-azure-sql-db-dotnet/blob/main/.github/workflows/ContosoUniversity%20-%20Infra.yml) and modify the default AZ_RG_NAME as required:
      
  ```yaml
   AZ_RG_NAME: 'rg-ContosoUniversityDemo'
  ```

View your {subscription-id}

```
az account show
```

![Subscription Id](./assets/img02.png)

### Azure Configuration for GitHub  

The newly created GitHub repo uses GitHub Actions to deploy Azure resources and application code automatically. Your subscription is accessed using an Azure Service Principal. This is an identity created for use by applications, hosted services, and automated tools to access Azure resources. The following steps show how to [set up GitHub Actions to deploy Azure applications](https://github.com/Azure/actions-workflow-samples/blob/master/assets/create-secrets-for-GitHub-workflows.md)

Create an [Azure Service Principal](https://docs.microsoft.com/en-us/cli/azure/create-an-azure-service-principal-azure-cli) with **contributor** permissions on the subscription. The subscription-level permission is needed because the deployment includes creation of the resource group itself.
 * Run the following [az cli](https://docs.microsoft.com/en-us/cli/azure/?view=azure-cli-latest) command, either locally on your command line or on the Cloud Shell. 
   Replace {app-name} {subscription-id} with the id of the subscription in GUID format.
    ```bash  
       az ad sp create-for-rbac --name {app-name} --role contributor --scopes /subscriptions/{subscription-id}/resourceGroups/rg-ContosoUniversityDemo --sdk-auth     
      ```
 * The command should output a JSON object similar to this:
 ```
      {
        "clientId": "<GUID>",
        "clientSecret": "<GUID>",
        "subscriptionId": "<GUID>",
        "tenantId": "<GUID>",
        "activeDirectoryEndpointUrl": "<URL>",
        "resourceManagerEndpointUrl": "<URL>",
        "activeDirectoryGraphResourceId": "<URL>",
        "sqlManagementEndpointUrl": "<URL>",
        "galleryEndpointUrl": "<URL>",
        "managementEndpointUrl": "<URL>"
      }
   ```
 * Store the output JSON as the value of a GitHub Actions secret named 'AZURE_CREDENTIALS'
   + Under your repository name, click Settings. 
   + In the "Security" section of the sidebar, select Secrets. 
   + At the top of the page, click New repository secret
   + Provide the secret name as AZURE_CREDENTIALS
   + Add the output JSON as secret value
   
### Azure Resource Prefix

Create AZURE_APP_NAME as New Repository Secret and provide a secret value. This will be used as the prefix for webapp and web api names.

### Azure SQL Configuration

Add AZURE_SQL_PASSWORD as New Repository Secret and provide a secret value. This will be used as the user credential password for Azure SQL resource.

### GitHub Workflow 
Run the GitHub Action workflow "ContosoUniversity-Infra"
* If workflows are enabled for this repository it should run automatically. To enable the workflow run automatically, Go to Actions and enable the workflow if needed.
* Workflow can be manually run 
     + Under your repository name, click Actions .
     + In the left sidebar, click the workflow "ContosoUniversity-Infra".
     + Above the list of workflow runs, select Run workflow .
     + Use the Branch dropdown to select the workflow's main branch, Click Run workflow .

After deployment, below resources will be created

<img width="542" alt="image" src="https://user-images.githubusercontent.com/61921020/175617180-401ffc9b-1ccf-4b2d-af53-61473843fc22.png">


## Deploy Application

1. Create new secret for connection string - AZURE_CONTOSO_CONN_STRING. For secret value, copy the connection string by navigating to Azure SqlServer DB via portal, change {your_password} with AZURE_PASSWORD secret value. 

![SQLConnectionString](https://user-images.githubusercontent.com/61921020/175643833-13319e8d-8610-4259-a71e-5de54d3250eb.jpg)


![Connection String Database](./assets/img05.png)

2. Create new secret for Application Insights - AZURE_CONTOSO_APP_INSIGHTS.  For secret value, copy the connection string by navigating to Azure Application Insights resource overview via portal. 

![image](https://user-images.githubusercontent.com/61921020/175645187-1f457164-3d09-47ce-a053-a147f5e84170.png)


![Connection String - Application Insights](./assets/img06.png)

3. Run the GitHub Actions workflow "ContosoUniversity"
* If workflows are enabled for this repository it should run automatically. To enable the workflow run automatically, Go to Actions and enable the workflow if needed.
* Workflow can be manually run 
     + Under your repository name, click Actions .
     + In the left sidebar, click the workflow "ContosoUniversity".
     + Above the list of workflow runs, select Run workflow .
     + Use the Branch dropdown to select the workflow's main branch, Click Run workflow .

## Contoso University website

Once successfully deployed, the application is accessible through the webapp appservice endpoint hostname ending in -app.azurewebsites.net. Under your resource group, you can look at the "App Service" resource with "-app" suffixed to its name through the [Azure Portal](https://portal.azure.com) 

<img width="497" alt="image" src="https://user-images.githubusercontent.com/61921020/218220729-2dcc60cb-b3b6-47a9-9f62-508d2be83490.png">


## Clean up resources
When you are done, you can delete all the Azure resources created with this template by running the following command:

```
resourceGroup=rg-ContosoUniversityDemo
az group delete --name $resourceGroup
```
## Next steps

Now, you had an hands on introduction to .NET web applications and deployment on Azure, use the following articles for enterprise app patterns and landing zones
1. [How to apply the reliable web app pattern for .NET](https://review.learn.microsoft.com/en-us/azure/architecture/reference-architectures/reliable-web-app/dotnet/pattern-overview?branch=pr-en-us-9528)
2. [Azure App Service Landing Zone Accelerator](https://github.com/Azure/appservice-landing-zone-accelerator) is deployment architecture guidance for hardening and scaling Azure App Service deployments.


## Contributing

This project welcomes contributions and suggestions.  Most contributions require you to agree to a
Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us
the rights to use your contribution. For details, visit https://cla.opensource.microsoft.com.

When you submit a pull request, a CLA bot will automatically determine whether you need to provide
a CLA and decorate the PR appropriately (e.g., status check, comment). Simply follow the instructions
provided by the bot. You will only need to do this once across all repos using our CLA.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or
contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

## Trademarks

This project may contain trademarks or logos for projects, products, or services. Authorized use of Microsoft 
trademarks or logos is subject to and must follow 
[Microsoft's Trademark & Brand Guidelines](https://www.microsoft.com/en-us/legal/intellectualproperty/trademarks/usage/general).
Use of Microsoft trademarks or logos in modified versions of this project must not cause confusion or imply Microsoft sponsorship.
Any use of third-party trademarks or logos are subject to those third-party's policies.

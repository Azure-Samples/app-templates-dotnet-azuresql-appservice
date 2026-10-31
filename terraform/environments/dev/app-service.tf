resource "azurerm_service_plan" "app" {
  name                = "asp-cirrus-dev"
  resource_group_name = azurerm_resource_group.app.name
  location            = azurerm_resource_group.app.location
  os_type             = "Linux"
  sku_name            = "B1"

  tags = azurerm_resource_group.app.tags
}

resource "azurerm_linux_web_app" "web" {
  name                = "app-cirrus-web-dev-patty01"
  resource_group_name = azurerm_resource_group.app.name
  location            = azurerm_resource_group.app.location
  service_plan_id     = azurerm_service_plan.app.id
  https_only          = true

  site_config {
    minimum_tls_version = "1.2"

    application_stack {
      dotnet_version = "10.0"
    }
  }

  app_settings = {
    APPLICATIONINSIGHTS_CONNECTION_STRING = azurerm_application_insights.dev.connection_string
    Api__Address                          = "https://${azurerm_linux_web_app.api.default_hostname}/"
  }

  tags = azurerm_resource_group.app.tags
}

resource "azurerm_linux_web_app" "api" {
  name                      = "app-cirrus-api-dev-patty01"
  resource_group_name       = azurerm_resource_group.app.name
  location                  = azurerm_resource_group.app.location
  service_plan_id           = azurerm_service_plan.app.id
  https_only                = true
  virtual_network_subnet_id = module.networking.integration_subnet_id

  identity {
    type = "SystemAssigned"
  }
  site_config {
    minimum_tls_version = "1.2"

    application_stack {
      dotnet_version = "10.0"
    }
  }

  app_settings = {
    APPLICATIONINSIGHTS_CONNECTION_STRING          = azurerm_application_insights.dev.connection_string
    ConnectionStrings__ContosoUniversityAPIContext = "Server=tcp:${azurerm_mssql_server.app.fully_qualified_domain_name},1433;Database=${azurerm_mssql_database.app.name};Authentication=Active Directory Managed Identity;Encrypt=True;TrustServerCertificate=False;"
  }

  tags = azurerm_resource_group.app.tags
}


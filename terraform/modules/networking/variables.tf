variable "resource_group_name" {
  description = "Resource group where the network will be created."
  type        = string
}

variable "location" {
  description = "Azure region for the network."
  type        = string
}

variable "vnet_name" {
  description = "Name of the virtual network."
  type        = string
}

variable "address_space" {
  description = "IP address ranges for the virtual network."
  type        = list(string)
}

variable "integration_subnet_prefixes" {
  description = "IP address ranges for the App Service integration subnet."
  type        = list(string)
}

variable "private_endpoint_subnet_prefixes" {
  description = "IP address ranges for the private endpoint subnet."
  type        = list(string)
}